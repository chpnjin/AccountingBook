<template>
  <div id="app">
    <router-view v-if="isAuthenticated" />
    <LoginPage v-else /> <!-- 未登入時顯示登入頁 -->
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import LoginPage from '@/views/LoginPage.vue';

export default {
  components: {
    LoginPage,
  },
  computed: {
    ...mapGetters(['user_id']),
    isAuthenticated() {
      return !!this.user_id; // 判斷是否已登入
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
