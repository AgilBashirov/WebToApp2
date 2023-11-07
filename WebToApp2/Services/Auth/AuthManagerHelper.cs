using System.Text;
using System.Text.Json;
using WebToApp2.Helpers;
using WebToApp2.Models.Sima;
using Contract = WebToApp2.Models.Sima.Contract;

namespace WebToApp2.Services.Auth
{
    public partial class AuthManager 
    {
        private Contract CreateContract(string operationId) =>
        new()
        {
            SignableContainer = new SignableContainer
            {
                ProtoInfo = new ProtoInfo
                {
                    Name = ConfigurationAccessor.AppConfiguration!["SimaContainer:SignableContainer:ProtoInfo:Name"]!,
                    Version = ConfigurationAccessor.AppConfiguration["SimaContainer:SignableContainer:ProtoInfo:Version"]!
                },
                OperationInfo = new OperationInfo
                {
                    OperationId = operationId,
                    // Type = operationType is OperationTypeEnum.Auth ? "Auth" : "Sign",
                    Type = "Sign",
                    NbfUTC = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                    ExpUTC = new DateTimeOffset(DateTime.UtcNow.AddMinutes(int.Parse(
                            ConfigurationAccessor.AppConfiguration[
                                "SimaContainer:SignableContainer:OperationInfo:QrExpireMinute"]!)))
                        .ToUnixTimeSeconds(),
                    Assignee = new List<string>()
                },
                ClientInfo = new ClientInfo
                {
                    ClientId = int.Parse(
                        ConfigurationAccessor.AppConfiguration["SimaContainer:SignableContainer:ClientInfo:ClientId"]!),
                    ClientName = "Web2App",
                    IconURI = ConfigurationAccessor.AppConfiguration["SimaContainer:SignableContainer:ClientInfo:IconUri"]!,
                    Callback = _urlGenerator.GenerateCallbackUrl(),
                    HostName = null
                },
            },
            Header = new Header
            {
                AlgorithmName = ConfigurationAccessor.AppConfiguration["SimaContainer:Header:AlgorithmName"]!,
            }
        };

        private static byte[] CreateSignature<T>(T model) where T : SignableContainer
        {
            var json = JsonSerializer.Serialize(model).Trim();

            var computedHashAsByte = CryptHelper.ComputeSha256HashAsByte(json);
            var hMac = CryptHelper.GetHMAC(computedHashAsByte,
                ConfigurationAccessor.AppConfiguration!["SimaContainer:Header:SecretKey"]!);
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
