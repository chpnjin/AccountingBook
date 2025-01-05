<template>
  <nav class="sidebar">
    <ul>
      <li>
        <a href="#" class="nav-item" @click.prevent="toggleSubMenu('JournalEntry')">傳票作業</a>
        <ul v-show="isSubMenuOpen.JournalEntry" class="sub-menu">
          <li>
            <a href="#" class="sub-nav-item" @click.prevent="navigateTo('je')">編輯</a>
          </li>
          <li>
            <a href="#" class="sub-nav-item" @click.prevent="navigateTo('je-approval')">審核</a>
          </li>
        </ul>
      </li>
      <li>
        <a href="#" class="nav-item" @click.prevent="toggleSubMenu('Report')">報表</a>
        <ul v-show="isSubMenuOpen.Report" class="sub-menu">
          <li>
            <a href="#" class="sub-nav-item" @click.prevent="navigateTo('report-income-statement')">損益表</a>
          </li>
          <li>
            <a href="#" class="sub-nav-item" @click.prevent="navigateTo('report-balance-sheet')">資產負債表</a>
          </li>
        </ul>
      </li>
      <li>
        <a href="#" class="nav-item" @click.prevent="toggleSubMenu('Setup')">設定</a>
        <ul v-show="isSubMenuOpen.Setup" class="sub-menu">
          <li>
            <a href="#" class="sub-nav-item" @click.prevent="navigateTo('setup-user')">使用者</a>
          </li>
          <li>
            <a href="#" class="sub-nav-item" @click.prevent="navigateTo('setup-accounts')">科目</a>
          </li>
        </ul>
      </li>
    </ul>
  </nav>
</template>

<script>
export default {
  name: 'SidebarNav',
  data() {
    return {
      //預設隱藏
      isSubMenuOpen: {
        JournalEntry: false,
        Report: false,
        Setup: false,
      },
    };
  },
  methods: {
    //點選主項目時收放子項目清單
    toggleSubMenu(menu) {
      this.isSubMenuOpen[menu] = !this.isSubMenuOpen[menu];
    },
    navigateTo(routeName) {
      this.$router.push({ name: routeName }); // 切換到對應路由
    },
  },
};
</script>

<style scoped>
.sidebar {
  width: 250px;
  background: #34495e;
  color: #ecf0f1;
  padding: 0;
  box-sizing: border-box;
  overflow-y: auto;
  height: 100%;
}

ul {
  list-style-type: none; /* 去除左側的項目符號 */
  padding: 0; /* 去除內邊距 */
  margin: 0; /* 去除外邊距 */
}

.nav-item {
  display: block; /* 使 <a> 標籤像塊級元素一樣 */
  width: 100%; /* 全寬 */
  padding: 15px;
  font-size: px;
  font-weight: bold;
  background-color: #2c3e50;
  border: none;
  color: white;
  text-align: left;
  cursor: pointer;
  box-sizing: border-box;
  border-bottom: 1px solid #7f8c8d;
  transition: background-color 0.3s;
  text-decoration: none; /* 移除下劃線 */
}

.nav-item:hover {
  background-color: #1abc9c;
}

.sub-menu {
  padding-left: 20px;
  display: none;
  list-style: none; /* 去除子菜單的項目符號 */
  padding: 0;
  margin: 0;
}

.sub-nav-item {
  display: block; /* 使 <a> 標籤像塊級元素一樣 */
  width: 100%; /* 全寬 */
  padding: 10px;
  font-size: 14px;
  font-weight: normal;
  background-color: #16a085;
  border: none;
  color: white;
  text-align: left;
  cursor: pointer;
  box-sizing: border-box;
  text-decoration: none; /* 移除下劃線 */
  transition: background-color 0.3s;
}

.sub-nav-item:hover {
  background-color: #1abc9c;
}

.sub-menu[style] {
  display: block;
  transition: all 0.3s ease-in-out;
}
</style>
