<!--
 通用選擇器:含說明文字+單選表格
-->
<template>
  <Dialog :visible="visible" :title="title" @close="closeDialog">
    <div class="choicer-body">
      <div class="search-box">
        <slot name="search" :onSearch="onSearch"></slot>
      </div>

      <div ref="tableRef" class="tabulator-container"></div>
    </div>

    <template #buttons>
      <button class="btn-confirm" :disabled="!selectedData" @click="confirmSelection">確認選擇</button>
      <button class="btn-cancel" @click="closeDialog">取消</button>
    </template>
  </Dialog>
</template>
<script setup>
import { ref, watch, nextTick } from 'vue';
import { TabulatorFull as Tabulator } from 'tabulator-tables';
import Dialog from './Dialog_.vue';
// 可設定參數
const props = defineProps({
  visible: { type: Boolean, required: true },
  title: { type: String, default: "通用選擇器" }, //彈窗標題
  data: { type: Array, default: () => [] }, // 資料來源
  columns: { type: Array, required: true }, // 💡 由父組件傳入欄位定義
  filterFunc: { type: Function }           // 💡 自定義過濾邏輯
});

const emit = defineEmits(["confirm", "close"]);

const tableRef = ref(null);
const tabulator = ref(null);
const selectedData = ref(null);

// 初始化表格元件
const initTable = () => {
  if (tabulator.value) tabulator.value.destroy();

  tabulator.value = new Tabulator(tableRef.value, {
    data: props.data,
    layout: "fitColumns",
    height: "300px",
    selectable: 1,
    headerSort: false,
    columns: props.columns, // 使用傳入的欄位
  });

  tabulator.value.on("rowClick", (e, row) => {
    selectedData.value = row.getData();
  });

  tabulator.value.on("rowDblClick", (e, row) => {
    selectedData.value = row.getData();
    confirmSelection();
  });
};

// 暴露給插槽使用的搜尋方法
const onSearch = (searchFilters) => {
  if (tabulator.value) {
    tabulator.value.setFilter(searchFilters);
  }
};

watch(() => props.visible, async (newVal) => {
  if (newVal) {
    selectedData.value = null;
    await nextTick();
    initTable();
  }
});
//觸發自訂事件:close
const closeDialog = () => emit("close");
//觸發自訂事件:confirm
const confirmSelection = () => { if (selectedData.value) emit("confirm", selectedData.value); };
</script>

<style scoped>
.tabulator-container :deep(.tabulator) {
  font-size: 16px;
}
</style>
