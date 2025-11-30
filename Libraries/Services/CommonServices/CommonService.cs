using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.CommonModels;
using Models.ConfigurationModels;
using Models.UserModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.CommonServices
{
    public class CommonService : ICommonService
    {
        private readonly JwtConfig _jWTConfig;
        private readonly AppKeyConfig _appKeyConfig;
        public CommonService(IOptionsSnapshot<JwtConfig> jWTConfigOptions, IOptionsSnapshot<AppKeyConfig> appKeyConfigOptions)
        {
            _jWTConfig = jWTConfigOptions.Value;
            _appKeyConfig = appKeyConfigOptions.Value;
        }
        private static readonly string EncryptionKey = "My@Vision$To*Make!My#MoM^Proude";
        private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        

        public Task<string> Encrypt(string CipherText)
        {
            if (CipherText == null) throw new ArgumentNullException(nameof(CipherText));

            byte[] clearBytes = Encoding.Unicode.GetBytes(CipherText);

            using var aes = Aes.Create();
            using var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt);
            aes.Key = pdb.GetBytes(32);
            aes.IV = pdb.GetBytes(16);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
            }

            var result = Convert.ToBase64String(ms.ToArray());
            return Task.FromResult(result);
        }
        public async Task CreateJWTToken(CommonResponse<LoginResponse> response, LoginResponse result, LoginRequestwithIP request)
        {
            response.Status = Status.Success;
            response.Message = "Welcome " + result.UserName;
            result.Token = GenerateToken(result, request.IPAddress);
        }
        private string GenerateToken(LoginResponse response, string IPAddress)
        {
            var claims = new[]
            {
                new Claim("UserName",response.UserName ?? ""),
                new Claim("SysId",response.SysId.ToString()),
                new Claim("IpAddress",IPAddress.ToString()),
                new Claim("InstitutionCode",response.InstitutionCode.ToString()),
                new Claim("LoginType",response.LoginType ?? ""),
                new Claim("Guid", response.Guid.HasValue ? response.Guid.Value.ToString() : ""),
                new Claim("IsPrincipal", response.IsPrincipal.ToString()),
            };
            return GenerateJWTToken(claims);
        }
        private string GenerateJWTToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey.AuthKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jWTConfig.Issuer,
                _jWTConfig.Audince,
                claims,
                expires: DateTime.Now.AddMinutes(_appKeyConfig.TokenExpiry ?? 15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public APIRequestDetails GetAPIRequestDetails(ClaimsPrincipal user)
        {
            return new APIRequestDetails
            {
                InstitutionCode = Convert.ToInt32(user.FindFirst("InstitutionCode")?.Value),
                UserName = user.FindFirst("UserName")?.Value,
                LoginType = user.FindFirst("LoginType").Value,
                SysId = Convert.ToInt32(user.FindFirst("SysId")?.Value),
                Ispricipal = Convert.ToBoolean(user.FindFirst("Ispricipal")?.Value)
            };
        }
    }
}
