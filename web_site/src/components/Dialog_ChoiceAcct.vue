<!-- 彈窗:編輯傳票選擇科目 -->
<template>
  <div v-if="visible" class="dialog-overlay">
    <div class="dialog">
      <div class="dialog-header">
        <h2>{{ title }}</h2>
        <!-- click動作綁定Method : closeDialog -->
        <button @click="closeDialog" class="close-button">&times;</button>
      </div>
      <div class="dialog-body">
        <div class="dropdown">
          <div class="dropdown-container">
            <label>主科目</label>
            <input
              type="text"
              v-model="selectedMainAcctName"
              placeholder="請選擇..."
              @focus="handleFocus"
              @blur="handleBlur"
              @input="filterOptions"
              class="search-input"
            />
          </div>
          <ul v-if="showMainList" class="options">
            <!-- 只顯示過濾的項目 -->
            <li
              v-for="item in filteredAcctList"
              :key="item.id"
              @click="selectMain(item)"
            >
              {{ item.text }}
            </li>
          </ul>
        </div>
        <div>
          <label>子科目</label>
          <div ref="dtDialogElm"></div>
        </div>
      </div>
      <div class="button-container">
        <button @click="choiceAcct" :disabled="choicedSubAcct">確認</button>
        <button @click="closeDialog">取消</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, nextTick} from "vue";
import { TabulatorFull as Tabulator } from "tabulator-tables";
import "tabulator-tables/dist/css/tabulator.min.css";
import service from "@/services/accountService"; //API

const props = defineProps({
  title: {
    type: String,
    default: "選擇分錄科目",
  },
  //視窗是否可見
  visible: {
    type: Boolean,
    default: false,
  },
});

//定義父組件可訂閱事件
const emit = defineEmits(["selected", "close"]);
const selectedMainAcctName = ref(""); //主科目查詢欄位文字
const mainAcctList = ref([]); //主科目
const filteredAcctList = ref([]);
const showMainList = ref(false);
const choicedSubAcct = ref(true);

const dtObj = ref(null);
const dtDialogElm = ref(null);
const dtSubAcctList = [];
let selectedSubAcct = {};
let loading = false;

//視窗開啟時，重建子科目清單
watch(
  () => props.visible,
  async (isOpen) => {
    if (isOpen) {
      // 確保 DOM 已經渲染
      await nextTick()
        .then(() => {
          //子科目表格
          dtObj.value = new Tabulator(dtDialogElm.value, {
            layout: "fitColumns",
            height: "400px",
            selectableRows: 1,
            columns: [
              { title: "ID", field: "id", visible: false, sorter: "number" },
              { title: "科目編號", field: "no", width: 100 },
              { title: "科目名稱", field: "name", width: 320 },
            ],
            placeholder: () => {
              if (loading) {
                return "資料載入中..."; // 顯示載入中訊息
              } else if (dtSubAcctList.length == 0) {
                return "沒有資料"; // 顯示沒有資料訊息
              } else {
                return ""; // 有資料時不顯示任何訊息
              }
            },
          });
          return;
        })
        .then(() => {
          dtObj.value.on("rowClick", function (e, row) {
            if (row.isSelected()) {
              selectedSubAcct = row.getData();
              console.info(selectedSubAcct);
            }
            choicedSubAcct.value = row.isSelected() ? false : true;
          });
          dtDialogElm.value.classList.add("my-table");
        });
    }
    //從DB抓取主科目選項
    let result = await service.getMainAccounts();
    mainAcctList.value = result.map((account) => ({
      id: account.id, // 將 id 存為 key
      text: account.name, // 將 name 存為 text
    }));
    filteredAcctList.value = [...mainAcctList.value];
  }
);

// 過濾主科目選項
const filterOptions = () => {
  const query = selectedMainAcctName.value.toLowerCase();
  filteredAcctList.value = mainAcctList.value.filter((option) =>
    option.text.toLowerCase().includes(query)
  );
};

// 點選主科目欄位後顯示下拉選單
const handleFocus = () => {
  showMainList.value = true;
};

// 選擇選項，並關閉主科目下拉選單
const selectMain = (item) => {
  selectedMainAcctName.value = item.text;
  showMainList.value = false; // 隱藏選單
  UpdateSubAcctList(item.id);
};

// 失去滑鼠焦點後隱藏選單
const handleBlur = () => {
  setTimeout(() => {
    showMainList.value = false;
  }, 300); //延遲隱藏選項，為了能觸發 click 事件
};

//更新子科目清單
const UpdateSubAcctList = async (id) => {
  loading = true;
  let data = await service.getSubAccounts(id);

  if (data.length > 0) {
    dtObj.value.setData(data);
  }
};

// 關閉對話框
const closeDialog = () => {
  selectedMainAcctName.value = "";
  emit("close"); // 觸發對話框關閉事件
};

//選擇科目傳回父組件
const choiceAcct = () => {
  selectedMainAcctName.value = "";
  emit("selected", selectedSubAcct);
};
</script>

<style scoped>
.dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.dialog {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  min-width: 390px;
}

.dialog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
  border-bottom: 1px solid #eee;
  padding-bottom: 10px;
}

/* 整個主科目 */
.dropdown {
  position: relative;
}

.dropdown-container {
  /* display: flex; */
  align-items: center;
}

.search-input {
  width: 100%; /* 輸入框寬度設置為 100% */
  flex: 1;
  padding: 8px;
  border: 1px solid #4caf50;
  border-radius: 4px;
  font-size: 16px;
}

.options {
  position: absolute; /* 絕對定位下拉選項 */
  top: 100%; /* 讓選項在下方顯示 */
  left: 0;
  right: 0; /* 寬度與輸入框一致 */
  max-height: 200px; /* 設定最大高度 */
  overflow-y: auto; /* 當選項超過最大高度時，啟用滾動條 */
  background: white; /* 設定背景顏色 */
  border: 1px solid #ccc; /* 邊框設定 */
  z-index: 10; /* 在其他元素之上顯示 */
  padding-left: 10px;
}

li {
  cursor: pointer; /* 鼠標懸停時變成指針 */
  list-style-type: none; /* 消除 li 項目的點點 */
}

li:hover {
  background-color: #f0f0f0; /* 鼠標懸停時的背景顏色 */
}

.form-group {
  margin-bottom: 10px;
}

label {
  margin-right: 10px;
}

textarea {
  width: 100%;
  height: 100px;
  resize: vertical;
}

.close-button {
  font-size: 20px;
  cursor: pointer;
  border: none;
}

button {
  margin-top: 10px;
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  background-color: #4caf50;
  color: white;
}

button:hover {
  background-color: #38a372;
}

.button-container {
  display: flex;
  flex-direction: row-reverse;
  gap: 10px; /*按鈕間距*/
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

/* 表格外框 */
.my-table {
  border: 2px solid #4caf50; /* 設定外框的顏色和寬度 */
  border-radius: 5px; /* 可選：添加圓角 */
  overflow: hidden; /* 確保邊框正確顯示 */
}
</style>
