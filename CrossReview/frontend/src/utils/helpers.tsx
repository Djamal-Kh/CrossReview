import React from 'react';

export const initials = (firstName: string, lastName: string) => 
  `${firstName[0]}${lastName[0]}`;

export const statusBadge = (status: string) => {
  const badgeColorMap: Record<string, string> = {
    'Active': 'green',
    'Closed': 'gray',
    'Draft': 'amber',
    'Archive': 'red',
    'Submitted': 'blue',
  };
  const color = badgeColorMap[status] || 'gray';
  return <span className={`badge badge-${color}`}>{status}</span>;
};

export const roleBadge = (role: string) => {
  const badgeColorMap: Record<string, string> = {
    'Developer': 'blue',
    'TeamLead': 'purple',
    'Manager': 'amber',
  };
  const color = badgeColorMap[role] || 'gray';
  return <span className={`badge badge-${color}`}>{role}</span>;
};

interface ScoreBarProps {
  score: number;
  max?: number;
}

export const ScoreBar: React.FC<ScoreBarProps> = ({ score, max = 10 }) => {
  const percentage = (score / max) * 100;
  return (
    <div className="score-bar-wrap">
      <div className="score-bar-bg">
        <div className="score-bar-fill" style={{ width: `${percentage}%` }} />
      </div>
      <span className="score-val">{score.toFixed(1)}</span>
    </div>
  );
};
