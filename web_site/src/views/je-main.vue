<!-- je -->
<template>
  <div id="topTools">
    <div class="button-container">
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
import { ref, reactive, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import { useRouter } from "vue-router";
import service from "@/services/voucherService"; //API呼叫服務
import "tabulator-tables/dist/css/tabulator.min.css";
import valList from "@/config/text-value.js";

const router = useRouter();
const dtObj = ref(Tabulator);
const dtElm = ref(null);
const pagerElm = ref(null);
let dtData = reactive([]);
let updateBtnDisable = ref(true);
let loading = false;
let selectedNo = ""; //被選中的傳票編號

//重抓交易紀錄
const reload_je = async () => {
  loading = true;
  let response = await service.getVoucherList({});
  dtObj.value.setData(response);
  loading = false;
};

//初次讀取
onMounted(async () => {
  dtObj.value = new Tabulator(dtElm.value, {
    layout: "fitColumns",
    height: "400px",
    selectableRows: 1, //只允許單行選擇
    reactiveData: true, //設定響應式資料
    columns: [
      { title: "編號", field: "no", width: 80 },
      {
        title: "交易日期",
        field: "entry_date",
        width: 100,
      },
      // {
      //   title: "金額",
      //   field: "amount",
      //   width: 100,
      //   formatter: function (cell) {
      //     const value = cell.getValue();
      //     if (isNaN(value)) {
      //       return value;
      //     }
      //     return new Intl.NumberFormat("en-US", {
      //       minimumFractionDigits: 2,
      //       maximumFractionDigits: 2,
      //     }).format(value);
      //   },
      //   hozAlign: "right",
      // },
      { title: "摘要", field: "summary", width: 500 },
      {
        title: "狀態",
        field: "status",
        width: 100,
        formatter: (cell) => {
          const value = cell.getValue();
          const foundType = valList.find((x) => x.value === value);
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
    pagination: true,
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
  await reload_je();
});

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
#topTools {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px; /*按鈕間距*/
  padding: 2px 2px 10px 2px; /* 容器內距 */
}

.button-container {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 10px; /*按鈕間距*/
  padding: 2px 2px 10px 2px; /* 容器內距 */
}

.button-container button {
  padding: 5px 20px;
  border: none; /* 移除預設邊框 */
  border-radius: 5px;
  background-color: #4caf50; /* 設定背景顏色 */
  color: white; /* 設定文字顏色 */
  cursor: pointer;
  transition: background-color 0.3s ease; /* 加入過渡效果 */
  font-size: 18px;
}
/* 滑鼠懸停時改變背景顏色 */
.button-container button:hover {
  background-color: #45a049;
}
 /* 只有在未 disabled 時才套用 hover 效果 */
.button-container button:hover:not(:disabled) {
  background-color: #45a049;
}
/* 無效按鈕 */
.button-container button:disabled {
  background-color: #cccccc; /* disabled 時的背景顏色 (灰色) */
  color: #666666; /* disabled 時的文字顏色 (深灰色) */
  cursor: default; /* disabled 時的滑鼠游標 */
  opacity: 0.6; /* 降低透明度，更明顯區分 */
}

</style>
