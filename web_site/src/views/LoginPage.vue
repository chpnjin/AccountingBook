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
    handleSubmit() {
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

<style>

</style>
