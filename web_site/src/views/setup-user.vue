<template>
  <h1>使用者設定</h1>
  <div class="border">
    <span class="borderTitle">篩選</span>
    <div class="items">
      <div class="filter-item">
        <label>名稱</label>
        <input type="text" />
      </div>
      <div class="filter-item">
        <label>電子信箱</label>
        <input type="text" />
      </div>
      <div class="filter-item">
        <label>電話</label>
        <input type="text" />
      </div>
    </div>
    <div class="multiSelectDropdown">
        <label>權限群組</label><MultiSelectDropdown />
      </div>
    <div class="container-filter-btns">
      <button @click="search">查詢</button>
      <button @click="clearCondition">清除條件</button>
    </div>
  </div>
  <div class="button-container">
    <button id="add" @click="openDialog">
      新增
    </button>
    <button id="update" @click="openDialog" :disabled="disableEdit">
      編輯
    </button>
    <button @click="inActiveSub" :disabled="canInactive">停用</button>
  </div>
  <div ref="elmDt"></div>
  <dialogEdit
    :visible="editDialogVisible"
    :title="dialogTitle"
    :input-data="editData"
    @close="closeDialog"
  ></dialogEdit>
</template>

<script setup>
import { ref, reactive, onMounted, nextTick } from "vue";
import MultiSelectDropdown from "@/components/_MultiSelectDropdown.vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import "tabulator-tables/dist/css/tabulator_simple.min.css"; // 引入 simple 主題
import formatter from "@/config/formatter.js";
import { jeSearchCondition } from "@/stores/filter"; //保留查詢條件
import dialogEdit from "@/components/Dialog_User.vue";

const elmDt = ref(null);
let objDt = null; //主科目物件
const editDialogVisible = ref(false);
const canInactive = ref(false);
const dialogTitle = ref("");
const disableEdit = ref(true);

const dtData = reactive([]);
const editData = reactive([]);

onMounted(async () => {
  objDt = new Tabulator(elmDt.value, {
    layout: "fitColumns",
    height: "420px",
    selectableRows: 1, //只允許單行選擇
    data: dtData,
    locale: "zh-tw", // 設定使用繁體中文
    langs: formatter.langsSettings(),
    columns: [
      { title: "名稱", field: "name", width: 100 },
      { title: "電子信箱", field: "email", width: 150 },
      { title: "電話", field: "jobTitle", width: 150 },
      { title: "所屬權限群組", field: "group", width: 250 },
      { title: "狀態", field: "status", width: 70 },
    ],
  })
});

nextTick().then(()=>{

})

const openDialog = (e) => {
  if (e.target.id == "add") {
    dialogTitle.value = "新增使用者";
  } else {
    dialogTitle.value = "編輯使用者";
  }
  editDialogVisible.value = true;
};

const closeDialog = () => {
  editDialogVisible.value = false;
};
</script>

<style scoped>
.border {
  display: flex;
  flex-direction: column;
  border: 1px solid #ccc;
  position: relative;
  padding-top: 25px;
  padding-left: 10px;
  padding-right: 10px;
  padding-bottom: 10px; /* 底部padding加大，確保按鈕有足夠空間 */
  margin-bottom: 10px;
  width: 100%;
  box-sizing: border-box;
  gap: 10px; /* 項目間距 */
}

/* 外框標題 */
.borderTitle {
  position: absolute; /* 關鍵：設定絕對定位 */
  top: -10px; /* 向上移動，使其超出外框 */
  left: 10px; /* 從左邊開始 */
  /* 讓標題底色與背景相同，遮蓋外框線 */
  background-color: white;
  padding: 0 5px; /* 增加左右內距，使文字周圍有空間 */
  font-weight: bold;
  font-size: 20px;
}

/* 查詢動作按鈕容器 */
.container-filter-btns {
  display: flex;
  flex-direction: row-reverse;
  /* 按鈕間距 */
  gap: 10px;
  bottom: 10px;
  right: 10px;
}

.items {
  display: flex;
  flex-wrap: wrap; /* 允許每個 filter-item 在寬度不足時換行 */
  gap: 10px; /* 控制每個項目間距 */
  width: 100%;
}

.filter-item {
  display: inline-flex; /* 讓 label 和 input 綁定為一個整體 */
  align-items: center;
  gap: 10px; /* 控制 label 和 input 的間距 */
  flex: 1 1 200px; /* 彈性寬度，最小 200px，最大填滿剩餘空間 */
  max-width: 100%; /* 防止超過容器 */
  box-sizing: border-box;
}

.filter-item label {
  width: 50px;
  margin-right: 10px;
  white-space: nowrap; /* 防止 label 自動換行 */
}

.filter-item input {
  flex: 1; /* 讓 input 填滿剩餘空間 */
  min-width: 100px; /* 防止過小 */
  max-width: 100%; /* 確保不超過 filter-item 的寬度 */
  box-sizing: border-box;
  padding: 5px;
}

.button-container {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 10px; /*按鈕間距*/
  padding: 2px 2px 10px 2px; /* 容器內距 */
}

.button-container button:disabled {
  background-color: #cccccc; /* disabled 時的背景顏色 (灰色) */
  color: #666666; /* disabled 時的文字顏色 (深灰色) */
  cursor: default; /* disabled 時的滑鼠游標 */
  opacity: 0.6; /* 降低透明度，更明顯區分 */
}
</style>
