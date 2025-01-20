<!-- 簽核情形顯示 -->
<template>
  <DialogComponent :visible="dialogVisible" title="簽核情形">
    <template #default>
      內容
    </template>
  </DialogComponent>
</template>
<script setup>
import DialogComponent from "./Dialog_.vue";
//定義父組件可設定屬性
defineProps({
  dialogVisible: { type: Boolean, required: true }, // 控制顯示
});
</script>
