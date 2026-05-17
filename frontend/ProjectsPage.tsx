import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { projectAPI, projectMembersAPI, reviewPeriodAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { Project, ReviewPeriod } from '../types/types';
import { statusBadge, roleBadge } from '../utils/helpers';

// в”Ђв”Ђ helpers в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('ru', { day: '2-digit', month: 'short', year: 'numeric' });

const periodStatusBadge = (status: string) => {
  const map: Record<string, string> = {
    Draft: 'gray', Active: 'green', Closed: 'amber', Archive: 'red',
  };
  return <span className={`badge badge-${map[status] ?? 'gray'}`}>{status}</span>;
};

// в”Ђв”Ђ modals в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ

interface ModalProps { onClose: () => void }

const CreateProjectModal: React.FC<ModalProps> = ({ onClose }) => {
  const qc = useQueryClient();
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState('');

  const mutation = useMutation({
    mutationFn: () => projectAPI.create({ title, description }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['projects'] }); onClose(); },
    onError: () => setError('РќРµ СѓРґР°Р»РѕСЃСЊ СЃРѕР·РґР°С‚СЊ РїСЂРѕРµРєС‚'),
  });

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title">РЎРѕР·РґР°С‚СЊ РїСЂРѕРµРєС‚</div>
          <button className="btn-icon" onClick={onClose}>вњ•</button>
        </div>
        <div className="form-group">
          <label>РќР°Р·РІР°РЅРёРµ</label>
          <input
            value={title}
            onChange={e => setTitle(e.target.value)}
            placeholder="РќР°Р·РІР°РЅРёРµ РїСЂРѕРµРєС‚Р°"
            autoFocus
          />
        </div>
        <div className="form-group">
          <label>РћРїРёСЃР°РЅРёРµ</label>
          <textarea
            value={description}
            onChange={e => setDescription(e.target.value)}
            placeholder="РљСЂР°С‚РєРѕРµ РѕРїРёСЃР°РЅРёРµ"
            rows={3}
            style={{ resize: 'vertical' }}
          />
        </div>
        {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}
        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="btn btn-ghost" onClick={onClose}>РћС‚РјРµРЅР°</button>
          <button
            className="btn btn-primary"
            onClick={() => mutation.mutate()}
            disabled={!title.trim() || mutation.isPending}
          >
            {mutation.isPending ? 'РЎРѕР·РґР°РЅРёРµвЂ¦' : 'РЎРѕР·РґР°С‚СЊ'}
          </button>
        </div>
      </div>
    </div>
  );
};

const AddPeriodModal: React.FC<ModalProps & { projectId: string }> = ({ projectId, onClose }) => {
  const qc = useQueryClient();
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [error, setError] = useState('');

  const mutation = useMutation({
    mutationFn: () => reviewPeriodAPI.create({ projectId, startDate, endDate }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['projects'] }); onClose(); },
    onError: () => setError('РќРµ СѓРґР°Р»РѕСЃСЊ СЃРѕР·РґР°С‚СЊ РїРµСЂРёРѕРґ'),
  });

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title">Р”РѕР±Р°РІРёС‚СЊ РїРµСЂРёРѕРґ СЂРµРІСЊСЋ</div>
          <button className="btn-icon" onClick={onClose}>вњ•</button>
        </div>
        <div className="form-group">
          <label>Р”Р°С‚Р° РЅР°С‡Р°Р»Р°</label>
          <input type="date" value={startDate} onChange={e => setStartDate(e.target.value)} />
        </div>
        <div className="form-group">
          <label>Р”Р°С‚Р° РѕРєРѕРЅС‡Р°РЅРёСЏ</label>
          <input type="date" value={endDate} onChange={e => setEndDate(e.target.value)} />
        </div>
        {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}
        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="btn btn-ghost" onClick={onClose}>РћС‚РјРµРЅР°</button>
          <button
            className="btn btn-primary"
            onClick={() => mutation.mutate()}
            disabled={!startDate || !endDate || mutation.isPending}
          >
            {mutation.isPending ? 'РЎРѕР·РґР°РЅРёРµвЂ¦' : 'Р”РѕР±Р°РІРёС‚СЊ'}
          </button>
        </div>
      </div>
    </div>
  );
};

// в”Ђв”Ђ project detail panel в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ

interface DetailProps {
  project: Project;
  isAdmin: boolean;
  onBack: () => void;
}

