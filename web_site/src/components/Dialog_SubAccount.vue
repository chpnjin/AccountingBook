<template>
  <DialogComponent
    :visible="visible"
    :title="title"
    @close="closeDialog"
    @confirm="saveAccount"
  >
    <template #default>
      <input type="hidden" v-model="account.id" />
      <div class="form-group">
        <label>父科目名稱：</label>
        <span>{{ parentAccount?.name }}</span>
      </div>
      <div class="form-group">
        <label>父科目類型：</label>
        <span>{{ typeName }}</span>
      </div>
      <div class="form-group" :class="{ 'has-error': noError }">
        <label>子科目編號：</label>
        <input type="text" v-model="account.no" />
        <div v-if="noError" class="error-message">
          {{ noErrorMessage }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': nameError }">
        <label for="subAccountName">子科目名稱：</label>
        <input type="text" id="subAccountName" v-model="account.name" />
        <div v-if="nameError" class="error-message">
          {{ nameErrorMessage }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': descriptionError }">
        <label for="subAccountDescription">描述：</label>
        <textarea
          id="subAccountDescription"
          v-model="account.description"
        ></textarea>
        <div v-if="descriptionError" class="error-message">
          {{ descriptionErrorMessage }}
        </div>
      </div>
    </template>
  </DialogComponent>
</template>

<script setup>
import DialogComponent from "./Dialog_.vue";
import { ref, reactive, watch, onMounted, toRaw } from "vue";
import accountService from "@/services/accountService"; //API呼叫服務

//定義可在父組件設定屬性
const props = defineProps({
  title: {
    type: String,
    default: "新增子科目",
  },
  //視窗是否可見
  visible: {
    type: Boolean,
    default: false,
  },
  //科目種類中文
  typeName: {
    type: String,
    default: "",
  },
  parentAccount: Object, //傳入的主科目資料
  editingAccount: Object, //用於編輯既有科目
});

const emit = defineEmits(["save", "close"]);

//欄位驗證用
const noError = ref(false);
const nameError = ref(false);
const descriptionError = ref(false);
const noErrorMessage = ref("");
const nameErrorMessage = ref("");
const descriptionErrorMessage = ref("");

const account = reactive({
  id: null,
  no: "",
  name: "",
  description: "",
}); //紀錄子科目輸入值

const resetAccount = () => {
  // 使用 Object.assign 更新 account 物件，避免整個物件被替換
  Object.assign(account, {
    id: null,
    no: "",
    name: "",
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

onMounted(async () => {
  resetAccount(props.editingAccount);
});

//利用監聽機制設定預設值
watch(
  () => props.editingAccount,
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
  }
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

//關閉彈窗
const closeDialog = () => {
  //取消編輯時回填原始資料
  if (account.id != null) {
    Object.assign(account, props.editingAccount);
  }
  emit("close"); // 觸發對話框關閉事件
};

const saveAccount = async () => {
  noError.value = !account.no.trim();
  nameError.value = !account.name.trim();
  descriptionError.value = !account.description.trim();

  noErrorMessage.value = noError.value ? "子科目編號不得為空" : "";
  nameErrorMessage.value = nameError.value ? "子科目名稱不得為空" : "";
  descriptionErrorMessage.value = descriptionError.value ? "描述不得為空" : "";

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



  account.main_id = props.parentAccount.id;
  account.type = props.parentAccount.type;

  emit("save", toRaw(account)); //觸發save事件
  closeDialog();
};
</script>

<style scoped>
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
