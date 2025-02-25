using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    /// <summary>
    /// 傳票單頭
    /// </summary>
    public class Voucher
    {
        [Key]
        [MaxLength(20)]
        public string? no { get; set; }

        [Required(ErrorMessage = "交易日期為必填")]
        [DataType(DataType.Date)]
        public DateTime entry_date { get; set; }

        [Required(ErrorMessage = "傳票類型為必填")]
        public required string voucher_type { get; set; }

        public string? summary { get; set; }

        public decimal amount { get; set; }

        public int handler { get; set; }

        public int? reviewer { get; set; }

        [Required(ErrorMessage = "狀態為必填")]
        public required string status { get; set; }
    }
    /// <summary>
    /// 傳票單身
    /// </summary>
    public class VoucherDetail
    {
        [Key]
        [MaxLength(20)]
        public string? no { get; set; }

        [Key]
        public int? seq { get; set; } = 0;

        [Required(ErrorMessage = "科目代號必填")]
        public int account_id { get; set; }
        public string? account_no { get; set; }
        public string? account_name { get; set; }
        public string? summary { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal debit_amount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal credit_amount { get; set; } = 0.00m;
    }
    /// <summary>
    /// Input:編輯傳票明細
    /// </summary>
    public class VoucherInput
    {
        public Voucher master {  get; set; }
        public List<VoucherDetail> detail { get; set; }
    }
    /// <summary>
    /// Input:總攬查詢條件
    /// </summary>
    public class SearchCondition
    {
        public string? date_start { get; set; }
        public string? date_end { get; set; }
        public string? summary { get; set; }
        public int[]? account_list { get; set; }
    }
}
