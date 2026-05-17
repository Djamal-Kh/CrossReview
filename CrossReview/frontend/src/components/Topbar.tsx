import React from 'react';
import { useAuth } from '../context/AuthContext';
import { initials } from '../utils/helpers';

interface TopbarProps {
  title: string;
  subtitle: string;
}

export const Topbar: React.FC<TopbarProps> = ({ title, subtitle }) => {
  const { user } = useAuth();

  return (
    <div className="topbar">
      <div>
        <div className="page-title">{title}</div>
        <div className="page-sub">{subtitle}</div>
      </div>
      <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
        <span className={`badge badge-${user?.role === 'Admin' ? 'purple' : 'blue'}`}>{user?.role}</span>
        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
          <div className="avatar sm">{initials(user?.firstName || '', user?.lastName || '')}</div>
          <span style={{ fontSize: 13, fontWeight: 500 }}>{user?.firstName} {user?.lastName}</span>
        </div>
      </div>
    </div>
  );
};
