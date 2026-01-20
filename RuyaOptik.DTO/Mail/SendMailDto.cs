namespace RuyaOptik.DTO.Mail
{
    public class SendMailDto
    {

        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}