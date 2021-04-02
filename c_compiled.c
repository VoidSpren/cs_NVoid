#include <stdio.h>
int main(){
float a;
float b;
float c;
a = 5.5;
if(0 == scanf("%f", &b)) {
b = 0;
scanf("%*s");
}
c = a + b * b * a / 2;
printf("%.2f\n", (float)(c));
return 0;
}
