using Razorpay.Api;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.Models.Other;
using Microsoft.Extensions.Options;
using Medi_Connect.Domain.Common;
using System.Text;
using Medi_Connect.Domain.DTOs.PaymentDTOs;

namespace Medi_Connect.Infrastructure.Services
{
    public class RazorpayService : IRazorpayService
    {
        private readonly RazorPayOptions _razorpayOptions;

        public RazorpayService(IOptions<RazorPayOptions> options)
        {
            _razorpayOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
        }


        public async Task<PaymentResult> ProcessPayment(Guid userId, decimal amount)
        {

            try
            {
                Console.WriteLine("🟢 Entered ProcessPayment");
                Console.WriteLine("Key: " + (_razorpayOptions?.Key ?? "NULL"));
                Console.WriteLine("Secret: " + (_razorpayOptions?.Secret ?? "NULL"));

            if (_razorpayOptions?.Key == null || _razorpayOptions?.Secret == null)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Razorpay credentials missing"
                };
            }
                RazorpayClient client = new RazorpayClient(_razorpayOptions.Key, _razorpayOptions.Secret);

                var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 20);
                var receiptId = $"rcpt_{shortGuid}";
                var options = new Dictionary<string, object>
                {
                    { "amount", (int)(amount * 100) },
                    { "currency", "INR" },
                    { "receipt", receiptId },
                    { "payment_capture", 1 }
                };


                Order order = client.Order.Create(options);

                return new PaymentResult
                {
                    Success = true,
                    TransactionId = order["id"].ToString(),
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Razorpay Exception: " + ex.ToString());

                if (ex is Razorpay.Api.Errors.BadRequestError badRequest)
                {
                    Console.WriteLine("🟡 Razorpay BadRequestError: " + badRequest);
                }

                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = ex.Message + " | " + (ex.InnerException?.Message ?? "")
                };
            }

        }


        public bool VerifySignature(VerifyPaymentDTO dto)
        {
            var key = _razorpayOptions.Secret;
            string payload = $"{dto.RazorpayOrderId}|{dto.RazorpayPaymentId}";
            string expectedSignature;

            using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            return SecureEquals(expectedSignature, dto.RazorpaySignature);
        }

        private static bool SecureEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);
            return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
        }


        public string GetKey()
        {
            return _razorpayOptions.Key;
        }

    }
}
