import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

export default defineConfig({
  plugins: [react()],
  base: '/', // Ensure this matches your custom path configuration
  build: {
    outDir: 'dist'
  },
  resolve: {
    alias: {
      '@': '/src' // Example alias
    }
  }
});
