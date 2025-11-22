using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CommonModels
{
    public class CommonResponse<T> where T : class
    {
        public string? Message { get; set; }
        public Status Status { get; set; }
        public T? Data { get; set; }
    }
    public enum Status
    {
        Success = 200,
        Failed = 300
    }

}
