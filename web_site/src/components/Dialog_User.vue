<template>
  <DialogComponent
    :visible="visible"
    :title="title"
    @close="closeDialog"
    @confirm="saveAccount"
  >
    <template #default>
      <div class="form-group" :class="{ 'has-error': errorFlags.name }">
        <label>帳號</label>
        <input
          type="text"
          v-model="localData.name"
          @keypress="clearOneError('name')"
        />
        <div v-if="errorFlags.name" class="error-message">
          {{ errorMsgs.name }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': errorFlags.email }">
        <label>E-mail</label>
        <input
          type="text"
          v-model="localData.email"
          @keypress="clearOneError('email')"
        />
        <div v-if="errorFlags.email" class="error-message">
          {{ errorMsgs.email }}
        </div>
      </div>
      <div class="multiSelectDropdown">
        <label>權限群組</label>
        <MultiSelectDropdown
          @keypress="clearOneError('groupRole')"
          ref="authorityGroupRef"
          :class="{ 'has-error': errorFlags.groupRole }"
        />
        <div v-if="grouproleError" class="error-message">
          {{ errorMsgs.groupRole }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': errorFlags.fullName }">
        <label>全名(顯示於登入資訊)</label>
        <input
          type="text"
          v-model="localData.fullName"
          @keypress="clearOneError('fullName')"
        />
        <div v-if="errorFlags.fullName" class="error-message">
          {{ errorMsgs.fullName }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': errorFlags.department }">
        <label>部門</label>
        <input
          type="text"
          v-model="localData.department"
          @keypress="clearOneError('department')"
        />
        <div v-if="errorFlags.department" class="error-message">
          {{ errorMsgs.department }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': errorFlags.jobTitle }">
        <label>職稱</label>
        <input
          type="text"
          v-model="localData.job_title"
          @keypress="clearOneError('jobTitle')"
        />
        <div v-if="errorFlags.jobTitle" class="error-message">
          {{ errorMsgs.jobTitle }}
        </div>
      </div>
      <div class="form-group" :class="{ 'has-error': errorFlags.phone }">
        <label>電話</label>
        <input
          type="text"
          v-model="localData.phone"
          @keypress="clearOneError('phone')"
        />
        <div v-if="errorFlags.phone" class="error-message">
          {{ errorMsgs.phone }}
        </div>
      </div>
    </template>
  </DialogComponent>
</template>
<script setup>
import DialogComponent from "./Dialog_.vue";
import { ref, reactive, watch, onMounted, toRaw } from "vue";
import MultiSelectDropdown from "@/components/_MultiSelectDropdown.vue";
import service from "@/services/userService.js";

const emit = defineEmits(["save", "close"]);
const props = defineProps({
  visible: { type: Boolean, required: true },
  title: { type: String, default: "新增使用者" },
  inputData: {
    type: Object,
    default: () => ({
      name: "",
      email: "",
      role_group: "",
      fullName: "",
      department: "",
      job_title: "",
      phone: 0,
    }),
  },
});

const localData = reactive({});

const errorFlags = reactive({
  name: false,
  email: false,
  groupRole: false,
  fullName: false,
  department: false,
  jobTitle: false,
  phone: false,
});

const errorMsgs = reactive({
  name: "",
  email: "",
  groupRole: "",
  fullName: "",
  department: "",
  jobTitle: "",
  phone: "",
});

const authorityGroupRef = ref(null);

onMounted(() => {
  // 判斷父组件是否傳入 inputData，若有則合併
  if (props.inputData) {
    Object.assign(localData, props.inputData);
  }
});

watch(
  () => props.inputData,
  (newValue) => {
    if (Object.keys(newValue).length === 0) {
      console.info();
    } else {
      Object.assign(localData, newValue);
    }
  },
  { immediate: true }
);

const clearOneError = (target) => {
  errorFlags[target] = false; // 動態設定 error flag
  errorMsgs[target] = "";
};

const clearAll = () => {
  Object.keys(errorFlags).forEach((key) => {
    errorFlags[key] = false;
  });

  Object.keys(errorMsgs).forEach((key) => {
    errorMsgs[key] = false;
  });

  Object.keys(localData).forEach((key) => {
    localData[key] = "";
  });
};

const closeDialog = () => {
  clearAll();
  emit("close");
};

const saveAccount = async () => {
  localData.role_group = authorityGroupRef.value.getSelectedItems();

  errorFlags.name = !(localData.name ?? "").trim();
  errorFlags.email = !(localData.email ?? "").trim();
  errorFlags.groupRole = localData.role_group.length == 0 ? true : false;
  errorFlags.fullName = !(localData.fullName ?? "").trim();
  errorFlags.department = !(localData.department ?? "").trim();
  errorFlags.jobTitle = !(localData.job_title ?? "").trim();
  errorFlags.phone = !(localData.phone ?? "").trim();

  errorMsgs.name = errorFlags.name ? "未輸入帳號" : "";
  errorMsgs.email = errorFlags.email ? "未輸入E-mail" : "";
  errorMsgs.groupRole = errorFlags.groupRole ? "未選擇權限群組" : "";
  errorMsgs.fullName = errorFlags.fullName ? "未輸入全名" : "";
  errorMsgs.department = errorFlags.department ? "未輸入部門" : "";
  errorMsgs.jobTitle = errorFlags.jobTitle ? "未輸入職稱" : "";
  errorMsgs.phone = errorFlags.phone ? "未輸入電話" : "";

  const hasError = !Object.values(errorFlags).every((flag) => flag === false);
  if (hasError) {
    return;
  }

  if (props.title.includes("新增")) {
    const checkResult = await service.userExist(localData.name,localData.email);
    if (checkResult.name_exist) {
      errorFlags.name = true;
      errorMsgs.name = "該帳號已存在";
      return;
    }

    if(checkResult.email_exist){
      errorFlags.email = true;
      errorMsgs.email = "該E-mail已存在";
      return;
    }

  }

  emit("save", toRaw(localData));
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
