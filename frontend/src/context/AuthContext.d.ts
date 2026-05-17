import React, { ReactNode } from 'react';
import { User } from '../types/types';
interface AuthContextType {
    user: User | null;
    isLoading: boolean;
    isAuthenticated: boolean;
    login: (email: string, password: string) => Promise<void>;
    register: (firstName: string, lastName: string, email: string, password: string, phoneNumber?: string) => Promise<void>;
    logout: () => void;
    refetchUser: () => Promise<void>;
}
export declare const useAuth: () => AuthContextType;
interface AuthProviderProps {
    children: ReactNode;
}
export declare const AuthProvider: React.FC<AuthProviderProps>;
export {};
//# sourceMappingURL=AuthContext.d.ts.map