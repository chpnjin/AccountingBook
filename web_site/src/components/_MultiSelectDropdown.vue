<template>
  <div class="multi-select">
    <div class="dropdown-input" @click="toggleDropdown">
      <div class="selected-items">
        <span v-for="(item, index) in selectedItems" :key="item">
          {{ item.text }}
          <button type="button" @click.stop="removeItem(index)">×</button>
        </span>
      </div>
      <span class="dropdown-arrow">▼</span>
    </div>
    <ul v-if="showDropdown" class="dropdown-list" @focusout="toggleDropdown">
      <li
        v-for="item in filteredOptions"
        :key="item"
        :class="{ selected: selectedItems.includes(item) }"
        @click="selectItem(item)"
      >
        {{ item.text }}
      </li>
    </ul>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import textValues from "@/config/text-value.js";

// 模擬選項資料
const options = computed(() => {
  return textValues
    .filter((item) => item.type === "authority")
    .map((item) => ({
      value: item.value,
      text: item.text,
    }));
});

const selectedItems = ref([]); // 已選中的選項
const showDropdown = ref(false); // 控制下拉菜單顯示
const filteredOptions = ref([...options.value]); // 篩選後的選項

// 切換下拉菜單顯示
const toggleDropdown = () => {
  showDropdown.value = !showDropdown.value;
};

// 選擇項目
const selectItem = (item) => {
  if (!selectedItems.value.includes(item)) {
    selectedItems.value.push(item);
  }
};

// 移除已選擇項目
const removeItem = (index) => {
  selectedItems.value.splice(index, 1);
};

const getSelectedItems = () => {
  return selectedItems.value.map(item => item.value); // 假設 selectedItems 中的每個項目有 value 屬性
};

//定義可在父組件取用的方法
defineExpose({
  getSelectedItems
});

</script>

<style scoped>
.multi-select {
  position: relative;
}

.dropdown-input {
  display: flex;
  align-items: center;
  border: 1px solid #ccc;
  border-radius: 4px;
  /* padding: 8px; */
  cursor: pointer;
  background-color: #fff;
}

.selected-items {
  display: flex;
  flex-wrap: wrap;
  gap: 4px;
  flex: 1;
}

.selected-items span {
  display: flex;
  align-items: center;
  background: #e9e9e9;
  border-radius: 4px;
  font-size: 0.9em;
}
/* 項目x按鈕 */
.selected-items span button {
  margin-left: 4px;
  border: none;
  background: transparent;
  cursor: pointer;
  color: #4caf50;
}

.dropdown-arrow {
  margin-left: 8px;
}
/* 下拉選單整體 */
.dropdown-list {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  max-height: 200px;
  overflow-y: auto;
  border: 1px solid #ccc;
  border-radius: 4px;
  background-color: #fff;
  z-index: 10;
  list-style: none;
  padding: 0;
}
/* 下拉選單選項 */
.dropdown-list li {
  padding: 2px;
  cursor: pointer;
}

.dropdown-list li:hover {
  background-color: #f5f5f5;
}

.dropdown-list li.selected {
  background-color: #d5f5e3;
}
</style>
