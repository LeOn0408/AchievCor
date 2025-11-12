import { createWebHistory, createRouter } from 'vue-router'
import HomeView from './views/HomeView.vue'
import LoginView from './views/LoginView.vue'
import NotFoundView from './views/NotFoundView.vue'
import { useAuthStore } from './stores'

const router = createRouter({
  history: createWebHistory(),
  routes:[
    {
      path: '',
      component: HomeView
    },
    {
      path: '/login',
      component: LoginView
    },
    {
      path: '/profile',
      component: NotFoundView
    },
    {
      path: '/:pathMatch(.*)*',
      component: NotFoundView
    }
  ]
})
router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()
  if (!authStore.isInitialized)
  {
    await authStore.init()
  }

  // Если пользователь неавторизован — отправляем на логин
  if (!authStore.isAuthenticated && to.path !== '/login') {
    return next({ path: '/login', query: { redirect: to.fullPath } })
  }

  next()
})

export default router;
