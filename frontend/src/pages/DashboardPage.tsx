import React, { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, RadarChart, Radar, PolarGrid, PolarAngleAxis, Cell, PieChart, Pie } from 'recharts';
import { projectAPI, reviewAPI, resultAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { ScoreBar } from '../utils/helpers';

const PAGE_TITLE = 'Дашборд';
const PAGE_SUB = 'Обзор производительности команды';

interface MetricResult {
  id: string;
  userId: string | { id: string; name?: string };
  user?: { name: string };
  finalScore: number;
  calculatedAt: string;
}

export const DashboardPage: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';

  // Запросы данных с безопасными ключами и флагами активации
  const { data: projectsData } = useQuery({
    queryKey: ['projects'],
    queryFn: () => projectAPI.getAll(),
    select: (response) => response.data,
  });

  const { data: reviewsData } = useQuery({
    queryKey: ['reviews', user?.id],
    queryFn: () => reviewAPI.getByReviewers({ userId: user?.id || '' }),
    select: (response) => response.data,
    enabled: !!user?.id,
  });

  const { data: resultsData } = useQuery({
    queryKey: ['results', user?.id, isAdmin],
    queryFn: () => isAdmin
      ? resultAPI.getAll()
      : resultAPI.getResults({ userId: user?.id }),
    select: (response) => response.data,
    enabled: !!user?.id,
  });

  // Безопасный расчет базовых метрик
  const activeProjects = useMemo(() => projectsData?.filter(p => p.status).length ?? 0, [projectsData]);
  const totalReviews = reviewsData?.length ?? 0;
  const closedReviews = useMemo(() => reviewsData?.filter(r => r.status === 'Closed').length ?? 0, [reviewsData]);

  const avgScore = useMemo(() => {
    if (!resultsData || resultsData.length === 0) return 0;
    const total = resultsData.reduce((a: number, r: MetricResult) => a + (r.finalScore || 0), 0);
    return total / resultsData.length;
  }, [resultsData]);

  // Статические данные для графиков активности (Ревью)
  const barData = [
    { month: 'Янв', reviews: 4, closed: 3 },
    { month: 'Фев', reviews: 6, closed: 5 },
    { month: 'Мар', reviews: 8, closed: 7 },
    { month: 'Апр', reviews: 5, closed: 3 },
    { month: 'Май', reviews: 9, closed: 6 },
  ];

  // Возвращен зашитый хардкод по вашему запросу
  const radarData = [
    { subject: 'Код', A: 8.4 },
    { subject: 'Дедлайны', A: 7.8 },
    { subject: 'Команда', A: 8.1 },
    { subject: 'Инициатива', A: 7.2 },
    { subject: 'Качество', A: 8.6 },
  ];

  // Динамическое распределение по реальным статусам
  const pieData = useMemo(() => {
    const submittedCount = reviewsData?.filter(r => r.status === 'Submitted').length ?? 0;
    const draftCount = reviewsData?.filter(r => r.status === 'Draft').length ?? 0;

    return [
      { name: 'Closed', value: closedReviews, fill: '#22c55e' },
      { name: 'Submitted', value: submittedCount, fill: '#f59e0b' },
      { name: 'Draft', value: draftCount, fill: '#5a6278' },
    ];
  }, [reviewsData, closedReviews]);

  // Безопасная сортировка топа результатов
  const topResults = useMemo(() => {
    return [...(resultsData ?? [])]
      .sort((a: MetricResult, b: MetricResult) => (b.finalScore || 0) - (a.finalScore || 0))
      .slice(0, 3);
  }, [resultsData]);

  // Хелперы для безопасного парсинга имен пользователей без падения UI
  const renderUserIdentity = (r: MetricResult) => {
    if (r.user?.name) return r.user.name;
    if (typeof r.userId === 'object' && r.userId?.name) return r.userId.name;
    if (typeof r.userId === 'string') return `Сотрудник ID: ${r.userId.slice(0, 8)}`;
    return 'Сотрудник';
  };

  const renderAvatarLetters = (r: MetricResult) => {
    if (r.user?.name) return r.user.name.slice(0, 2).toUpperCase();
    if (typeof r.userId === 'object' && r.userId?.name) return r.userId.name.slice(0, 2).toUpperCase();
    if (typeof r.userId === 'string') return r.userId.slice(0, 2).toUpperCase();
    return '??';
  };

  return (
    <div className="fade-in">
      <div className="stat-grid">
        <div className="stat-card">
          <div className="stat-label">Активные проекты</div>
          <div className="stat-value" style={{ color: 'var(--accent)' }}>{activeProjects}</div>
          <div className="stat-trend">из {projectsData?.length ?? 0} всего</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Всего ревью</div>
          <div className="stat-value" style={{ color: 'var(--info)' }}>{totalReviews}</div>
          <div className="stat-trend">
            <span className="up">↑ 23%</span> к прошлому периоду
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Завершено</div>
          <div className="stat-value" style={{ color: 'var(--success)' }}>{closedReviews}</div>
          <div className="stat-trend">{totalReviews > 0 ? Math.round((closedReviews / totalReviews) * 100) : 0}% от всех ревью</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">Средний балл</div>
          <div className="stat-value" style={{ color: 'var(--accent2)' }}>{avgScore.toFixed(1)}</div>
          <div className="stat-trend">из 10.0 максимум</div>
        </div>
      </div>

      <div className="grid-2" style={{ marginBottom: 16 }}>
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Активность ревью</div>
              <div className="card-sub">По месяцам</div>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={180}>
            <BarChart data={barData} barGap={4}>
              <XAxis dataKey="month" axisLine={false} tickLine={false} tick={{ fill: '#5a6278', fontSize: 11 }} />
              <YAxis axisLine={false} tickLine={false} tick={{ fill: '#5a6278', fontSize: 11 }} />
              <Tooltip contentStyle={{ background: '#1e2535', border: '1px solid rgba(255,255,255,0.07)', borderRadius: 8, color: '#e8eaf0', fontSize: 12 }} />
              <Bar dataKey="reviews" name="Создано" fill="rgba(79,124,255,0.4)" radius={[4, 4, 0, 0]} />
              <Bar dataKey="closed" name="Закрыто" fill="#4f7cff" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Профиль оценок</div>
              <div className="card-sub">Средние по категориям</div>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={180}>
            <RadarChart data={radarData}>
              <PolarGrid stroke="rgba(255,255,255,0.07)" />
              <PolarAngleAxis dataKey="subject" tick={{ fill: '#5a6278', fontSize: 11 }} />
              <Radar dataKey="A" fill="rgba(79,124,255,0.2)" stroke="#4f7cff" strokeWidth={2} />
              <Tooltip contentStyle={{ background: '#1e2535', border: '1px solid rgba(255,255,255,0.07)', borderRadius: 8, color: '#e8eaf0', fontSize: 12 }} />
            </RadarChart>
          </ResponsiveContainer>
        </div>
      </div>

      <div className="grid-2">
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Статус ревью</div>
              <div className="card-sub">Текущее распределение</div>
            </div>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: 20 }}>
            <PieChart width={120} height={120}>
              <Pie data={pieData} cx={55} cy={55} innerRadius={30} outerRadius={50} dataKey="value" strokeWidth={0}>
                {pieData.map((d, i) => (
                  <Cell key={i} fill={d.fill} />
                ))}
              </Pie>
            </PieChart>
            <div style={{ flex: 1 }}>
              {pieData.map(d => (
                <div key={d.name} style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 8 }}>
                  <div style={{ width: 10, height: 10, borderRadius: 2, background: d.fill, flexShrink: 0 }} />
                  <span style={{ fontSize: 12, color: 'var(--text2)' }}>{d.name}</span>
                  <span style={{ marginLeft: 'auto', fontSize: 13, fontWeight: 600 }}>{d.value}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Топ результаты</div>
              <div className="card-sub">Лучшие показатели за все время</div>
            </div>
          </div>
          <div style={{ display: 'flex', flexDirection: 'column' }}>
            {topResults.length === 0 ? (
              <div style={{ color: 'var(--text3)', fontSize: 14, padding: '16px 0' }}>
                Нет рассчитанных результатов
              </div>
            ) : (
              topResults.map((r: MetricResult, index: number) => (
                <div
                  key={r.id}
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    padding: '14px 0', // Увеличили вертикальный отступ для баланса
                    borderBottom: index < topResults.length - 1 ? '1px solid rgba(255, 255, 255, 0.05)' : 'none'
                  }}
                >
                  <div style={{ display: 'flex', alignItems: 'center', gap: 14 }}>
                    {/* Номер в топе — теперь крупнее и заметнее */}
                    <span style={{ fontSize: 13, fontWeight: 700, color: index === 0 ? 'var(--accent2)' : 'var(--text3)', width: 20 }}>
                      #{index + 1}
                    </span>
                    {/* Дата — увеличили до комфортных 15px */}
                    <span style={{ fontSize: 15, fontWeight: 500, color: 'var(--text2)' }}>
                      {r.calculatedAt
                        ? new Date(r.calculatedAt).toLocaleDateString('ru', { day: 'numeric', month: 'long', year: 'numeric' })
                        : '—'}
                    </span>
                  </div>
                  {/* Балл — теперь 16px, выглядит весомо */}
                  <div
                    style={{
                      fontSize: 16,
                      fontWeight: 700,
                      color: 'var(--accent2)',
                      background: 'rgba(255, 255, 255, 0.04)',
                      padding: '6px 12px', // Чуть просторнее внутри бейджа
                      borderRadius: 6,
                      border: '1px solid rgba(255, 255, 255, 0.08)'
                    }}
                  >
                    {r.finalScore.toFixed(1)}
                  </div>
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
};