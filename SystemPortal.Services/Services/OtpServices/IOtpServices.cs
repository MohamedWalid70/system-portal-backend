using FluentResults;
using SystemPortal.Data.Entities;
using SystemPortal.Services.Services.OtpServices.Dtos;

namespace SystemPortal.Services.Services.OtpServices
{
    public interface IOtpServices
    {
        ValueTask<OtpOutputDto> GenerateOtpAsync();
        IAsyncEnumerable<Otp> GetOtpsAsync();
        ValueTask<Otp?> GetOtpByIdAsync(Guid id);
        ValueTask<Result> VerifyOtp(Guid id);
    }
}
