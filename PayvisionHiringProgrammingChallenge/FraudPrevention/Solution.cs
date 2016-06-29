using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FraudPrevention
{ 
    class Solution
    {
        static void Main(String[] args)
        {
            /* Enter your code here. Read input from STDIN. Print output to STDOUT. Your class should be named Solution */

            // first read input 
            Console.Write("Number of records: ");
            int N = int.Parse(Console.ReadLine()); //Numbers of Line
                                                   
            List<Record> input = new List<Record>();

            // add read item to list
            string line;
            while (input.Count < N)
            {
                
                line = Console.ReadLine();

                var arrData = line.Split(',');
                var record = new Record()
                {
                    OrderId = int.Parse(arrData[0]),
                    DealId = int.Parse(arrData[1]),
                    EmailAddress = arrData[2].ToLower(),
                    StreetAddress = ValidStreet(arrData[3]),
                    City = arrData[4],
                    State = ValidState(arrData[5].ToUpper()),
                    ZipCode = arrData[6],
                    CreditCard = arrData[7]
                };

                input.Add(record);
            }

            validators(input);

           var ar = input.Where(p => p.isFraudulent == true).Select(i => i.OrderId.ToString()).ToArray();
                   
            Console.WriteLine("fraudulent Orders Result:");
            Console.WriteLine(string.Join(",", ar));
            Console.ReadLine();
        }

        public static string ValidStreet(string dato) {
            if (dato.Contains("St."))
                return dato.Replace("St.", "Street");
            else return dato;
        }

        public static string ValidState(string dato)
        {
            if (dato.Contains("NEW YORK"))
                return dato.Replace("NEW YORK", "NY");
            else if (dato.Contains("CALIFORNIA"))
                return dato.Substring(0, 2);
            else if (dato.Contains("ILLINOIS"))
                return dato.Substring(0, 2);
            else return dato;
        }

        public static void validators(List<Record> list) {
            if(list.Count > 0) {
                foreach (Record r1 in list)
                {
                    var stadd = r1.StreetAddress;
                    var deal = r1.DealId;
                    var card = r1.CreditCard;
                    var city = r1.City;
                    var state = r1.State;
                    var zip = r1.ZipCode;
                    bool isfraude = false;

                    foreach (Record r2 in list) {
                        if (r1.OrderId != r2.OrderId) {

                            if (r2.StreetAddress.Equals(stadd) && r2.DealId.Equals(deal) && !r2.CreditCard.Equals(card))
                                isfraude = true;
                        

                            if (r2.City.Equals(city) && r2.State.Equals(state) && r2.ZipCode.Equals(zip) && !r2.CreditCard.Equals(card) && !r2.DealId.Equals(deal))
                                 isfraude = false; 

                            if (isfraude) { 
                                var upd = list.Where(d => d.OrderId == r1.OrderId).FirstOrDefault();
                                if (upd != null) { upd.isFraudulent = true; }
                            }
                        }
                    }

                }
            }
        }



    }
}