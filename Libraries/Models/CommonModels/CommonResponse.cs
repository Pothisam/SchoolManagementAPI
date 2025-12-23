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
    public class APIRequestDetails
    {
        public required string UserName { get; set; }
        public required int InstitutionCode { get; set; }
        public required string LoginType { get; set; }
        public required int SysId { get; set; }
        public required bool Ispricipal { get; set; }


    }
    public class InstitutionLogoResponse
    {
        public string? Logo { get; set; }
        public string? LogoWithText { get; set; }
        public string? FavIcon { get; set; }
    }
    public class RecordHistoryResponse
    {
        public required string ModifiedBy { get; set; }
        public required DateTime ModifiedDate { get; set; }
        public required string Data { get; set; }
    }
    public class PostOfficeResponse
    {
        public string? OfficeName { get; set; }
        public string? Districtname { get; set; }
        public string? StateName { get; set; }
    }
    public class AutoCompleteResponse
    {
        public string? Column { get; set; }
    }
}
