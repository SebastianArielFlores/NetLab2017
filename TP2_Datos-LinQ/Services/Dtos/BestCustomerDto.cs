using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class BestCustomerDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal? TotalPurchased { get; set; }
    }
}
