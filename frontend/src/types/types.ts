import { ReviewPeriodStatus } from './enums';

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: 'Admin' | 'User';
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
}

export interface Project {
  id: string;
  title: string;
  description: string;
  status: boolean;
  members: ProjectMember[];
  reviewPeriods: ReviewPeriod[];
}

export interface ProjectMember {
  userId: string;
  role: 'Developer' | 'TeamLead' | 'Manager';
}

export interface ReviewPeriod {
  id: string;
  name: string;
  status: ReviewPeriodStatus;
  startDate: string;
  endDate: string;
}

export interface Template {
  id: string;
  projectId: string;
  title: string;
  isActive: boolean;
  questions: TemplateQuestion[];
}

export interface TemplateQuestion {
  id: string;
  title: string;
  weight: number;
  order: number;
}

export interface Review {
  id: string;
  reviewerId: string;
  revieweeId: string;
  projectId: string;
  periodId: string;
  // templateId comes from backend — mark as optional for safety (old reviews may lack it)
  templateId?: string;
  status: 'Draft' | 'Submitted' | 'Closed';
  answers: ReviewAnswer[];
}

export interface ReviewAnswer {
  questionId: string;
  score: number;
  comment: string;
}

export interface EvaluationResult {
  id: string;
  userId: string;
  projectId: string;
  periodId: string;
  finalScore: number;
  calculatedAt: string;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}
