<template>
  <div class="main-layout">
    <header class="header">
      <router-link to="/" class="home-button">首頁</router-link> <div class="user-info"></div>
      <div class="user-info">
        <span>登入者 : {{ username }}</span>
        <button @click="logout">登出</button>
      </div>
    </header>
    <div class="layout">
      <!-- 側邊欄 -->
      <SidebarNav class="sidebar" />

      <!-- 內容區域 -->
      <main class="content">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script>
import SidebarNav from "@/components/SidebarNav.vue";

export default {
  name: "MainLayout",
  components: {
    SidebarNav,
  },
  data() {
    return {
      username: localStorage.getItem("name") || "未知用戶",
    };
  },
  methods: {
    logout() {
      localStorage.clear();
      this.$router.push("/login");
    },
  },
};
</script>

<style scoped>
.main-layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
  width: 100vw;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.header {
  display: flex; /* 使用 Flexbox 排版 */
  background: #2c3e50;
  color: #fff;
  padding: 10px 20px; /* 增加內邊距 */
  justify-content: space-between; /* 將內容推到右邊 */
  align-items: end; /* 垂直居中 */
  box-sizing: border-box;
}

.home-button {
  background: #3498db; /* 或其他你喜歡的顏色 */
  border: none;
  color: #fff;
  padding: 5px 10px;
  cursor: pointer;
  border-radius: 4px;
  text-decoration: none; /* 移除連結底線 */
  margin-right: 10px;
  font-size: 18px;
}

.user-info {
  display: flex;
  align-items: end;
  gap: 10px; /* 文字和按鈕之間的間距 */
}

.user-info span {
  font-size: 20px; /* 調整字體大小 */
}

button {
  background: #e74c3c;
  border: none;
  color: #fff;
  padding: 5px 10px;
  cursor: pointer;
  border-radius: 4px; /* 圓角按鈕 */
  font-size: 18px;
}

.layout {
  display: flex;
  flex: 1;
  overflow: hidden;
  width: 100%;
}

.sidebar {
  width: 200px;
  background: #34495e;
  color: #ecf0f1;
  padding: 0;
  box-sizing: border-box;
  overflow-y: auto;
  height: 100%;
}

.content {
  flex: 1;
  background: #ecf0f1;
  box-sizing: border-box;
  overflow-y: auto;
  padding: 10px;
  height: 100%;
}
</style>
