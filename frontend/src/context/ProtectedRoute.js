import { jsx as _jsx, Fragment as _Fragment } from "react/jsx-runtime";
import { Navigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
export const ProtectedRoute = ({ children, requiredRole }) => {
    const { user, isLoading } = useAuth();
    if (isLoading) {
        return (_jsx("div", { style: { display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100vh' }, children: _jsx("div", { style: { fontSize: 18, color: '#8b93a8' }, children: "\u0417\u0430\u0433\u0440\u0443\u0437\u043A\u0430\u2026" }) }));
    }
    if (!user) {
        return _jsx(Navigate, { to: "/login" });
    }
    if (requiredRole && user.role !== requiredRole) {
        return _jsx(Navigate, { to: "/dashboard" });
    }
    return _jsx(_Fragment, { children: children });
};
//# sourceMappingURL=ProtectedRoute.js.map