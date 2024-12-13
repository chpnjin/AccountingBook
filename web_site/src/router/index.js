import { createRouter, createWebHistory } from 'vue-router';
import store from '../store'; // 使用 Vuex 管理狀態
import Home from '@/views/Home.vue';
import About from '@/views/About.vue';
import Login from '@/views/LoginPage.vue';


//定義路由
const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: { requiresAuth: true } //需要驗證
  },
  {
    path: '/about',
    name: 'About',
    component: About,
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  }
];
//創建路由實例
const router = createRouter({
  history: createWebHistory(),
  routes,
});

//導航守衛-每次進入任何一個路由前，都會作呼叫。
router.beforeEach((to, from, next) => {
  // 從 Vuex 取得登入狀態
  const id = store.getters("user_id");

  //若此路由需要驗證且無驗證資訊時,導向登入頁
  if (to.matched.some(record => record.meta.requiresAuth) && id == "" || id == null) {
    next('/login');
  } else {
    next();
  }
});

export default router;
