using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;

namespace Hotel.Infrastructure.Services
{
    public class RazorpayService
    {
        private readonly RazorpayClient _client;
        private readonly string _keySecret;

        public RazorpayService(IConfiguration config)
        {
            var keyId = config["Razorpay:KeyId"]!;
            _keySecret = config["Razorpay:KeySecret"]!;
            _client = new RazorpayClient(keyId, _keySecret);
        }

        public async Task<string> CreateOrderAsync(decimal amountInRupees, string receipt)
        {
            var options = new Dictionary<string, object>
            {
                ["amount"] = (int)(amountInRupees * 100),
                ["currency"] = "INR",
                ["receipt"] = receipt
            };

            var order = await Task.Run(() => _client.Order.Create(options));
            return order["id"].ToString()!;
        }

        public bool VerifySignature(string orderId, string? paymentId, string? signature)
        {
            if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(signature))
                return false;

            string payload = $"{orderId}|{paymentId}";
            string generated = ComputeHmacSha256(payload, _keySecret);

            return string.Equals(generated, signature, StringComparison.OrdinalIgnoreCase);
        }

        private static string ComputeHmacSha256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}