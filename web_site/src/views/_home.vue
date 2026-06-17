<!-- 首頁 -->
<template>
  <div id="homePage">
    <div v-for="type in accountTypes" :key="type" class="homeBorder">
      <p class="borderTitle">{{ text(type) }}</p>
      <div :ref="el => tableElements[type] = el" class="dt"></div>
    </div>
  </div>
  <!-- 科目餘額異動明細彈窗 -->
  <acctDialog
    :visible="dialogVisible"
    :title="dialogTitle"
    :accountName="detailAcctName"
    @close="dialogClosed"
    @jumpToEdit="jumpToVoucherEdit"
  ></acctDialog>
</template>

<script setup>
import { ref, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import { useRouter } from "vue-router";
import "tabulator-tables/dist/css/tabulator_simple.min.css"; // 引入tabulator主題
import service from "@/services/reportService";
import formatters from "@/config/formatter.js"; // 導入格式化函式陣列
import options from "@/config/text-value";
import acctDialog from "@/components/Dialog_AcctLogs.vue"; //科目餘額異動明細彈窗

const router = useRouter();
const accountTypes = ["Asset", "Liability", "Revenue", "Expense", "Equity"];

//綁定元素
const tableElements = {};
//Tabulator物件
const tabulatorInstances = {};

const dialogVisible = ref(false);
const dialogTitle = ref("");
const detailAcctName = ref("");
//關鍵字尋找對應顯示文字
const text = (value) => {
  return options.find((x) => x.value == value).text;
};

//通用設定
let tbSettings = {
  layout: "fitColumns",
  height: "98%",
  columns: [
    { title: "科目", field: "name", width: 255, resizable: false },
    {
      title: "餘額",
      field: "balance",
      formatter: formatters.moneyFormatter,
      hozAlign: "right",
      resizable: false,
      width: 100,
    }
  ]
};

// 當畫面掛載後，實例化所有表格
onMounted(() => {
  accountTypes.forEach((type) => {
    if (tableElements[type]) {
      tabulatorInstances[type] = new Tabulator(tableElements[type], {
        ...tbSettings
      });
    }
  });
});

//載入完成後抓取餘額資料與排序
nextTick().then(async () => {
  for (const type of accountTypes) {
    const instance = tabulatorInstances[type];
    if (instance) {
      const data = await service.getAccountBalance(type);
      instance.setData(data);

      // 設定以科目名稱排序
      instance.setSort([{ column: "name", dir: "asc" }]);

      //點擊科目後，彈窗顯示有紀錄該科目的傳票清單
      instance.on("rowDblClick", (e, row) => {
        dialogTitle.value = "餘額異動明細";
        detailAcctName.value = row.getData().name;
        dialogVisible.value = true;
      });
    }
  }
});

//關閉彈窗事件
const dialogClosed = () => {
  dialogVisible.value = false;
};

/**
 * 跳轉至指定傳票編輯頁面
 * @param {string} voucherNo - 傳票的唯一編號
 */
const jumpToVoucherEdit = (voucherNo) => {
  console.log("要跳轉的傳票編號:" + voucherNo);
  dialogVisible.value = false;
  router.push({ path: "je/edit", query: { no: voucherNo } });
}

</script>

<style scoped>
#homePage {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  /* 關鍵：允許換行 */
}

/* 外框 */
.homeBorder {
  margin: 10px 10px;
  /* 上下左右都加上間距 */
  border: 1px solid #ccc;
  padding: 10px;
  position: relative;
  /* 關鍵：設定相對定位 */
  width: 400px;
  height: 390px;
  box-sizing: border-box;
  /* 確保 padding 和 border 不會影響寬度 */
}

/* 外框標題 */
.borderTitle {
  position: absolute;
  /* 關鍵：設定絕對定位 */
  top: -10px;
  /* 向上移動，使其超出外框 */
  left: 10px;
  /* 從左邊開始 */
  /* 讓標題底色與背景相同，遮蓋外框線 */
  background-color: white;
  padding: 0 5px;
  /* 增加左右內距，使文字周圍有空間 */
  font-weight: bold;
  font-size: 20px;
}

.dt {
  margin-top: 10px;
}

.tabulator {
  font-size: 20px;
}
</style>
