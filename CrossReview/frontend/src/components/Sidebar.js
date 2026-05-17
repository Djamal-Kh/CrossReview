import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { initials } from '../utils/helpers';
export const Sidebar = ({ currentPage, onNavigate, draftReviewCount = 0 }) => {
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
    return (_jsxs("div", { className: "sidebar", children: [_jsxs("div", { className: "sidebar-logo", children: [_jsx("div", { className: "logo-icon", children: "CR" }), _jsxs("div", { children: [_jsx("div", { className: "logo-name", children: "CrossReview" }), _jsx("div", { className: "logo-sub", children: "v2.0" })] })] }), _jsx("div", { className: "sidebar-nav", children: sections.map(sec => (_jsxs("div", { children: [_jsx("div", { className: "nav-section-title", children: sec }), navItems.filter(n => n.section === sec).map(item => (_jsxs("button", { className: `nav-item ${currentPage === item.id ? 'active' : ''}`, onClick: () => onNavigate(item.id), children: [_jsx("span", { style: { fontSize: 16 }, children: item.icon }), item.label, item.badge && _jsx("span", { className: "nav-badge", children: item.badge })] }, item.id)))] }, sec))) }), _jsx("div", { className: "sidebar-footer", children: _jsxs("div", { className: "user-card", children: [_jsx("div", { className: "avatar sm", children: initials(user?.firstName || '', user?.lastName || '') }), _jsxs("div", { style: { flex: 1, minWidth: 0 }, children: [_jsxs("div", { className: "user-name truncate", children: [user?.firstName, " ", user?.lastName] }), _jsx("div", { className: "user-role", children: user?.role })] }), _jsx("button", { className: "btn-icon", style: { fontSize: 12 }, title: "\u0412\u044B\u0439\u0442\u0438", onClick: handleLogout, children: "\u21A9" })] }) })] }));
};
//# sourceMappingURL=Sidebar.js.map