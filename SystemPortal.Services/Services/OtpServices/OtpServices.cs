using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SystemPortal.Data.Entities;
using SystemPortal.Repository.Repositories.OtpRepository;
using SystemPortal.Repository.UnitOfWork;
using SystemPortal.Services.Services.OtpServices.Dtos;

namespace SystemPortal.Services.Services.OtpServices
{
    public class OtpServices : IOtpServices
    {
        IOtpRepository _otpRepository;
        IUnitOfWork _unitOfWork;
        public OtpServices(IOtpRepository otpRepository, IUnitOfWork unitOfWork)
        {
            _otpRepository = otpRepository;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<OtpOutputDto> GenerateOtpAsync()
        {
            Random randomCode = new Random();

            Otp otp = new()
            {
                TimeToExpireInSeconds = 60,
                TransmissionTime = DateTime.UtcNow,
                Value = randomCode.Next(100000, 999999),
            };

            await _otpRepository.AddOtpInfoAsync(otp);
            await _unitOfWork.CommitAsync();

            OtpOutputDto outputDto = new()
            {
                Id = otp.Id,
                Value = otp.Value,
                TimeToExpireInSeconds = otp.TimeToExpireInSeconds
            };

            return outputDto;
        }

        public async ValueTask<Otp?> GetOtpByIdAsync(Guid id)
        {

            return await _otpRepository.GetOtpByIdAsync(id);
        }

        public IAsyncEnumerable<Otp> GetOtpsAsync()
        {
            return _otpRepository.GetAll().AsAsyncEnumerable();
        }

        public async ValueTask<Result> VerifyOtp(Guid id)
        {
            Otp? otp = await _otpRepository.GetOtpByIdAsync(id);

            if (otp == null)
            {
                return Result.Fail("The OTP does not exist");
            }

            _otpRepository.BeginUpdateOtpData(otp);

            otp.VerificationTime = DateTime.UtcNow;

            if (otp.VerificationTime.Value.Ticks > otp.TransmissionTime.AddSeconds(otp.TimeToExpireInSeconds).Ticks)
            {
                await _unitOfWork.CommitAsync();

                return Result.Fail("Verification failed as the OTP is already expired");
            }

            otp.IsVerified = true;

            await _unitOfWork.CommitAsync();

            return Result.Ok();

   
        }
    }
}
