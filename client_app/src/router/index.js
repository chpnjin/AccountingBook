//引用vue-router套件
import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/home.vue'


const routes = [
  {
    path: '/',
    name: 'home',
    component: Home
  },
  {
    path: '/query',
    name: 'query',
    component: () => import('../views/query.vue')
  },
  {
    path: '/edit',
    name: 'edit',
    component: () => import('../views/edit.vue')
  },
  {
    path: '/settings',
    name: 'settings',
    component: () => import('../views/settings.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes:routes
})

export default router