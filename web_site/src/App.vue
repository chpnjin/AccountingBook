<template>
  <div id="app">
    <router-view v-if="isAuthenticated" />
    <LoginPage v-else /> <!-- 未登入時顯示登入頁 -->
  </div>
</template>

<script>
import LoginPage from '@/views/LoginPage.vue';

export default {
  components: {
    LoginPage,
  },
  computed: {
    isAuthenticated() {
      return localStorage.getItem('token') !== null; // 檢查localStorage中是否存在token
    },
  },
};
</script>

<style>
/* App 全域樣式 */
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  margin: 0;
}
</style>
