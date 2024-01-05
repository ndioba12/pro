namespace SIGRHBack.Services.Messagerie.Mail.Dtos
{
    public class ResponseSendMail
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<object>? Errors { get; set; }
    }
}
