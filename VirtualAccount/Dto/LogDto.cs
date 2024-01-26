namespace VirtualAccount.Dto
{
    public class LogDto
    {
        public int LogId { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public int statusCode { get; set; } 
        public string ApiUrl { get; set; }
        public DateTime logDate { get; set; }
    }
}
