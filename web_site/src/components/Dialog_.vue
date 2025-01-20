<!-- 彈窗模板 -->
<template>
  <!-- 外框 -->
  <div v-if="visible" class="dialog-overlay">
    <div class="dialog">
      <!-- 標題與關閉按鈕 -->
      <div class="dialog-header">
        <h2>{{ title }}</h2>
        <button @click="closeDialog" class="close-button">&times;</button>
      </div>
      <!-- 主要內容插槽 -->
      <div class="dialog-body">
        <slot></slot>
      </div>
      <!-- 按鈕區域 -->
      <div class="button-container">
        <slot name="buttons">
          <!-- 預設按鈕 -->
          <button @click="confirm">確認</button>
          <button @click="closeDialog">取消</button>
        </slot>
      </div>
    </div>
  </div>
</template>

<script setup>
//定義父組件可訂閱事件
const emit = defineEmits(["confirm", "close"]);
//定義父組件可設定屬性
const props = defineProps({
  visible: { type: Boolean, required: true }, // 控制顯示
  title: { type: String, default: "對話框" }, // 標題文字
});

const closeDialog = () => {
  emit("close"); // 通知父層關閉對話框
};

const confirm = () => {
  emit("confirm"); // 通知父層確認操作
};
</script>

<style scoped>
/* 顯示／隱藏屬性 */
.dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.dialog {
  background: white;
  border-radius: 8px;
  width: 400px;
  max-width: 90%;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.3);
  padding: 16px;
}

.dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}
/* 關閉按鈕 */
.close-button {
  font-size: 20px;
  cursor: pointer;
  border: none;
}

button {
  margin-top: 10px;
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  background-color: #4caf50;
  color: white;
}

button:hover {
  background-color: #38a372;
}

.button-container {
  display: flex;
  flex-direction: row-reverse;
  gap: 10px; /*按鈕間距*/
}

.button-container button {
  padding: 5px 20px;
  border: none; /* 移除預設邊框 */
  border-radius: 5px;
  background-color: #4caf50; /* 設定背景顏色 */
  color: white; /* 設定文字顏色 */
  cursor: pointer;
  font-size: 15px;
  transition: background-color 0.3s ease; /* 加入過渡效果 */
}
</style>
