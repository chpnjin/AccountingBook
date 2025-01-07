<!-- je/edit -->
<template>
  <h3>編輯傳票</h3>
  <div class="container">
    <div class="item">
      <label>編號</label> <input type="text" :value="txNo" />
    </div>
    <div class="item">
      <label for="select-option">類型</label>
      <select id="select-option" v-model="vchrType">
        <option v-for="item in vchrType" :key="item.value" :value="item.value">
          {{ item.text }}
        </option>
      </select>
    </div>
    <div class="item"><label>日期</label> <input type="date" /></div>
    <div class="summary">
      <label for="summary-input">摘要</label>
      <input type="text" id="summary-input" v-model="summary" />
    </div>
  </div>
  <div id="topTools">
    <div>
      <button @click="addAccount">＋</button>
      <button @click="deleteAccount" :disabled="noSelected">Ｘ</button>
    </div>
    <p ref="pagerElm"></p>
  </div>
  <div ref="dtElm"></div>
  <button @click="save">儲存</button>
  <button @click="submit">送審</button>
  <button @click="cancel">取消</button>
  <editDialog
    :visible="selectAcctDialogVisible"
    :title="dialogTitle"
    @close="dialogClosed"
    @selected="insertAcct"
  ></editDialog>
</template>

<script setup>
import { ref, reactive, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import editDialog from "@/components/Dialog_ChoiceAcct.vue"; //選擇科目彈窗
import "tabulator-tables/dist/css/tabulator.min.css";
import { useRouter } from "vue-router";

const router = new useRouter();
const txNo = ref("D20250103001");
const dtObj = ref(Tabulator);
const dtElm = ref(null);
const pagerElm = ref(null);
const dtData = reactive([]);
const summary = ref("");
const selectAcctDialogVisible = ref(false);
const dialogTitle = ref("");
const vchrType = [
  { text: "期初開帳", value: "" },
  { text: "現金收入", value: "" },
  { text: "現金支出", value: "" },
  { text: "轉帳", value: "" },
];
const noSelected = ref(true);
let loading = false;
let selectedRow = null;

//初次讀取
onMounted(async () => {
  dtObj.value = new Tabulator(dtElm.value, {
    layout: "fitColumns",
    height: "400px",
    selectableRows: 1, //只允許單行選擇
    columns: [
      { title: "ID", field: "id", visible: false, sorter: "number" },
      { title: "科目編號", field: "accountNo", width: 100 },
      { title: "科目名稱", field: "accountName", width: 100 },
      {
        title: "摘要",
        field: "summary",
        editor: "input",
        width: 380,
      },
      {
        title: "借方",
        field: "amount-D",
        editor: "input",
        width: 100,
        bottomCalc: "sum", // 底部顯示總和
        bottomCalcFormatter: moneyFormatter, // 格式化為金額
        formatter: moneyFormatter,
        hozAlign: "right",
      },
      {
        title: "貸方",
        field: "amount-C",
        editor: "input",
        width: 100,
        bottomCalc: "sum", // 底部顯示總和
        bottomCalcFormatter: moneyFormatter, // 格式化為金額
        formatter: moneyFormatter,
        hozAlign: "right",
      },
    ],
    data: [], // 將假資料放入
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
  //選中資料
  dtObj.value.on("rowClick", (e, row) => {
    noSelected.value = row.isSelected() ? false : true;
    if (row.isSelected()) {
      selectedRow = row;
    }
  });

  //初次載入時抓取資料
  await nextTick();
});

//金額格式
const moneyFormatter = (cell) => {
  const value = cell.getValue();
  if (isNaN(value)) {
    return value;
  }
  return new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  }).format(value);
};
//檢查借貸平衡
const checkBalance = () => {
  // 取得底部計算的結果
  let calcResults = dtObj.value.getCalcResults();

  // 獲取借方與貸方總和
  let sumAmountD = calcResults.bottom["amount-D"];
  let sumAmountC = calcResults.bottom["amount-C"];

  if (sumAmountD == 0 && sumAmountC == 0) {
    console.info("借貸金額不得同時為0");
    return false;
  }

  if (sumAmountD != sumAmountC) {
    console.info("借貸未平衡，請再檢查輸入金額");
    return false;
  }

  return true;
};
//開啟科目選擇彈窗
const addAccount = () => {
  dialogTitle.value = "新增分錄科目";
  selectAcctDialogVisible.value = true;
};
//關閉彈窗事件
const dialogClosed = () => {
  selectAcctDialogVisible.value = false;
};
//選定科目後按下確定
const insertAcct = (acct) => {
  selectAcctDialogVisible.value = false;
  dtObj.value.addRow({
    id: acct.id,
    accountNo: acct.no,
    accountName: acct.name,
    summary: "",
    "amount-D": "",
    "amount-C": "",
  });
};

const deleteAccount = () => {
   selectedRow.delete();
};

//打包畫面資料
const packData = () => {};

//儲存
const save = () => {
  console.info("save");
  if (checkBalance() == false) {
    return;
  }

  

  //router.push({ path: "/je" });
};

//送審
const submit = () => {
  console.info("submit");
  if (checkBalance == false) {
    return;
  }

  // router.push({ path: "/je" });
};
//取消編輯
const cancel = () => {
  console.info("cancel");
  router.push({ path: "/je" });
};
</script>

<style scoped>
.container {
  display: flex;
  flex-wrap: wrap;
  gap: 1px;
}

.item {
  flex: 1; /* 項目平均分配空間 */
  padding: 5px;
  display: flex; /* 讓 label 和 input 並排 */
  align-items: center; /* 垂直置中 */
}
.item label {
  width: 60px; /* 根據需要調整 label 寬度 */
  margin-right: 5px;
  text-align: right;
}
.item input,
.item select {
  width: 50px;
  font-size: 18px;
  flex: 1;
}

.summary {
  width: 100%; /* 摘要佔滿整列 */
  padding: 5px;
  margin-top: 10px;
  display: flex;
  align-items: center;
}
.summary label {
  width: 60px;
  margin-right: 5px;
  text-align: right;
  font-size: 15px;
}
.summary input {
  flex: 1;
  font-size: 20px;
}

@media (max-width: 768px) {
  /* 手機等小螢幕 */
  .item {
    flex-direction: column;
    align-items: flex-start;
  }
  .item label {
    text-align: left;
    width: 100%;
    margin-bottom: 5px;
  }
}
/* 工具列 */
#topTools {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
