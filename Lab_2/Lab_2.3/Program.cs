using System;
using System.Xml.Linq;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            new A();
            new B();
            new C();
        }
    }

    class A
    {
        public A()
        {
            Console.WriteLine("Class A constructor called.");
        }
    }

    class B : A
    {
        public B() : base()
        {
            Console.WriteLine("Class B constructor called.");
        }
    }

    class C : B
    {
        public C() : base()
        {
            Console.WriteLine("Class C constructor called.");
        }
    }
}
