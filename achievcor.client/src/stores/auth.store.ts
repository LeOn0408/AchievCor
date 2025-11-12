import { defineStore } from 'pinia'
import { createAxios } from '@/services/api';
import type { AxiosInstance } from 'axios';
import type { UserDto } from '@/dtos/user.dto';
import router from '@/router';

export const useAuthStore = defineStore('auth',
  {
    state: () => {
      return {
        user: null as UserDto | null,
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
            this.user = result.data.user;
            this.jwtToken = result.data.jwtToken;
            this.tokenExpiryDate = result.data.tokenExpiryDate;

            localStorage.setItem('user', JSON.stringify(result.data.user));
            localStorage.setItem('jwtToken', result.data.jwtToken);
            localStorage.setItem('tokenExpiryDate', result.data.tokenExpiryDate);

            const redirectPath = (router.currentRoute.value.query.redirect as string) || '/'
            router.push(redirectPath)
          }
          catch (error: any)
          {
            console.error(error);
          }
        
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
          this.logout();
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
        catch (error)
        {
          this.logout();
        }
        finally
        {
          this.isInitialized = true
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
