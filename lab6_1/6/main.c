#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include "f4.h"

int main()
{
	int x;
	int y;
	printf("Введіть Х:");
	scanf("%d", &x);
	printf("Введіть Y:");
	scanf("%d", &y);
	int z = f4(x,y);
	printf("%d \n",z);
	return 0;
}
