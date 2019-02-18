using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Previsao
{
    class Ipma
    {
            public string owner { get; set; }
            public string country { get; set; }
            public string forecastDate { get; set; }
            public List<Data> data { get; set; }
            public DateTime dataUpdate { get; set; }
        
    }
}
