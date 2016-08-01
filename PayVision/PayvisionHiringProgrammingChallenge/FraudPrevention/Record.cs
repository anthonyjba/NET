using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FraudPrevention
{
    class Record
    {
        public int OrderId { get; set; }
        public int DealId { get; set; }
        public string EmailAddress { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CreditCard { get; set; }

        public bool isFraudulent
        {
            get; set;
        }
    }
}
