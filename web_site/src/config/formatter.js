export default {
  moneyFormatter(cell) {
    const value = cell.getValue();
    if (isNaN(value)) {
      return value;
    }
    return new Intl.NumberFormat("en-US", {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
    }).format(value);
  }, langsSettings() {
    return {
      "zh-tw": { // 設定繁體中文的語言設定
        "pagination": {
          "page_size": "每頁筆數",
          "first": "|<",
          "first_title": "前往第一頁",
          "last": ">|",
          "last_title": "前往最後一頁",
          "prev": "<",
          "prev_title": "前往上一頁",
          "next": ">",
          "next_title": "前往下一頁",
          "all": "全部",
        },
      }
    }
  }
}
