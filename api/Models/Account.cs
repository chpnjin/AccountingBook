namespace api.Models
{
    /// <summary>
    /// 科目基本內容
    /// </summary>
    public class Account
    {
        public int id { get; set; }
        public string no { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int? main_id { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
    }

    /// <summary>
    /// 查詢子科目膗入參數
    /// </summary>
    public class GetSubAccountInput
    {
        public int main_id { get; set; }
    }
}
