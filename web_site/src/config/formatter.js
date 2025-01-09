export default [
  function moneyFormatter(cell) {
    const value = cell.getValue();
    if (isNaN(value)) {
      return value;
    }
    return new Intl.NumberFormat("en-US", {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2,
    }).format(value);
  }
]
