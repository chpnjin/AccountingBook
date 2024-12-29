<template>
  <h1>科目設定</h1>
  <p>主科目</p>
  <div class="button-container">
    <button @click="openMainEditDialog">新增</button>
    <button>編輯</button>
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
      <button @click="openSubEditDialog">新增</button>
    </div>
    <div ref="table2" class="table-container"></div>
  </div>

  <!-- 主科目編輯彈窗 -->
  <editDialog
    title="新增主科目"
    v-model:visible="mainEditDialogVisible"
    :editTargetId="selectedMain.id"
    @save="handleSaveMainAccount"
    @close="handleDialogClose"
  ></editDialog>
  <!-- 子科目編輯彈窗 v-model 綁定屬性-->
  <editSubDialog
    title="新增子科目"
    v-model:visible="subEditDialogVisible"
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
import editDialog from "@/components/Dialog_Account.vue"; //引用彈出視窗組件
import editSubDialog from "@/components/Dialog_SubAccount.vue"; //引用彈出視窗組件

//引用物件
const table1 = ref(null);
const table2 = ref(null);
const obj_TMain = ref(Tabulator); //主科目物件
const obj_TSub = ref(Tabulator); //子科目物件
const data_Main = ref([]); //主科目資料
const data_Sub = ref([]); //子科目資料
const loading = ref(false);
const selectedMain = ref([]); //被選中的主科目
const selectedSub = ref([]); //被選中的子科目

//控制編輯彈窗隱藏
const mainEditDialogVisible = ref(false);
const subEditDialogVisible = ref(false);

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
      { title: "科目類型", field: "type", width: 80 },
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
  obj_TMain.value.on("rowClick", function (e, row) {
    let rowData = row.getData();
    // console.info(rowData.id + ":" + rowData.name);
    //選中時紀錄選住得主科目資料
    selectedMain.value = row.isSelected() ? rowData : [];
    reloadSub(selectedMain.value.id);
  });

  // 使用 nextTick 確保 DOM 更新完成後再載入資料
  await nextTick();
  await reloadMain();
  obj_TSub.value.setData([]);
});

//事件:開啟編輯彈窗
const openMainEditDialog = () => {
  mainEditDialogVisible.value = true;
};
const openSubEditDialog = () => {
  subEditDialogVisible.value = true;
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
}

//子組件事件:儲存主科目
const handleSaveMainAccount = (model) => {
  console.log(model);
  accountService.addMainAccount(model);
  reloadMain();
};
//子組件事件:儲存子科目
const handleSaveSub = (model) => {
  console.log(model);
  accountService.addSubAccount(model);
  reloadSub(selectedMain.value.id);
};

//子組件事件:關閉視窗
const handleDialogClose = () => {};
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
  background-color: #4CAF50; /* 設定背景顏色 */
  color: white; /* 設定文字顏色 */
  cursor: pointer;
  font-size: 15px;
  transition: background-color 0.3s ease; /* 加入過渡效果 */
}

.button-container button:hover {
  background-color: #45a049; /* 滑鼠懸停時改變背景顏色 */
}
</style>
