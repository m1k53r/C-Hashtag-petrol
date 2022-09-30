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

        public Petrol(int burning, int totalSpace)
        {
            this.burning = burning;
            this.totalSpace = totalSpace;
            remainingSpace = totalSpace;
            timesTanked = 0;
        }
    }
}
