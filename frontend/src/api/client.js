import axios from 'axios';
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5171/api';
const apiClient = axios.create({
    baseURL: API_BASE_URL,
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
    },
});
// Request interceptor to add JWT token
apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, (error) => Promise.reject(error));
// Response interceptor to handle token expiration
apiClient.interceptors.response.use((response) => response, (error) => {
    if (error.response?.status === 401) {
        localStorage.removeItem('authToken');
        window.location.href = '/login';
    }
    return Promise.reject(error);
});
// Auth API
export const authAPI = {
    login: (data) => apiClient.post('/user/login', undefined, { params: data }),
    register: (data) => apiClient.post('/user/register', undefined, { params: data }),
    getMe: () => apiClient.get('/user/me'),
    getUser: (userId) => apiClient.get(`/user/id/${userId}`),
    getUserByEmail: (email) => apiClient.get(`/user/email/${email}`),
};
// Project API
export const projectAPI = {
    getAll: () => apiClient.get('/project/all'),
    getById: (id) => apiClient.get(`/project/${id}`),
    create: (data) => apiClient.post('/project/create', undefined, { params: data }),
    update: (id, data) => apiClient.put(`/project/${id}/update`, undefined, { params: data }),
    updateTitle: (id, title) => apiClient.patch(`/project/${id}/update/title`, undefined, { params: { title } }),
    updateDescription: (id, description) => apiClient.patch(`/project/${id}/update/description`, undefined, { params: { description } }),
    start: (id) => apiClient.patch(`/project/${id}/start`),
    close: (id) => apiClient.delete(`/project/${id}/close`),
};
// Project Members API
export const projectMembersAPI = {
    getMembers: (projectId) => apiClient.get(`/project/members/project/${projectId}`),
    addMember: (data) => apiClient.post('/project/members/add', undefined, { params: data }),
    updateRole: (data) => apiClient.patch('/project/members/update-role', undefined, { params: data }),
    removeMember: (projectId, userId) => apiClient.delete('/project/members/remove-member', { params: { projectId, userId } }),
};
// Review Period API
export const reviewPeriodAPI = {
    create: (data) => apiClient.post('/project/review-period/create', undefined, { params: data }),
    activate: (projectId, periodId) => apiClient.patch('/project/review-period/activate', undefined, { params: { projectId, periodId } }),
    close: (projectId, periodId) => apiClient.patch('/project/review-period/close', undefined, { params: { projectId, periodId } }),
    archive: (projectId, periodId) => apiClient.patch('/project/review-period/archive', undefined, { params: { projectId, periodId } }),
    update: (data) => apiClient.patch('/project/review-period/update', undefined, { params: data }),
};
// Review API
export const reviewAPI = {
    create: (data) => apiClient.post('/review/create', undefined, { params: data }),
    getById: (id) => apiClient.get('/review/id', { params: { id } }),
    getByParameters: (data) => apiClient.get('/review/by-parameters', { params: data }),
    getByReviewers: (data) => apiClient.get('/review/by-reviewers', { params: data }),
    addAnswer: (data) => apiClient.post('/review/answer/add', undefined, { params: data }),
    updateAnswer: (data) => apiClient.patch('/review/answer/update', undefined, { params: data }),
    removeAnswer: (reviewId, questionId) => apiClient.patch('/review/answer/remove', undefined, { params: { reviewId, questionId } }),
    submit: (data) => apiClient.patch('/review/submit', undefined, { params: data }),
    close: (reviewId) => apiClient.patch('/review/close', undefined, { params: { reviewId } }),
};
// Template API
export const templateAPI = {
    getById: (templateId) => apiClient.get(`/template/${templateId}`),
    create: (data) => apiClient.post('/template/create', undefined, { params: data }),
    activate: (templateId) => apiClient.patch('/template/activate', undefined, { params: { templateId } }),
    deactivate: (templateId) => apiClient.patch('/template/deactivate', undefined, { params: { templateId } }),
    updateTitle: (data) => apiClient.patch('/template/update-title', undefined, { params: data }),
    addQuestion: (data) => apiClient.patch('/template/question/add', undefined, { params: data }),
    updateQuestion: (data) => apiClient.put('/template/question/update', undefined, { params: data }),
    removeQuestion: (templateId, questionId) => apiClient.delete('/template/question/remove', { params: { templateId, questionId } }),
    delete: (templateId) => apiClient.delete('/template/delete', { params: { templateId } }),
};
// Evaluation Result API
export const resultAPI = {
    getResults: (data) => apiClient.get('/review/evaluation-result', { params: data }),
    calculate: (data) => apiClient.post('/review/evaluation-result/calculate', undefined, { params: data }),
    recalculate: (evaluationResultId) => apiClient.patch('/review/evaluation-result/recalculate', undefined, { params: { evaluationResultId } }),
};
// User Management API (Admin only)
export const userManagementAPI = {
    addAdmin: (data) => apiClient.post('/user/add-admin', undefined, { params: data }),
    delete: (userId) => apiClient.delete('/user/delete', { params: { userId } }),
};
export default apiClient;
//# sourceMappingURL=client.js.map