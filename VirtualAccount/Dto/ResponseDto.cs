using VirtualAccount.Entity;

namespace VirtualAccount.Dto
{
    public class ResponseDto
    {
        public string ErrorMessage { get; set; }
        public string DisplayMessage { get; set; }
        public int Status {get; set; }
        public PayStackVirtualAccount Result { get; set; }
    }
}
