<template>
  <h1>科目設定</h1>
  <p>主科目</p>
  <div class="button-container">
    <button id="main-add" @click="openDialog">新增</button>
    <button id="main-update" @click="openDialog" :disabled="btnDisableMain">
      編輯
    </button>
    <button @click="inActiveMain" :disabled="btnDisableMain">停用</button>
  </div>
  <!-- 主科目表 -->
  <div ref="table1"></div>
  <!-- 子科目表 -->
  <div class="subsection">
    <p class="subsection-title">子科目</p>
    <label for="select-mainAccount">對應主科目：</label>
    <span>{{ selectedMain.name }}</span>
    <br />
    <div class="button-container">
      <button id="sub-add" @click="openDialog" :disabled="btnDisableMain">
        新增
      </button>
      <button id="sub-update" @click="openDialog" :disabled="btnDisableSub">
        編輯
      </button>
      <button @click="inActiveSub" :disabled="btnDisableSub">停用</button>
    </div>
    <div ref="table2" class="table-container"></div>
  </div>

  <!-- 主科目編輯彈窗 -->
  <editDialog
    :title="dialogTitle"
    :visible="mainDialogVisible"
    :input-data="selectedMain"
    @save="handleSaveMainAccount"
    @close="handleDialogClose"
  ></editDialog>
  <!-- 子科目編輯彈窗 v-model 綁定屬性-->
  <editSubDialog
    :title="dialogTitle"
    :visible="subDialogVisible"
    :type-name="typeName"
    :parent-account="selectedMain"
    :editing-account="selectedSub"
    @save="handleSaveSub"
    @close="handleDialogClose"
  >
  </editSubDialog>
</template>

<script setup>
import { ref, onMounted, nextTick } from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import accountService from "@/services/accountService"; //API呼叫服務
import "tabulator-tables/dist/css/tabulator_simple.min.css"; // 引入 simple 主題
import editDialog from "@/components/Dialog_Account.vue";
import editSubDialog from "@/components/Dialog_SubAccount.vue"; //引用彈出視窗組件
import options from "@/config/text-value.js";

//引用物件
const dialogTitle = ref(""); //彈窗標題
const table1 = ref(null);
const table2 = ref(null);
const obj_TMain = ref(Tabulator); //主科目物件
const obj_TSub = ref(Tabulator); //子科目物件
const data_Main = ref([]); //主科目資料
const data_Sub = ref([]); //子科目資料
const loading = ref(false);
const selectedMain = ref([]); //被選中的主科目
const selectedSub = ref([]); //被選中的子科目
const btnDisableMain = ref(true); //
const btnDisableSub = ref(true);
const typeName = ref("");

//控制編輯彈窗隱藏
const mainDialogVisible = ref(false);
const subDialogVisible = ref(false);

onMounted(async () => {
  //主科目表
  obj_TMain.value = new Tabulator(table1.value, {
    layout: "fitColumns",
    height: "200px",
    selectableRows: 1, //只允許單行選擇
    columns: [
      { title: "ID", field: "id", visible: false, sorter: "number" },
      { title: "科目編號", field: "no", width: 100 },
      { title: "科目名稱", field: "name", width: 150 },
      {
        title: "科目類型",
        field: "type",
        width: 80,
        formatter: (cell) => {
          const value = cell.getValue();
          const foundType = options.find((x) => x.value === value);
          return foundType ? foundType.text : "未找到"; // 如果找不到則回傳 "未找到"
        },
      },
      { title: "描述", field: "description", widthGrow: 1 }, // 描述欄位填滿剩餘寬度
      {
        title: "啟用?",
        field: "active",
        formatter: "tickCross",
        hozAlign: "center",
        width: 80,
      },
    ],
    placeholder: () => {
      if (loading.value) {
        return "資料載入中..."; // 顯示載入中訊息
      } else if (data_Main.value.length == 0) {
        return "沒有資料"; // 顯示沒有資料訊息
      } else {
        return ""; // 有資料時不顯示任何訊息
      }
    },
  });

  //子科目表
  obj_TSub.value = new Tabulator(table2.value, {
    layout: "fitColumns",
    height: "200px",
    selectableRows: 1, //只允許單行選擇
    columns: [
      { title: "ID", field: "id", visible: false, sorter: "number" },
      { title: "科目編號", field: "no", width: 100 },
      { title: "科目名稱", field: "name", width: 150 },
      { title: "描述", field: "description", widthGrow: 1 }, // 描述欄位填滿剩餘寬度
      {
        title: "啟用?",
        field: "active",
        formatter: "tickCross",
        hozAlign: "center",
        width: 80,
      },
    ],
    placeholder: () => {
      if (loading.value) {
        return "資料載入中..."; // 顯示載入中訊息
      } else if (data_Sub.value.length == 0) {
        return "沒有資料"; // 顯示沒有資料訊息
      } else {
        return ""; // 有資料時不顯示任何訊息
      }
    },
  });

  //事件:主科目列選取
  obj_TMain.value.on("rowClick", (e, row) => {
    let rowData = row.getData();

    //選中時紀錄選住得主科目資料
    selectedMain.value = row.isSelected() ? rowData : [];
    //被選中時允許編輯
    btnDisableMain.value = !row.isSelected();

    if (row.isSelected()) {
      btnDisableSub.value = true;
      //科目類型中文名稱
      typeName.value = options.find(
        (x) => x.value === selectedMain.value.type
      ).text;
      //重讀子科目
      reloadSub(selectedMain.value.id);
    }
  });

  //事件:子科目列選取
  obj_TSub.value.on("rowClick", (e, row) => {
    let rowData = row.getData();

    selectedSub.value = row.isSelected() ? rowData : [];
    btnDisableSub.value = !row.isSelected();
  });

  // 使用 nextTick 確保 DOM 更新完成後再載入資料
  await nextTick();
  await reloadMain();
  obj_TSub.value.setData([]);
});

