namespace SystemPortal.Services.Services.OtpServices.Dtos
{
    public class OtpOutputDto
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public int TimeToExpireInSeconds { get; set; }
    }
}
