namespace VirtualAccount.Dto
{
    public class RequestAssignDVA
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set;}
        public string phone { get; set; }
        public string preferred_bank { get; set; }
        public string country { get; set; }
    }
}
