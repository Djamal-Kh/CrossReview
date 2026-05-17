import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { reviewAPI, projectAPI, templateAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { Review, ReviewAnswer } from '../types/types';
import { statusBadge } from '../utils/helpers';

// ── helpers ───────────────────────────────────────────────────────────────────

const statusLabel: Record<string, string> = {
  Draft: 'Черновик',
  Submitted: 'Отправлено',
  Closed: 'Закрыто',
};

// ── Review Detail ─────────────────────────────────────────────────────────────

interface DetailProps {
  review: Review;
  isAdmin: boolean;
  onBack: () => void;
}

const ReviewDetail: React.FC<DetailProps> = ({ review, isAdmin, onBack }) => {
  const qc = useQueryClient();
  const [editingAnswer, setEditingAnswer] = useState<string | null>(null);
  const [score, setScore] = useState(0);
  const [comment, setComment] = useState('');

  const { data: template } = useQuery({
    queryKey: ['template', review.templateId ?? ''],
    queryFn: () => templateAPI.getById(review.templateId ?? ''),
    select: r => r.data,
    enabled: !!review.templateId,
  });

  const submitMutation = useMutation({
    mutationFn: () => reviewAPI.submit({ reviewId: review.id, templateId: review.templateId ?? '' }),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['reviews'] }),
  });

  const closeMutation = useMutation({
    mutationFn: () => reviewAPI.close(review.id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['reviews'] }),
  });

  const addAnswerMutation = useMutation({
    mutationFn: (data: { questionId: string; score: number; comment: string }) =>
      reviewAPI.addAnswer({ reviewId: review.id, ...data }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['reviews'] }); setEditingAnswer(null); },
  });

  const updateAnswerMutation = useMutation({
    mutationFn: (data: { questionId: string; score: number; comment: string }) =>
      reviewAPI.updateAnswer({ reviewId: review.id, ...data }),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['reviews'] }); setEditingAnswer(null); },
  });

  const removeAnswerMutation = useMutation({
    mutationFn: (questionId: string) => reviewAPI.removeAnswer(review.id, questionId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['reviews'] }),
  });

  const startEdit = (questionId: string, existing?: ReviewAnswer) => {
    setEditingAnswer(questionId);
    setScore(existing?.score ?? 5);
    setComment(existing?.comment ?? '');
  };

  const saveAnswer = (questionId: string, existing?: ReviewAnswer) => {
    if (existing) {
      updateAnswerMutation.mutate({ questionId, score, comment });
    } else {
      addAnswerMutation.mutate({ questionId, score, comment });
    }
  };

  const questions = template?.questions ?? [];
  const answersMap = Object.fromEntries((review.answers ?? []).map(a => [a.questionId, a]));
  const answeredCount = review.answers?.length ?? 0;
  const totalCount = questions.length;

  return (
    <div className="fade-in">
      {/* Breadcrumb */}
      <div className="breadcrumb">
        <span onClick={onBack}>Ревью</span>
        <span className="breadcrumb-sep">›</span>
        <span style={{ color: 'var(--text2)', fontFamily: 'var(--mono)', fontSize: 11 }}>
          {review.id.slice(0, 8)}…
        </span>
      </div>

      {/* Header */}
      <div className="card" style={{ marginBottom: 16 }}>
        <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: 16 }}>
          <div>
            <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 8 }}>
              <div style={{ fontSize: 16, fontWeight: 700 }}>Ревью</div>
              {statusBadge(review.status)}
            </div>
            <div style={{ display: 'flex', flexDirection: 'column', gap: 4, fontSize: 12, color: 'var(--text2)' }}>
              <div>
                <span style={{ color: 'var(--text3)' }}>Ревьюер: </span>
                <span style={{ fontFamily: 'var(--mono)' }}>{review.reviewerId}</span>
              </div>
              <div>
                <span style={{ color: 'var(--text3)' }}>Оцениваемый: </span>
                <span style={{ fontFamily: 'var(--mono)' }}>{review.revieweeId}</span>
              </div>
              <div>
                <span style={{ color: 'var(--text3)' }}>Проект: </span>
                <span style={{ fontFamily: 'var(--mono)' }}>{review.projectId}</span>
              </div>
            </div>
            {totalCount > 0 && (
              <div style={{ marginTop: 12 }}>
                <div style={{ fontSize: 11, color: 'var(--text3)', marginBottom: 4 }}>
                  Ответов: {answeredCount} / {totalCount}
                </div>
                <div style={{ height: 4, background: 'var(--bg3)', borderRadius: 2, width: 200 }}>
                  <div style={{
                    height: '100%', borderRadius: 2,
                    background: 'linear-gradient(90deg, var(--accent), var(--accent2))',
                    width: `${totalCount > 0 ? (answeredCount / totalCount) * 100 : 0}%`,
                    transition: 'width 0.4s ease',
                  }} />
                </div>
              </div>
            )}
          </div>

          {isAdmin && (
            <div style={{ display: 'flex', gap: 8, flexShrink: 0 }}>
              {review.status === 'Draft' && (
                <button
                  className="btn btn-primary btn-sm"
                  onClick={() => submitMutation.mutate()}
                  disabled={submitMutation.isPending}
                >
                  ↑ Отправить
                </button>
              )}
              {review.status === 'Submitted' && (
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

      {/* Questions */}
      <div className="card">
        <div className="card-header">
          <div>
            <div className="card-title">Вопросы</div>
            <div className="card-sub">
              {template ? template.title : 'Загрузка шаблона…'}
            </div>
          </div>
        </div>

        {questions.length === 0 && (
          <div className="empty">
            <div className="empty-icon">✦</div>
            <p>Нет вопросов в шаблоне</p>
          </div>
        )}

        <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
          {questions.map((q, i) => {
            const existing = answersMap[q.id];
            const isEditing = editingAnswer === q.id;

            return (
              <div key={q.id} style={{
                background: 'var(--bg2)',
                border: `1px solid ${existing ? 'rgba(79,124,255,0.2)' : 'var(--border)'}`,
                borderRadius: 10,
                padding: '14px 16px',
              }}>
                <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: 12 }}>
                  <div style={{ flex: 1 }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 6 }}>
                      <span style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)' }}>
                        #{i + 1}
                      </span>
                      <span style={{ fontSize: 13, fontWeight: 500 }}>{q.title}</span>
                      <span style={{ marginLeft: 'auto', fontSize: 11, color: 'var(--text3)' }}>
                        вес: {q.weight}
                      </span>
                    </div>

                    {/* Existing answer */}
                    {existing && !isEditing && (
                      <div style={{ marginTop: 8 }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 4 }}>
                          <div style={{ display: 'flex', gap: 3 }}>
                            {[1,2,3,4,5,6,7,8,9,10].map(n => (
                              <div key={n} style={{
                                width: 14, height: 14, borderRadius: 3,
                                background: n <= existing.score ? 'var(--accent)' : 'var(--bg3)',
                              }} />
                            ))}
                          </div>
                          <span style={{ fontSize: 13, fontWeight: 600, color: 'var(--accent)' }}>
                            {existing.score}/10
                          </span>
                        </div>
                        {existing.comment && (
                          <div style={{ fontSize: 12, color: 'var(--text2)', marginTop: 4 }}>
                            {existing.comment}
                          </div>
                        )}
                      </div>
                    )}

                    {/* Edit form */}
                    {isEditing && (
                      <div style={{ marginTop: 10 }}>
                        <div style={{ marginBottom: 8 }}>
                          <div style={{ fontSize: 11, color: 'var(--text3)', marginBottom: 6 }}>
                            Оценка: <strong style={{ color: 'var(--accent)' }}>{score}</strong>/10
                          </div>
                          <input
                            type="range" min={1} max={10} value={score}
                            onChange={e => setScore(Number(e.target.value))}
                            style={{ width: '100%', accentColor: 'var(--accent)' }}
                          />
                        </div>
                        <div className="form-group" style={{ marginBottom: 8 }}>
                          <textarea
                            value={comment}
                            onChange={e => setComment(e.target.value)}
                            placeholder="Комментарий (необязательно)"
                            rows={2}
                            style={{ resize: 'vertical' }}
                          />
                        </div>
                        <div style={{ display: 'flex', gap: 6 }}>
                          <button
                            className="btn btn-primary btn-sm"
                            onClick={() => saveAnswer(q.id, existing)}
                            disabled={addAnswerMutation.isPending || updateAnswerMutation.isPending}
                          >
                            Сохранить
                          </button>
                          <button
                            className="btn btn-ghost btn-sm"
                            onClick={() => setEditingAnswer(null)}
                          >
                            Отмена
                          </button>
                        </div>
                      </div>
                    )}
                  </div>

                  {/* Action buttons */}
                  {!isEditing && isAdmin && review.status === 'Draft' && (
                    <div style={{ display: 'flex', gap: 6, flexShrink: 0 }}>
                      <button
                        className="btn btn-ghost btn-sm"
                        onClick={() => startEdit(q.id, existing)}
                      >
                        {existing ? 'Изменить' : '+ Ответить'}
                      </button>
                      {existing && (
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => removeAnswerMutation.mutate(q.id)}
                          disabled={removeAnswerMutation.isPending}
                        >
                          ✕
                        </button>
                      )}
                    </div>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

// ── Main Page ─────────────────────────────────────────────────────────────────

export const ReviewsPage: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';
  const [selected, setSelected] = useState<Review | null>(null);
  const [filter, setFilter] = useState<'all' | 'Draft' | 'Submitted' | 'Closed'>('all');

  const { data: reviews, isLoading } = useQuery({
    queryKey: ['reviews', 'all'],
    queryFn: () => reviewAPI.getByReviewers({ userId: user?.id ?? '' }),
    select: r => r.data,
    enabled: !!user?.id,
  });

  const { data: allReviews } = useQuery({
    queryKey: ['reviews', 'by-parameters'],
    queryFn: () => reviewAPI.getByParameters({}),
    select: r => r.data,
    enabled: isAdmin,
  });

  const displayReviews = isAdmin ? (allReviews ?? reviews ?? []) : (reviews ?? []);

  const filtered = displayReviews.filter(r => {
    if (filter !== 'all' && r.status !== filter) return false;
    return true;
  });

  // Refresh selected review from cache
  const currentReview = selected
    ? displayReviews.find(r => r.id === selected.id) ?? selected
    : null;

  if (currentReview) {
    return (
      <ReviewDetail
        review={currentReview}
        isAdmin={isAdmin}
        onBack={() => setSelected(null)}
      />
    );
  }

  const countByStatus = (s: string) => displayReviews.filter(r => r.status === s).length;

  return (
    <div className="fade-in">
      {/* Stats */}
      <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(4,1fr)', marginBottom: 20 }}>
        <div className="stat-card">
          <div className="stat-label">Всего</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{displayReviews.length}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Черновики</div>
          <div className="stat-value" style={{ color: 'var(--text3)' }}>{countByStatus('Draft')}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Отправлено</div>
          <div className="stat-value" style={{ color: 'var(--warning)' }}>{countByStatus('Submitted')}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Закрыто</div>
          <div className="stat-value" style={{ color: 'var(--success)' }}>{countByStatus('Closed')}</div>
        </div>
      </div>

      {/* Filter tabs */}
      <div className="tabs" style={{ marginBottom: 16 }}>
        {(['all', 'Draft', 'Submitted', 'Closed'] as const).map(f => (
          <button
            key={f}
            className={`tab ${filter === f ? 'active' : ''}`}
            onClick={() => setFilter(f)}
          >
            {{ all: 'Все', Draft: 'Черновики', Submitted: 'Отправленные', Closed: 'Закрытые' }[f]}
          </button>
        ))}
      </div>

      {/* List */}
      {isLoading ? (
        <div style={{ color: 'var(--text3)', padding: '40px 0', textAlign: 'center' }}>Загрузка…</div>
      ) : filtered.length === 0 ? (
        <div className="empty">
          <div className="empty-icon">✦</div>
          <p>Нет ревью</p>
        </div>
      ) : (
        <div className="card">
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Ревьюер</th>
                  <th>Оцениваемый</th>
                  <th>Статус</th>
                  <th>Ответов</th>
                </tr>
              </thead>
              <tbody>
                {filtered.map(r => (
                  <tr
                    key={r.id}
                    style={{ cursor: 'pointer' }}
                    onClick={() => setSelected(r)}
                  >
                    <td>
                      <span style={{ fontFamily: 'var(--mono)', fontSize: 11, color: 'var(--text3)' }}>
                        {r.id.slice(0, 8)}…
                      </span>
                    </td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <div className="avatar sm">{r.reviewerId.slice(0, 2).toUpperCase()}</div>
                        <span style={{ fontFamily: 'var(--mono)', fontSize: 11, color: 'var(--text2)' }}>
                          {r.reviewerId.slice(0, 8)}…
                        </span>
                      </div>
                    </td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <div className="avatar sm">{r.revieweeId.slice(0, 2).toUpperCase()}</div>
                        <span style={{ fontFamily: 'var(--mono)', fontSize: 11, color: 'var(--text2)' }}>
                          {r.revieweeId.slice(0, 8)}…
                        </span>
                      </div>
                    </td>
                    <td>{statusBadge(r.status)}</td>
                    <td>
                      <span style={{ fontSize: 13, color: 'var(--text2)' }}>
                        {r.answers?.length ?? 0}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};
