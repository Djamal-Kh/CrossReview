import React, { useState } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClientProvider, QueryClient } from '@tanstack/react-query';
import { AuthProvider, useAuth } from './context/AuthContext';
import { ReviewsPage } from './pages/ReviewsPage';
import { LoginPage } from './pages/LoginPage';
import { DashboardPage } from './pages/DashboardPage';
import { ProjectsPage } from './pages/ProjectsPage';
import { ResultsPage } from './pages/ResultsPage';
import { TemplatesPage } from './pages/TemplatesPage';
import { UsersPage } from './pages/UsersPage';
import { Sidebar } from './components/Sidebar';
import { Topbar } from './components/Topbar';
import './styles/global.css';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
      staleTime: 1000 * 60 * 5, // 5 minutes
    },
  },
});

const PAGE_META: Record<string, { title: string; sub: string }> = {
  dashboard: { title: 'Дашборд', sub: 'Обзор производительности команды' },
  projects: { title: 'Проекты', sub: 'Управление проектами и периодами' },
  reviews: { title: 'Ревью', sub: 'Список и детали ревью' },
  results: { title: 'Результаты', sub: 'Итоговые оценки и аналитика' },
  templates: { title: 'Шаблоны ревью', sub: 'Настройка вопросов и весов' },
  users: { title: 'Пользователи', sub: 'Управление учётными записями' },
};

interface MainLayoutProps {
  currentPage: string;
  onPageChange: (page: string) => void;
  draftReviewCount: number;
}

const MainLayout: React.FC<MainLayoutProps> = ({ currentPage, onPageChange, draftReviewCount }) => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';
  const meta = PAGE_META[currentPage] || { title: currentPage, sub: '' };

  const renderPage = () => {
    switch (currentPage) {
      case 'dashboard':
        return <DashboardPage />;
      case 'projects':
            return <ProjectsPage />;
      case 'reviews':
        return <ReviewsPage />;
      case 'results':
        return <ResultsPage />;
      case 'templates':
            return isAdmin ? <TemplatesPage /> : <Navigate to="/dashboard" />;
      case 'users':
            return isAdmin ? <UsersPage /> : <Navigate to="/dashboard" />;
      default:
        return <Navigate to="/dashboard" />;
    }
  };

  return (
    <div className="layout">
      <Sidebar currentPage={currentPage} onNavigate={onPageChange} draftReviewCount={draftReviewCount} />
      <div className="main">
        <Topbar title={meta.title} subtitle={meta.sub} />
        <div className="content">{renderPage()}</div>
      </div>
    </div>
  );
};

function AppContent() {
  const { isLoading, isAuthenticated } = useAuth();
  const [currentPage, setCurrentPage] = useState('dashboard');
  const [draftReviewCount] = useState(0); // TODO: fetch from API

  if (isLoading) {
    return (
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100vh', background: '#0e1117' }}>
        <div style={{ fontSize: 18, color: '#8b93a8' }}>Загрузка…</div>
      </div>
    );
  }

  if (!isAuthenticated) {
    return <LoginPage />;
  }

  return <MainLayout currentPage={currentPage} onPageChange={setCurrentPage} draftReviewCount={draftReviewCount} />;
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <AuthProvider>
          <AppContent />
        </AuthProvider>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default  App;
