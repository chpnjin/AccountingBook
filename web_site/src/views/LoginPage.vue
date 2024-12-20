<template>
  <div class="login-page">
    <h1>塵世會計系統</h1>
    <form @submit.prevent="handleSubmit">
      <input type="text" v-model="username" placeholder="帳號" />
      <input type="password" v-model="password" placeholder="密碼" />
      <button type="submit">登入</button>
    </form>
  </div>
</template>

<script>
  import UserService from '@/services/userService'; //引用服務層

  export default {
    data() {
      return {
        username: '',
        password: ''
      };
    },
    methods: {
      //捕捉按下button type="submit"之後的事件
      async handleSubmit() {
        try {
          // 檢查是否有輸入用戶名和密碼
          if (this.username.trim() === '' || this.password.trim() === '') {
            alert('請輸入有效的帳號和密碼');
            return;
          }

          const user = await UserService.getUser(this.username, this.password);

          // 檢查回傳的用戶數據是否包含必要的字段
          if (user && user.id && user.name) {
            // 儲存使用者資訊至 Vuex
            this.$store.commit('setUserId', user.id); // 設定 user_id
            this.$store.commit('setUserName', user.name); // 設定 name

            // 導向主頁
            this.$router.push('/');
          } else {
            alert('登錄失敗，請檢查您的帳號或密碼');
          }
        } catch (error) {
          console.error('用戶登錄失敗:', error);
          alert('登錄失敗，請稍後重試');
        }
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
