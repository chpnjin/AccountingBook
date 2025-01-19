<!-- je/edit -->
<template>
  <div class="container">
    <div class="item">
      <label>編號</label> <input type="text" v-model="master.no" disabled />
    </div>
    <div class="item">
      <label for="select-option">類型</label>
      <select id="select-option" v-model="master.voucher_type">
        <option v-for="item in vchrType" :key="item.value" :value="item.value">
          {{ item.text }}
        </option>
      </select>
    </div>
    <div class="item">
      <label>日期</label> <input type="date" v-model="master.entry_date" />
    </div>
    <div class="summary">
      <label for="summary-input">摘要</label>
      <input type="text" id="summary-input" v-model="master.summary" />
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
  <!-- 狀態與簽核者資訊 -->
  <div class="statusBar">
    <span>狀態:{{ options.find((x) => x.value == master.status).text }}</span>
    <button @click="showSignedStatus">簽核狀態</button>
  </div>
  <div class="btnBar">
    <button @click="save">儲存</button>
    <button @click="submit">送審</button>
    <button @click="cancel">取消</button>
  </div>
  <editDialog
    :visible="selectAcctDialogVisible"
    :title="dialogTitle"
    @close="dialogClosed"
    @selected="insertAcct"
  ></editDialog>
</template>

