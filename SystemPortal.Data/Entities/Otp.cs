namespace SystemPortal.Data.Entities
{
    public class Otp
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public DateTime TransmissionTime { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerificationTime { get; set; }
        public int TimeToExpireInSeconds { get; set; }
    }
}
