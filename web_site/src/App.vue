<template>
  <div id="app">
    <router-view v-if="isLoggedIn" />
    <LoginPage v-else @login_success="handleLoginSuccess" />
    <!-- 未登入時顯示登入頁 -->
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import LoginPage from "@/views/LoginPage.vue";

const router = useRouter();
const isLoggedIn = ref(false); //判斷是否已成功登入

onMounted(() => {
  isLoggedIn.value = localStorage.getItem("token") !== null;
});

const handleLoginSuccess = () => {
  isLoggedIn.value = true;
  router.push("/");
};
</script>

<style>
/* App 全域樣式 */
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  margin: 0;
}
</style>
