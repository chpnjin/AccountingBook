import { createRouter, createWebHistory } from 'vue-router';
import store from '../store'; // 使用 Vuex 管理狀態
import Main from '@/layouts/MainLayout.vue';

import Login from '@/views/LoginPage.vue';
import JE_Edit from '@/views/je-edit.vue';
import JE_Approval from '@/views/je-approval.vue';
import Report_IncomeStatement from '@/views/report-income-statement.vue';
import Report_BalanceSheet from '@/views/report-balance-sheet.vue';
import Setup_User from '@/views/setup-user.vue';
import Setup_Accounts from '@/views/setup-accounts.vue';


//定義路由
const routes = [
  {
    path: '/',
    name: 'Main',
    component: Main,
    children:[
      { path: 'je-edit', name: 'je-edit', component: JE_Edit },
      { path: 'je-approval', name: 'je-approval', component: JE_Approval },
      { path: 'report-income-statement', name: 'report-income-statement', component: Report_IncomeStatement },
      { path: 'report-balance-sheet', name: 'report-balance-sheet', component: Report_BalanceSheet },
      { path: 'setup-user', name: 'setup-user', component: Setup_User },
      { path: 'setup-accounts', name: 'setup-accounts', component: Setup_Accounts }
    ],
    meta: { requiresAuth: true } //需要驗證
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
router.beforeEach(async (to, from, next) => {
  // 從 Vuex 取得登入狀態
  const isAuthenticated = await verifyAuth();

  //若此路由需要驗證且無驗證資訊時,導向登入頁
  if (to.meta.requiresAuth && (!isAuthenticated)) {
    next('/login');
  } else {
    next();
  }
});

// 驗證函式（模擬異步操作）
async function verifyAuth() {
  try {
    // 檢查 Vuex 是否有已登入的使用者 ID
    const userId = store.getters['user_id'];

    // 若有 ID，直接回傳 true
    if (userId){
      return true;
    }else{
      return false;
    }

    // // 若無 ID，可發送 API 請求確認登入狀態
    // const response = await store.dispatch('fetchUserSession');
    // return !!response.user_id; // 回傳是否有有效的 user_id
  } catch (error) {
    console.error('驗證失敗：', error);
    return false; // 若發生錯誤，視為未驗證
  }
}

export default router;
