#include <stdio.h>
#include "f2.h"
#include "f3.h"
int f4 (int x, int y)
{
        return 3*f2(x,y) - f3(x,y);
}