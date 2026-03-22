<!-- 簽核情形顯示 -->
<template>
  <Dialog :visible="dialogVisible" title="簽核情形">
    <template #default>
      內容
    </template>
  </Dialog>
</template>
<script setup>
import Dialog from "./Dialog_.vue";
//定義父組件可設定屬性
defineProps({
  dialogVisible: { type: Boolean, required: true }, // 控制顯示
});
</script>
