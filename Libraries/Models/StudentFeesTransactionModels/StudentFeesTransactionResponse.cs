using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.StudentFeesTransactionModels
{
    public class StudentFeesTransactionResponse
    {
        public int SysId { get; set; }
        public string Stdid { get; set; }
        public string rollno { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string Year { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public Guid? Guid { get; set; }
        public int AcadamicYear { get; set; }
    }
    public class GetDebitItemResponse
    {
        public int SysId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public string Status { get; set; }
        public string EntryBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime GenerateDate { get; set; }
        public int FeesId { get; set; }
    }
    public class GetDebitResponse
    {
        public List<GetDebitItemResponse> R1 { get; set; } = new List<GetDebitItemResponse>();
        public decimal R2 { get; set; }
    }
    public class GetCreditItemResponse
    {
        public int SysId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public decimal Credit { get; set; }
        public string Status { get; set; }
        public string EntryBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime GenerateDate { get; set; }
        public int FeesId { get; set; }
    }
    public class GetCreditResponse
    {
        public List<GetCreditItemResponse> R1 { get; set; } = new List<GetCreditItemResponse>();
        public decimal R2 { get; set; }
    }
}
