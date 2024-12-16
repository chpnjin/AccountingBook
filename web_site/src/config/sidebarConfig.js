export default [
  {
    menu: 'JournalEntry',
    title: '傳票作業',
    submenu: [
      { name: 'je-edit', label: '編輯' },
      { name: 'je-approval', label: '審核' },
    ],
  },
  {
    menu: 'Report',
    title: '報表',
    submenu: [
      { name: 'report-income-statement', label: '損益表' },
      { name: 'report-balance-sheet', label: '資產負債表' },
    ],
  },
  {
    menu: 'Setup',
    title: '設定',
    submenu: [
      { name: 'setup-user', label: '使用者' },
      { name: 'setup-accounts', label: '科目' },
    ],
  },
];
