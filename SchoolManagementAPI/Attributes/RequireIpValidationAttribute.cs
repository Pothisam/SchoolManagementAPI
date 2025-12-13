namespace SchoolManagementAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequireIpValidationAttribute : Attribute
    {
    }
}
