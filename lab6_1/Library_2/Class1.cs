using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_2
{
    public class KI3_Class_2
    {
        public double F2(double x, double y)
        {
            Library_1.KI3_Class_1 fun1 = new Library_1.KI3_Class_1();
            return 3 * fun1.F1(x, y) + 7;
        }
    }
}