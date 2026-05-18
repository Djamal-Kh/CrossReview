import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { authAPI, userManagementAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { User } from '../types/types';

// ── Add Admin Modal ───────────────────────────────────────────────────────────

const AddAdminModal: React.FC<{ onClose: () => void }> = ({ onClose }) => {
  const qc = useQueryClient();
  const [form, setForm] = useState({
    firstName: '', lastName: '', email: '', password: '', phoneNumber: '',
  });
  const [error, setError] = useState('');

  const set = (k: keyof typeof form) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [k]: e.target.value }));

  const mutation = useMutation({
    mutationFn: () => userManagementAPI.addAdmin(form),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['users'] }); onClose(); },
    onError: () => setError('Не удалось создать администратора'),
  });

  const valid = form.firstName && form.lastName && form.email && form.password;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title">Добавить администратора</div>
          <button className="btn-icon" onClick={onClose}>✕</button>
        </div>

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
          <div className="form-group" style={{ marginBottom: 0 }}>
            <label>Имя</label>
            <input value={form.firstName} onChange={set('firstName')} placeholder="Иван" autoFocus />
          </div>
          <div className="form-group" style={{ marginBottom: 0 }}>
            <label>Фамилия</label>
            <input value={form.lastName} onChange={set('lastName')} placeholder="Иванов" />
          </div>
        </div>

        <div className="form-group" style={{ marginTop: 12 }}>
          <label>Email</label>
          <input type="email" value={form.email} onChange={set('email')} placeholder="admin@company.com" />
        </div>
        <div className="form-group">
          <label>Пароль</label>
          <input type="password" value={form.password} onChange={set('password')} placeholder="Минимум 8 символов" />
        </div>
        <div className="form-group">
          <label>Телефон</label>
          <input value={form.phoneNumber} onChange={set('phoneNumber')} placeholder="+71234567890" />
        </div>

        {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}

        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
          <button
            className="btn btn-primary"
            onClick={() => mutation.mutate()}
            disabled={!valid || mutation.isPending}
          >
            {mutation.isPending ? 'Создание…' : 'Создать'}
          </button>
        </div>
      </div>
    </div>
  );
};

// ── User Detail Panel ─────────────────────────────────────────────────────────

interface DetailProps {
  user: User;
  currentUserId: string;
  onBack: () => void;
}

