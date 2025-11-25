using System;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {

            ComplexNumber c4 = new ComplexNumber(7, -8);
            ComplexNumber c3 = new ComplexNumber(-5, 6);
            ComplexNumber c2 = new ComplexNumber(3, -4);
            ComplexNumber c5 = new ComplexNumber(-9, 10);
            ComplexNumber c1 = new ComplexNumber(-1, 2);

            ComplexNumber[] complexNumbers = { c4, c3, c2, c5, c1 };

            foreach (ComplexNumber c in complexNumbers)
            {
                Console.WriteLine(c.ToString());
            }

            Array.Sort(complexNumbers, (x, y) => x.CompareTo(y));

            Console.WriteLine("\nSorted complex numbers by their modulus:");
            foreach (ComplexNumber c in complexNumbers)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine($"Min: {complexNumbers[0].ToString()}");
            Console.WriteLine($"Max: {complexNumbers[complexNumbers.Length - 1].ToString()}");

            complexNumbers.Where(c => c.Im > 0).ToList().ForEach(c => Console.WriteLine($"Imaginery > 0: {c.ToString()}"));

            List<ComplexNumber> list = new List<ComplexNumber>(complexNumbers);

            list.RemoveAt(2);
            Console.WriteLine("\nAfter removing the element at index 2:");
            foreach (ComplexNumber c in list)
            {
                Console.WriteLine(c.ToString());
            }

            list.Remove(list.Min());
            Console.WriteLine("\nAfter removing the minimum element:");
            foreach (ComplexNumber c in list)
            {
                Console.WriteLine(c.ToString());
            }

            list.Clear();
            Console.WriteLine($"\nList cleared. Count: {list.Count}");

            HashSet<ComplexNumber> complexSet = new HashSet<ComplexNumber>(complexNumbers);
            complexSet.Add(new ComplexNumber(6, 7));
            complexSet.Add(new ComplexNumber(1, 2));
            complexSet.Add(new ComplexNumber(6, 7));
            complexSet.Add(new ComplexNumber(1, -2));
            complexSet.Add(new ComplexNumber(-5, 9));

            Console.WriteLine("\nHashSet contents:");
            foreach (ComplexNumber c in complexSet)
            {
                Console.WriteLine(c.ToString());
            }
            complexSet.Min();
            complexSet.Max();


            Dictionary<string, ComplexNumber> complexDict = new Dictionary<string, ComplexNumber>();
            complexDict["z1"] = new ComplexNumber(6, 7);
            complexDict["z2"] = new ComplexNumber(1, 2);
            complexDict["z3"] = new ComplexNumber(6, 7);
            complexDict["z4"] = new ComplexNumber(1, -2);
            complexDict["z5"] = new ComplexNumber(-5, 9);

            Console.WriteLine("\nDictionary contents:");
            foreach (var record in complexDict)
            {
                Console.WriteLine($"{record.Key}: {record.Value.ToString()}");
            }

            complexDict.Keys.ToList().ForEach(key => Console.WriteLine($"Key: {key}"));
            complexDict.Values.ToList().ForEach(value => Console.WriteLine($"Value: {value.ToString()}"));
                
            if (complexDict.ContainsKey("z6"))
            {
                Console.WriteLine($"\nDictionary does not contain key 'z6'");
            }

            Console.WriteLine($"\nDict min: {complexDict.Values.Min()}");
            Console.WriteLine($"\nDict max: {complexDict.Values.Max()}");
            foreach (var record in complexDict)
            {
                if (record.Value.Im < 0)
                {
                    complexDict.Remove(record.Key);
                }   
            }

            Console.WriteLine("\nDictionary after removing values with negative imaginary part:");
            foreach (var record in complexDict)
            {
                Console.WriteLine($"{record.Key}: {record.Value.ToString()}");
            }

            complexDict.Remove("z3");
            Console.WriteLine("\nDictionary after removing key 'z3':");
            foreach (var record in complexDict)
            {
                Console.WriteLine($"{record.Key}: {record.Value.ToString()}");
            }

            complexDict.Remove(complexDict.ElementAt(1).Key);
            Console.WriteLine("\nDictionary after removing the element at index 2:");
            foreach (var record in complexDict)
            {
                Console.WriteLine($"{record.Key}: {record.Value.ToString()}");
            }

            complexDict.Clear();
            Console.WriteLine($"\nDictionary cleared. Count: {complexDict.Count}");
        }

        class ComplexNumber : ICloneable, IEquatable<ComplexNumber>, IModular, IComparable<ComplexNumber>
        {
            private double re;
            private double im;

            public double Re { get => re; set => re = value; }
            public double Im { get => im; set => im = value; }

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
                => c1?.Equals(c2) ?? c2 is null;

            public static bool operator !=(ComplexNumber c1, ComplexNumber c2)
            {
                if (c1.re == c2.re && c1.im == c2.im) return false;
                return true;
            }

            public bool Equals(ComplexNumber? other)
            {
                if (other == null) return false;
                return re == other.re && im == other.im;
            }

            public object Clone()
            {
                return new ComplexNumber(re, im);
            }

            public double Module()
            {
                return Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
            }

            public int CompareTo(ComplexNumber? other)
            {
                if (other == null) return 1;
                return this.Module().CompareTo(other.Module());
            }
        }

        public interface IModular
        {
            double Module();
        }

    }
}
