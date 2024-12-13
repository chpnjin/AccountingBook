<template>
  <div id="app">
    <header class="header">
      <div class="user-info">
        <span>登入者 : {{ username }}</span>
        <button @click="logout">登出</button>
      </div>
    </header>

    <div class="layout">
      <nav class="sidebar">
        <ul>
          <li>
            <a href="#" class="nav-item" @click.prevent="currentView = 'Home'">Home</a>
          </li>
          <li>
            <a href="#" class="nav-item" @click.prevent="currentView = 'About'">About</a>
          </li>
        </ul>
      </nav>
      <!-- 實際內容 -->
      <main class="content">
        <component :is="currentView" />
      </main>

    </div>
  </div>
</template>

<script>
import Home from '../views/Home.vue';
import About from '../views/About.vue';

export default {
  data() {
    return {
      currentView: 'Home',
    };
  },
  computed: {
    username(){
      return this.$store.state.name; //從Vuex中取得name
    }
  },
  components: {
    Home,
    About,
  },
  methods: {
    logout() {
      // 登出後清除 Vuex 中的 name 和其他登入狀態
      this.$store.commit('setUserName', '');  // 清除名稱
      this.$store.commit('setUserId', null);  // 清除使用者ID（如果有的話）

      console.log("Logged out");
      this.$router.push('/login');  // 可以選擇導向到登入頁
    },
  },
};
</script>

<style scoped>
#app {
  display: flex;
  flex-direction: column;
  height: 100vh;
  width: 100vw;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.header {
  background: #2c3e50;
  color: #fff;
  padding: 10px;
  display: flex;
  justify-content: flex-end;
  align-items: center;
  box-sizing: border-box;
}

.layout {
  display: flex;
  flex: 1;
  overflow: hidden;
  width: 100%;
}

.sidebar {
  width: 250px;
  background: #34495e;
  color: #ecf0f1;
  padding: 0;
  box-sizing: border-box;
  overflow-y: auto;
  height: 100%;
}

.nav-item {
  font-weight: bold;
  margin: 5px 0;
  padding: 10px;
  box-sizing: border-box;
}

.content {
  flex: 1;
  background: #ecf0f1;
  box-sizing: border-box;
  overflow-y: auto;
  padding: 10px;
  height: 100%;
}

button {
  background: #e74c3c;
  border: none;
  color: #fff;
  padding: 5px 10px;
  cursor: pointer;
}

button:hover {
  background: #c0392b;
}
</style>
