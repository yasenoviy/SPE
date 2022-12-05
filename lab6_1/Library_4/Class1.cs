using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_4
{
    public static class KI3_Class_4
    {
        public static double F4(double x, double y)
        {

            Library_2.KI3_Class_2 fun2 = new Library_2.KI3_Class_2();
            Library_3.KI3_Class_3 fun3 = new Library_3.KI3_Class_3();
            return 3 * fun2.F2(x, y) - fun3.F3(x, y);
        }
    }
}
