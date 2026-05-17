import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { initials } from '../utils/helpers';

interface SidebarProps {
  currentPage: string;
  onNavigate: (page: string) => void;
  draftReviewCount?: number;
}

export const Sidebar: React.FC<SidebarProps> = ({ currentPage, onNavigate, draftReviewCount = 0 }) => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const isAdmin = user?.role === 'Admin';

  const navItems = [
    { id: 'dashboard', label: 'Дашборд', icon: '⊞', section: 'Главное' },
    { id: 'projects', label: 'Проекты', icon: '◫', section: 'Главное' },
    { id: 'reviews', label: 'Ревью', icon: '✦', section: 'Главное', badge: draftReviewCount || null },
    { id: 'results', label: 'Результаты', icon: '◈', section: 'Аналитика' },
    ...(isAdmin ? [
      { id: 'templates', label: 'Шаблоны', icon: '▤', section: 'Управление' },
      { id: 'users', label: 'Пользователи', icon: '◎', section: 'Управление' },
    ] : []),
  ];

  const sections = [...new Set(navItems.map(n => n.section))];

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="sidebar">
      <div className="sidebar-logo">
        <div className="logo-icon">CR</div>
        <div>
          <div className="logo-name">CrossReview</div>
          <div className="logo-sub">v2.0</div>
        </div>
      </div>
      <div className="sidebar-nav">
        {sections.map(sec => (
          <div key={sec}>
            <div className="nav-section-title">{sec}</div>
            {navItems.filter(n => n.section === sec).map(item => (
              <button
                key={item.id}
                className={`nav-item ${currentPage === item.id ? 'active' : ''}`}
                onClick={() => onNavigate(item.id)}
              >
                <span style={{ fontSize: 16 }}>{item.icon}</span>
                {item.label}
                {item.badge && <span className="nav-badge">{item.badge}</span>}
              </button>
            ))}
          </div>
        ))}
      </div>
      <div className="sidebar-footer">
        <div className="user-card">
          <div className="avatar sm">{initials(user?.firstName || '', user?.lastName || '')}</div>
          <div style={{ flex: 1, minWidth: 0 }}>
            <div className="user-name truncate">{user?.firstName} {user?.lastName}</div>
            <div className="user-role">{user?.role}</div>
          </div>
          <button className="btn-icon" style={{ fontSize: 12 }} title="Выйти" onClick={handleLogout}>
            ↩
          </button>
        </div>
      </div>
    </div>
  );
};
