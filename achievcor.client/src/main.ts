import './assets/main.css'
import './scss/styles.scss'
import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router'
import { useAuthStore } from './stores'

const pinia = createPinia()

const app = createApp(App);
app.use(router);
app.use(pinia);

const authStore = useAuthStore()
authStore.init()

app.mount('#app');
