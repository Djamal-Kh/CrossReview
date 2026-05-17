import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { resultAPI, projectAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { EvaluationResult, Project } from '../types/types';
import { ScoreBar } from '../utils/helpers';

// ── helpers ───────────────────────────────────────────────────────────────────

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('ru', { day: '2-digit', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' });

const scoreColor = (score: number) => {
  if (score >= 8) return 'var(--success)';
  if (score >= 6) return 'var(--warning)';
  return 'var(--danger)';
};

// ── Calculate Modal ───────────────────────────────────────────────────────────

interface CalcModalProps {
  projects: Project[];
  onClose: () => void;
}

const CalculateModal: React.FC<CalcModalProps> = ({ projects, onClose }) => {
  const qc = useQueryClient();
  const [userId, setUserId] = useState('');
  const [projectId, setProjectId] = useState('');
  const [periodId, setPeriodId] = useState('');
  const [error, setError] = useState('');

  const selectedProject = projects.find(p => p.id === projectId);
  const periods = selectedProject?.reviewPeriods ?? [];

  const mutation = useMutation({
    mutationFn: () => resultAPI.calculate({ userId, projectId, periodId }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['results'] }); onClose(); },
    onError: () => setError('Не удалось рассчитать результат'),
  });

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-title">Рассчитать результат</div>
          <button className="btn-icon" onClick={onClose}>✕</button>
        </div>

        <div className="form-group">
          <label>ID пользователя</label>
          <input
            value={userId}
            onChange={e => setUserId(e.target.value)}
            placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
            style={{ fontFamily: 'var(--mono)', fontSize: 12 }}
          />
        </div>

        <div className="form-group">
          <label>Проект</label>
          <select value={projectId} onChange={e => { setProjectId(e.target.value); setPeriodId(''); }}>
            <option value="">Выберите проект</option>
            {projects.map(p => (
              <option key={p.id} value={p.id}>{p.title}</option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label>Период ревью</label>
          <select value={periodId} onChange={e => setPeriodId(e.target.value)} disabled={!projectId}>
            <option value="">Выберите период</option>
            {periods.map(p => (
              <option key={p.id} value={p.id}>
                {new Date(p.startDate).toLocaleDateString('ru')} — {new Date(p.endDate).toLocaleDateString('ru')} ({p.status})
              </option>
            ))}
          </select>
        </div>

        {error && <div style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</div>}

        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="btn btn-ghost" onClick={onClose}>Отмена</button>
          <button
            className="btn btn-primary"
            onClick={() => mutation.mutate()}
            disabled={!userId || !projectId || !periodId || mutation.isPending}
          >
            {mutation.isPending ? 'Расчёт…' : 'Рассчитать'}
          </button>
        </div>
      </div>
    </div>
  );
};

// ── Result Detail ─────────────────────────────────────────────────────────────

interface DetailProps {
  result: EvaluationResult;
  onBack: () => void;
  isAdmin: boolean;
}

const ResultDetail: React.FC<DetailProps> = ({ result, onBack, isAdmin }) => {
  const qc = useQueryClient();

  const { data: project } = useQuery({
    queryKey: ['project', result.projectId],
    queryFn: () => projectAPI.getById(result.projectId),
    select: r => r.data,
  });

  const recalcMutation = useMutation({
    mutationFn: () => resultAPI.recalculate(result.id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['results'] }),
  });

  const period = project?.reviewPeriods?.find(p => p.id === result.periodId);

  return (
    <div className="fade-in">
      <div className="breadcrumb">
        <span onClick={onBack}>Результаты</span>
        <span className="breadcrumb-sep">›</span>
        <span style={{ color: 'var(--text2)' }}>
          {project?.title ?? result.projectId.slice(0, 8)}
        </span>
      </div>

      {/* Score card */}
      <div className="card" style={{ marginBottom: 16 }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: 24 }}>
          {/* Big score */}
          <div style={{
            width: 100, height: 100, borderRadius: 20, flexShrink: 0,
            background: 'var(--bg2)',
            border: `2px solid ${scoreColor(result.finalScore)}`,
            display: 'flex', flexDirection: 'column',
            alignItems: 'center', justifyContent: 'center',
          }}>
            <div style={{ fontSize: 28, fontWeight: 700, color: scoreColor(result.finalScore), lineHeight: 1 }}>
              {result.finalScore.toFixed(1)}
            </div>
            <div style={{ fontSize: 11, color: 'var(--text3)', marginTop: 2 }}>из 10</div>
          </div>

          <div style={{ flex: 1 }}>
            <div style={{ fontSize: 16, fontWeight: 700, marginBottom: 8 }}>
              {project?.title ?? 'Проект'}
            </div>
            <div style={{ display: 'flex', flexDirection: 'column', gap: 4, fontSize: 12 }}>
              <div>
                <span style={{ color: 'var(--text3)' }}>Пользователь: </span>
                <span style={{ fontFamily: 'var(--mono)' }}>{result.userId}</span>
              </div>
              <div>
                <span style={{ color: 'var(--text3)' }}>Период: </span>
                <span>
                  {period
                    ? `${new Date(period.startDate).toLocaleDateString('ru')} — ${new Date(period.endDate).toLocaleDateString('ru')}`
                    : result.periodId.slice(0, 8)}
                </span>
              </div>
              <div>
                <span style={{ color: 'var(--text3)' }}>Рассчитано: </span>
                <span>{formatDate(result.calculatedAt)}</span>
              </div>
            </div>
          </div>

          {isAdmin && (
            <button
              className="btn btn-ghost btn-sm"
              onClick={() => recalcMutation.mutate()}
              disabled={recalcMutation.isPending}
              style={{ flexShrink: 0 }}
            >
              ↻ Пересчитать
            </button>
          )}
        </div>

        {/* Score bar full */}
        <div style={{ marginTop: 20 }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: 11, color: 'var(--text3)', marginBottom: 6 }}>
            <span>0</span>
            <span>5</span>
            <span>10</span>
          </div>
          <div style={{ height: 10, background: 'var(--bg3)', borderRadius: 5, overflow: 'hidden' }}>
            <div style={{
              height: '100%', borderRadius: 5,
              background: `linear-gradient(90deg, ${scoreColor(result.finalScore)}, ${scoreColor(result.finalScore)}88)`,
              width: `${(result.finalScore / 10) * 100}%`,
              transition: 'width 0.6s ease',
            }} />
          </div>
          <div style={{ marginTop: 8, fontSize: 12, color: 'var(--text2)' }}>
            {result.finalScore >= 8
              ? '🟢 Отличный результат'
              : result.finalScore >= 6
              ? '🟡 Хороший результат'
              : '🔴 Требует внимания'}
          </div>
        </div>
      </div>

      {/* Meta */}
      <div className="card">
        <div className="card-title" style={{ marginBottom: 16 }}>Детали</div>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {[
            { label: 'ID результата', value: result.id },
            { label: 'ID пользователя', value: result.userId },
            { label: 'ID проекта', value: result.projectId },
            { label: 'ID периода', value: result.periodId },
          ].map(({ label, value }) => (
            <div key={label} style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', fontSize: 12 }}>
              <span style={{ color: 'var(--text3)' }}>{label}</span>
              <span style={{ fontFamily: 'var(--mono)', color: 'var(--text2)', fontSize: 11 }}>{value}</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

// ── Main Page ─────────────────────────────────────────────────────────────────

export const ResultsPage: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';
  const qc = useQueryClient();
  const [selected, setSelected] = useState<EvaluationResult | null>(null);
  const [showCalc, setShowCalc] = useState(false);
  const [sortBy, setSortBy] = useState<'score' | 'date'>('date');

  const { data: results, isLoading } = useQuery({
    queryKey: ['results'],
    queryFn: () => resultAPI.getResults(isAdmin ? {} : { userId: user?.id }),
    select: r => r.data,
  });

  const { data: projects } = useQuery({
    queryKey: ['projects'],
    queryFn: () => projectAPI.getAll(),
    select: r => r.data,
  });

  const sorted = [...(results ?? [])].sort((a, b) =>
    sortBy === 'score'
      ? b.finalScore - a.finalScore
      : new Date(b.calculatedAt).getTime() - new Date(a.calculatedAt).getTime()
  );

  const currentResult = selected
    ? sorted.find(r => r.id === selected.id) ?? selected
    : null;

  if (currentResult) {
    return (
      <ResultDetail
        result={currentResult}
        onBack={() => setSelected(null)}
        isAdmin={isAdmin}
      />
    );
  }

  const avgScore = results && results.length > 0
    ? results.reduce((a, r) => a + r.finalScore, 0) / results.length
    : 0;

  const best = results && results.length > 0
    ? Math.max(...results.map(r => r.finalScore))
    : 0;

  return (
    <div className="fade-in">
      {showCalc && projects && (
        <CalculateModal projects={projects} onClose={() => setShowCalc(false)} />
      )}

      {/* Stats */}
      <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(4,1fr)', marginBottom: 20 }}>
        <div className="stat-card">
          <div className="stat-label">Результатов</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{results?.length ?? 0}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Средний балл</div>
          <div className="stat-value" style={{ color: scoreColor(avgScore) }}>{avgScore.toFixed(1)}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Лучший балл</div>
          <div className="stat-value" style={{ color: 'var(--success)' }}>{best.toFixed(1)}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Выше 8.0</div>
          <div className="stat-value" style={{ color: 'var(--accent)' }}>
            {results?.filter(r => r.finalScore >= 8).length ?? 0}
          </div>
        </div>
      </div>

      {/* Toolbar */}
      <div style={{ display: 'flex', alignItems: 'center', gap: 12, marginBottom: 16 }}>
        <div className="tabs" style={{ marginBottom: 0 }}>
          <button className={`tab ${sortBy === 'date' ? 'active' : ''}`} onClick={() => setSortBy('date')}>
            По дате
          </button>
          <button className={`tab ${sortBy === 'score' ? 'active' : ''}`} onClick={() => setSortBy('score')}>
            По баллу
          </button>
        </div>
        {isAdmin && (
          <button className="btn btn-primary" style={{ marginLeft: 'auto' }} onClick={() => setShowCalc(true)}>
            + Рассчитать
          </button>
        )}
      </div>

      {/* List */}
      {isLoading ? (
        <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>Загрузка…</div>
      ) : sorted.length === 0 ? (
        <div className="empty">
          <div className="empty-icon">◈</div>
          <p>Нет результатов оценки</p>
          {isAdmin && (
            <button className="btn btn-primary btn-sm" style={{ marginTop: 12 }} onClick={() => setShowCalc(true)}>
              Рассчитать первый результат
            </button>
          )}
        </div>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {sorted.map((r, i) => {
            const project = projects?.find(p => p.id === r.projectId);
            return (
              <div
                key={r.id}
                className="card"
                style={{ cursor: 'pointer', transition: 'border-color 0.15s' }}
                onClick={() => setSelected(r)}
                onMouseEnter={e => (e.currentTarget.style.borderColor = 'var(--border2)')}
                onMouseLeave={e => (e.currentTarget.style.borderColor = 'var(--border)')}
              >
                <div style={{ display: 'flex', alignItems: 'center', gap: 16 }}>
                  {/* Rank */}
                  {sortBy === 'score' && (
                    <div style={{
                      width: 32, height: 32, borderRadius: 8, flexShrink: 0,
                      background: i === 0 ? 'rgba(245,158,11,0.15)' : i === 1 ? 'rgba(139,147,168,0.1)' : 'var(--bg2)',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      fontSize: 13, fontWeight: 700,
                      color: i === 0 ? 'var(--warning)' : i === 1 ? 'var(--text2)' : 'var(--text3)',
                    }}>
                      #{i + 1}
                    </div>
                  )}

                  {/* Avatar */}
                  <div className="avatar sm">{r.userId.slice(0, 2).toUpperCase()}</div>

                  {/* Info */}
                  <div style={{ flex: 1, minWidth: 0 }}>
                    <div style={{ fontSize: 13, fontWeight: 500, marginBottom: 2 }}>
                      {project?.title ?? 'Проект'}
                    </div>
                    <div style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)' }}>
                      {r.userId.slice(0, 16)}… · {formatDate(r.calculatedAt)}
                    </div>
                  </div>

                  {/* Score */}
                  <div style={{ width: 200, flexShrink: 0 }}>
                    <ScoreBar score={r.finalScore} />
                  </div>

                  <div style={{
                    fontSize: 18, fontWeight: 700, flexShrink: 0, minWidth: 40, textAlign: 'right',
                    color: scoreColor(r.finalScore),
                  }}>
                    {r.finalScore.toFixed(1)}
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
