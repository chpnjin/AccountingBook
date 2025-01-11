<template>
  <div class="login-page">
    <h1>塵世會計系統</h1>
    <form @submit.prevent="handleSubmit">
      <input type="text" v-model="id" placeholder="帳號" />
      <input type="password" v-model="password" placeholder="密碼" />
      <button @click="btnLogin_click">登入</button>
    </form>
    <p style="height: 20px"></p>
    <!-- 新增測試按鈕 -->
    <button @click="test">單元測試按鈕</button>
    <span id="msg">{{ message }}</span>
  </div>
</template>

<script>
import UserService from "@/services/userService"; //引用服務層

export default {
  data() {
    return {
      id: "",
      password: "",
      message: "",
    };
  },
  methods: {
    async btnLogin_click() {
      try {
        const loginParams = {
          id: this.id,
          password: this.password,
        };

        // 檢查是否有輸入用戶名和密碼
        if (
          loginParams.id.trim() === "" ||
          loginParams.password.trim() === ""
        ) {
          this.message = "請輸入有效的帳號和密碼";
          return;
        }

        this.message = "登入中...";
        UserService.loginVerification(loginParams)
          .then((response) => {
            //檢查結果是否包含token
            if (response.token) {
              //儲存token備用
              localStorage.setItem("token", response.token);
              localStorage.setItem("id", response.id);
              localStorage.setItem("name", response.name);
              // 導向主頁
              this.message = "登入成功，準備導向主頁...";
              this.$router.push("/");
            } else {
              this.message = response.msg;
            }
          })
          .catch((error) => {
            this.message = "登錄失敗，請稍後重試(" + error + ")";
          });
      } catch (error) {
        this.message = "登錄失敗，請稍後重試(" + error + ")";
      }
    },
    //單元測試按鈕
    async test() {
      console.log(import.meta.env);
    },
  },
};
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
  background-color: #fff; /* 設定背景顏色為白色，與 Firefox 預設一致 */
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

msg {
  color: aqua;
}
</style>
