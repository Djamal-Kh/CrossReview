import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export const LoginPage: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [tab, setTab] = useState<'login' | 'register'>('login');

  const [registerData, setRegisterData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    phoneNumber: '',
  });

  const { login, register } = useAuth();
  const navigate = useNavigate();

  const handleLogin = async () => {
    if (!email || !password) {
      setError('Введите email и пароль');
      return;
    }
    setLoading(true);
    setError('');
    try {
      await login(email, password);
      navigate('/');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при входе');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async () => {
    if (!registerData.firstName || !registerData.lastName || !registerData.email || !registerData.password) {
      setError('Заполните все обязательные поля');
      return;
    }
    setLoading(true);
    setError('');
    try {
      await register(
        registerData.firstName,
        registerData.lastName,
        registerData.email,
        registerData.password,
        registerData.phoneNumber
      );
      setError('Регистрация успешна! Пожалуйста, войдите.');
      setTab('login');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при регистрации');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-wrap">
      <div className="auth-card fade-in">
        <div className="auth-logo">
          <div className="logo-icon">CR</div>
          <div>
            <div className="logo-name">CrossReview</div>
            <div className="logo-sub" style={{ fontSize: 10, color: 'var(--text3)' }}>
              Performance Review Platform
            </div>
          </div>
        </div>

        <div className="tabs" style={{ marginBottom: 24 }}>
          <button
            className={`tab ${tab === 'login' ? 'active' : ''}`}
            onClick={() => {
              setTab('login');
              setError('');
            }}
          >
            Вход
          </button>
          <button
            className={`tab ${tab === 'register' ? 'active' : ''}`}
            onClick={() => {
              setTab('register');
              setError('');
            }}
          >
            Регистрация
          </button>
        </div>

        {tab === 'login' ? (
          <>
            <div className="form-group">
              <label>Email</label>
              <input
                type="email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                placeholder="you@company.com"
              />
            </div>
            <div className="form-group">
              <label>Пароль</label>
              <input
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                placeholder="••••••••"
              />
            </div>
            {error && <p style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</p>}
            <button
              className="btn btn-primary"
              style={{ width: '100%', justifyContent: 'center', padding: '10px' }}
              onClick={handleLogin}
              disabled={loading}
            >
              {loading ? 'Входим…' : 'Войти'}
            </button>
          </>
        ) : (
          <>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
              <div className="form-group">
                <label>Имя</label>
                <input
                  placeholder="Иван"
                  value={registerData.firstName}
                  onChange={e => setRegisterData({ ...registerData, firstName: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Фамилия</label>
                <input
                  placeholder="Иванов"
                  value={registerData.lastName}
                  onChange={e => setRegisterData({ ...registerData, lastName: e.target.value })}
                />
              </div>
            </div>
            <div className="form-group">
              <label>Email</label>
              <input
                type="email"
                placeholder="ivan@company.com"
                value={registerData.email}
                onChange={e => setRegisterData({ ...registerData, email: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label>Телефон</label>
              <input
                placeholder="+7 (999) 000-00-00"
                value={registerData.phoneNumber}
                onChange={e => setRegisterData({ ...registerData, phoneNumber: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label>Пароль</label>
              <input
                type="password"
                placeholder="Минимум 8 символов"
                value={registerData.password}
                onChange={e => setRegisterData({ ...registerData, password: e.target.value })}
              />
            </div>
            {error && <p style={{ color: 'var(--danger)', fontSize: 12, marginBottom: 12 }}>{error}</p>}
            <button
              className="btn btn-primary"
              style={{ width: '100%', justifyContent: 'center', padding: '10px' }}
              onClick={handleRegister}
              disabled={loading}
            >
              {loading ? 'Создаём аккаунт…' : 'Создать аккаунт'}
            </button>
          </>
        )}
      </div>
    </div>
  );
};
