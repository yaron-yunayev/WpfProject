using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cripto_Coin
{
    public class Coin
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string price_usd { get; set; }
        public string percent_change_24h { get; set; }
        public string market_cap_usd { get; set; }
    }
}
