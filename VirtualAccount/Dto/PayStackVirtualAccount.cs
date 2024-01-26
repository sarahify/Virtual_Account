using Org.BouncyCastle.Asn1.Cms;

namespace VirtualAccount.Dto
{
    public class PayStackVirtualAccount
    {
            public bool Status { get; set; }
            public string Message { get; set; }
            public CustomerData Data { get; set; }
      

        public class CustomerData
        {
            public List<object> transactions { get; set; }
            public List<object> subscriptions { get; set; }
            public List<object> authorizations { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public Metadata metadata { get; set; }
            public string domain { get; set; }
            public string customer_code { get; set; }
            public string risk_action { get; set; }
            public int id { get; set; }
            public int integration { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public bool identified { get; set; }
            public object identifications { get; set; }
        }

        public class Metadata
        {

        }
    }
}
