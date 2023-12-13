//List<Point> Inters = Method.Intersection_Line_Circle(new Point(4,4), new Point(6,6), new Point(5,5), 5);
//Console.WriteLine();





public static class Method
{
    public static List<Point> Intersection_Line_Circle(Point line_p1, Point line_p2, Point circle_center, double radius)
    {
        List<Point> Result = new List<Point>();
        //Si la distancia del centro a la recta es mayor que el radio, no hay intersección
        if (Distancia_Punto_Recta(circle_center, line_p1, line_p2) > radius)
        {
            return Result;
        }
        //Si la distancia del punto a la recta es igual o menor al radio, se intersectan en un solo punto o en dos
        else
        {
            //Si no podemos calcular la pendiente por la via trivial, hay que hacerlo de otra forma
            if (line_p2.X - line_p1.X == 0)
            {
                double X = line_p1.X;
                double Y = circle_center.Y + Math.Sqrt((radius * radius) - ((X - circle_center.X) * (X - circle_center.X)));
                Result.Add(new Point(X, Y));
                Y = circle_center.Y - Math.Sqrt((radius * radius) - ((X - circle_center.X) * (X - circle_center.X)));
                Result.Add(new Point(X, Y));
            }
            else
            {
                //Hallando m y n
                double m = (line_p2.Y - line_p1.Y) / (line_p2.X - line_p1.X);
                double n = line_p2.Y - (m * line_p2.X);
                //Parametrizando
                double A = 1 + (m * m);
                double B = (2 * m * n) - (2 * circle_center.Y * m) - (2 * circle_center.X);
                double C = (circle_center.X * circle_center.X) + (circle_center.Y * circle_center.Y) - (radius * radius) - (2 * n * circle_center.Y) + (n * n);
                double Discriminante = (B * B) - (4 * A * C);
                //Si el dicriminante es 0, tiene una sola intersección
                if (Discriminante == 0)
                {
                    double X = (-B) / (2 * A);
                    double Y = (m * X) + n;
                    Result.Add(new Point(X, Y));
                }
                //Si no es 0, tiene 2 intersecciones
                else
                {
                    double X = ((-B) + Math.Sqrt(Discriminante)) / (2 * A);
                    double Y = (m * X) + n;
                    Result.Add(new Point(X, Y));
                    X = ((-B) - Math.Sqrt(Discriminante)) / (2 * A);
                    Y = (m * X) + n;
                    Result.Add(new Point(X, Y));
                }
            }
        }
        return Result;
    }
    public static double Distancia_Punto_Recta(Point punto, Point recta_p1, Point recta_p2)
    {
        double distance;
        if (recta_p1.X == recta_p2.X)
        {
            distance = recta_p1.X - punto.X;
        }
        else if (recta_p1.Y == recta_p2.Y)
        {
            distance = recta_p1.Y - punto.Y;
        }
        else
        {
            //Calculando ecuación cartesiana
            double m = (recta_p2.Y - recta_p1.Y) / (recta_p2.X - recta_p1.X);
            double n = recta_p2.Y - (m * recta_p2.X);
            //Declarando parámetros
            double A = m;
            double B = -1;
            double C = n;
            //Calcuando distancia
            distance = ((A * punto.X) + (B * punto.Y) + C) / Math.Sqrt((A * A) + (B * B));
        }
        //Devolver el módulo de la distancia
        if (distance < 0) return -distance;
        return distance;
    }
}
public class Point
{
    public double X;
    public double Y;
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
}
