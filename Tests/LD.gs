point a;
point b;
m = measure(a,b);
color blue;
color red;
draw a;
restore;
draw b;
restore;
restore;
draw circle(a,m);

a = point (200,200);
c = point(200,300);
b = point (300,200);
m = measure(a,b);
ar = arc(a,b,c,measure(a,b));
draw a "A";
draw b "B";
draw c "C";
draw ray(a,b);
draw ray(a,c);
draw ar;
random =points(ar);
draw random;

ra = ray(a,random);
draw ra;
draw points(ra);

