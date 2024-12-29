<template>
  <div v-if="visible" class="dialog-overlay">
    <div class="dialog">
      <div class="dialog-header">
        <slot name="header">
          {{ title }}
        </slot>
        <!-- click動作綁定Method : closeDialog -->
        <button @click="closeDialog" class="close-button">&times;</button>
      </div>
      <div class="dialog-body">
        <!--  -->
        <input type="hidden" v-bind="editTargetId" />
        <div class="form-group" :class="{ 'has-error': noError }">
          <label for="input-text">科目編號</label>
          <input type="text" id="input-text" v-model="accountNo" />
          <div v-if="noError" class="error-message">
            {{ noErrorMessage }}
          </div>
        </div>
        <div class="form-group" :class="{ 'has-error': nameError }">
          <label for="input-text">科目名稱</label>
          <input type="text" id="input-text" v-model="accountName" />
          <div v-if="nameError" class="error-message">
            {{ nameErrorMessage }}
          </div>
        </div>
        <div class="form-group" :class="{ 'has-error': typeError }">
          <label for="select-option">科目類別：</label>
          <select id="select-option" v-model="accountType">
            <option
              v-for="option in typeList"
              :key="option.value"
              :value="option.value"
            >
              {{ option.title }}
            </option>
          </select>
          <div v-if="typeError" class="error-message">
            {{ typeErrorMessage }}
          </div>
        </div>
        <div class="form-group" :class="{ 'has-error': descriptionError }">
          <label for="description">科目描述：</label>
          <textarea id="description" v-model="accountDescription"></textarea>
          <div v-if="descriptionError" class="error-message">
            {{ descriptionErrorMessage }}
          </div>
        </div>
        <slot name="body"></slot>
      </div>
      <div class="dialog-footer">
        <slot name="footer">
          <button @click="closeDialog">取消</button>
          <button @click="saveAccount">確定</button>
        </slot>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, watch } from "vue";
/*
ref:         用來創建一個響應式引用資料型態
watch:       用來監聽響應式數據的變化，並在變化發生時執行相應的副作用函數
defineProps: 用於定義從父組件傳入子組件的屬性
defineEmits: 用來定義組件可以發射的事件，這些事件可以被父組件監聽和處理
*/

//定義屬性,可以在父組件叫用(v-model:)
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
  //用於編輯既有科目
  editTargetId: {
    type: Number,
    default: 0,
  },
});

//定義事件
const emit = defineEmits(["save", "close", "update:visible"]);

// 本地狀態
const accountNo = ref(""); //科目編號
const accountName = ref(""); //科目名稱
const accountType = ref(""); // 科目類型(預設資產)

const typeList = ref([
  { title: "資產", value: "Asset" },
  { title: "負債", value: "Liability" },
  { title: "權益", value: "Equity" },
  { title: "收入", value: "Revenue" },
  { title: "費用", value: "Expense" },
]);

const accountDescription = ref(""); //科目描述

const noError = ref(false);
const nameError = ref(false);
const typeError = ref(false);
const descriptionError = ref(false);
const noErrorMessage = ref("");
const nameErrorMessage = ref("");
const typeErrorMessage = ref("");
const descriptionErrorMessage = ref("");

// 監聽 visible prop 的變化，當 Dialog 開啟時設定初始值
watch(
  () => props.visible,
  (newValue) => {
    if (newValue) {
      resetForm();
    }
  }
);

// 監聽 accountNo 的變化
watch(accountNo, () => {
  noError.value = false; // 清除 nameError
  noError.value = "";
});

// 監聽 accountName 的變化
watch(accountName, () => {
  nameError.value = false; // 清除 nameError
  nameErrorMessage.value = "";
});

// 監聽 accountType 的變化
watch(accountType, () => {
  typeError.value = false; // 清除 typeError
  typeErrorMessage.value = "";
});

// 監聽 accountDescription 的變化
watch(accountDescription, () => {
  descriptionError.value = false; // 清除 descriptionError
  descriptionErrorMessage.value = "";
});

//清除未驗證資訊
const resetForm = () => {
  //重置表單
  accountNo.value = "";
  accountName.value = "";
  accountType.value = "";
  accountDescription.value = "";

  //重置錯誤訊息
  noError.value = false;
  nameError.value = false;
  typeError.value = false;
  descriptionError.value = false;
  nameErrorMessage.value = "";
  typeErrorMessage.value = "";
  descriptionErrorMessage.value = "";
};

// 關閉對話框
const closeDialog = () => {
  emit("update:visible", false); //視窗visible屬性
  emit("close"); // 觸發對話框關閉事件
  resetForm(); // 關閉時重置表單和錯誤訊息
};

// 保存科目
const saveAccount = () => {
  noError.value = !accountNo.value.trim();
  nameError.value = !accountName.value.trim();
  typeError.value = !accountType.value;
  descriptionError.value = !accountDescription.value.trim();

  noErrorMessage.value = noError.value ? "科目編號不得為空" : "";
  nameErrorMessage.value = nameError.value ? "科目名稱不得為空" : "";
  typeErrorMessage.value = typeError.value ? "請選擇科目類別" : "";
  descriptionErrorMessage.value = descriptionError.value
    ? "科目描述不得為空"
    : "";

  if (noError.value || nameError.value || typeError.value || descriptionError.value) {
    return;
  }

  //觸發事件:save
  emit("save", {
    id: props.editTargetId,
    no:accountNo.value,
    name: accountName.value,
    type: accountType.value,
    description: accountDescription.value,
  });
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
  background: none;
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

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  margin-top: 10px;
}

.dialog-footer button {
  margin-left: 5px;
  padding: 5px 10px;
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
