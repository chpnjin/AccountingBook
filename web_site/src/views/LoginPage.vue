<template>
  <div class="login-page">
    <h1>登入</h1>
    <form @submit.prevent="handleSubmit">
      <input type="text" v-model="username" placeholder="帳號" />
      <input type="password" v-model="password" placeholder="密碼" />
      <button type="submit">登入</button>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      username: '',
      password: ''
    };
  },
  methods: {
    handleSubmit() { //捕捉按下button type="submit"之後的事件

      //測試用,隨意輸入姓名後將值帶入主頁
      if (this.username.trim()) {
        // 儲存使用者資訊至 Vuex
        this.$store.commit('setUserId', this.username); // 設定 user_id
        this.$store.commit('setUserName', this.username); // 設定 name（可選）

        // 導向主頁
        this.$router.push('/');
      } else {
        alert('請輸入有效的帳號');
      }

      // 這邊會發送請求到後端驗證帳號密碼
      // 例如：
      // axios.post('/api/login', { username: this.username, password: this.password })
      //   .then(response => {
      //     // 登入成功，導向首頁
      //     this.$router.push('/');
      //   })
      //   .catch(error => {
      //     // 登入失敗，顯示錯誤訊息
      //     console.error(error);
      //   });
    }
  }
}
</script>

<style scoped>
/* 特定登入頁樣式，確保不受全域影響 */
.login-page {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh; /* 全螢幕高度 */
  background-color: #282c34;
  color: white;
}

form {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 15px;
}

input {
  padding: 10px;
  width: 250px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

button {
  padding: 10px 20px;
  font-size: 16px;
  color: white;
  background-color: #007bff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}
</style>
