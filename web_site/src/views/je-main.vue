<!-- je -->
<template>
  <div class="border">
    <span class="borderTitle">篩選</span>
    <div class="filter-item">
      <label>日期</label>
      <input type="date" v-model="filter.date_start" />
      ～<input type="date" v-model="filter.date_end" />
    </div>
    <div class="filter-item">
      <label>摘要</label>
      <input type="text" v-model="filter.summary" />
    </div>
    <div class="filter-item">
      <!-- <label>包含科目 : </label> -->
    </div>
    <div class="container-filter-btns">
      <button @click="reload_je">查詢</button>
      <button @click="clearCondition">清除條件</button>
    </div>
  </div>
  <div id="topTools">
    <div class="container-action-btns">
      <button id="add" @click="editBtn_clicked">新增</button>
      <button id="edit" :disabled="updateBtnDisable" @click="editBtn_clicked">
        修改
      </button>
      <button id="void" :disabled="updateBtnDisable" @click="editBtn_clicked">
        作廢
      </button>
    </div>
    <p ref="pagerElm"></p>
  </div>
  <div ref="dtElm"></div>
</template>

<script setup>
import { ref, reactive, toRaw, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import { useRouter } from "vue-router";
import service from "@/services/voucherService"; //API呼叫服務
import "tabulator-tables/dist/css/tabulator.min.css";
import options from "@/config/text-value.js";
import formatter from "@/config/formatter.js";
import { jeSearchCondition } from "@/stores/filter"; //保留查詢條件

const router = useRouter();
const dtObj = ref(Tabulator);
const dtElm = ref(null);
const pagerElm = ref(null);

let dtData = reactive([]);
let updateBtnDisable = ref(true);
let loading = false;
let selectedNo = ""; //被選中的傳票編號
const filter = jeSearchCondition(); //跨域容器:查詢條件保留

//重抓交易紀錄
const reload_je = async () => {
  loading = true;
  let params = { ...filter.$state };
  let response = await service.getVoucherList(params);
  dtObj.value.setData(response);
  loading = false;
};
//初始化篩選條件
const initFilterCondition = async () => {
  let today = new Date();
  let year = today.getFullYear();
  let month = String(today.getMonth() + 1).padStart(2, "0");
  let day = String(today.getDate()).padStart(2, "0");

  if (filter.date_start == "") {
    filter.date_start = `${year}-${month}-01`; // 更簡潔的設定當月第一天
  }

  if (filter.date_end == "") {
    filter.date_end = `${year}-${month}-${day}`;
  }
};

//初次讀取
onMounted(async () => {
  dtObj.value = new Tabulator(dtElm.value, {
    layout: "fitColumns",
    height: "420px",
    selectableRows: 1, //只允許單行選擇
    reactiveData: true, //設定響應式資料
    locale: "zh-tw", // 設定使用繁體中文
    langs: formatter.langsSettings(),
    columns: [
      { title: "編號", field: "no", width: 100 },
      {
        title: "交易日期",
        field: "entry_date",
        width: 110,
      },
      { title: "摘要", field: "summary", width: 550 },
      {
        title: "狀態",
        field: "status",
        width: 70,
        formatter: (cell) => {
          const value = cell.getValue();
          const foundType = options.find((x) => x.value === value);
          return foundType ? foundType.text : "未知"; // 如果找不到則回傳 "未知"
        },
      },
    ],
    data: dtData,
    placeholder: () => {
      if (loading) {
        return "資料載入中..."; // 顯示載入中訊息
      } else if (dtData.length == 0) {
        return "沒有資料"; // 顯示沒有資料訊息
      } else {
        return ""; // 有資料時不顯示任何訊息
      }
    },
    // pagination: true,
    paginationSize: 10,
    paginationElement: pagerElm.value,
    paginationAddRow: "table",
  });

  dtObj.value.on("rowClick", (e, row) => {
    let rowData = row.getData();

    selectedNo = row.isSelected() ? rowData : "";
    updateBtnDisable.value = row.isSelected() ? false : true;
  });

  //初次載入時抓取資料
  await nextTick();

  initFilterCondition().then(()=>{
    reload_je();
  });
});

//清除查詢條件
const clearCondition = () => {
  filter.date_start = '';
  filter.date_end = '';
  filter.summary = '';
  reload_je();
};

//按下編輯按鈕
const editBtn_clicked = (e) => {
  let action = e.target.id;

  switch (action) {
    case "add":
      router.push({ path: "je/edit" });
      break;
    case "edit":
      router.push({ path: "je/edit", query: { no: selectedNo.no } });
      break;
    case "void":
      break;
  }
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

.border label,
input {
  font-size: 18px;
}

.filter-item {
  display: flex; /* 在 filter-item 內使用 flexbox */
  align-items: center; /* 垂直對齊 */
}

.filter-item label {
  display: inline-block;
  margin-right: 10px;
}

.filter-item input {
  font-size: 18px;
  flex: 1; /* 讓 input 填滿剩餘空間 */
}

.filter-item input[type="date"] {
  width: 100px;
}

#topTools {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px; /*按鈕間距*/
  padding: 2px 2px 10px 2px; /* 容器內距 */
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

/* 編輯動作按鈕容器 */
.container-action-btns {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 10px; /*按鈕間距*/
  padding: 2px 2px 10px 2px; /* 容器內距 */
}

button {
  padding: 5px 10px;
  border: none; /* 移除預設邊框 */
  border-radius: 5px;
  background-color: #4caf50; /* 設定背景顏色 */
  color: white; /* 設定文字顏色 */
  cursor: pointer;
  transition: background-color 0.3s ease; /* 加入過渡效果 */
  font-size: 18px;
  max-width: 150px;
}
/* 滑鼠懸停時改變背景顏色 */
button:hover {
  background-color: #45a049;
}
/* 只有在未 disabled 時才套用 hover 效果 */
button:hover:not(:disabled) {
  background-color: #45a049;
}
/* 無效按鈕 */
button:disabled {
  background-color: #cccccc; /* disabled 時的背景顏色 (灰色) */
  color: #666666; /* disabled 時的文字顏色 (深灰色) */
  cursor: default; /* disabled 時的滑鼠游標 */
  opacity: 0.6; /* 降低透明度，更明顯區分 */
}

.tabulator {
  font-size: 18px;
}
</style>
