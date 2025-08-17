using SystemPortal.Data.Entities;

namespace SystemPortal.Repository.Repositories.OtpRepository
{
    public interface IOtpRepository
    {
        ValueTask AddOtpInfoAsync(Otp otpData);
        ValueTask<Otp?> GetOtpByIdAsync(Guid id);
        IQueryable<Otp> GetAll();
        void DeleteOtpData(Otp otpData);
        void BeginUpdateOtpData(Otp otpData);
    }
}
