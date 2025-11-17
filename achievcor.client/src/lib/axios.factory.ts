import axios, { type AxiosInstance } from 'axios'
import type { AuthResponseDto } from '../dtos/auth.response.dto';
import { useAuthStore } from '@/stores'


export function createAxios(): AxiosInstance {
  const instance = axios.create();
  instance.defaults.headers.post['Content-Type'] = 'application/json';
  instance.interceptors.request.use(config => {

    config.headers.Authorization = getAuthHeader();
    return config;
  });
  return instance;
}

function getAuthHeader(): string {

  const token: string | null = localStorage.getItem('jwtToken');
  return token ? `Bearer ${token}` : '';
}
