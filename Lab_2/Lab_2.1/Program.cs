using System;

namespace App
{
     class Program
    {
        static void Main(string[] args)
        {
            List<Animal> animals = new List<Animal>();
            animals.Add(new Dog("Buddy"));
            animals.Add(new Cat("Whiskers"));
            animals.Add(new Snake("Slither"));
            foreach (Animal animal in animals) {
                animal.Speak();
                
            }
            say_smth(animals[1]);
        }

        public static void say_smth(Animal animal)
        {
            animal.Speak();
            Console.WriteLine(animal.GetType());
        }
    }

    class Animal
    {
        protected String name;
        public Animal(String name)
        {
            this.name = name;
        }

        public virtual void Speak()
        {
            Console.WriteLine("The animal makes a sound.");
        }
    }

    class Dog : Animal
    {
        public Dog(String name) : base(name)
        {
        }
        public override void Speak()
        {
            Console.WriteLine($"{name} says: Woof!");
        }
    }

    class Cat : Animal
    {
        public Cat(String name) : base(name)
        {
        }
        public override void Speak()
        {
            Console.WriteLine($"{name} says: Meow!");
        }
    }

    class Snake : Animal
    {
        public Snake(String name) : base(name)
        {
        }
        public override void Speak()
        {
            Console.WriteLine($"{name} says: Hiss!");
        }
    }

}
        