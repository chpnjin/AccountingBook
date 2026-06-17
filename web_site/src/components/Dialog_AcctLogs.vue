<template>
  <div hidden>首頁彈窗顯示包含科目的傳票明細清單</div>
  <Dialog :visible="visible" :title="title" :confirm-btn-text="btnText" :confirm-btn-disabled="!isRowSelected"
    @close="closeDialog" @confirm="jumpToEdit">
    <span>{{ accountName }}</span>
    <!-- 表格 -->
    <div ref="dtElm"></div>
  </Dialog>
</template>

<script setup>
import { ref, reactive, watch, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import "tabulator-tables/dist/css/tabulator.min.css";
import Dialog from "./Dialog_.vue";
import voucherService from "@/services/voucherService";
import formatters from "@/config/formatter.js"; // 導入格式化函式陣列

//父組件可設定屬性
const props = defineProps({
  visible: { type: Boolean, required: true },
  title: { type: String, default: "科目餘額異動明細" },
  accountName: { type: String }
});

//定義事件
const emit = defineEmits(["close","jumpToEdit"]);

const btnText = "跳至傳票編輯"
const dtElm = ref(null);
const dtData = reactive([]);
const isRowSelected = ref(false); //選中表格某列

let dtObj = ref(Tabulator); //Tabulator 實例

//每次開啟彈窗時重抓資料
watch(() => props.visible, (newValue) => {
  if (newValue) {
    nextTick(async () => {
      dtData.value = await voucherService.getAcctTranDetails(props.accountName);

      dtObj.value = new Tabulator(dtElm.value, {
        data: dtData.value,
        height: "400px",
        selectableRows: 1, //只允許單行選擇
        columns: [
          {
            title: "交易日期",
            field: "entry_date",
            width: 90,
            resizable: false
          },
          {
            title: "摘要",
            field: "summary",
            width: 200,
          },
          {
            title: "借方",
            field: "debit_amount",
            formatter: formatters.moneyFormatter,
            hozAlign: "right",
            resizable: false,
            width: 100,
          },
          {
            title: "貸方",
            field: "credit_amount",
            formatter: formatters.moneyFormatter,
            hozAlign: "right",
            resizable: false,
            width: 100,
          },
          {
            title: "編號",
            field: "no",
            visible: false //隱藏編號欄位,僅作為跳轉頁面時使用
          },
        ]
      });
      //事件:選中一行以上才能跳轉傳票編輯
      dtObj.value.on("rowSelectionChanged", function (data) {
        isRowSelected.value = data.length > 0;
      });

    }); //End of nextTick
  } else {
    dtObj.value.destroy();
  }
});

//關閉視窗
const closeDialog = () => {
  emit("close");
};
//跳至傳票編輯頁面
const jumpToEdit = () => {
  let selectedRows = dtObj.value.getSelectedData();

  if (selectedRows.length > 0) {
    let voucherNo = selectedRows[0].no;
    emit("jumpToEdit", voucherNo);
  }
}

</script>
<style scoped></style>
