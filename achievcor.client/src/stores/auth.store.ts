import { defineStore } from 'pinia'
import { createAxios } from '@/lib/axios.factory';
import type { AxiosInstance } from 'axios';
import type { UserInfoDto } from '@/dtos/user.info.dto';
import router from '@/router';
import type { AuthResponseDto } from '../dtos/auth.response.dto';

export const useAuthStore = defineStore('auth',
  {
    state: () => {
      return {
        user: null as UserInfoDto | null,
        jwtToken: null as string | null,
        tokenExpiryDate: null as string | null,
        isInitialized: false
      }
    },
    getters: {
      isAuthenticated(state) {
        if (!state.jwtToken || !state.tokenExpiryDate) return false
        const expiry = new Date(state.tokenExpiryDate)
        return expiry > new Date()
      }
    },
    actions: {
      async login(username: string, password: string)
      {
          try
          {
            const axios: AxiosInstance = createAxios();
            const result = await axios.post("/api/auth/authenticate", {
              username,
              password
            });

            let authData: AuthResponseDto = {
              user: result.data.user,
              jwtToken: result.data.jwtToken,
              tokenExpiryDate: result.data.tokenExpiryDate
            }
            this.setAuthData(authData);

            const redirectPath = (router.currentRoute.value.query.redirect as string) || '/'
            router.push(redirectPath)
          }
          catch (error: any)
          {
            console.error(error);
          }
        
      },

      setAuthData(authData: AuthResponseDto)
      {
        this.user = authData.user
        this.jwtToken = authData.jwtToken
        this.tokenExpiryDate = authData.tokenExpiryDate
        localStorage.setItem('user', JSON.stringify(authData.user))
        localStorage.setItem('jwtToken', authData.jwtToken)
        localStorage.setItem('tokenExpiryDate', authData.tokenExpiryDate)
      },

      async init()
      {
        const tokenExpiryDate = localStorage.getItem('tokenExpiryDate');
        const userStr = localStorage.getItem('user');
        const jwtToken = localStorage.getItem('jwtToken');

        if (!tokenExpiryDate || !userStr || !jwtToken) {
          this.isInitialized = true
          return;
        }
        const expiry = new Date(tokenExpiryDate);
        const now = new Date();

        if (expiry <= new Date(now.getTime() + 60000)) {
          await this.handleTokenRefresh();
          this.isInitialized = true
          return;
        }
        const axios: AxiosInstance = createAxios();
        try
        {
          await axios.get("/api/auth/validate");

          this.user = JSON.parse(userStr);
          this.jwtToken = jwtToken;
          this.tokenExpiryDate = tokenExpiryDate;
        }
        catch (error:any)
        {
          if (error.response?.status === 401) {
            await this.handleTokenRefresh();
          } else {
            await this.logout();
          }
        }
        finally
        {
          this.isInitialized = true
        }
      },

      async handleTokenRefresh() {
        try {
          const axiosInstance = createAxios();
          const response = await axiosInstance.post("/api/auth/refresh-token");

          const authData: AuthResponseDto = {
            user: response.data.user,
            tokenExpiryDate: response.data.tokenExpiryDate,
            jwtToken: response.data.jwtToken
          };

          this.setAuthData(authData);
        } catch (refreshError) {
          await this.logout();
        }
      },

      async logout() {
        this.user = null;
        this.jwtToken = null;
        this.tokenExpiryDate = null;

        localStorage.removeItem('user');
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('tokenExpiryDate');
        if (router.currentRoute.value.path !== '/login') {
          router.push('/login')
        }
      }
    }
  }
)
