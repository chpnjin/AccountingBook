<template>
  <div class="main-layout">
    <header class="header">
      <div>
        <img src="@/assets/menu.svg" class="icon-menu" @click="toggleSidebar" />
        <router-link to="/" class="home-button">首頁</router-link>
        <div class="user-info"></div>
      </div>
      <div class="user-info">
        <span>登入者 : {{ username }}</span>
        <button @click="logout">登出</button>
      </div>
    </header>
    <div class="layout">
      <!-- 側邊欄 -->
      <SidebarNav class="sidebar" v-show="showSidebar" />

      <!-- 內容區域 -->
      <main class="content" @click="hideSidebar">
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
      showSidebar: false,
    };
  },
  methods: {
    logout() {
      localStorage.clear();
      this.$router.push("/login");
    },
    //切換隱藏／顯示側邊欄
    toggleSidebar() {
      this.showSidebar = !this.showSidebar;
      // 取得sidebar元素
      const sidebar = document.querySelector(".sidebar");
      if (this.showSidebar) {
        sidebar.classList.add("show");
      } else {
        sidebar.classList.remove("show");
      }
    },
    hideSidebar() {
      this.showSidebar = false;
      const sidebar = document.querySelector(".sidebar");
      if (sidebar && sidebar.classList.contains("show")) {
        sidebar.classList.remove("show");
      }
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
  position: relative; /* 重要的：設定相對定位，作為 sidebar 的定位參考點 */
}

.header {
  display: flex; /* 使用 Flexbox 排版 */
  background: #2c3e50;
  color: #fff;
  padding: 10px 20px; /* 增加內邊距 */
  justify-content: space-between; /* 將內容推到右邊 */
  align-items: end; /* 垂直居中 */
  box-sizing: border-box;
  height: 60px; /* 假設 header 的高度為 60px */
}

.icon-menu {
  width: 30px;
  background: #fff;
  margin-right: 10px;
  background-color: transparent;
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
  position: absolute; /* 或 position: fixed; */
  top: 60px; /* 設定 top 值為 header 的高度 */
  left: 0;
  height: calc(100% - 60px); /* 高度扣除 header 的高度 */
  width: 200px;
  background: #34495e;
  color: #ecf0f1;
  padding: 0;
  box-sizing: border-box;
  overflow-y: auto;
  z-index: 10; /* 確保 sidebar 在最上層 */
  transition: transform 0.3s ease-in-out; /* 加入動畫效果 */
  transform: translateX(-100%); /* 預設隱藏在左側 */
}

.sidebar.show {
  transform: translateX(0); /* 顯示時移回原位 */
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
