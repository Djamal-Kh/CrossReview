import React, { useState } from 'react';
import { BrowserRouter, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { QueryClientProvider, QueryClient } from '@tanstack/react-query';
import { AuthProvider, useAuth } from './context/AuthContext';

// Импорты страниц и компонентов...
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
      staleTime: 1000 * 60 * 5,
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
  draftReviewCount: number;
}

const MainLayout: React.FC<MainLayoutProps> = ({ draftReviewCount }) => {
  const { user } = useAuth();
  const location = useLocation();
  const isAdmin = user?.role === 'Admin';
  
  // Извлекаем текущую страницу из URL (например, "/projects" -> "projects")
  const currentPage = location.pathname.replace('/', '') || 'dashboard';
  const meta = PAGE_META[currentPage] || { title: 'Загрузка...', sub: '' };

  return (
    <div className="layout">
      {/* Sidebar теперь должен использовать внутри себя <Link to="/..."> или useNavigate() */}
      <Sidebar currentPage={currentPage} draftReviewCount={draftReviewCount} />
      
      <div className="main">
        <Topbar title={meta.title} subtitle={meta.sub} />
        <div className="content">
          <Routes>
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
            <Route path="/dashboard" element={<DashboardPage />} />
            <Route path="/projects" element={<ProjectsPage />} />
            <Route path="/reviews" element={<ReviewsPage />} />
            <Route path="/results" element={<ResultsPage />} />
            <Route path="/templates" element={<TemplatesPage />} />
            {/* Если не админ, перенаправляем на дашборд. Replace предотвращает бесконечную петлю в истории браузера */}
            <Route 
              path="/users" 
              element={isAdmin ? <UsersPage /> : <Navigate to="/dashboard" replace />} 
            />
            {/* Для любых несуществующих URL */}
            <Route path="*" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </div>
      </div>
    </div>
  );
};

function AppContent() {
  const { isLoading, isAuthenticated } = useAuth();
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

  return <MainLayout draftReviewCount={draftReviewCount} />;
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

export default App;