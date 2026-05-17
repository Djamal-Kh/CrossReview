import { AxiosInstance, AxiosResponse } from 'axios';
import { User, LoginRequest, RegisterRequest, Project, Review, Template, EvaluationResult } from '../types/types';
declare const apiClient: AxiosInstance;
export declare const authAPI: {
    login: (data: LoginRequest) => Promise<AxiosResponse<string | LoginResponse, any, {}>>;
    register: (data: RegisterRequest) => Promise<AxiosResponse<string | LoginResponse, any, {}>>;
    getMe: () => Promise<AxiosResponse<User, any, {}>>;
    getUser: (userId: string) => Promise<AxiosResponse<User, any, {}>>;
    getUserByEmail: (email: string) => Promise<AxiosResponse<User, any, {}>>;
};
export declare const projectAPI: {
    getAll: () => Promise<AxiosResponse<Project[], any, {}>>;
    getById: (id: string) => Promise<AxiosResponse<Project, any, {}>>;
    create: (data: {
        title: string;
        description: string;
    }) => Promise<AxiosResponse<Project, any, {}>>;
    update: (id: string, data: {
        title?: string;
        description?: string;
    }) => Promise<AxiosResponse<Project, any, {}>>;
    updateTitle: (id: string, title: string) => Promise<AxiosResponse<Project, any, {}>>;
    updateDescription: (id: string, description: string) => Promise<AxiosResponse<Project, any, {}>>;
    start: (id: string) => Promise<AxiosResponse<any, any, {}>>;
    close: (id: string) => Promise<AxiosResponse<any, any, {}>>;
};
export declare const projectMembersAPI: {
    getMembers: (projectId: string) => Promise<AxiosResponse<any, any, {}>>;
    addMember: (data: {
        projectId: string;
        userId: string;
        role: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    updateRole: (data: {
        projectId: string;
        userId: string;
        role: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    removeMember: (projectId: string, userId: string) => Promise<AxiosResponse<any, any, {}>>;
};
export declare const reviewPeriodAPI: {
    create: (data: {
        projectId: string;
        startDate: string;
        endDate: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    activate: (projectId: string, periodId: string) => Promise<AxiosResponse<any, any, {}>>;
    close: (projectId: string, periodId: string) => Promise<AxiosResponse<any, any, {}>>;
    archive: (projectId: string, periodId: string) => Promise<AxiosResponse<any, any, {}>>;
    update: (data: {
        projectId: string;
        periodId: string;
        startDate: string;
        endDate: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
};
export declare const reviewAPI: {
    create: (data: {
        reviewerId: string;
        revieweeId: string;
        projectId: string;
        templateId: string;
        periodId: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    getById: (id: string) => Promise<AxiosResponse<Review, any, {}>>;
    getByParameters: (data: {
        projectId?: string;
        revieweeId?: string;
        reviewerId?: string;
        periodId?: string;
    }) => Promise<AxiosResponse<Review[], any, {}>>;
    getByReviewers: (data: {
        userId: string;
        projectId?: string;
        periodId?: string;
    }) => Promise<AxiosResponse<Review[], any, {}>>;
    addAnswer: (data: {
        reviewId: string;
        questionId: string;
        score: number;
        comment: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    updateAnswer: (data: {
        reviewId: string;
        questionId: string;
        score: number;
        comment: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    removeAnswer: (reviewId: string, questionId: string) => Promise<AxiosResponse<any, any, {}>>;
    submit: (data: {
        reviewId: string;
        templateId: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    close: (reviewId: string) => Promise<AxiosResponse<any, any, {}>>;
};
export declare const templateAPI: {
    getById: (templateId: string) => Promise<AxiosResponse<Template, any, {}>>;
    create: (data: {
        projectId: string;
        title: string;
    }) => Promise<AxiosResponse<Template, any, {}>>;
    activate: (templateId: string) => Promise<AxiosResponse<any, any, {}>>;
    deactivate: (templateId: string) => Promise<AxiosResponse<any, any, {}>>;
    updateTitle: (data: {
        templateId: string;
        title: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    addQuestion: (data: {
        templateId: string;
        title: string;
        weight: number;
    }) => Promise<AxiosResponse<any, any, {}>>;
    updateQuestion: (data: {
        templateId: string;
        questionId: string;
        title: string;
        weight: number;
    }) => Promise<AxiosResponse<any, any, {}>>;
    removeQuestion: (templateId: string, questionId: string) => Promise<AxiosResponse<any, any, {}>>;
    delete: (templateId: string) => Promise<AxiosResponse<any, any, {}>>;
};
export declare const resultAPI: {
    getResults: (data: {
        userId?: string;
        projectId?: string;
        periodId?: string;
    }) => Promise<AxiosResponse<EvaluationResult[], any, {}>>;
    calculate: (data: {
        userId: string;
        projectId: string;
        periodId: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    recalculate: (evaluationResultId: string) => Promise<AxiosResponse<any, any, {}>>;
};
export declare const userManagementAPI: {
    addAdmin: (data: {
        firstName: string;
        lastName: string;
        email: string;
        password: string;
        phoneNumber?: string;
    }) => Promise<AxiosResponse<any, any, {}>>;
    delete: (userId: string) => Promise<AxiosResponse<any, any, {}>>;
};
export type LoginResponse = {
    token: string;
    user: User;
};
export default apiClient;
//# sourceMappingURL=client.d.ts.map