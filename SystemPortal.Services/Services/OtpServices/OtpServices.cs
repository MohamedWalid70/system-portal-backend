namespace SystemPortal.Services.services.OtpServices
{
    public class OtpServices : IOtpServices
    {
        public int GetOTP()
        {
            Random randomCode = new Random();

            return randomCode.Next(100000, 999999);

        }
    }
}
