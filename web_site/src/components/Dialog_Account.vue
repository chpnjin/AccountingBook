<template>
  <div v-if="visible" class="dialog-overlay">
    <div class="dialog">
      <div class="dialog-header">
        <h2>{{ title }}</h2>
        <!-- click動作綁定Method : closeDialog -->
        <button @click="closeDialog" class="close-button">&times;</button>
      </div>
      <div class="dialog-body">
        <!--  -->
        <input type="hidden" v-model="account.id" />
        <div class="form-group" :class="{ 'has-error': noError }">
          <label>科目編號</label>
          <input type="text" v-model="account.no" />
          <div v-if="noError" class="error-message">
            {{ noErrorMessage }}
          </div>
        </div>
        <div class="form-group" :class="{ 'has-error': nameError }">
          <label>科目名稱</label>
          <input type="text" v-model="account.name" />
          <div v-if="nameError" class="error-message">
            {{ nameErrorMessage }}
          </div>
        </div>
        <div class="form-group">
          <label for="select-option">科目類別：</label>
          <select id="select-option" v-model="account.type">
            <option
              v-for="option in typeList"
              :key="option.value"
              :value="option.value"
            >
              {{ option.title }}
            </option>
          </select>
        </div>
        <div class="form-group" :class="{ 'has-error': descriptionError }">
          <label for="description">科目描述：</label>
          <textarea id="description" v-model="account.description"></textarea>
          <div v-if="descriptionError" class="error-message">
            {{ descriptionErrorMessage }}
          </div>
        </div>
      </div>
      <div class="button-container">
        <button @click="closeDialog">取消</button>
        <button @click="saveAccount">確認</button>
      </div>
    </div>
  </div>
</template>

<script setup>
/*
ref         :用來創建一個響應式引用資料型態
reactive    :處裡Proxy 物件
watch       :用來監聽響應式數據的變化，並在變化發生時執行相應的副作用函數
defineProps :用於定義從父組件傳入子組件的屬性
defineEmits :用來定義組件可以發射的事件，這些事件可以被父組件監聽和處理
onMounted   :組件初次載入時觸發
*/
import {
  ref,
  reactive,
  watch,
  defineProps,
  defineEmits,
  onMounted,
  toRaw,
} from "vue";

import accountService from "@/services/accountService"; //API呼叫服務

//定義屬性,可以在父組件叫用('v-model:'或':')
const props = defineProps({
  title: {
    type: String,
    default: "新增主科目",
  },
  //視窗是否可見
  visible: {
    type: Boolean,
    default: false,
  },
  //用於編輯既有科目(在父組件改名input-data)
  inputData: {
    type: Object,
    default: () => ({
      id: "",
      no: 0,
      name: "",
      type: "Asset",
      description: "",
    }),
  },
});

//定義事件
const emit = defineEmits(["save", "close"]);

const account = reactive({
  id: null,
  no: "",
  name: "",
  type: "Asset",
  description: "",
});

//欄位驗證用
const noError = ref(false);
const nameError = ref(false);
const descriptionError = ref(false);
const noErrorMessage = ref("");
const nameErrorMessage = ref("");
const descriptionErrorMessage = ref("");

const typeList = ref([
  { title: "資產", value: "Asset" },
  { title: "負債", value: "Liability" },
  { title: "權益", value: "Equity" },
  { title: "收入", value: "Revenue" },
  { title: "費用", value: "Expense" },
]);

const resetAccount = () => {
  // 使用 Object.assign 更新 account 物件，避免整個物件被替換
  Object.assign(account, {
    id: null,
    no: "",
    name: "",
    type: "Asset",
    description: "",
  });
  resetErrorMessages();
};

// 重置錯誤訊息
const resetErrorMessages = () => {
  noError.value = false;
  nameError.value = false;
  descriptionError.value = false;
  noErrorMessage.value = "";
  nameErrorMessage.value = "";
  descriptionErrorMessage.value = "";
};

// 使用 onMounted 確保組件掛載後再處理 props.inputData
onMounted(() => {
  resetAccount(props.inputData);
});

//每次開窗時檢查是否有傳入資料
watch(
  () => props.inputData,
  (newValue) => {
    if (Object.keys(newValue).length == 0) {
      resetAccount();
    } else {
      Object.assign(account, newValue);
    }
  },
  { immediate: true }
);

watch(
  () => account.no,
  () => {
    noError.value = false;
    noErrorMessage.value = "";
  },
  { deep: true }
);

watch(
  () => account.name,
  () => {
    nameError.value = false;
    nameErrorMessage.value = "";
  }
);

watch(
  () => account.description,
  () => {
    descriptionError.value = false;
    descriptionErrorMessage.value = "";
  }
);

// 關閉對話框
const closeDialog = () => {
  //取消編輯時回填原始資料
  if (account.id != null) {
    Object.assign(account, props.inputData);
  }
  emit("close"); // 觸發對話框關閉事件
};

// 保存科目
const saveAccount = async () => {
  noError.value = !account.no.trim();
  nameError.value = !account.name.trim();
  descriptionError.value = !account.description.trim();

  noErrorMessage.value = noError.value ? "科目編號不得為空" : "";
  nameErrorMessage.value = nameError.value ? "科目名稱不得為空" : "";
  descriptionErrorMessage.value = descriptionError.value
    ? "科目描述不得為空"
    : "";

  if (noError.value || nameError.value || descriptionError.value) {
    return;
  }

  if (props.title.includes("新增")) {
    let idExist = await accountService.accountIdExist(account.no);

    if (idExist) {
      noError.value = true;
      noErrorMessage.value = "該科目編號已存在";
      return;
    }
  }

  //觸發事件:save
  emit("save", toRaw(account));
  closeDialog();
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
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  min-width: 500px;
}

.dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
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
  display: block;
  margin-bottom: 5px;
}

textarea {
  width: 100%;
  height: 100px;
  resize: vertical;
}

input[type="text"],
select,
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

.close-button {
  font-size: 20px;
  cursor: pointer;
  border: none;
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
