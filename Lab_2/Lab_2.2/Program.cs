using System;
using System.Xml.Linq;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    abstract class Worker
    {
        public abstract void Work();
    }

    class Baker : Worker
    {
        public override void Work()
        {
            Console.WriteLine("Baking bread...");
        }
    }
}