import React, { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import { BarChart, Bar, LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer, RadarChart, Radar, PolarGrid, PolarAngleAxis, Cell, PieChart, Pie } from 'recharts';
import { projectAPI, reviewAPI, resultAPI } from '../api/client';
import { useAuth } from '../context/AuthContext';
import { ScoreBar } from '../utils/helpers';

const PAGE_TITLE = 'Дашборд';
const PAGE_SUB = 'Обзор производительности команды';

export const DashboardPage: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';

  // Fetch data
  const { data: projectsData } = useQuery({
    queryKey: ['projects'],
    queryFn: () => projectAPI.getAll(),
    select: (response) => response.data,
  });

  const { data: reviewsData } = useQuery({
    queryKey: ['reviews'],
    queryFn: () => reviewAPI.getByReviewers({ userId: user?.id || '' }),
    select: (response) => response.data,
  });

  const { data: resultsData } = useQuery({
    queryKey: ['results'],
    queryFn: () => resultAPI.getResults(isAdmin ? {} : { userId: user?.id }),
    select: (response) => response.data,
  });

  // Calculate stats
  const activeProjects = useMemo(() => projectsData?.filter(p => p.status).length ?? 0, [projectsData]);
  const totalReviews = reviewsData?.length ?? 0;
  const closedReviews = useMemo(() => reviewsData?.filter(r => r.status === 'Closed').length ?? 0, [reviewsData]);
  const avgScore = useMemo(() => {
    if (!resultsData || resultsData.length === 0) return 0;
    return resultsData.reduce((a, r) => a + r.finalScore, 0) / resultsData.length;
  }, [resultsData]);

  // Chart data
  const barData = [
    { month: 'Янв', reviews: 4, closed: 3 },
    { month: 'Фев', reviews: 6, closed: 5 },
    { month: 'Мар', reviews: 8, closed: 7 },
    { month: 'Апр', reviews: 5, closed: 3 },
    { month: 'Май', reviews: 9, closed: 6 },
  ];

  const radarData = [
    { subject: 'Код', A: 8.4 },
    { subject: 'Дедлайны', A: 7.8 },
    { subject: 'Команда', A: 8.1 },
    { subject: 'Инициатива', A: 7.2 },
    { subject: 'Качество', A: 8.6 },
  ];

  const pieData = [
    { name: 'Closed', value: closedReviews, fill: '#22c55e' },
    { name: 'Submitted', value: 1, fill: '#f59e0b' },
    { name: 'Draft', value: Math.max(0, totalReviews - closedReviews - 1), fill: '#5a6278' },
  ];

  const topResults = useMemo(() => {
    return (resultsData ?? []).sort((a, b) => b.finalScore - a.finalScore).slice(0, 3);
  }, [resultsData]);

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
              <PolarAngleAxis dataKey="subject" />
              <Radar dataKey="A" fill="rgba(79,124,255,0.2)" stroke="#4f7cff" strokeWidth={2} />
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
              <div className="card-sub">За последний период</div>
            </div>
          </div>
          <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
            {topResults.length === 0 ? (
              <div style={{ color: 'var(--text3)', fontSize: 12 }}>Нет результатов</div>
            ) : (
              topResults.map(r => (
                <div key={r.id}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 6 }}>
                    <div className="avatar sm">{r.userId.slice(0, 2).toUpperCase()}</div>
                    <span style={{ fontSize: 12, flex: 1 }}>Сотрудник {r.userId.slice(0, 8)}</span>
                    <span style={{ fontSize: 11, color: 'var(--text3)' }}>{new Date(r.calculatedAt).toLocaleDateString('ru')}</span>
                  </div>
                  <ScoreBar score={r.finalScore} />
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