<script setup>
import { ref, reactive, toRaw, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import editDialog from "@/components/Dialog_ChoiceAcct.vue"; //選擇科目彈窗
import "tabulator-tables/dist/css/tabulator.min.css";
import { useRouter } from "vue-router";
import service from "@/services/voucherService"; //API
import formatters from "@/config/formatter.js"; // 導入格式化函式陣列
import options from "@/config/text-value";

const router = new useRouter();
const dtElm = ref(null);
const pagerElm = ref(null);
const selectAcctDialogVisible = ref(false);
const dialogTitle = ref("");
const vchrType = [
  { text: "期初開帳", value: "opening" },
  { text: "現金收入", value: "income" },
  { text: "現金支出", value: "expense" },
  { text: "轉帳", value: "transfer" },
];
const noSelected = ref(true);
const dtData = reactive([]);

let loading = false;
let selectedRow;
let dtObj = null; //Tabulator 實例

//單頭資料
const master = reactive({
  no: "新增完成後自動編號",
  entry_date: "",
  voucher_type: "",
  summary: "",
  handler: localStorage.getItem("id"),
  status: "unapproved",
});

//初次讀取
onMounted(async () => {
  await nextTick()
    .then(() => {
      dtObj = new Tabulator(dtElm.value, {
        data: dtData,
        layout: "fitColumns",
        height: "340px",
        selectableRows: 1, //只允許單行選擇
        movableRows: true,
        columns: [
          {
            title: "ID",
            field: "account_id",
            visible: false,
            sorter: "number",
          },
          { title: "科目編號", field: "account_no", width: 100 },
          { title: "科目名稱", field: "account_name", width: 100 },
          {
            title: "摘要",
            field: "summary",
            editor: "input",
            width: 380,
          },
          {
            title: "借方",
            field: "debit_amount",
            editor: "input",
            width: 100,
            bottomCalc: "sum", // 底部顯示總和
            bottomCalcFormatter: formatters.moneyFormatter, // 格式化為金額
            formatter: formatters.moneyFormatter,
            hozAlign: "right",
          },
          {
            title: "貸方",
            field: "credit_amount",
            editor: "input",
            width: 100,
            bottomCalc: "sum", // 底部顯示總和
            bottomCalcFormatter: formatters.moneyFormatter, // 格式化為金額
            formatter: formatters.moneyFormatter,
            hozAlign: "right",
          },
        ],
        placeholder: () => {
          if (loading) {
            return "資料載入中..."; // 顯示載入中訊息
          } else if (dtData.length == 0) {
            return "沒有資料"; // 顯示沒有資料訊息
          } else {
            return ""; // 有資料時不顯示任何訊息
          }
        },
      });
    })
    .then(() => {
      //選中資料
      dtObj.on("rowClick", (e, row) => {
        noSelected.value = row.isSelected() ? false : true;
        selectedRow = row.isSelected() ? row : null;
      });

      //移動資料
      dtObj.on("rowMoved", function () {
        // 取得目前所有行
        const rows = dtObj.getRows();
        // 根據行順序更新原始資料
        const updatedData = rows.map((r) => r.getData());
        dtData.length = 0; // 清空原始資料
        updatedData.forEach((item) => dtData.push(item)); // 更新 Vue 的反應式資料
      });
    })
    .then(async () => {
      // 網址帶參數時重抓資料
      let voucherNo = router.currentRoute.value.query.no;

      if (voucherNo) {
        let result = await service.getVoucherDetail(voucherNo);

        if (result) {
          Object.assign(master, result.master);
          Object.assign(dtData, result.detail);
          dtObj.replaceData(dtData);
        }
      }
    });
});

//檢查填寫內容
const checkForm = () => {
  //單頭欄位檢查
  if (master.voucher_type == "") {
    alert("請選擇傳票類型");
    return false;
  }

  if (master.entry_date == "") {
    alert("請選擇交易日期");
    return false;
  }

  if (master.summary == "") {
    alert("請輸入摘要");
    return false;
  }

  // 單身科目驗證
  let data = dtObj.getData();

  // 取得底部計算的結果
  let calcResults = dtObj.getCalcResults();

  // 獲取借方與貸方總和
  let sumAmountD = calcResults.bottom["debit_amount"];
  let sumAmountC = calcResults.bottom["credit_amount"];

  if (data.length < 2) {
    alert("科目至少要有兩個以上");
    return false;
  }

  if (sumAmountD == 0 && sumAmountC == 0) {
    alert("借貸金額不得同時為0");
    return false;
  }

  if (sumAmountD != sumAmountC) {
    alert("借貸未平衡，請再檢查輸入金額");
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
  dtObj.addRow({
    account_id: acct.id,
    account_no: acct.no,
    account_name: acct.name,
    summary: "",
    debit_amount: 0,
    credit_amount: 0,
  });
};

const deleteAccount = () => {
  selectedRow.delete();
};

//打包畫面資料
const packData = () => {
  let data = {};
  let items = dtObj.getData();

  if (router.currentRoute.value.query.no == null) {
    master.no = "";
  }

  data.master = toRaw(master);
  data.detail = items;

  return data;
};

//顯示簽核狀態
const showSignedStatus = () => {};

//儲存
const save = async () => {
  if (checkForm() == false) {
    return;
  }

  let result = await service.editVoucher(packData());

  if (result == "Done") {
    alert("儲存成功");
    router.push({ path: "/je" });
  } else {
    let msg = "儲存失敗，訊息: \n" + result;
    alert(msg);
  }
};

//送審
const submit = () => {
  if (checkForm() == false) {
    return;
  }
  console.info(packData().detail);

  // router.push({ path: "/je" });
};

//取消編輯
const cancel = () => {
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
  padding: 1px;
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
  width: 100%;
  font-size: 18px;
  flex: 1;
}

.item input[type="date"] {
  width: 130px;
}

.summary {
  width: 100%; /* 摘要佔滿整列 */
  display: flex;
  flex-flow: column;
}
.summary label {
  width: 60px;
  margin-right: 5px;
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
  margin: 5px 5px 5px 0px;
}

#topTools button {
  font-size: 20px;
  margin-right: 5px;
}

.btnBar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px; /*按鈕間距*/
  padding: 5px 2px 2px 2px; /* 容器內距 */
}

.btnBar button {
  padding: 5px 10px;
  cursor: pointer;
  transition: background-color 0.3s ease; /* 加入過渡效果 */
  font-size: 18px;
  width: 100px;
}

.statusBar {
  display: flex;
  padding: 5px 2px 5px 2px; /* 容器內距 */
  font-size: 20px;
}

.statusBar button {
  margin-left: auto; /* 確保按鈕在右側 */
  font-size: 20px;
}

button {
  border: none; /* 移除預設邊框 */
  border-radius: 5px;
  background-color: #4caf50; /* 設定背景顏色 */
  color: white; /* 設定文字顏色 */
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
</style>
