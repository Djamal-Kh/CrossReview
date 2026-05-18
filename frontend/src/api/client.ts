import axios, { AxiosInstance, AxiosError, AxiosResponse } from 'axios';
import { User, LoginRequest, RegisterRequest, Project, Review, Template, EvaluationResult } from '../types/types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5171/api';

const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add JWT token
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor to handle token expiration
apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  login: (data: LoginRequest) =>
    apiClient.post<string | LoginResponse>('/user/login', undefined, { params: data }),
  
  register: (data: RegisterRequest) =>
    apiClient.post<string | LoginResponse>('/user/register', undefined, { params: data }),
  
  getMe: () =>
    apiClient.get<User>('/user/me'),
  
  getUser: (userId: string) =>
    apiClient.get<User>(`/user/id/${userId}`),
  
  getUserByEmail: (email: string) =>
        apiClient.get<User>(`/user/email/${email}`),

  getAll: () =>
        apiClient.get<User[]>('/user/all'),
};

// Project API
export const projectAPI = {
  getAll: () =>
    apiClient.get<Project[]>('/project/all'),
  
  getById: (id: string) =>
    apiClient.get<Project>(`/project/${id}`),
  
  create: (data: { title: string; description: string }) =>
    apiClient.post<Project>('/project/create', undefined, { params: data }),
  
  update: (id: string, data: { title?: string; description?: string }) =>
    apiClient.put<Project>(`/project/${id}/update`, undefined, { params: data }),
  
  updateTitle: (id: string, title: string) =>
    apiClient.patch<Project>(`/project/${id}/update/title`, undefined, { params: { title } }),
  
  updateDescription: (id: string, description: string) =>
    apiClient.patch<Project>(`/project/${id}/update/description`, undefined, { params: { description } }),
  
  start: (id: string) =>
    apiClient.patch(`/project/${id}/start`),
  
  close: (id: string) =>
    apiClient.delete(`/project/${id}/close`),
};

// Project Members API
export const projectMembersAPI = {
  getMembers: (projectId: string) =>
    apiClient.get(`/project/members/project/${projectId}`),
  
  addMember: (data: { projectId: string; userId: string; role: string }) =>
    apiClient.post('/project/members/add', undefined, { params: data }),
  
  updateRole: (data: { projectId: string; userId: string; role: string }) =>
    apiClient.patch('/project/members/update-role', undefined, { params: data }),
  
  removeMember: (projectId: string, userId: string) =>
    apiClient.delete('/project/members/remove-member', { params: { projectId, userId } }),
};

// Review Period API
export const reviewPeriodAPI = {
  create: (data: { projectId: string; startDate: string; endDate: string }) =>
    apiClient.post('/project/review-period/create', undefined, { params: data }),
  
  activate: (projectId: string, periodId: string) =>
    apiClient.patch('/project/review-period/activate', undefined, { params: { projectId, periodId } }),
  
  close: (projectId: string, periodId: string) =>
    apiClient.patch('/project/review-period/close', undefined, { params: { projectId, periodId } }),
  
  archive: (projectId: string, periodId: string) =>
    apiClient.patch('/project/review-period/archive', undefined, { params: { projectId, periodId } }),
  
  update: (data: { projectId: string; periodId: string; startDate: string; endDate: string }) =>
    apiClient.patch('/project/review-period/update', undefined, { params: data }),
};

// Review API
export const reviewAPI = {
  create: (data: { reviewerId: string; revieweeId: string; projectId: string; templateId: string; periodId: string }) =>
    apiClient.post('/review/create', undefined, { params: data }),
  
  getById: (id: string) =>
    apiClient.get<Review>('/review/id', { params: { id } }),
  
  getByParameters: (data: { projectId?: string; revieweeId?: string; reviewerId?: string; periodId?: string }) =>
    apiClient.get<Review[]>('/review/by-parameters', { params: data }),
  
  getByReviewers: (data: { userId: string; projectId?: string; periodId?: string }) =>
    apiClient.get<Review[]>('/review/by-reviewers', { params: data }),
  
  addAnswer: (data: { reviewId: string; questionId: string; score: number; comment: string }) =>
    apiClient.post('/review/answer/add', undefined, { params: data }),
  
  updateAnswer: (data: { reviewId: string; questionId: string; score: number; comment: string }) =>
    apiClient.patch('/review/answer/update', undefined, { params: data }),
  
  removeAnswer: (reviewId: string, questionId: string) =>
    apiClient.patch('/review/answer/remove', undefined, { params: { reviewId, questionId } }),
  
  submit: (data: { reviewId: string; templateId: string }) =>
    apiClient.patch('/review/submit', undefined, { params: data }),
  
  close: (reviewId: string) =>
    apiClient.patch('/review/close', undefined, { params: { reviewId } }),
};

// Template API
export const templateAPI = {
  getAll: () =>
    apiClient.get<Template[]>('/template/all'),

  getById: (templateId: string) =>
    apiClient.get<Template>(`/template/${templateId}`),
  
  create: (data: { projectId: string; title: string }) =>
    apiClient.post<Template>('/template/create', undefined, { params: data }),
  
  activate: (templateId: string) =>
    apiClient.patch('/template/activate', undefined, { params: { templateId } }),
  
  deactivate: (templateId: string) =>
    apiClient.patch('/template/deactivate', undefined, { params: { templateId } }),
  
  updateTitle: (data: { templateId: string; title: string }) =>
    apiClient.patch('/template/update-title', undefined, { params: data }),
  
  addQuestion: (data: { templateId: string; title: string; weight: number }) =>
    apiClient.patch('/template/question/add', undefined, { params: data }),
  
  updateQuestion: (data: { templateId: string; questionId: string; title: string; weight: number }) =>
    apiClient.put('/template/question/update', undefined, { params: data }),
  
  removeQuestion: (templateId: string, questionId: string) =>
    apiClient.delete('/template/question/remove', { params: { templateId, questionId } }),
  
  delete: (templateId: string) =>
        apiClient.delete('/template/delete', { params: { templateId } }),

};

// Evaluation Result API
export const resultAPI = {
  getResults: (data: { userId?: string; projectId?: string; periodId?: string }) =>
    apiClient.get<EvaluationResult[]>('/review/evaluation-result/by-parameters', { params: data }),
  
  calculate: (data: { userId: string; projectId: string; periodId: string }) =>
    apiClient.post('/review/evaluation-result/calculate', undefined, { params: data }),
  
  recalculate: (evaluationResultId: string) =>
    apiClient.patch('/review/evaluation-result/recalculate', undefined, { params: { evaluationResultId } }),
};

// User Management API (Admin only)
export const userManagementAPI = {
  addAdmin: (data: { firstName: string; lastName: string; email: string; password: string; phoneNumber?: string }) =>
    apiClient.post('/user/add-admin', undefined, { params: data }),
  
  delete: (userId: string) =>
    apiClient.delete('/user/delete', { params: { userId } }),
};

export type LoginResponse = {
  token: string;
  user: User;
};

export default apiClient;