const ProjectDetail: React.FC<DetailProps> = ({ project, isAdmin, onBack }) => {
  const qc = useQueryClient();
  const [tab, setTab] = useState<'members' | 'periods'>('members');
  const [showAddPeriod, setShowAddPeriod] = useState(false);

  const { data: membersData } = useQuery({
    queryKey: ['members', project.id],
    queryFn: () => projectMembersAPI.getMembers(project.id),
    select: r => r.data,
  });

  const startMutation = useMutation({
    mutationFn: () => projectAPI.start(project.id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['projects'] }),
  });

  const closeMutation = useMutation({
    mutationFn: () => projectAPI.close(project.id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['projects'] }),
  });

  const activatePeriod = useMutation({
    mutationFn: (periodId: string) =>
      reviewPeriodAPI.activate(project.id, periodId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['projects'] }),
  });

  const closePeriod = useMutation({
    mutationFn: (periodId: string) =>
      reviewPeriodAPI.close(project.id, periodId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['projects'] }),
  });

  const isActive = project.status;

  return (
    <div className="fade-in">
      {showAddPeriod && (
        <AddPeriodModal projectId={project.id} onClose={() => setShowAddPeriod(false)} />
      )}

      {/* Breadcrumb */}
      <div className="breadcrumb">
        <span onClick={onBack}>РџСЂРѕРµРєС‚С‹</span>
        <span className="breadcrumb-sep">вЂє</span>
        <span style={{ color: 'var(--text2)' }}>{project.title}</span>
      </div>

      {/* Header */}
      <div className="card" style={{ marginBottom: 16 }}>
        <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: 16 }}>
          <div>
            <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 6 }}>
              <div style={{ fontSize: 18, fontWeight: 700 }}>{project.title}</div>
              {statusBadge(isActive ? 'Active' : 'Closed')}
            </div>
            <div style={{ fontSize: 13, color: 'var(--text2)' }}>
              {project.description || <span style={{ color: 'var(--text3)' }}>Р‘РµР· РѕРїРёСЃР°РЅРёСЏ</span>}
            </div>
            <div style={{ display: 'flex', gap: 16, marginTop: 12, fontSize: 12, color: 'var(--text3)' }}>
              <span>в—« {project.members?.length ?? 0} СѓС‡Р°СЃС‚РЅРёРєРѕРІ</span>
              <span>в—· {project.reviewPeriods?.length ?? 0} РїРµСЂРёРѕРґРѕРІ</span>
            </div>
          </div>
          {isAdmin && (
            <div style={{ display: 'flex', gap: 8, flexShrink: 0 }}>
              {!isActive ? (
                <button
                  className="btn btn-primary btn-sm"
                  onClick={() => startMutation.mutate()}
                  disabled={startMutation.isPending}
                >
                  в–¶ Р—Р°РїСѓСЃС‚РёС‚СЊ
                </button>
              ) : (
                <button
                  className="btn btn-danger btn-sm"
                  onClick={() => closeMutation.mutate()}
                  disabled={closeMutation.isPending}
                >
                  вњ• Р—Р°РєСЂС‹С‚СЊ
                </button>
              )}
            </div>
          )}
        </div>
      </div>

      {/* Tabs */}
      <div className="tabs">
        <button className={`tab ${tab === 'members' ? 'active' : ''}`} onClick={() => setTab('members')}>
          РЈС‡Р°СЃС‚РЅРёРєРё
        </button>
        <button className={`tab ${tab === 'periods' ? 'active' : ''}`} onClick={() => setTab('periods')}>
          РџРµСЂРёРѕРґС‹ СЂРµРІСЊСЋ
        </button>
      </div>

      {/* Members tab */}
      {tab === 'members' && (
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">РЈС‡Р°СЃС‚РЅРёРєРё РїСЂРѕРµРєС‚Р°</div>
              <div className="card-sub">{membersData?.length ?? 0} С‡РµР»РѕРІРµРє</div>
            </div>
          </div>
          {!membersData || membersData.length === 0 ? (
            <div className="empty">
              <div className="empty-icon">в—Ћ</div>
              <p>РќРµС‚ СѓС‡Р°СЃС‚РЅРёРєРѕРІ</p>
            </div>
          ) : (
            <div className="table-wrap">
              <table>
                <thead>
                  <tr>
                    <th>РџРѕР»СЊР·РѕРІР°С‚РµР»СЊ</th>
                    <th>Р РѕР»СЊ</th>
                    <th>РЎС‚Р°С‚СѓСЃ</th>
                  </tr>
                </thead>
                <tbody>
                  {membersData.map((m: any) => (
                    <tr key={m.userId}>
                      <td>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                          <div className="avatar sm">
                            {m.userId.slice(0, 2).toUpperCase()}
                          </div>
                          <span style={{ fontFamily: 'var(--mono)', fontSize: 11, color: 'var(--text3)' }}>
                            {m.userId}
                          </span>
                        </div>
                      </td>
                      <td>{roleBadge(m.role)}</td>
                      <td>
                        <span className={`badge ${m.isActive ? 'badge-green' : 'badge-gray'}`}>
                          {m.isActive ? 'РђРєС‚РёРІРµРЅ' : 'РќРµР°РєС‚РёРІРµРЅ'}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}

      {/* Periods tab */}
      {tab === 'periods' && (
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">РџРµСЂРёРѕРґС‹ СЂРµРІСЊСЋ</div>
              <div className="card-sub">{project.reviewPeriods?.length ?? 0} РїРµСЂРёРѕРґРѕРІ</div>
            </div>
            {isAdmin && (
              <button className="btn btn-primary btn-sm" onClick={() => setShowAddPeriod(true)}>
                + Р”РѕР±Р°РІРёС‚СЊ
              </button>
            )}
          </div>
          {!project.reviewPeriods || project.reviewPeriods.length === 0 ? (
            <div className="empty">
              <div className="empty-icon">в—·</div>
              <p>РќРµС‚ РїРµСЂРёРѕРґРѕРІ СЂРµРІСЊСЋ</p>
            </div>
          ) : (
            <div className="period-list">
              {project.reviewPeriods.map((p: ReviewPeriod) => (
                <div key={p.id} className="period-item">
                  <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
                    {periodStatusBadge(p.status)}
                    <div>
                      <div style={{ fontSize: 13, fontWeight: 500 }}>
                        {formatDate(p.startDate)} вЂ” {formatDate(p.endDate)}
                      </div>
                      <div style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)' }}>
                        {p.id}
                      </div>
                    </div>
                  </div>
                  {isAdmin && (
                    <div style={{ display: 'flex', gap: 6 }}>
                      {p.status === 'Draft' && (
                        <button
                          className="btn btn-ghost btn-sm"
                          onClick={() => activatePeriod.mutate(p.id)}
                          disabled={activatePeriod.isPending}
                        >
                          в–¶ РђРєС‚РёРІРёСЂРѕРІР°С‚СЊ
                        </button>
                      )}
                      {p.status === 'Active' && (
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => closePeriod.mutate(p.id)}
                          disabled={closePeriod.isPending}
                        >
                          вњ• Р—Р°РєСЂС‹С‚СЊ
                        </button>
                      )}
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
};

// в”Ђв”Ђ main page в”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђв”Ђ

export const ProjectsPage: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';
  const [selected, setSelected] = useState<Project | null>(null);
  const [showCreate, setShowCreate] = useState(false);
  const [filter, setFilter] = useState<'all' | 'active' | 'closed'>('all');
  const [search, setSearch] = useState('');

  const { data: projects, isLoading } = useQuery({
    queryKey: ['projects'],
    queryFn: () => projectAPI.getAll(),
    select: r => r.data,
  });

  // If a project was selected, refresh it from cache after mutations
  const currentProject = selected
    ? projects?.find(p => p.id === selected.id) ?? selected
    : null;

  const filtered = (projects ?? []).filter(p => {
    if (filter === 'active' && !p.status) return false;
    if (filter === 'closed' && p.status) return false;
    if (search && !p.title.toLowerCase().includes(search.toLowerCase())) return false;
    return true;
  });

  if (currentProject) {
    return (
      <ProjectDetail
        project={currentProject}
        isAdmin={isAdmin}
        onBack={() => setSelected(null)}
      />
    );
  }

  return (
    <div className="fade-in">
      {showCreate && <CreateProjectModal onClose={() => setShowCreate(false)} />}

      {/* Toolbar */}
      <div style={{ display: 'flex', alignItems: 'center', gap: 12, marginBottom: 20 }}>
        <div className="input-group" style={{ flex: 1, maxWidth: 300 }}>
          <span className="input-icon">вЊ•</span>
          <input
            value={search}
            onChange={e => setSearch(e.target.value)}
            placeholder="РџРѕРёСЃРє РїСЂРѕРµРєС‚РѕРІвЂ¦"
          />
        </div>

        <div className="tabs" style={{ marginBottom: 0 }}>
          {(['all', 'active', 'closed'] as const).map(f => (
            <button
              key={f}
              className={`tab ${filter === f ? 'active' : ''}`}
              onClick={() => setFilter(f)}
            >
              {{ all: 'Р’СЃРµ', active: 'РђРєС‚РёРІРЅС‹Рµ', closed: 'Р—Р°РєСЂС‹С‚С‹Рµ' }[f]}
            </button>
          ))}
        </div>

        {isAdmin && (
          <button className="btn btn-primary" onClick={() => setShowCreate(true)}>
            + РЎРѕР·РґР°С‚СЊ
          </button>
        )}
      </div>

      {/* Stats row */}
      <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(3,1fr)', marginBottom: 20 }}>
        <div className="stat-card">
          <div className="stat-label">Р’СЃРµРіРѕ РїСЂРѕРµРєС‚РѕРІ</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{projects?.length ?? 0}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">РђРєС‚РёРІРЅС‹С…</div>
          <div className="stat-value" style={{ color: 'var(--success)' }}>
            {projects?.filter(p => p.status).length ?? 0}
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Р—Р°РєСЂС‹С‚С‹С…</div>
          <div className="stat-value" style={{ color: 'var(--text3)' }}>
            {projects?.filter(p => !p.status).length ?? 0}
          </div>
        </div>
      </div>

      {/* Project list */}
      {isLoading ? (
        <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>
          Р—Р°РіСЂСѓР·РєР°вЂ¦
        </div>
      ) : filtered.length === 0 ? (
        <div className="empty">
          <div className="empty-icon">в—«</div>
          <p>{search ? 'РќРёС‡РµРіРѕ РЅРµ РЅР°Р№РґРµРЅРѕ' : 'РќРµС‚ РїСЂРѕРµРєС‚РѕРІ'}</p>
        </div>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {filtered.map(p => (
            <div
              key={p.id}
              className="card"
              style={{ cursor: 'pointer', transition: 'border-color 0.15s' }}
              onClick={() => setSelected(p)}
              onMouseEnter={e => (e.currentTarget.style.borderColor = 'var(--border2)')}
              onMouseLeave={e => (e.currentTarget.style.borderColor = 'var(--border)')}
            >
              <div style={{ display: 'flex', alignItems: 'center', gap: 14 }}>
                {/* Icon */}
                <div style={{
                  width: 40, height: 40, borderRadius: 10, flexShrink: 0,
                  background: p.status
                    ? 'linear-gradient(135deg,rgba(79,124,255,.2),rgba(124,92,252,.2))'
                    : 'var(--bg3)',
                  display: 'flex', alignItems: 'center', justifyContent: 'center',
                  fontSize: 18, color: p.status ? 'var(--accent)' : 'var(--text3)',
                }}>
                  в—«
                </div>

                {/* Info */}
                <div style={{ flex: 1, minWidth: 0 }}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 2 }}>
                    <span style={{ fontWeight: 600, fontSize: 14 }}>{p.title}</span>
                    {statusBadge(p.status ? 'Active' : 'Closed')}
                  </div>
                  <div style={{ fontSize: 12, color: 'var(--text3)' }} className="truncate">
                    {p.description || 'Р‘РµР· РѕРїРёСЃР°РЅРёСЏ'}
                  </div>
                </div>

                {/* Meta */}
                <div style={{ display: 'flex', gap: 20, flexShrink: 0, fontSize: 12, color: 'var(--text3)' }}>
                  <div style={{ textAlign: 'center' }}>
                    <div style={{ fontWeight: 600, color: 'var(--text)', fontSize: 15 }}>
                      {p.members?.length ?? 0}
                    </div>
                    <div>СѓС‡Р°СЃС‚РЅРёРєРѕРІ</div>
                  </div>
                  <div style={{ textAlign: 'center' }}>
                    <div style={{ fontWeight: 600, color: 'var(--text)', fontSize: 15 }}>
                      {p.reviewPeriods?.length ?? 0}
                    </div>
                    <div>РїРµСЂРёРѕРґРѕРІ</div>
                  </div>
                  {p.reviewPeriods?.some(rp => rp.status === 'Active') && (
                    <div style={{ textAlign: 'center' }}>
                      <span className="badge badge-green">РџРµСЂРёРѕРґ Р°РєС‚РёРІРµРЅ</span>
                    </div>
                  )}
                </div>

                <span style={{ color: 'var(--text3)', fontSize: 16 }}>вЂє</span>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};
