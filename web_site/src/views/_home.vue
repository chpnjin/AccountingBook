<!-- 首頁 -->
<template>
  <div id="homePage">
    <div class="homeBorder">
      <p class="borderTitle">{{ text("Asset") }}</p>
      <div ref="elmAsset" class="dt"></div>
    </div>
    <div class="homeBorder">
      <p class="borderTitle">{{ text("Liability") }}</p>
      <div ref="elmLiability" class="dt"></div>
    </div>
    <div class="homeBorder">
      <p class="borderTitle">{{ text("Revenue") }}</p>
      <div ref="elmRevenue" class="dt"></div>
    </div>
    <div class="homeBorder">
      <p class="borderTitle">{{ text("Expense") }}</p>
      <div ref="elmExpense" class="dt"></div>
    </div>
    <div class="homeBorder">
      <p class="borderTitle">{{ text("Equity") }}</p>
      <div ref="elmEquity" class="dt"></div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import "tabulator-tables/dist/css/tabulator_simple.min.css"; // 引入tabulator主題
import service from "@/services/reportService";
import { default as formatters } from "@/config/formatter.js"; // 導入格式化函式陣列
const [moneyFormatter] = formatters;
import options from "@/config/text-value";

//綁定元素
const elmAsset = ref(null);
const elmLiability = ref(null);
const elmRevenue = ref(null);
const elmExpense = ref(null);
const elmEquity = ref(null);

//Tabulator物件
let dtAsset = reactive({});
let dtLiability = reactive({});
let dtRevenue = reactive({});
let dtExpense = reactive({});
let dtEquity = reactive({});

const text = (value) => {
  return options.find((x) => x.value == value).text;
};

//通用設定
let tbSettings = {
  layout: "fitColumns",
  height: "98%",
  columns: [
    { title: "科目", field: "name",width:250 },
    {
      title: "餘額",
      field: "balance",
      formatter: moneyFormatter,
      hozAlign: "right",
    },
  ],
};

onMounted(() => {
  dtAsset = new Tabulator(elmAsset.value, tbSettings);
  dtLiability = new Tabulator(elmLiability.value, tbSettings);
  dtRevenue = new Tabulator(elmRevenue.value, tbSettings);
  dtExpense = new Tabulator(elmExpense.value, tbSettings);
  dtEquity = new Tabulator(elmEquity.value, tbSettings);
});

//載入完成後抓取餘額資料
nextTick().then(async () => {
  dtAsset.setData(await service.getAccountBalance("Asset"));
  dtLiability.setData(await service.getAccountBalance("Liability"));
  dtRevenue.setData(await service.getAccountBalance("Revenue"));
  dtExpense.setData(await service.getAccountBalance("Expense"));
  dtEquity.setData(await service.getAccountBalance("Equity"));
});
</script>

<style scoped>
#homePage {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap; /* 關鍵：允許換行 */
}
/* 外框 */
.homeBorder {
  margin: 10px 10px; /* 上下左右都加上間距 */
  border: 1px solid #ccc;
  padding: 10px;
  position: relative; /* 關鍵：設定相對定位 */
  width: 400px;
  height: 390px;
  box-sizing: border-box; /* 確保 padding 和 border 不會影響寬度 */
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
.dt {
  margin-top: 10px;
}

.tabulator {
  font-size: 20px;
}
</style>
