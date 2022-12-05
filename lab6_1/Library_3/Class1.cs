using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_3
{
    public class KI3_Class_3 : Library_1.KI3_Class_1
    {
        public double F3(double x, double y)
        {
            Library_1.KI3_Class_1 fun1 = new Library_1.KI3_Class_1();
            var result = 2 * F1(x, y) * fun1.F1(x, y);
            return result;
        }
    }
}
