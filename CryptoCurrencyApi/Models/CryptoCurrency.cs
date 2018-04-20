using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.Models
{
    public class CryptoCurrency
    {
        public string apiResponse { get; set; }

        public Dictionary<string, string> currencies
        {
            get; set;
        }
    }
}
