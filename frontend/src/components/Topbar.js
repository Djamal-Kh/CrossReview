import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useAuth } from '../context/AuthContext';
import { initials } from '../utils/helpers';
export const Topbar = ({ title, subtitle }) => {
    const { user } = useAuth();
    return (_jsxs("div", { className: "topbar", children: [_jsxs("div", { children: [_jsx("div", { className: "page-title", children: title }), _jsx("div", { className: "page-sub", children: subtitle })] }), _jsxs("div", { style: { display: 'flex', alignItems: 'center', gap: 12 }, children: [_jsx("span", { className: `badge badge-${user?.role === 'Admin' ? 'purple' : 'blue'}`, children: user?.role }), _jsxs("div", { style: { display: 'flex', alignItems: 'center', gap: 8 }, children: [_jsx("div", { className: "avatar sm", children: initials(user?.firstName || '', user?.lastName || '') }), _jsxs("span", { style: { fontSize: 13, fontWeight: 500 }, children: [user?.firstName, " ", user?.lastName] })] })] })] }));
};
//# sourceMappingURL=Topbar.js.map