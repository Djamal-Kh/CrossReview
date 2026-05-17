import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
export const initials = (firstName, lastName) => `${firstName[0]}${lastName[0]}`;
export const statusBadge = (status) => {
    const badgeColorMap = {
        'Active': 'green',
        'Closed': 'gray',
        'Draft': 'amber',
        'Archive': 'red',
        'Submitted': 'blue',
    };
    const color = badgeColorMap[status] || 'gray';
    return _jsx("span", { className: `badge badge-${color}`, children: status });
};
export const roleBadge = (role) => {
    const badgeColorMap = {
        'Developer': 'blue',
        'TeamLead': 'purple',
        'Manager': 'amber',
    };
    const color = badgeColorMap[role] || 'gray';
    return _jsx("span", { className: `badge badge-${color}`, children: role });
};
export const ScoreBar = ({ score, max = 10 }) => {
    const percentage = (score / max) * 100;
    return (_jsxs("div", { className: "score-bar-wrap", children: [_jsx("div", { className: "score-bar-bg", children: _jsx("div", { className: "score-bar-fill", style: { width: `${percentage}%` } }) }), _jsx("span", { className: "score-val", children: score.toFixed(1) })] }));
};
//# sourceMappingURL=helpers.js.map