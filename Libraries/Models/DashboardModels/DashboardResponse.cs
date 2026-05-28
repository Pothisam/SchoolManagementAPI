namespace Models.DashboardModels
{
    public class StudentCountClassWiseResponse
    {
        public int ClassSysId { get; set; }
        public string? ClassName { get; set; }
        public int StudentCount { get; set; }
    }

    public class FeesSummaryClassWiseResponse
    {
        public int ClassSysId { get; set; }
        public string? ClassName { get; set; }
        public decimal TotalFeesAmount { get; set; }
        public decimal FeesAmountReceived { get; set; }
        public decimal FeesBalance { get; set; }
    }
}