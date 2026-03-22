<!-- 彈窗:結帳確認 -->
<template>
  <Dialog :visible="visible">
    <p>是否確認生成結帳傳票？</p>
    <p class="warning-note">注意：選中結帳年份的虛帳戶資料將被清零並結算至目標科目。</p>
    <template #buttons>
      <button class="btn-confirm-danger" @click="$emit('confirm')">確認結帳</button>
      <button class="btn-cancel" @click="$emit('close')">取消</button>
    </template>
  </Dialog>
</template>
<script setup>
import Dialog from './Dialog_.vue';

defineProps({
  visible: Boolean
});

defineEmits(['confirm', 'close']);
</script>

<style scoped>
.confirm-body {
  padding: 10px 5px;
  font-size: 18px;
  line-height: 1.5;
}

/* 讓警告文字稍微縮排或變色，增加辨識度 */
.warning-note {
  font-size: 16px;
  color: #d32f2f; /* 紅色警告 */
  margin-top: 8px;
  font-weight: bold;
}

.desc-text {
  font-size: 16px;
  color: #555;
}
.btn-confirm-danger {
  background-color: #d32f2f;
  color: white;
}
.btn-cancel {
  background-color: #6c757d;
  color: white;
}
</style>
