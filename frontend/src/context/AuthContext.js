import { jsx as _jsx } from "react/jsx-runtime";
import { createContext, useContext, useState, useEffect } from 'react';
import { authAPI } from '../api/client';
const AuthContext = createContext(undefined);
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within AuthProvider');
    }
    return context;
};
export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    // Check if user is already logged in on mount
    useEffect(() => {
        const initAuth = async () => {
            const token = localStorage.getItem('authToken');
            if (token) {
                try {
                    const response = await authAPI.getMe();
                    setUser(response.data);
                }
                catch {
                    localStorage.removeItem('authToken');
                    setUser(null);
                }
            }
            setIsLoading(false);
        };
        initAuth();
    }, []);
    const login = async (email, password) => {
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
        }
        finally {
            setIsLoading(false);
        }
    };
    const register = async (firstName, lastName, email, password, phoneNumber) => {
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
        }
        finally {
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
        }
        catch {
            logout();
        }
    };
    const value = {
        user,
        isLoading,
        isAuthenticated: !!user,
        login,
        register,
        logout,
        refetchUser,
    };
    return _jsx(AuthContext.Provider, { value: value, children: children });
};
//# sourceMappingURL=AuthContext.js.map