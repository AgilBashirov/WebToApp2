using System.Diagnostics.Contracts;
using System.Text;
using System.Text.Json;
using WebToApp2.Enum;
using WebToApp2.Helpers;
using WebToApp2.Models.Sima;
using Contract = WebToApp2.Models.Sima.Contract;

namespace WebToApp2.Services
{
    public partial class AuthManager 
    {
        private Contract CreateContract(string operationId, OperationTypeEnum operationType) =>
        new()
        {
            SignableContainer = new SignableContainer
            {
                ProtoInfo = new ProtoInfo
                {
                    Name = ConfigurationAccessor.AppConfiguration!["SimaLogin:SignableContainer:ProtoInfo:Name"]!,
                    Version = ConfigurationAccessor.AppConfiguration["SimaLogin:SignableContainer:ProtoInfo:Version"]!
                },
                OperationInfo = new OperationInfo
                {
                    OperationId = operationId,
                    Type = operationType is OperationTypeEnum.Auth ? "Auth" : "Sign",
                    NbfUTC = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                    ExpUTC = new DateTimeOffset(DateTime.UtcNow.AddMinutes(int.Parse(
                            ConfigurationAccessor.AppConfiguration[
                                "SimaLogin:SignableContainer:OperationInfo:QrExpireMinute"]!)))
                        .ToUnixTimeSeconds(),
                    Assignee = new List<string>()
                },
                ClientInfo = new ClientInfo
                {
                    ClientId = int.Parse(
                        ConfigurationAccessor.AppConfiguration["SimaLogin:SignableContainer:ClientInfo:ClientId"]!),
                    ClientName = "SimaLegalEntity",
                    IconURI = ConfigurationAccessor.AppConfiguration["SimaLogin:SignableContainer:ClientInfo:IconUri"]!,
                    Callback = _urlGenerator.GenerateCallbackUrl(),
                    HostName = null
                },
            },
            Header = new Header
            {
                AlgorithmName = ConfigurationAccessor.AppConfiguration["SimaLogin:Header:AlgorithmName"]!,
            }
        };

        private static byte[] CreateSignature<T>(T model) where T : SignableContainer
        {
            var json = JsonSerializer.Serialize(model).Trim();

            var computedHashAsByte = CryptHelper.ComputeSha256HashAsByte(json);
            var hMac = CryptHelper.GetHMAC(computedHashAsByte,
                ConfigurationAccessor.AppConfiguration!["SimaLogin:Header:SecretKey"]!);
            return hMac;
        }

        private static string EncodeContract(Contract model)
        {
            var json = JsonSerializer.Serialize(model);

            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            byte[] byteArray = Convert.FromBase64String(base64);
            string fileFormBase64 = Convert.ToBase64String(byteArray);
            return fileFormBase64;
        }
    }
}
