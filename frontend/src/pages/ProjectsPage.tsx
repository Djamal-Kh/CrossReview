import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { projectAPI, projectMembersAPI, reviewPeriodAPI, authAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { Project, ReviewPeriod, User } from '../types/types';
import { statusBadge, roleBadge } from '../utils/helpers'; // Возвращаем statusBadge для проекта
import { ReviewPeriodStatus } from '../types/enums';

// ── helpers ───────────────────────────────────────────────────────────────────

const formatDate = (iso: string) =>
    new Date(iso).toLocaleDateString('ru', { day: '2-digit', month: 'short', year: 'numeric' });

// Новая логика для периодов на месте, мы её не трогаем
const periodStatusBadge = (status: ReviewPeriodStatus) => {
    const colorMap: Record<ReviewPeriodStatus, string> = {
        [ReviewPeriodStatus.Draft]: 'gray',
        [ReviewPeriodStatus.Active]: 'green',
        [ReviewPeriodStatus.Closed]: 'amber',
        [ReviewPeriodStatus.Archive]: 'red',
    };

    const textMap: Record<ReviewPeriodStatus, string> = {
        [ReviewPeriodStatus.Draft]: 'Draft',
        [ReviewPeriodStatus.Active]: 'Active',
        [ReviewPeriodStatus.Closed]: 'Closed',
        [ReviewPeriodStatus.Archive]: 'Archive',
    };

    const color = colorMap[status] ?? 'gray';
    const text = textMap[status] ?? 'Unknown';

    return <span className={`badge badge-${color}`}>{text}</span>;
};

// ── modals ────────────────────────────────────────────────────────────────────

interface ModalProps { onClose: () => void }

const CreateProjectModal: React.FC<ModalProps> = ({ onClose }) => {
    const qc = useQueryClient();
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [error, setError] = useState('');

    const mutation = useMutation({
        mutationFn: () => projectAPI.create({ title, description }),
        onSuccess: () => { qc.invalidateQueries({ queryKey: ['projects'] }); onClose(); },
        onError: () => setError('Не удалось создать проект'),
    });

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal" onClick={e => e.stopPropagation()}>
                <div className="modal-header">
                    <div className="modal-title">Создать проект</div>
                    <button className="btn-icon" onClick={onClose}>✕</button>
                </div>
                <div className="form-group">
                    <label>Название</label>
                    <input
                        value={title}
                        onChange={e => setTitle(e.target.value)}
                        placeholder="Название проекта"
                        autoFocus
                    />
                </div>
                <div className="form-group">
                    <label>Описание</label>
                    <textarea
                        value={description}
                        onChange={e => setDescription(e.target.value)}
                        placeholder="Краткое описание"
                        rows={3}
                        style={{ resize: 'vertical' }}
                    />
                </div>
                {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}
                <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                    <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
                    <button
                        className="btn btn-primary"
                        onClick={() => mutation.mutate()}
                        disabled={!title.trim() || mutation.isPending}
                    >
                        {mutation.isPending ? 'Создание…' : 'Создать'}
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
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['projects'] });
            qc.invalidateQueries({ queryKey: ['project', projectId] });
            onClose();
        },
        onError: () => setError('Не удалось создать период'),
    });

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal" onClick={e => e.stopPropagation()}>
                <div className="modal-header">
                    <div className="modal-title">Добавить период ревью</div>
                    <button className="btn-icon" onClick={onClose}>✕</button>
                </div>
                <div className="form-group">
                    <label>Дата начала</label>
                    <input type="date" value={startDate} onChange={e => setStartDate(e.target.value)} />
                </div>
                <div className="form-group">
                    <label>Дата окончания</label>
                    <input type="date" value={endDate} onChange={e => setEndDate(e.target.value)} />
                </div>
                {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}
                <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                    <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
                    <button
                        className="btn btn-primary"
                        onClick={() => mutation.mutate()}
                        disabled={!startDate || !endDate || mutation.isPending}
                    >
                        {mutation.isPending ? 'Создание…' : 'Добавить'}
                    </button>
                </div>
            </div>
        </div>
    );
};

