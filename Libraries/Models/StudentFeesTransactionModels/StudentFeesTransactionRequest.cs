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
}
