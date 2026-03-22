<!-- je/closing -->
<template>
  <div>
    <div class="toolbar">
      <span>結帳年份</span>
      <select id="year-select" v-model="selectedYear">
        <option v-for="year in yearOptions" :key="year" :value="year">
          {{ year }} 年
        </option>
      </select>
      <button @click="trialBalance">試算</button>
    </div>
    <div class="flex-container">
      <div class="border">
        <p class="borderTitle">{{ text("Revenue") }}</p>
        <div ref="elmRevenue" class="dt"></div>
        <input readonly class="readonly-input readonly-input-bottom" :value="formatInt(summary.totalRevenue)">
      </div>
      <div class="border">
        <p class="borderTitle">{{ text("Expense") }}</p>
        <div ref="elmExpense" class="dt"></div>
        <input readonly class="readonly-input readonly-input-bottom" :value=formatInt(summary.totalExpense)>
      </div>
    </div>
    <div class="toolbar">
      <span class="item-title">損益彙總</span>
      <input readonly class="readonly-input-bottom" :value=formatInt(summary.netIncome)>
    </div>
    <div class="toolbar">
      <span class="item-title">匯總目標科目(權益類)</span>
      <input readonly :value=selectedAccountName>
      <button @click="openAccountSelector">...</button>
    </div>
    <button :disabled="closeingBtnDisabled" @click="handleCloseConfirm">生成結帳傳票</button>
  </div>
  <!-- 選擇要結算加總的權益科目 -->
  <Dialog_Selector title="選擇彙總目標淨值科目" :visible="isSelectorVisible" :data="accountList" :columns="accountColumns"
    @confirm="handleConfirm" @close="isSelectorVisible = false">
    <template #search="{ onSearch }">
      <input v-model="searchKey" placeholder="關鍵字搜尋..." class="my-custom-input" @input="onSearch([
        [{ field: 'no', type: 'like', value: searchKey },
        { field: 'name', type: 'like', value: searchKey }]
      ])">
    </template>
  </Dialog_Selector>
  <Dialog_ClosingConfirm :visible="isConfirmVisible" @confirm="handleGenerateVoucher"
    @close="isConfirmVisible = false" />
</template>

<script setup>
import { ref, reactive, onMounted, computed } from 'vue';
import { TabulatorFull as Tabulator } from "tabulator-tables";
import "tabulator-tables/dist/css/tabulator.min.css";
import { useRouter } from "vue-router";
import voucher from "@/services/voucherService"; //API
import account from "@/services/accountService"; //API
import options from "@/config/text-value"; //通用設定
import formatters from "@/config/formatter.js"; // 導入格式化函式陣列
import Dialog_Selector from "@/components/Dialog_Selector.vue"
import Dialog_ClosingConfirm from "@/components/Dialog_ClosingConfirm.vue"

const elmRevenue = ref(Tabulator);
const elmExpense = ref(Tabulator);
let dtRevenue = reactive({});
let dtExpense = reactive({});

//彈窗可見Flag
const isSelectorVisible = ref(false);
const isConfirmVisible = ref(false);

const accountList = ref([]);     // 存放 API 回傳的所有科目

const router = new useRouter();
const currentYear = new Date().getFullYear();
const yearOptions = ref([]);
const text = (value) => {
  return options.find((x) => x.value == value).text;
};

const fetchYears = async () => {
  try {
    const years = await voucher.getCanClosingYears();
    yearOptions.value = years;
  } catch (error) {
    console.error("無法取得可結帳年份:", error);
  }
}

const searchKey = ref("");       // 搜尋關鍵字
const selectedAccount = ref(null); // 存放最終選中的科目物件
const selectedAccountName = ref("");
const selectedYear = ref(currentYear); //選中的年份

const isPending = ref(false); // API執行中狀態

// 定義彈窗表格的欄位
const accountColumns = [
  { title: "ID", field: "id", visible: false }, // 💡 設定隱藏欄位
  { title: "科目編號", field: "no", width: 120, headerSort: false },
  { title: "科目名稱", field: "name", headerSort: false }
];

//通用設定
const tbSettings = {
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
    },
  ],
};

const summary = ref({
  totalExpense: 0,
  totalRevenue: 0,
  netIncome: 0
});

//金額通用顯示格式
const formatInt = (value) => {
  if (value === undefined || value === null) return "0";
  // 轉成整數並加上千分位
  return Math.floor(value).toLocaleString('zh-TW');
};

//[基本] 開啟頁面事件
onMounted(async () => {
  fetchYears();
  closeingBtnDisabled.value = true;
});

