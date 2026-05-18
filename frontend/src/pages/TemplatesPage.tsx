import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { templateAPI, projectAPI } from '../api/client';
import { Template, TemplateQuestion, Project } from '../types/types';

// ── helpers ───────────────────────────────────────────────────────────────────

const totalWeight = (questions: TemplateQuestion[]) =>
  questions.reduce((s, q) => s + q.weight, 0);

// ── Create Template Modal ─────────────────────────────────────────────────────

const CreateTemplateModal: React.FC<{ projects: Project[]; onClose: () => void }> = ({ projects, onClose }) => {
  const qc = useQueryClient();
  const [title, setTitle] = useState('');
  const [projectId, setProjectId] = useState('');
  const [error, setError] = useState('');

  const mutation = useMutation({
    mutationFn: () => templateAPI.create({ projectId, title }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['templates'] }); onClose(); },
    onError: () => setError('Не удалось создать шаблон'),
  });

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title">Создать шаблон</div>
          <button className="btn-icon" onClick={onClose}>✕</button>
        </div>
        <div className="form-group">
          <label>Проект</label>
          <select value={projectId} onChange={e => setProjectId(e.target.value)}>
            <option value="">Выберите проект</option>
            {projects.map(p => (
              <option key={p.id} value={p.id}>{p.title}</option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label>Название шаблона</label>
          <input
            value={title}
            onChange={e => setTitle(e.target.value)}
            placeholder="Например: Q2 2026 Review"
            autoFocus
          />
        </div>
        {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}
        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
          <button
            className="btn btn-primary"
            onClick={() => mutation.mutate()}
            disabled={!title.trim() || !projectId || mutation.isPending}
          >
            {mutation.isPending ? 'Создание…' : 'Создать'}
          </button>
        </div>
      </div>
    </div>
  );
};

// ── Add / Edit Question Modal ─────────────────────────────────────────────────

interface QuestionModalProps {
  templateId: string;
  existing?: TemplateQuestion;
  onClose: () => void;
}

const QuestionModal: React.FC<QuestionModalProps> = ({ templateId, existing, onClose }) => {
  const qc = useQueryClient();
  const [title, setTitle] = useState(existing?.title ?? '');
  const [weight, setWeight] = useState(existing?.weight ?? 1);
  const [error, setError] = useState('');

  const addMutation = useMutation({
    mutationFn: () => templateAPI.addQuestion({ templateId, title, weight }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['templates'] }); onClose(); },
    onError: () => setError('Не удалось добавить вопрос'),
  });

  const updateMutation = useMutation({
    mutationFn: () => templateAPI.updateQuestion({ templateId, questionId: existing!.id, title, weight }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['templates'] }); onClose(); },
    onError: () => setError('Не удалось обновить вопрос'),
  });

  const isEditing = !!existing;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title">{isEditing ? 'Редактировать вопрос' : 'Добавить вопрос'}</div>
          <button className="btn-icon" onClick={onClose}>✕</button>
        </div>
        <div className="form-group">
          <label>Текст вопроса</label>
          <textarea
            value={title}
            onChange={e => setTitle(e.target.value)}
            placeholder="Например: Как оцениваете качество кода сотрудника?"
            rows={3}
            style={{ resize: 'vertical' }}
            autoFocus
          />
        </div>
        <div className="form-group">
          <label>
            Вес вопроса: <strong style={{ color: 'var(--accent)' }}>{weight}</strong>
          </label>
          <input
            type="range" min={0.1} max={5} step={0.1} value={weight}
            onChange={e => setWeight(Number(e.target.value))}
            style={{ accentColor: 'var(--accent)' }}
          />
          <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: 11, color: 'var(--text3)', marginTop: 2 }}>
            <span>0.1</span><span>5.0</span>
          </div>
        </div>
        {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}
        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
          <button
            className="btn btn-primary"
            onClick={() => isEditing ? updateMutation.mutate() : addMutation.mutate()}
            disabled={!title.trim() || addMutation.isPending || updateMutation.isPending}
          >
            {addMutation.isPending || updateMutation.isPending ? 'Сохранение…' : 'Сохранить'}
          </button>
        </div>
      </div>
    </div>
  );
};

