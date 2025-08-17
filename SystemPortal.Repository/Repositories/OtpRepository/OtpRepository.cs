using SystemPortal.Data.Context;
using SystemPortal.Data.Entities;

namespace SystemPortal.Repository.Repositories.OtpRepository
{
    public class OtpRepository : IOtpRepository
    {
        AppDbContext _appDbContext;
        public OtpRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async ValueTask AddOtpInfoAsync(Otp otpData)
        {
            await _appDbContext.Otps.AddAsync(otpData);
        }

        public void BeginUpdateOtpData(Otp otpData)
        {
            _appDbContext.Otps.Attach(otpData);
        }

        public void DeleteOtpData(Otp otpData)
        {
            _appDbContext.Otps.Remove(otpData);
        }

        public IQueryable<Otp> GetAll()
        {
            return _appDbContext.Otps;
        }

        public async ValueTask<Otp?> GetOtpByIdAsync(Guid id)
        {
            return await _appDbContext.Otps.FindAsync(id);
        }
    }
}
