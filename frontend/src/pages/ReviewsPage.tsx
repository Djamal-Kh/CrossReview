import React, { useState, useMemo } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { reviewAPI, projectAPI, templateAPI, authAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { Review, ReviewAnswer, User } from '../types/types';
import { statusBadge } from '../utils/helpers';

// ── helpers ───────────────────────────────────────────────────────────────────

const getUserName = (users: User[], userId: string | undefined) => {
  if (!userId) return 'Не указан';
  const u = users.find(u => u.id === userId);
  return u ? `${u.firstName} ${u.lastName}` : `${userId.slice(0, 8)}…`;
};

const getUserInitials = (users: User[], userId: string | undefined) => {
  if (!userId) return '??';
  const u = users.find(u => u.id === userId);
  return u
    ? `${u.firstName[0] ?? ''}${u.lastName[0] ?? ''}`.toUpperCase()
    : userId.slice(0, 2).toUpperCase();
};

// ── Review Detail ─────────────────────────────────────────────────────────────

interface DetailProps {
  review: Review;
  isAdmin: boolean;
  users: User[];
  onBack: () => void;
}

const ReviewDetail: React.FC<DetailProps> = ({ review, isAdmin, users, onBack }) => {
  const qc = useQueryClient();
  const { user: currentUser } = useAuth();
  const [editingAnswer, setEditingAnswer] = useState<string | null>(null);
  const [score, setScore] = useState(0);
  const [comment, setComment] = useState('');

  const templateId = review.templateId;

  const { data: template, isLoading: templateLoading } = useQuery({
    queryKey: ['template', templateId ?? ''],
    queryFn: () => templateAPI.getById(templateId!),
    select: r => r.data,
    enabled: !!templateId,
  });

  // Fetch fresh review data to get latest answers
  const { data: freshReview } = useQuery({
    queryKey: ['review', review.id],
    queryFn: () => reviewAPI.getById(review.id),
    select: r => r.data,
  });

  const displayReview = freshReview ?? review;

  const submitMutation = useMutation({
    mutationFn: () => reviewAPI.submit({ reviewId: review.id, templateId: templateId ?? '' }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['reviews'] });
      qc.invalidateQueries({ queryKey: ['review', review.id] });
    },
  });

  const closeMutation = useMutation({
    mutationFn: () => reviewAPI.close(review.id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['reviews'] });
      qc.invalidateQueries({ queryKey: ['review', review.id] });
    },
  });

  const addAnswerMutation = useMutation({
    mutationFn: (data: { questionId: string; score: number; comment: string }) =>
      reviewAPI.addAnswer({ reviewId: review.id, ...data }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['reviews'] });
      qc.invalidateQueries({ queryKey: ['review', review.id] });
      setEditingAnswer(null);
    },
  });

  const updateAnswerMutation = useMutation({
    mutationFn: (data: { questionId: string; score: number; comment: string }) =>
      reviewAPI.updateAnswer({ reviewId: review.id, ...data }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['reviews'] });
      qc.invalidateQueries({ queryKey: ['review', review.id] });
      setEditingAnswer(null);
    },
  });

  const removeAnswerMutation = useMutation({
    mutationFn: (questionId: string) => reviewAPI.removeAnswer(review.id, questionId),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['reviews'] });
      qc.invalidateQueries({ queryKey: ['review', review.id] });
    },
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
  const answersMap = Object.fromEntries((displayReview.answers ?? []).map(a => [a.questionId, a]));
  const answeredCount = displayReview.answers?.length ?? 0;
  const totalCount = questions.length;

  const revieweeName = getUserName(users, review.revieweeId);
  const revieweeInitials = getUserInitials(users, review.revieweeId);
  const reviewerName = getUserName(users, review.reviewerId);
  const isMyReview = review.reviewerId === currentUser?.id;

  // Can edit: admin always, or if it's my review and status is Draft
  const canEdit = isAdmin || (isMyReview && displayReview.status === 'Draft');

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
          <div style={{ flex: 1 }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 12 }}>
              <div style={{ fontSize: 16, fontWeight: 700 }}>Ревью</div>
              {statusBadge(displayReview.status)}
              {isMyReview && <span className="badge badge-blue">Моё ревью</span>}
            </div>

            {/* Reviewee card */}
            <div style={{
              background: 'var(--bg2)',
              border: '1px solid var(--border)',
              borderRadius: 10,
              padding: '12px 14px',
              marginBottom: 12,
            }}>
              <div style={{ fontSize: 11, color: 'var(--text3)', marginBottom: 6, textTransform: 'uppercase', letterSpacing: '0.5px' }}>
                Кого оцениваю
              </div>
              <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                <div style={{
                  width: 36, height: 36, borderRadius: 10, flexShrink: 0,
                  background: 'linear-gradient(135deg,rgba(79,124,255,.25),rgba(124,92,252,.25))',
                  border: '1px solid rgba(79,124,255,.3)',
                  display: 'flex', alignItems: 'center', justifyContent: 'center',
                  fontSize: 13, fontWeight: 700, color: 'var(--accent)',
                }}>
                  {revieweeInitials}
                </div>
                <div>
                  <div style={{ fontSize: 14, fontWeight: 600 }}>{revieweeName}</div>
                  <div style={{ fontSize: 11, color: 'var(--text3)', fontFamily: 'var(--mono)' }}>
                    {review.revieweeId}
                  </div>
                </div>
              </div>
            </div>

            {/* Meta */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: 4, fontSize: 12, color: 'var(--text2)' }}>
              <div>
                <span style={{ color: 'var(--text3)' }}>Ревьюер: </span>
                <span>{reviewerName}</span>
                {isMyReview && <span style={{ color: 'var(--text3)' }}> (вы)</span>}
              </div>
              <div>
                <span style={{ color: 'var(--text3)' }}>Шаблон: </span>
                <span>{templateLoading ? 'Загрузка…' : template ? template.title : templateId ? templateId.slice(0, 8) + '…' : 'Не указан'}</span>
              </div>
            </div>

            {/* Progress */}
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

          {/* Actions */}
          {canEdit && (
            <div style={{ display: 'flex', gap: 8, flexShrink: 0 }}>
              {displayReview.status === 'Draft' && (
                <button
                  className="btn btn-primary btn-sm"
                  onClick={() => submitMutation.mutate()}
                  disabled={submitMutation.isPending || answeredCount === 0}
                  title={answeredCount === 0 ? 'Добавьте хотя бы один ответ' : ''}
                >
                  ↑ Отправить
                </button>
              )}
              {isAdmin && displayReview.status === 'Submitted' && (
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
            <div className="card-title">Вопросы для оценки</div>
            <div className="card-sub">
              {templateLoading
                ? 'Загрузка шаблона…'
                : template
                ? template.title
                : !templateId
                ? 'Шаблон не привязан к ревью'
                : 'Шаблон не найден'}
            </div>
          </div>
        </div>

        {templateLoading && (
          <div style={{ color: 'var(--text3)', padding: '20px 0', textAlign: 'center', fontSize: 13 }}>
            Загрузка вопросов…
          </div>
        )}

        {!templateLoading && !templateId && (
          <div className="empty">
            <div className="empty-icon">⚠</div>
            <p style={{ color: 'var(--warning)' }}>К этому ревью не привязан шаблон</p>
            <p style={{ fontSize: 12, marginTop: 8 }}>Это может быть ошибкой данных. Попробуйте создать ревью заново.</p>
          </div>
        )}

        {!templateLoading && templateId && questions.length === 0 && (
          <div className="empty">
            <div className="empty-icon">✦</div>
            <p>В шаблоне нет вопросов</p>
          </div>
        )}

        {questions.length > 0 && (
          <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
            {questions.map((q, i) => {
              const existing = answersMap[q.id];
              const isEditing = editingAnswer === q.id;

              return (
                <div key={q.id} style={{
                  background: 'var(--bg2)',
                  border: `1px solid ${existing ? 'rgba(79,124,255,0.25)' : 'var(--border)'}`,
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
                              {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map(n => (
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

                    {/* Action buttons — show for admin or reviewer when draft */}
                    {!isEditing && canEdit && displayReview.status === 'Draft' && (
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
        )}
      </div>
    </div>
  );
};

// ── Main Page ─────────────────────────────────────────────────────────────────

export const ReviewsPage: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';
  const [selected, setSelected] = useState<Review | null>(null);
  const [filter, setFilter] = useState<'all' | 'mine' | 'Draft' | 'Submitted' | 'Closed'>('all');

  // Load all users for name resolution
  const { data: users = [] } = useQuery({
    queryKey: ['users'],
    queryFn: () => authAPI.getAll(),
    select: r => r.data as User[],
  });

  // My reviews (as reviewer)
  const { data: myReviews, isLoading: myLoading } = useQuery({
    queryKey: ['reviews', 'mine', user?.id],
    queryFn: () => reviewAPI.getByReviewers({ userId: user?.id ?? '' }),
    select: r => r.data,
    enabled: !!user?.id,
  });

  // All reviews (admin only)
  const { data: allReviews, isLoading: allLoading } = useQuery({
    queryKey: ['reviews', 'all-params'],
    queryFn: () => reviewAPI.getByParameters({}),
    select: r => r.data,
    enabled: isAdmin,
  });

  const isLoading = myLoading || (isAdmin && allLoading);

  // Merge: admins see all reviews, regular users see only their own
  const baseReviews = useMemo(() => {
    if (isAdmin && allReviews && allReviews.length > 0) return allReviews;
    return myReviews ?? [];
  }, [isAdmin, allReviews, myReviews]);

  const filtered = useMemo(() => baseReviews.filter(r => {
    if (filter === 'mine') return r.reviewerId === user?.id;
    if (filter === 'Draft' || filter === 'Submitted' || filter === 'Closed') return r.status === filter;
    return true;
  }), [baseReviews, filter, user?.id]);

  // Refresh selected review
  const currentReview = selected
    ? baseReviews.find(r => r.id === selected.id) ?? selected
    : null;

  if (currentReview) {
    return (
      <ReviewDetail
        review={currentReview}
        isAdmin={isAdmin}
        users={users}
        onBack={() => setSelected(null)}
      />
    );
  }

  const countByStatus = (s: string) => baseReviews.filter(r => r.status === s).length;
  const myCount = baseReviews.filter(r => r.reviewerId === user?.id).length;

  return (
    <div className="fade-in">
      {/* Stats */}
      <div className="stat-grid" style={{ gridTemplateColumns: 'repeat(4,1fr)', marginBottom: 20 }}>
        <div className="stat-card">
          <div className="stat-label">Всего</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{baseReviews.length}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Мои ревью</div>
          <div className="stat-value" style={{ color: 'var(--accent)' }}>{myCount}</div>
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
        {([
          { key: 'all', label: 'Все' },
          { key: 'mine', label: 'Мои' },
          { key: 'Draft', label: 'Черновики' },
          { key: 'Submitted', label: 'Отправленные' },
          { key: 'Closed', label: 'Закрытые' },
        ] as const).map(f => (
          <button
            key={f.key}
            className={`tab ${filter === f.key ? 'active' : ''}`}
            onClick={() => setFilter(f.key)}
          >
            {f.label}
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
        <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
          {filtered.map(r => {
            const revieweeName = getUserName(users, r.revieweeId);
            const reviewerName = getUserName(users, r.reviewerId);
            const revieweeInitials = getUserInitials(users, r.revieweeId);
            const isMyReview = r.reviewerId === user?.id;

            return (
              <div
                key={r.id}
                className="card"
                style={{ cursor: 'pointer', transition: 'border-color 0.15s' }}
                onClick={() => setSelected(r)}
                onMouseEnter={e => (e.currentTarget.style.borderColor = 'var(--border2)')}
                onMouseLeave={e => (e.currentTarget.style.borderColor = 'var(--border)')}
              >
                <div style={{ display: 'flex', alignItems: 'center', gap: 14 }}>
                  {/* Reviewee avatar */}
                  <div style={{
                    width: 40, height: 40, borderRadius: 10, flexShrink: 0,
                    background: 'linear-gradient(135deg,rgba(79,124,255,.15),rgba(124,92,252,.15))',
                    border: '1px solid rgba(79,124,255,.2)',
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                    fontSize: 14, fontWeight: 600, color: 'var(--accent)',
                  }}>
                    {revieweeInitials}
                  </div>

                  {/* Info */}
                  <div style={{ flex: 1, minWidth: 0 }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 3 }}>
                      <span style={{ fontWeight: 600, fontSize: 14 }}>{revieweeName}</span>
                      {statusBadge(r.status)}
                      {isMyReview && <span className="badge badge-blue">Моё</span>}
                    </div>
                    <div style={{ fontSize: 12, color: 'var(--text3)' }}>
                      Ревьюер: {reviewerName}
                      {isMyReview && ' (вы)'}
                    </div>
                  </div>

                  {/* Answers count */}
                  <div style={{ textAlign: 'center', flexShrink: 0 }}>
                    <div style={{ fontSize: 15, fontWeight: 600, color: 'var(--text)' }}>
                      {r.answers?.length ?? 0}
                    </div>
                    <div style={{ fontSize: 11, color: 'var(--text3)' }}>ответов</div>
                  </div>

                  {/* ID */}
                  <div style={{ flexShrink: 0 }}>
                    <span style={{ fontFamily: 'var(--mono)', fontSize: 11, color: 'var(--text3)' }}>
                      {r.id ? `${r.id.slice(0, 8)}…` : '—'}
                    </span>
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
