import { createWebHistory, createRouter } from 'vue-router'
import HomeView from './views/HomeView.vue'
import LoginView from './views/LoginView.vue'
import NotFoundView from './views/NotFoundView.vue'

export default createRouter({
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
