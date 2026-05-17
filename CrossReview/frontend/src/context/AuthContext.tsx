import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User } from '../types/types';
import { authAPI } from '../api/client';

interface AuthContextType {
  user: User | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (firstName: string, lastName: string, email: string, password: string, phoneNumber?: string) => Promise<void>;
  logout: () => void;
  refetchUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Check if user is already logged in on mount
  useEffect(() => {
    const initAuth = async () => {
      const token = localStorage.getItem('authToken');
      if (token) {
        try {
          const response = await authAPI.getMe();
          setUser(response.data);
        } catch {
          localStorage.removeItem('authToken');
          setUser(null);
        }
      }
      setIsLoading(false);
    };

    initAuth();
  }, []);

  const login = async (email: string, password: string) => {
    setIsLoading(true);
    try {
      const response = await authAPI.login({ email, password });
      const token = typeof response.data === 'string' ? response.data : response.data.token;
      if (!token) {
        throw new Error('Invalid login response');
      }
      localStorage.setItem('authToken', token);
      const userResponse = await authAPI.getMe();
      setUser(userResponse.data);
    } finally {
      setIsLoading(false);
    }
  };

  const register = async (firstName: string, lastName: string, email: string, password: string, phoneNumber?: string) => {
    setIsLoading(true);
    try {
      const response = await authAPI.register({ firstName, lastName, email, password, phoneNumber });
      const token = typeof response.data === 'string' ? response.data : response.data.token;
      if (!token) {
        throw new Error('Invalid registration response');
      }
      localStorage.setItem('authToken', token);
      const userResponse = await authAPI.getMe();
      setUser(userResponse.data);
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem('authToken');
    setUser(null);
  };

  const refetchUser = async () => {
    try {
      const response = await authAPI.getMe();
      setUser(response.data);
    } catch {
      logout();
    }
  };

  const value: AuthContextType = {
    user,
    isLoading,
    isAuthenticated: !!user,
    login,
    register,
    logout,
    refetchUser,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
