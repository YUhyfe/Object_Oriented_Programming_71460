using System;
using System.Xml.Linq;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            ComplexNumber c1 = new ComplexNumber(3, 4);
            ComplexNumber c2 = new ComplexNumber(1, -2);
            ComplexNumber c3 = new ComplexNumber(2, 3);
            Console.WriteLine($"c1: {c1}");
            Console.WriteLine($"c2: {c2}");
            Console.WriteLine($"c3: {c3}");
            Console.WriteLine($"c1 + c2: {c1 + c2}");
            Console.WriteLine($"c1 - c2: {c1 - c2}");
            Console.WriteLine($"c1 * c3: {c1 * c3}");
            Console.WriteLine($"c1 == c2: {c1 == c2}");
            Console.WriteLine($"c1 != c2: {c1 != c2}");
            Console.WriteLine($"c2 conjugate: {-c2}");
            Console.WriteLine($"Module of c1: {c1.Module()}");
        }
    }

    class ComplexNumber : ICloneable, IEquatable<ComplexNumber>, IModular
    {
        private double re { get; set; }
        private double im { get; set; }
        public ComplexNumber(double re, double im)
        {
            this.re = re;
            this.im = im;
        }

        public override string ToString()
        {
            string symbol = im >= 0 ? "+" : "-";
            return $"{re} {symbol} {Math.Abs(im)}i";
        }

        public static ComplexNumber operator +(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.re + c2.re, c1.im + c2.im);
        }

        public static ComplexNumber operator -(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.re - c2.re, c1.im - c2.im);
        }

        public static ComplexNumber operator -(ComplexNumber c)
        {
            return new ComplexNumber(c.re, -c.im);
        }

        public static ComplexNumber operator *(ComplexNumber c1, ComplexNumber c2)
        {
            double realPart = c1.re * c2.re - c1.im * c2.im;
            double imagPart = c1.re * c2.im + c1.im * c2.re;
            return new ComplexNumber(realPart, imagPart);
        }

        public static bool operator ==(ComplexNumber c1, ComplexNumber c2)
        {
            if (c1.re == c2.re && c1.im == c2.im) return true;
            return false;
        }

        public static bool operator !=(ComplexNumber c1, ComplexNumber c2)
        {
            if (c1.re == c2.re && c1.im == c2.im) return false;
            return true;
        }

        public bool Equals(ComplexNumber? other)
        {
            if (other == this) return true;
            return false;
        }

        public object Clone()
        {
            return new ComplexNumber(re, im);
        }

        public double Module()
        {
            return Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
        }
    }

    public interface IModular
    {
        double Module();
    }
}