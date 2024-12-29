<template>
  <div v-if="visible" class="dialog-overlay">
    <div class="dialog">
      <div class="dialog-header">
        <h2>{{ dialogTitle }}</h2>
        <!-- 右上關閉按鈕 -->
        <button @click="closeDialog" class="close-button">&times;</button>
      </div>
      <div class="dialog-body">
        <div class="form-group">
          <label>父科目名稱：</label>
          <span>{{ parentAccount?.name }}</span>
        </div>
        <div class="form-group">
          <label>父科目類型：</label>
          <span>{{ parentAccount?.type }}</span>
        </div>
        <div class="form-group" :class="{ 'has-error': noError }">
          <label for="subAccountNo">子科目編號：</label>
          <input
            type="text"
            id="subAccountNo"
            v-model="localEditingAccount.no"
          />
          <div v-if="noError" class="error-message">
            {{ noErrorMessage }}
          </div>
        </div>
        <div class="form-group" :class="{ 'has-error': nameError }">
          <label for="subAccountName">子科目名稱：</label>
          <input
            type="text"
            id="subAccountName"
            v-model="localEditingAccount.name"
          />
          <div v-if="nameError" class="error-message">
            {{ nameErrorMessage }}
          </div>
        </div>
        <div class="form-group" :class="{ 'has-error': descriptionError }">
          <label for="subAccountDescription">描述：</label>
          <textarea
            id="subAccountDescription"
            v-model="localEditingAccount.description"
          ></textarea>
          <div v-if="descriptionError" class="error-message">
            {{ descriptionErrorMessage }}
          </div>
        </div>
        <div class="button-container">
          <button @click="closeDialog">取消</button>
          <button @click="saveAccount">確認</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, watch, onMounted } from "vue";

//定義可在父組件設定屬性
const props = defineProps({
  //視窗是否可見
  visible: {
    type: Boolean,
    default: false,
  },
  parentAccount: Object, //傳入的主科目資料
  editingAccount: Object, //用於編輯既有科目
});

//定義事件
const emit = defineEmits(["save", "close", "update:visible"]);

const dialogTitle = ref("新增子科目");

const noError = ref(false);
const nameError = ref(false);
const descriptionError = ref(false);
const noErrorMessage = ref("");
const nameErrorMessage = ref("");
const descriptionErrorMessage = ref("");

const localEditingAccount = ref({}); //紀錄子科目輸入值

watch(
  //同步更新編輯中的科目資料
  () => props.editingAccount,
  (newValue) => {
    if (newValue) {
      resetForm();
      localEditingAccount.value = { ...newValue };
    }
  }
);

//初始化資料,帶入初始值
onMounted(async () => {
  if (Object.keys(localEditingAccount.value).length === 0 ) {
    localEditingAccount.value.id = 0;
    localEditingAccount.value.no = "";
    localEditingAccount.value.name = "";
    localEditingAccount.value.description = "";
  }
});

//清除未驗證資訊
const resetForm = () => {
  //重置表單
  localEditingAccount.value.no = "";
  localEditingAccount.value.name = "";
  localEditingAccount.value.description = "";

  //重置錯誤訊息
  noError.value = false;
  nameError.value = false;
  descriptionError.value = false;
  nameErrorMessage.value = "";
  descriptionErrorMessage.value = "";
};

//關閉彈窗
const closeDialog = () => {
  emit("update:visible", false); //視窗visible屬性
  emit("close"); // 觸發對話框關閉事件
  resetForm(); // 關閉時重置表單和錯誤訊息
};

const saveAccount = () => {
  noError.value = !localEditingAccount.value.no.trim();
  nameError.value = !localEditingAccount.value.name.trim();
  descriptionError.value = !localEditingAccount.value.description.trim();

  noErrorMessage.value = noError.value ? "子科目編號不得為空" : "";
  nameErrorMessage.value = nameError.value ? "子科目名稱不得為空" : "";
  descriptionErrorMessage.value = descriptionError.value ? "描述不得為空" : "";

  if (noError.value || nameError.value || descriptionError.value) {
    return;
  }

  //回填主科目id與類型
  localEditingAccount.value.main_id = props.parentAccount.id;
  localEditingAccount.value.type = props.parentAccount.type;
  const jsonObj = JSON.parse(JSON.stringify(localEditingAccount.value));

  emit("save", jsonObj); //觸發save事件
  closeDialog();
};
</script>

<style scoped>
.dialog-overlay {
  position: fixed; /* 讓對話框固定在畫面中央 */
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); /* 半透明背景 */
  display: flex;
  justify-content: center; /* 水平置中 */
  align-items: center; /* 垂直置中 */
  z-index: 1000; /* 確保在最上層 */
}
/* 整體寬度 */
.dialog {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  width: 500px; /* 設定對話框寬度 */
}

.dialog-header {
  display: flex;
  justify-content: space-between; /* 標題與關閉按鈕分開 */
  align-items: center;
  border-bottom: 1px solid #eee;
  margin-bottom: 10px;
}

.close-button {
  font-size: 20px;
  cursor: pointer;
  border: none;
}

.form-group {
  margin-bottom: 10px;
}

label {
  display: block; /* 讓 label 垂直排列 */
  margin-bottom: 5px;
}

textarea {
  width: 100%;
  height: 100px;
  resize: vertical; /* 允許垂直調整大小 */
}

input[type="text"],
textarea {
  width: calc(100% - 12px);
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

button {
  margin-top: 10px;
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  background-color: #42b983; /* Vue 的綠色 */
  color: white;
}

button:hover {
  background-color: #38a372;
}

.button-container {
  display: flex;
  justify-content: flex-end; /* 靠右對齊 */
  margin-top: 10px; /* 增加按鈕區塊與上方表單的間距 */
}

.button-container button {
  margin-left: 10px; /* 按鈕間隔 */
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

/* 錯誤訊息樣式 */
.has-error input,
.has-error select,
.has-error textarea {
  border-color: red;
  box-shadow: 0 0 5px rgba(255, 0, 0, 0.2);
}

.error-message {
  color: red;
  font-size: 0.8em;
  margin-top: 5px;
}
</style>
