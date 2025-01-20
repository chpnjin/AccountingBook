<template>
  <DialogComponent
    :visible="visible"
    :title="title"
    @close="closeDialog"
    @confirm="saveAccount"
  >
    <template #default>
      <input type="hidden" v-model="account.id" />
      <div class="form-group" :class="{ 'has-error': noError }">
        <label>科目編號</label>
        <input type="text" v-model="account.no" />
        <div v-if="noError" class="error-message">{{ noErrorMessage }}</div>
      </div>
      <div class="form-group" :class="{ 'has-error': nameError }">
        <label>科目名稱</label>
        <input type="text" v-model="account.name" />
        <div v-if="nameError" class="error-message">{{ nameErrorMessage }}</div>
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
    </template>
  </DialogComponent>
</template>

<script setup>
import DialogComponent from "./Dialog_.vue";
import { ref, reactive, watch, onMounted, toRaw } from "vue";
import accountService from "@/services/accountService";

const props = defineProps({
  visible: { type: Boolean, required: true },
  title: { type: String, default: "新增主科目" },
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

const emit = defineEmits(["save", "close"]);

const account = reactive({
  id: null,
  no: "",
  name: "",
  type: "Asset",
  description: "",
});

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
  Object.assign(account, {
    id: null,
    no: "",
    name: "",
    type: "Asset",
    description: "",
  });
  resetErrorMessages();
};

const resetErrorMessages = () => {
  noError.value = false;
  nameError.value = false;
  descriptionError.value = false;
  noErrorMessage.value = "";
  nameErrorMessage.value = "";
  descriptionErrorMessage.value = "";
};

onMounted(() => {
  resetAccount();
});
//監聽選中主科目Grid同步欄位值
watch(
  () => props.inputData,
  (newValue) => {
    if (Object.keys(newValue).length === 0) {
      resetAccount();
    } else {
      Object.assign(account, newValue);
    }
  },
  { immediate: true }
);

const closeDialog = () => {
  resetAccount();
  emit("close");
};
//儲存主科目
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
    const idExist = await accountService.accountIdExist(account.no);
    if (idExist) {
      noError.value = true;
      noErrorMessage.value = "該科目編號已存在";
      return;
    }
  }

  emit("save", toRaw(account));
  closeDialog();
};
</script>

<style scoped>
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
