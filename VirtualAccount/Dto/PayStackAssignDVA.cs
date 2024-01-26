using static VirtualAccount.Dto.PayStackCreateExistingCustomerDVA;

namespace VirtualAccount.Dto
{
    public class PayStackAssignDVA
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }


        public class Bank
        {
            public string name { get; set; }
            public int id { get; set; }
            public string slug { get; set; }
        }

        public class Customer
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string customer_code { get; set; }
            public string phone { get; set; }
            public Metadata metadata { get; set; }
            public string risk_action { get; set; }
            public string international_format_phone { get; set; }
        }

        public class Datum
        {
            public Customer customer { get; set; }
            public Bank bank { get; set; }
            public int id { get; set; }
            public string account_name { get; set; }
            public string account_number { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string currency { get; set; }
            public object split_config { get; set; }
            public bool active { get; set; }
            public bool assigned { get; set; }
        }

        public class Meta
        {
            public int total { get; set; }
            public int skipped { get; set; }
            public int perPage { get; set; }
            public int page { get; set; }
            public int pageCount { get; set; }
        }

        public class Metadata
        {
        }

    }
}