const AddMemberModal: React.FC<ModalProps & { projectId: string }> = ({ projectId, onClose }) => {
    const qc = useQueryClient();
    const [userId, setUserId] = useState('');
    const [role, setRole] = useState('Developer');
    const [error, setError] = useState('');

    const { data: users } = useQuery({
        queryKey: ['users'],
        queryFn: () => authAPI.getAll(),
        select: r => r.data,
    });

    const mutation = useMutation({
        mutationFn: () => projectMembersAPI.addMember({ projectId, userId, role }),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['members', projectId] });
            qc.invalidateQueries({ queryKey: ['projects'] });
            onClose();
        },
        onError: () => setError('Не удалось добавить участника'),
    });

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal" onClick={e => e.stopPropagation()}>
                <div className="modal-header">
                    <div className="modal-title">Добавить участника</div>
                    <button className="btn-icon" onClick={onClose}>✕</button>
                </div>

                <div className="form-group">
                    <label>Пользователь</label>
                    <select value={userId} onChange={e => setUserId(e.target.value)}>
                        <option value="">Выберите пользователя</option>
                        {users?.map((u: User) => (
                            <option key={u.id} value={u.id}>
                                {u.firstName} {u.lastName} ({u.email})
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Роль</label>
                    <select value={role} onChange={e => setRole(e.target.value)}>
                        <option value="Developer">Developer</option>
                        <option value="Manager">Manager</option>
                        <option value="TeamLead">TeamLead</option>
                    </select>
                </div>

                {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}

                <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                    <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
                    <button
                        className="btn btn-primary"
                        onClick={() => mutation.mutate()}
                        disabled={!userId || mutation.isPending}
                    >
                        {mutation.isPending ? 'Добавление…' : 'Добавить'}
                    </button>
                </div>
            </div>
        </div>
    );
};

// ── project detail panel ──────────────────────────────────────────────────────

interface DetailProps {
    project: Project;
    isAdmin: boolean;
    onBack: () => void;
}

const ProjectDetail: React.FC<DetailProps> = ({ project, isAdmin, onBack }) => {
    const qc = useQueryClient();
    const [tab, setTab] = useState<'members' | 'periods'>('members');
    const [showAddPeriod, setShowAddPeriod] = useState(false);
    const [showAddMember, setShowAddMember] = useState(false);

    // Загружаем полные данные проекта
    const { data: fullProject, isLoading: projectLoading } = useQuery({
        queryKey: ['project', project.id],
        queryFn: () => projectAPI.getById(project.id),
        select: r => r.data,
    });

    const currentProject = fullProject ?? project;
    const isActive = !!currentProject.status;

    const { data: membersData } = useQuery({
        queryKey: ['members', project.id],
        queryFn: () => projectMembersAPI.getMembers(project.id),
        select: r => r.data,
    });

    // Получаем список всех пользователей из кэша/API для сопоставления Id -> Имя
    const { data: users } = useQuery({
        queryKey: ['users'],
        queryFn: () => authAPI.getAll(),
        select: r => r.data,
    });

    const startMutation = useMutation({
        mutationFn: () => projectAPI.start(project.id),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['projects'] });
            qc.invalidateQueries({ queryKey: ['project', project.id] });
        },
    });

    const closeMutation = useMutation({
        mutationFn: () => projectAPI.close(project.id),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['projects'] });
            qc.invalidateQueries({ queryKey: ['project', project.id] });
        },
    });

    const activatePeriod = useMutation({
        mutationFn: (periodId: string) =>
            reviewPeriodAPI.activate(project.id, periodId),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['projects'] });
            qc.invalidateQueries({ queryKey: ['project', project.id] });
        },
    });

    const closePeriod = useMutation({
        mutationFn: (periodId: string) =>
            reviewPeriodAPI.close(project.id, periodId),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['projects'] });
            qc.invalidateQueries({ queryKey: ['project', project.id] });
        },
    });

    if (projectLoading) {
        return (
            <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>
                Загрузка…
            </div>
        );
    }

    return (
        <div className="fade-in">
            {showAddPeriod && (
                <AddPeriodModal projectId={project.id} onClose={() => setShowAddPeriod(false)} />
            )}
            {showAddMember && (
                <AddMemberModal projectId={project.id} onClose={() => setShowAddMember(false)} />
            )}

            <div className="breadcrumb">
                <span onClick={onBack}>Проекты</span>
                <span className="breadcrumb-sep">›</span>
                <span style={{ color: 'var(--text2)' }}>{currentProject.title}</span>
            </div>

            <div className="card" style={{ marginBottom: 16 }}>
                <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: 16 }}>
                    <div>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 6 }}>
                            <div style={{ fontSize: 18, fontWeight: 700 }}>{currentProject.title}</div>
                            {statusBadge(isActive ? 'Active' : 'Closed')}
                        </div>
                        <div style={{ fontSize: 13, color: 'var(--text2)' }}>
                            {currentProject.description || <span style={{ color: 'var(--text3)' }}>Без описания</span>}
                        </div>
                        <div style={{ display: 'flex', gap: 16, marginTop: 12, fontSize: 12, color: 'var(--text3)' }}>
                            <span>◫ {currentProject.members?.length ?? 0} участников</span>
                            <span>◷ {currentProject.reviewPeriods?.length ?? 0} периодов</span>
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
                                    ▶ Запустить
                                </button>
                            ) : (
                                <button
                                    className="btn btn-danger btn-sm"
                                    onClick={() => closeMutation.mutate()}
                                    disabled={closeMutation.isPending}
                                >
                                    ✕ Закрыть
                                </button>
                            )}
                        </div>
                    )}
                </div>
            </div>

            <div className="tabs">
                <button className={`tab ${tab === 'members' ? 'active' : ''}`} onClick={() => setTab('members')}>
                    Участники
                </button>
                <button className={`tab ${tab === 'periods' ? 'active' : ''}`} onClick={() => setTab('periods')}>
                    Периоды ревью
                </button>
            </div>

            {tab === 'members' && (
                <div className="card">
                    <div className="card-header">
                        <div>
                            <div className="card-title">Участники проекта</div>
                            <div className="card-sub">{membersData?.length ?? 0} человек</div>
                        </div>
                        {isAdmin && (
                            <button className="btn btn-primary btn-sm" onClick={() => setShowAddMember(true)}>
                                + Добавить
                            </button>
                        )}
                    </div>
                    {!membersData || membersData.length === 0 ? (
                        <div className="empty">
                            <div className="empty-icon">◎</div>
                            <p>Нет участников</p>
                        </div>
                    ) : (
                        <div className="table-wrap">
                            <table>
                                <thead>
                                    <tr>
                                        <th>Пользователь</th>
                                        <th>Роль</th>
                                        <th>Статус</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {membersData.map((m: any) => {
                                        // Ищем профиль пользователя по его Id
                                        const memberUser = users?.find((u: User) => u.id === m.userId);
                                        
                                        // Формируем отображаемое имя (дефолт — Id, если данные еще не подгрузились)
                                        const fullName = memberUser 
                                            ? `${memberUser.firstName} ${memberUser.lastName}`
                                            : m.userId;
                                            
                                        // Инициалы для аватара
                                        const initials = memberUser && memberUser.firstName && memberUser.lastName
                                            ? `${memberUser.firstName[0]}${memberUser.lastName[0]}`.toUpperCase()
                                            : m.userId.slice(0, 2).toUpperCase();

                                        return (
                                            <tr key={m.userId}>
                                                <td>
                                                    <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                                                        <div className="avatar sm" style={{ fontWeight: 600 }}>
                                                            {initials}
                                                        </div>
                                                        <div style={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                                                            <span style={{ fontWeight: 500, fontSize: 13 }}>
                                                                {fullName}
                                                            </span>
                                                            {memberUser?.email && (
                                                                <span style={{ fontSize: 11, color: 'var(--text3)' }}>
                                                                    {memberUser.email}
                                                                </span>
                                                            )}
                                                        </div>
                                                    </div>
                                                </td>
                                                <td>{roleBadge(m.role)}</td>
                                                <td>
                                                    <span className={`badge ${m.isActive ? 'badge-green' : 'badge-gray'}`}>
                                                        {m.isActive ? 'Активен' : 'Неактивен'}
                                                    </span>
                                                </td>
                                            </tr>
                                        );
                                    })}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            )}

            {tab === 'periods' && (
                <div className="card">
                    <div className="card-header">
                        <div>
                            <div className="card-title">Периоды ревью</div>
                            <div className="card-sub">{currentProject.reviewPeriods?.length ?? 0} периодов</div>
                        </div>
                        {isAdmin && (
                            <button className="btn btn-primary btn-sm" onClick={() => setShowAddPeriod(true)}>
                                + Добавить
                            </button>
                        )}
                    </div>
                    {!currentProject.reviewPeriods || currentProject.reviewPeriods.length === 0 ? (
                        <div className="empty">
                            <div className="empty-icon">◷</div>
                            <p>Нет периодов ревью</p>
                        </div>
                    ) : (
                        <div className="period-list">
                            {currentProject.reviewPeriods.map((p: ReviewPeriod) => (
                                <div key={p.id} className="period-item">
                                    <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
                                        {periodStatusBadge(p.status)}
                                        <div>
                                            <div style={{ fontSize: 13, fontWeight: 500 }}>
                                                {formatDate(p.startDate)} — {formatDate(p.endDate)}
                                            </div>
                                            <div style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)' }}>
                                                {p.id}
                                            </div>
                                        </div>
                                    </div>
                                    {isAdmin && (
                                        <div style={{ display: 'flex', gap: 6 }}>
                                            {p.status === ReviewPeriodStatus.Draft && (
                                                <button
                                                    className="btn btn-ghost btn-sm"
                                                    onClick={() => activatePeriod.mutate(p.id)}
                                                    disabled={activatePeriod.isPending}
                                                >
                                                    ▶ Активировать
                                                </button>
                                            )}
                                            {p.status === ReviewPeriodStatus.Active && (
                                                <button
                                                    className="btn btn-danger btn-sm"
                                                    onClick={() => closePeriod.mutate(p.id)}
                                                    disabled={closePeriod.isPending}
                                                >
                                                    ✕ Закрыть
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
// ── main page ─────────────────────────────────────────────────────────────────

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
                    <span className="input-icon">⌕</span>
                    <input
                        value={search}
                        onChange={e => setSearch(e.target.value)}
                        placeholder="Поиск проектов…"
                    />
                </div>

                <div className="tabs" style={{ marginBottom: 0 }}>
                    {(['all', 'active', 'closed'] as const).map(f => (
                        <button
                            key={f}
                            className={`tab ${filter === f ? 'active' : ''}`}
                            onClick={() => setFilter(f)}
                        >
                            {{ all: 'Все', active: 'Активные', closed: 'Закрытые' }[f]}
                        </button>
                    ))}
                </div>

                {isAdmin && (
                    <button className="btn btn-primary" onClick={() => setShowCreate(true)}>
                        + Создать
                    </button>
                )}
            </div>

            {/* Stats row */}
            <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(3,1fr)', marginBottom: 20 }}>
                <div className="stat-card">
                    <div className="stat-label">Всего проектов</div>
                    <div className="stat-value" style={{ color: 'var(--info)' }}>{projects?.length ?? 0}</div>
                </div>
                <div className="stat-card">
                    <div className="stat-label">Активных</div>
                    <div className="stat-value" style={{ color: 'var(--success)' }}>
                        {projects?.filter(p => p.status).length ?? 0}
                    </div>
                </div>
                <div className="stat-card">
                    <div className="stat-label">Закрытых</div>
                    <div className="stat-value" style={{ color: 'var(--text3)' }}>
                        {projects?.filter(p => !p.status).length ?? 0}
                    </div>
                </div>
            </div>

            {/* Project list */}
            {isLoading ? (
                <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>
                    Загрузка…
                </div>
            ) : filtered.length === 0 ? (
                <div className="empty">
                    <div className="empty-icon">◫</div>
                    <p>{search ? 'Ничего не найдено' : 'Нет проектов'}</p>
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
                                <div style={{
                                    width: 40, height: 40, borderRadius: 10, flexShrink: 0,
                                    background: p.status
                                        ? 'linear-gradient(135deg,rgba(79,124,255,.2),rgba(124,92,252,.2))'
                                        : 'var(--bg3)',
                                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                                    fontSize: 18, color: p.status ? 'var(--accent)' : 'var(--text3)',
                                }}>
                                    ◫
                                </div>

                                <div style={{ flex: 1, minWidth: 0 }}>
                                    <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 2 }}>
                                        <span style={{ fontWeight: 600, fontSize: 14 }}>{p.title}</span>
                                        {statusBadge(p.status ? 'Active' : 'Closed')}
                                    </div>
                                    <div style={{ fontSize: 12, color: 'var(--text3)' }} className="truncate">
                                        {p.description || 'Без описания'}
                                    </div>
                                </div>

                                <div style={{ display: 'flex', gap: 20, flexShrink: 0, fontSize: 12, color: 'var(--text3)' }}>
                                    <div style={{ textAlign: 'center' }}>
                                        <div style={{ fontWeight: 600, color: 'var(--text)', fontSize: 15 }}>
                                            {p.members?.length ?? 0}
                                        </div>
                                        <div>участников</div>
                                    </div>
                                    <div style={{ textAlign: 'center' }}>
                                        <div style={{ fontWeight: 600, color: 'var(--text)', fontSize: 15 }}>
                                            {p.reviewPeriods?.length ?? 0}
                                        </div>
                                        <div>периодов</div>
                                    </div>
                                    {p.reviewPeriods?.some(rp => rp.status === ReviewPeriodStatus.Active) && (
                                        <div style={{ textAlign: 'center' }}>
                                            <span className="badge badge-green">Период активен</span>
                                        </div>
                                    )}
                                </div>

                                <span style={{ color: 'var(--text3)', fontSize: 16 }}>›</span>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};