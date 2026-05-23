using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.StudentFeesTransactionModels
{
    public class StudentFeesTransactionRequest
    {
        public required int Batch { get; set; }
        public required int Class { get; set; }
        public required int Section { get; set; }
    }
    public class StudentFeesTransactionByNameRequest
    {
        public required string StudentName { get; set; }
    }
    public class GetDebitRequest
    {
        public int SysId { get; set; }
        public int Batch { get; set; }
    }
    public class DeleteFeesTransactionRequest
    {
        public int SysId { get; set; }
        public string Remark { get; set; }
    }
    public class AddStudentFeesTransactionRequest
    {
        public int StudentFkid { get; set; }
        public int FeesTypeFkid { get; set; }
        public int StudentClassDetailsFkid { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime GenerateDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string? ChequeNo { get; set; }
        public string? ChequeDate { get; set; }
        public string? BankName { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
    public class GetFeesReportDateWiseRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FeesTypeFkid { get; set; }
    }
    public class GetPrintCashReceiptValueRequest
    {
        public int SysId { get; set; }
    }
}
