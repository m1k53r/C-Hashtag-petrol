using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace palwio.Shared
{
    public class Petrol
    {
        public double burning { get; set; }
        public double totalSpace { get; set; }
        public double remainingSpace { get; set; }
        public int timesTanked { get; set; }
        public double totalUsedLiters{ get; set; }
        public double price{ get; set; }
        public double savedPrice { get; set; }

        public Petrol(int burning, int totalSpace, double price)
        {
            this.burning = burning;
            this.totalSpace = totalSpace;
            this.price = price;
            savedPrice = price;
            remainingSpace = totalSpace;
            timesTanked = 0;
            totalUsedLiters = 0;
        }
        public double CalculatePrice()
        {
            return totalUsedLiters * price;
        }
    }
}