const UserDetail: React.FC<DetailProps> = ({ user, currentUserId, onBack }) => {
  const qc = useQueryClient();
  const isSelf = user.id === currentUserId;

  const deleteMutation = useMutation({
    mutationFn: () => userManagementAPI.delete(user.id),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['users'] }); onBack(); },
  });

  const initials = `${user.firstName[0] ?? ''}${user.lastName[0] ?? ''}`.toUpperCase();

  return (
    <div className="fade-in">
      <div className="breadcrumb">
        <span onClick={onBack}>Пользователи</span>
        <span className="breadcrumb-sep">›</span>
        <span style={{ color: 'var(--text2)' }}>{user.firstName} {user.lastName}</span>
      </div>

      {/* Profile card */}
      <div className="card" style={{ marginBottom: 16 }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: 20 }}>
          {/* Avatar */}
          <div style={{
            width: 64, height: 64, borderRadius: 16, flexShrink: 0,
            background: user.role === 'Admin'
              ? 'linear-gradient(135deg,rgba(124,92,252,.3),rgba(79,124,255,.3))'
              : 'linear-gradient(135deg,rgba(79,124,255,.2),rgba(56,189,248,.2))',
            border: `2px solid ${user.role === 'Admin' ? 'rgba(124,92,252,.4)' : 'rgba(79,124,255,.3)'}`,
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            fontSize: 22, fontWeight: 700, color: user.role === 'Admin' ? 'var(--accent2)' : 'var(--accent)',
          }}>
            {initials}
          </div>

          <div style={{ flex: 1 }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 4 }}>
              <div style={{ fontSize: 18, fontWeight: 700 }}>{user.firstName} {user.lastName}</div>
              <span className={`badge ${user.role === 'Admin' ? 'badge-purple' : 'badge-blue'}`}>
                {user.role}
              </span>
              {isSelf && <span className="badge badge-green">Вы</span>}
            </div>
            <div style={{ fontSize: 13, color: 'var(--text2)' }}>{user.email}</div>
            <div style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)', marginTop: 4 }}>
              {user.id}
            </div>
          </div>

          {!isSelf && (
            <button
              className="btn btn-danger btn-sm"
              onClick={() => { if (confirm(`Удалить пользователя ${user.firstName} ${user.lastName}?`)) deleteMutation.mutate(); }}
              disabled={deleteMutation.isPending}
              style={{ flexShrink: 0 }}
            >
              ✕ Удалить
            </button>
          )}
        </div>
      </div>

      {/* Details */}
      <div className="card">
        <div className="card-title" style={{ marginBottom: 16 }}>Информация</div>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
          {[
            { label: 'Имя', value: user.firstName },
            { label: 'Фамилия', value: user.lastName },
            { label: 'Email', value: user.email },
            { label: 'Роль', value: user.role },
            { label: 'ID', value: user.id },
          ].map(({ label, value }) => (
            <div key={label} style={{
              display: 'flex', alignItems: 'center', justifyContent: 'space-between',
              padding: '10px 14px', background: 'var(--bg2)', borderRadius: 8,
            }}>
              <span style={{ fontSize: 12, color: 'var(--text3)' }}>{label}</span>
              <span style={{
                fontSize: 13,
                fontFamily: label === 'ID' ? 'var(--mono)' : undefined,
                color: label === 'ID' ? 'var(--text2)' : 'var(--text)',
              }}>
                {value}
              </span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

// ── Main Page ─────────────────────────────────────────────────────────────────

export const UsersPage: React.FC = () => {
  const { user: currentUser } = useAuth();
  const qc = useQueryClient();
  const [selected, setSelected] = useState<User | null>(null);
  const [showAddAdmin, setShowAddAdmin] = useState(false);
  const [filter, setFilter] = useState<'all' | 'Admin' | 'User'>('all');
  const [search, setSearch] = useState('');

  const { data: users, isLoading } = useQuery({
    queryKey: ['users'],
    queryFn: () => authAPI.getAll(),
    select: (r: any) => r.data as User[],
  });

  const currentSelected = selected
    ? users?.find(u => u.id === selected.id) ?? selected
    : null;

  const filtered = (users ?? []).filter(u => {
    if (filter !== 'all' && u.role !== filter) return false;
    if (search) {
      const q = search.toLowerCase();
      return (
        u.firstName.toLowerCase().includes(q) ||
        u.lastName.toLowerCase().includes(q) ||
        u.email.toLowerCase().includes(q)
      );
    }
    return true;
  });

  if (currentSelected) {
    return (
      <UserDetail
        user={currentSelected}
        currentUserId={currentUser?.id ?? ''}
        onBack={() => setSelected(null)}
      />
    );
  }

  const adminCount = users?.filter(u => u.role === 'Admin').length ?? 0;
  const userCount = users?.filter(u => u.role === 'User').length ?? 0;

  return (
    <div className="fade-in">
      {showAddAdmin && <AddAdminModal onClose={() => setShowAddAdmin(false)} />}

      {/* Stats */}
      <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(3,1fr)', marginBottom: 20 }}>
        <div className="stat-card">
          <div className="stat-label">Всего</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{users?.length ?? 0}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Администраторов</div>
          <div className="stat-value" style={{ color: 'var(--accent2)' }}>{adminCount}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Пользователей</div>
          <div className="stat-value" style={{ color: 'var(--accent)' }}>{userCount}</div>
        </div>
      </div>

      {/* Toolbar */}
      <div style={{ display: 'flex', alignItems: 'center', gap: 12, marginBottom: 16 }}>
        <div className="input-group" style={{ flex: 1, maxWidth: 300 }}>
          <span className="input-icon">⌕</span>
          <input
            value={search}
            onChange={e => setSearch(e.target.value)}
            placeholder="Поиск по имени или email…"
          />
        </div>

        <div className="tabs" style={{ marginBottom: 0 }}>
          {(['all', 'Admin', 'User'] as const).map(f => (
            <button
              key={f}
              className={`tab ${filter === f ? 'active' : ''}`}
              onClick={() => setFilter(f)}
            >
              {{ all: 'Все', Admin: 'Админы', User: 'Пользователи' }[f]}
            </button>
          ))}
        </div>

        <button className="btn btn-primary" style={{ marginLeft: 'auto' }} onClick={() => setShowAddAdmin(true)}>
          + Добавить админа
        </button>
      </div>

      {/* List */}
      {isLoading ? (
        <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>Загрузка…</div>
      ) : filtered.length === 0 ? (
        <div className="empty">
          <div className="empty-icon">◎</div>
          <p>{search ? 'Ничего не найдено' : 'Нет пользователей'}</p>
        </div>
      ) : (
        <div className="card">
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Пользователь</th>
                  <th>Email</th>
                  <th>Роль</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {filtered.map(u => {
                  const initials = `${u.firstName[0] ?? ''}${u.lastName[0] ?? ''}`.toUpperCase();
                  const isSelf = u.id === currentUser?.id;
                  return (
                    <tr
                      key={u.id}
                      style={{ cursor: 'pointer' }}
                      onClick={() => setSelected(u)}
                    >
                      <td>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                          <div style={{
                            width: 32, height: 32, borderRadius: 8, flexShrink: 0,
                            background: u.role === 'Admin'
                              ? 'linear-gradient(135deg,rgba(124,92,252,.2),rgba(79,124,255,.2))'
                              : 'var(--bg3)',
                            display: 'flex', alignItems: 'center', justifyContent: 'center',
                            fontSize: 12, fontWeight: 600,
                            color: u.role === 'Admin' ? 'var(--accent2)' : 'var(--text2)',
                          }}>
                            {initials}
                          </div>
                          <div>
                            <div style={{ fontSize: 13, fontWeight: 500 }}>
                              {u.firstName} {u.lastName}
                              {isSelf && <span className="badge badge-green" style={{ marginLeft: 6 }}>Вы</span>}
                            </div>
                            <div style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)' }}>
                              {u.id.slice(0, 16)}…
                            </div>
                          </div>
                        </div>
                      </td>
                      <td style={{ color: 'var(--text2)', fontSize: 13 }}>{u.email}</td>
                      <td>
                        <span className={`badge ${u.role === 'Admin' ? 'badge-purple' : 'badge-blue'}`}>
                          {u.role}
                        </span>
                      </td>
                      <td>
                        <span style={{ color: 'var(--text3)', fontSize: 16 }}>›</span>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};
