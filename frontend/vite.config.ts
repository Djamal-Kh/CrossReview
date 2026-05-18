import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000, // Порт вашего фронтенда
    proxy: {
      // Все запросы, которые начинаются с /api, пойдут на бэкенд
      '/api': {
        target: 'http://localhost:5171', 
        changeOrigin: true,
        secure: false,
      },
    },
  },
});