//試算
const trialBalance = async () => {
  console.info("選中的年份:" + selectedYear.value)
  const orgData = await voucher.getGetNominalAccountBalances(selectedYear.value)

  console.info(orgData)

  dtRevenue = new Tabulator(elmRevenue.value, tbSettings);
  dtExpense = new Tabulator(elmExpense.value, tbSettings);

  dtRevenue.on("tableBuilt", function () {
    dtRevenue.setData(orgData.revenue);
  });

  dtExpense.on("tableBuilt", function () {
    dtExpense.setData(orgData.expense);
  });

  summary.value.totalExpense = orgData.summary.totalExpense;
  summary.value.totalRevenue = orgData.summary.totalRevenue;
  summary.value.netIncome = orgData.summary.netIncome;


}

//選擇彙總科目彈窗
const openAccountSelector = async () => {
  try {
    const equityAccountList = await account.getEquityTypeAccounts()

    accountList.value = equityAccountList;
    // 開啟彈窗
    isSelectorVisible.value = true;
  } catch (error) {
    console.error("無法獲取科目清單:", error);
    alert("讀取科目清單失敗");
  }
};

//事件:確認選擇匯總目標科目
const handleConfirm = (account) => {
  // 這裡的 account 物件會包含隱藏的 id
  // 例如: { id: 55, no: '3101', name: '累積盈虧' }
  selectedAccount.value = account;
  selectedAccountName.value = account.name;

  isSelectorVisible.value = false;
};

//監視按鈕:生成結帳傳票
const closeingBtnDisabled = computed(() => {
  // 判斷條件：
  // 1. 是否選了年份 (selectedYear)
  // 2. 是否選了目標科目 (selectedAccount)
  // 3. 損益金額是否已經過計算 (這裡檢查 totalRevenue 或 totalExpense 是否大於 0)

  const hasYear = !!selectedYear.value;
  const hasAccount = !!selectedAccount.value;
  const hasCalculated = summary.value.totalRevenue !== 0 || summary.value.totalExpense !== 0;

  return !(hasYear && hasAccount && hasCalculated);
});

//事件: 結帳確認彈窗
const handleCloseConfirm = async () => {
  const currentYear = new Date().getFullYear();

  if (Number(selectedYear.value) >= currentYear) {
    alert(`無法執行結帳：\n${selectedYear.value} 年帳務尚未結束，僅能針對過去年度進行結帳作業。`);
    console.warn(`拒絕結帳請求：嘗試結帳當前或未來年份 (${selectedYear.value})`);
    return; // 強制結束，不執行後續動作
  }

  isConfirmVisible.value = true;
}

//事件: 生成結帳傳票
const handleGenerateVoucher = async () => {
  console.info("生成結帳傳票");
  if (isPending.value) return;

  isConfirmVisible.value = false;

  voucher.generateClosingVoucher(
    selectedYear.value,
    selectedAccount.value.id
  )

}

</script>

<style scoped>
/* 文字工具列 */
.toolbar {
  display: flex;
  gap: 10px;
  padding: 5px 5px 15px 5px;
  font-size: 18px;
  align-items: center;
}

.toolbar input {
  font-size: 18px;
  /* 設定文字大小 */
  padding: 4px 8px;
  /* 增加內距，字體大才不會擠在一起 */
  height: auto;
  /* 讓高度隨字體自動撐開 */
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 180px;
}

.flex-container {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  /* 關鍵：允許換行 */
}

/* 外框 */
.border {
  display: flex;
  flex-direction: column;
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

/* 表格本體 */
.dt {
  flex-grow: 1;
  /* 關鍵：讓表格佔滿剩餘空間，把 input 擠下去 */
  width: 100%;
}

.readonly-input-bottom {
  align-self: flex-end;
  /* 自己靠右對齊 */
  margin-top: 10px;
  /* 與上方表格保持間距 */
  width: 150px;
  /* 設定寬度 */
  text-align: right;
  /* 文字靠右，符合金額顯示習慣 */
}

/* 無效按鈕 */
button:disabled {
  background-color: #cccccc;
  /* disabled 時的背景顏色 (灰色) */
  color: #666666;
  /* disabled 時的文字顏色 (深灰色) */
  cursor: default;
  /* disabled 時的滑鼠游標 */
  opacity: 0.6;
  /* 降低透明度，更明顯區分 */
}

#year-select,
button {
  font-size: 18px;
  padding: 4px 8px;
  cursor: pointer;
}

#year-select option {
  font-size: 18px;
}

.item-title {
  width: 175px;
}

/* 針對「禁止輸入」狀態的樣式 */
input[readonly] {
  background-color: #eeeeee;
  /* 淺灰色背景 */
  color: #555;
  /* 深灰色文字 */
  cursor: not-allowed;
  /* 顯示禁止游標 */
  outline: none;
  /* 移除聚焦時的藍色外框 */
  font-size: 18px;
}

/* 穿透Tabulator物件內部字體設定 */
:deep(.tabulator) {
  font-size: 18px !important;
  background-color: transparent;
  /* 保持背景與你的容器一致 */
}
</style>