//事件:開啟編輯彈窗
const openDialog = (e) => {
  let dialog = e.target.id;

  switch (dialog) {
    case "main-add":
      dialogTitle.value = "新增主科目";

      selectedMain.value = [];
      mainDialogVisible.value = true;
      break;
    case "main-update":
      dialogTitle.value = "編輯主科目";
      mainDialogVisible.value = true;
      break;
    case "sub-add":
      dialogTitle.value = "新增子科目";
      selectedSub.value = {};
      subDialogVisible.value = true;
      break;
    case "sub-update":
      dialogTitle.value = "編輯子科目";
      selectedSub.value = obj_TSub.value
        .getSelectedRows()
        .map((row) => row.getData())[0];
      subDialogVisible.value = true;
      break;
  }
};

//重抓主科目表
const reloadMain = async () => {
  loading.value = true; // 設定載入狀態為 true
  try {
    const data = await accountService.getMainAccounts();
    loading.value = false; // 設定載入狀態為 false
    obj_TMain.value.setData(data); //重新載入主科目表格資料，此方法會觸發placeholder事件
  } catch (error) {
    console.error("載入資料錯誤:", error);
    loading.value = false; // 設定載入狀態為 false
    obj_TMain.value.setPlaceholder("載入資料發生錯誤");
  }
};

//重抓子科目表
const reloadSub = async (mainId) => {
  loading.value = true; // 設定載入狀態為 true
  obj_TSub.value.setData([]);
  try {
    const data = await accountService.getSubAccounts(mainId);
    loading.value = false; // 設定載入狀態為 false
    obj_TSub.value.setData(data); //重新載入主科目表格資料，此方法會觸發placeholder事件
  } catch (error) {
    console.error("載入資料錯誤:", error);
    loading.value = false; // 設定載入狀態為 false
    obj_TSub.value.setPlaceholder("載入資料發生錯誤");
  }
};

//子組件事件:儲存主科目
const handleSaveMainAccount = (model) => {
  accountService.editMainAccount(model).then(() => {
    reloadMain();
  });
};
//子組件事件:儲存子科目
const handleSaveSub = (model) => {
  accountService.editSubAccount(model).then(() => {
    reloadSub(selectedMain.value.id);
  });
};

//子組件事件:關閉視窗
const handleDialogClose = () => {
  let selectedRows = obj_TMain.value.getSelectedRows();
  if (selectedRows.length > 0) {
    selectedMain.value = selectedRows[0].getData();
  }

  selectedRows = obj_TSub.value.getSelectedRows();
  if (selectedRows.length > 0) {
    selectedSub.value = selectedRows[0].getData();
  }

  mainDialogVisible.value = false;
  subDialogVisible.value = false;
};
</script>

<style scoped>
.table-container {
  border: 1px solid #ccc;
}

.subsection {
  margin-top: 20px;
  border: 1px solid #ccc;
  padding: 10px;
  position: relative; /* 關鍵：設定相對定位 */
}

.subsection-title {
  position: absolute; /* 關鍵：設定絕對定位 */
  top: -10px; /* 向上移動，使其超出外框 */
  left: 10px; /* 從左邊開始 */
  /* background-color: white; 讓標題底色與背景相同，遮蓋外框線 */
  padding: 0 5px; /* 增加左右內距，使文字周圍有空間 */
  font-weight: bold;
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
  font-size: 15px;
  transition: background-color 0.3s ease; /* 加入過渡效果 */
}

.button-container button:hover {
  background-color: #45a049; /* 滑鼠懸停時改變背景顏色 */
}

.button-container button:hover:not(:disabled) {
  /* 只有在未 disabled 時才套用 hover 效果 */
  background-color: #45a049;
}

.button-container button:disabled {
  background-color: #cccccc; /* disabled 時的背景顏色 (灰色) */
  color: #666666; /* disabled 時的文字顏色 (深灰色) */
  cursor: default; /* disabled 時的滑鼠游標 */
  opacity: 0.6; /* 降低透明度，更明顯區分 */
}
</style>