// ── Template Detail ───────────────────────────────────────────────────────────

interface DetailProps {
  template: Template;
  projects: Project[];
  onBack: () => void;
}

const TemplateDetail: React.FC<DetailProps> = ({ template, projects, onBack }) => {
  const qc = useQueryClient();
  const [showAddQuestion, setShowAddQuestion] = useState(false);
  const [editingQuestion, setEditingQuestion] = useState<TemplateQuestion | null>(null);

  const project = projects.find(p => p.id === template.projectId);

  const activateMutation = useMutation({
    mutationFn: () => templateAPI.activate(template.id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['templates'] }),
  });

  const deactivateMutation = useMutation({
    mutationFn: () => templateAPI.deactivate(template.id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['templates'] }),
  });

  const deleteMutation = useMutation({
    mutationFn: () => templateAPI.delete(template.id),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['templates'] }); onBack(); },
  });

  const removeQuestionMutation = useMutation({
    mutationFn: (questionId: string) => templateAPI.removeQuestion(template.id, questionId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['templates'] }),
  });

  const questions = template.questions ?? [];
  const tw = totalWeight(questions);

  return (
    <div className="fade-in">
      {showAddQuestion && (
        <QuestionModal templateId={template.id} onClose={() => setShowAddQuestion(false)} />
      )}
      {editingQuestion && (
        <QuestionModal
          templateId={template.id}
          existing={editingQuestion}
          onClose={() => setEditingQuestion(null)}
        />
      )}

      {/* Breadcrumb */}
      <div className="breadcrumb">
        <span onClick={onBack}>Шаблоны</span>
        <span className="breadcrumb-sep">›</span>
        <span style={{ color: 'var(--text2)' }}>{template.title}</span>
      </div>

      {/* Header card */}
      <div className="card" style={{ marginBottom: 16 }}>
        <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: 16 }}>
          <div>
            <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 6 }}>
              <div style={{ fontSize: 17, fontWeight: 700 }}>{template.title}</div>
              <span className={`badge ${template.isActive ? 'badge-green' : 'badge-gray'}`}>
                {template.isActive ? 'Активен' : 'Неактивен'}
              </span>
            </div>
            <div style={{ fontSize: 12, color: 'var(--text3)' }}>
              Проект: <span style={{ color: 'var(--text2)' }}>{project?.title ?? template.projectId}</span>
            </div>
            <div style={{ display: 'flex', gap: 16, marginTop: 10, fontSize: 12, color: 'var(--text3)' }}>
              <span>▤ {questions.length} вопросов</span>
              <span>⊞ Суммарный вес: <strong style={{ color: 'var(--text)' }}>{tw.toFixed(1)}</strong></span>
            </div>
          </div>

          <div style={{ display: 'flex', gap: 8, flexShrink: 0 }}>
            {template.isActive ? (
              <button
                className="btn btn-ghost btn-sm"
                onClick={() => deactivateMutation.mutate()}
                disabled={deactivateMutation.isPending}
              >
                Деактивировать
              </button>
            ) : (
              <button
                className="btn btn-primary btn-sm"
                onClick={() => activateMutation.mutate()}
                disabled={activateMutation.isPending}
              >
                ✓ Активировать
              </button>
            )}
            <button
              className="btn btn-danger btn-sm"
              onClick={() => { if (confirm('Удалить шаблон?')) deleteMutation.mutate(); }}
              disabled={deleteMutation.isPending}
            >
              ✕ Удалить
            </button>
          </div>
        </div>
      </div>

      {/* Questions */}
      <div className="card">
        <div className="card-header">
          <div>
            <div className="card-title">Вопросы</div>
            <div className="card-sub">{questions.length} вопросов · суммарный вес {tw.toFixed(1)}</div>
          </div>
          <button className="btn btn-primary btn-sm" onClick={() => setShowAddQuestion(true)}>
            + Добавить
          </button>
        </div>

        {questions.length === 0 ? (
          <div className="empty">
            <div className="empty-icon">▤</div>
            <p>Нет вопросов</p>
            <button className="btn btn-primary btn-sm" style={{ marginTop: 12 }} onClick={() => setShowAddQuestion(true)}>
              Добавить первый вопрос
            </button>
          </div>
        ) : (
          <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
            {questions.map((q, i) => {
              const weightPct = tw > 0 ? (q.weight / tw) * 100 : 0;
              return (
                <div key={q.id} style={{
                  background: 'var(--bg2)',
                  border: '1px solid var(--border)',
                  borderRadius: 10,
                  padding: '12px 14px',
                }}>
                  <div style={{ display: 'flex', alignItems: 'flex-start', gap: 12 }}>
                    {/* Number */}
                    <div style={{
                      width: 28, height: 28, borderRadius: 6, flexShrink: 0,
                      background: 'var(--bg3)',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      fontSize: 11, fontWeight: 600, color: 'var(--text3)',
                    }}>
                      {i + 1}
                    </div>

                    {/* Content */}
                    <div style={{ flex: 1, minWidth: 0 }}>
                      <div style={{ fontSize: 13, fontWeight: 500, marginBottom: 8 }}>{q.title}</div>
                      {/* Weight bar */}
                      <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <div style={{ flex: 1, height: 4, background: 'var(--bg3)', borderRadius: 2, overflow: 'hidden' }}>
                          <div style={{
                            height: '100%', borderRadius: 2,
                            background: 'linear-gradient(90deg, var(--accent), var(--accent2))',
                            width: `${weightPct}%`,
                          }} />
                        </div>
                        <span style={{ fontSize: 11, color: 'var(--text3)', flexShrink: 0 }}>
                          вес {q.weight} · {weightPct.toFixed(0)}%
                        </span>
                      </div>
                    </div>

                    {/* Actions */}
                    <div style={{ display: 'flex', gap: 6, flexShrink: 0 }}>
                      <button
                        className="btn btn-ghost btn-sm"
                        onClick={() => setEditingQuestion(q)}
                      >
                        Изменить
                      </button>
                      <button
                        className="btn btn-danger btn-sm"
                        onClick={() => removeQuestionMutation.mutate(q.id)}
                        disabled={removeQuestionMutation.isPending}
                      >
                        ✕
                      </button>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

// ── Main Page ─────────────────────────────────────────────────────────────────

export const TemplatesPage: React.FC = () => {
  const qc = useQueryClient();
  const [selected, setSelected] = useState<Template | null>(null);
  const [showCreate, setShowCreate] = useState(false);
  const [filter, setFilter] = useState<'all' | 'active' | 'inactive'>('all');

  const { data: templates, isLoading } = useQuery({
    queryKey: ['templates'],
    queryFn: () => (templateAPI as any).getAll(),
    select: (r: any) => r.data as Template[],
  });

  const { data: projects } = useQuery({
    queryKey: ['projects'],
    queryFn: () => projectAPI.getAll(),
    select: r => r.data,
  });

  const currentTemplate = selected
    ? templates?.find(t => t.id === selected.id) ?? selected
    : null;

  const filtered = (templates ?? []).filter(t => {
    if (filter === 'active' && !t.isActive) return false;
    if (filter === 'inactive' && t.isActive) return false;
    return true;
  });

  if (currentTemplate) {
    return (
      <TemplateDetail
        template={currentTemplate}
        projects={projects ?? []}
        onBack={() => setSelected(null)}
      />
    );
  }

  const activeCount = templates?.filter(t => t.isActive).length ?? 0;
  const totalQuestions = templates?.reduce((s, t) => s + (t.questions?.length ?? 0), 0) ?? 0;

  return (
    <div className="fade-in">
      {showCreate && projects && (
        <CreateTemplateModal projects={projects} onClose={() => setShowCreate(false)} />
      )}

      {/* Stats */}
      <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(3,1fr)', marginBottom: 20 }}>
        <div className="stat-card">
          <div className="stat-label">Всего шаблонов</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{templates?.length ?? 0}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Активных</div>
          <div className="stat-value" style={{ color: 'var(--success)' }}>{activeCount}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Всего вопросов</div>
          <div className="stat-value" style={{ color: 'var(--accent)' }}>{totalQuestions}</div>
        </div>
      </div>

      {/* Toolbar */}
      <div style={{ display: 'flex', alignItems: 'center', gap: 12, marginBottom: 16 }}>
        <div className="tabs" style={{ marginBottom: 0 }}>
          {(['all', 'active', 'inactive'] as const).map(f => (
            <button
              key={f}
              className={`tab ${filter === f ? 'active' : ''}`}
              onClick={() => setFilter(f)}
            >
              {{ all: 'Все', active: 'Активные', inactive: 'Неактивные' }[f]}
            </button>
          ))}
        </div>
        <button className="btn btn-primary" style={{ marginLeft: 'auto' }} onClick={() => setShowCreate(true)}>
          + Создать
        </button>
      </div>

      {/* List */}
      {isLoading ? (
        <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>Загрузка…</div>
      ) : filtered.length === 0 ? (
        <div className="empty">
          <div className="empty-icon">▤</div>
          <p>Нет шаблонов</p>
          <button className="btn btn-primary btn-sm" style={{ marginTop: 12 }} onClick={() => setShowCreate(true)}>
            Создать первый шаблон
          </button>
        </div>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {filtered.map(t => {
            const project = projects?.find(p => p.id === t.projectId);
            const qCount = t.questions?.length ?? 0;
            const tw = totalWeight(t.questions ?? []);

            return (
              <div
                key={t.id}
                className="card"
                style={{ cursor: 'pointer', transition: 'border-color 0.15s' }}
                onClick={() => setSelected(t)}
                onMouseEnter={e => (e.currentTarget.style.borderColor = 'var(--border2)')}
                onMouseLeave={e => (e.currentTarget.style.borderColor = 'var(--border)')}
              >
                <div style={{ display: 'flex', alignItems: 'center', gap: 14 }}>
                  {/* Icon */}
                  <div style={{
                    width: 40, height: 40, borderRadius: 10, flexShrink: 0,
                    background: t.isActive
                      ? 'linear-gradient(135deg,rgba(79,124,255,.2),rgba(124,92,252,.2))'
                      : 'var(--bg3)',
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                    fontSize: 18, color: t.isActive ? 'var(--accent)' : 'var(--text3)',
                  }}>
                    ▤
                  </div>

                  {/* Info */}
                  <div style={{ flex: 1, minWidth: 0 }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 2 }}>
                      <span style={{ fontWeight: 600, fontSize: 14 }}>{t.title}</span>
                      <span className={`badge ${t.isActive ? 'badge-green' : 'badge-gray'}`}>
                        {t.isActive ? 'Активен' : 'Неактивен'}
                      </span>
                    </div>
                    <div style={{ fontSize: 12, color: 'var(--text3)' }}>
                      {project?.title ?? 'Проект не найден'}
                    </div>
                  </div>

                  {/* Meta */}
                  <div style={{ display: 'flex', gap: 20, flexShrink: 0, fontSize: 12, color: 'var(--text3)' }}>
                    <div style={{ textAlign: 'center' }}>
                      <div style={{ fontWeight: 600, color: 'var(--text)', fontSize: 15 }}>{qCount}</div>
                      <div>вопросов</div>
                    </div>
                    <div style={{ textAlign: 'center' }}>
                      <div style={{ fontWeight: 600, color: 'var(--text)', fontSize: 15 }}>{tw.toFixed(1)}</div>
                      <div>суммарный вес</div>
                    </div>
                  </div>

                  <span style={{ color: 'var(--text3)', fontSize: 16 }}>›</span>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};