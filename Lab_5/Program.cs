using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace ZadaniaPliki
{
    public class Student
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<int> Grades { get; set; }

        public Student() { }

        public Student(string name, string lastName, List<int> grades)
        {
            Name = name;
            LastName = lastName;
            Grades = grades;
        }

        public override string ToString()
        {
            return $"{Name} {LastName}, Grades: [{string.Join(", ", Grades)}]";
        }
    }

    class Program
    {
        const string TextFile = "data.txt";
        const string JsonFile = "students.json";
        const string XmlFile = "students.xml";
        const string CsvFile = "iris.csv";
        const string CsvFileFiltered = "iris_filtered.csv";

        static void Main(string[] args)
        {
            PrepareCsvFile();

            bool working = true;
            while (working)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("2. Write text to file (overwrite)");
                Console.WriteLine("3. Read text file");
                Console.WriteLine("4. Append text to file");
                Console.WriteLine("6. JSON serialization");
                Console.WriteLine("7. JSON deserialization");
                Console.WriteLine("8. XML serialization");
                Console.WriteLine("9. XML deserialization");
                Console.WriteLine("10. Read CSV file");
                Console.WriteLine("11. Average values from CSV");
                Console.WriteLine("12. Filter CSV file");
                Console.WriteLine("0. Exit");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "2": Task2_WriteFile(); break;
                        case "3": Task3_ReadFile(); break;
                        case "4": Task4_AppendToFile(); break;
                        case "6": Task6_WriteJson(); break;
                        case "7": Task7_ReadJson(); break;
                        case "8": Task8_WriteXml(); break;
                        case "9": Task9_ReadXml(); break;
                        case "10": Task10_ReadCsv(); break;
                        case "11": Task11_CsvAverages(); break;
                        case "12": Task12_FilterCsv(); break;
                        case "0": working = false; break;
                        default: Console.WriteLine("Unknown option."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void Task2_WriteFile()
        {
            Console.WriteLine("Enter 3 lines of text:");
            List<string> lines = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Line {i + 1}: ");
                lines.Add(Console.ReadLine());
            }

            File.WriteAllLines(TextFile, lines);
            Console.WriteLine($"Saved to file {TextFile}");
        }

        static void Task3_ReadFile()
        {
            if (!File.Exists(TextFile))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            Console.WriteLine($"--- Content of {TextFile} ---");
            string[] lines = File.ReadAllLines(TextFile);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        static void Task4_AppendToFile()
        {
            Console.Write("Enter line to append: ");
            string text = Console.ReadLine();
            using (StreamWriter sw = File.AppendText(TextFile))
            {
                sw.WriteLine(text);
            }
            Console.WriteLine("Appended.");
        }

        static List<Student> GenerateStudents()
        {
            return new List<Student>
            {
                new Student("John", "Smith", new List<int> { 3, 4, 5 }),
                new Student("Anna", "Brown", new List<int> { 5, 5, 4, 5 }),
                new Student("Peter", "Wilson", new List<int> { 2, 3, 3 })
            };
        }

        static void Task6_WriteJson()
        {
            var students = GenerateStudents();
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(JsonFile, json);
            Console.WriteLine($"Saved to {JsonFile}");
        }

        static void Task7_ReadJson()
        {
            if (!File.Exists(JsonFile))
            {
                Console.WriteLine("JSON file does not exist.");
                return;
            }

            string json = File.ReadAllText(JsonFile);
            var students = JsonSerializer.Deserialize<List<Student>>(json);

            Console.WriteLine("--- Students JSON ---");
            foreach (var s in students)
            {
                Console.WriteLine(s);
            }
        }

        static void Task8_WriteXml()
        {
            var students = GenerateStudents();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));

            using (StreamWriter writer = new StreamWriter(XmlFile))
            {
                serializer.Serialize(writer, students);
            }

            Console.WriteLine($"Saved to {XmlFile}");
        }

        static void Task9_ReadXml()
        {
            if (!File.Exists(XmlFile))
            {
                Console.WriteLine("XML file does not exist.");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));

            using (StreamReader reader = new StreamReader(XmlFile))
            {
                var students = (List<Student>)serializer.Deserialize(reader);

                Console.WriteLine("--- Students XML ---");
                foreach (var s in students)
                {
                    Console.WriteLine(s);
                }
            }
        }

        static void Task10_ReadCsv()
        {
            if (!File.Exists(CsvFile))
            {
                Console.WriteLine("Missing CSV file.");
                return;
            }

            var lines = File.ReadAllLines(CsvFile);
            Console.WriteLine("--- First 5 rows ---");
            for (int i = 0; i < Math.Min(6, lines.Length); i++)
            {
                Console.WriteLine(lines[i].Replace(',', '\t'));
            }
        }

        static void Task11_CsvAverages()
        {
            if (!File.Exists(CsvFile))
            {
                Console.WriteLine("Missing CSV file.");
                return;
            }

            var lines = File.ReadAllLines(CsvFile).Skip(1);
            double sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;
            int count = 0;

            foreach (var line in lines)
            {
                var cols = line.Split(',');
                sum1 += double.Parse(cols[0], CultureInfo.InvariantCulture);
                sum2 += double.Parse(cols[1], CultureInfo.InvariantCulture);
                sum3 += double.Parse(cols[2], CultureInfo.InvariantCulture);
                sum4 += double.Parse(cols[3], CultureInfo.InvariantCulture);
                count++;
            }

            Console.WriteLine("--- Averages ---");
            Console.WriteLine($"Col1: {(sum1 / count):F2}");
            Console.WriteLine($"Col2: {(sum2 / count):F2}");
            Console.WriteLine($"Col3: {(sum3 / count):F2}");
            Console.WriteLine($"Col4: {(sum4 / count):F2}");
        }

        static void Task12_FilterCsv()
        {
            if (!File.Exists(CsvFile))
            {
                Console.WriteLine("Missing CSV file.");
                return;
            }

            var lines = File.ReadAllLines(CsvFile);
            List<string> filteredLines = new List<string>();

            filteredLines.Add("col1,col2,class");

            for (int i = 1; i < lines.Length; i++)
            {
                var col = lines[i].Split(',');
                if (double.TryParse(col[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                {
                    if (val < 5)
                    {
                        filteredLines.Add($"{col[0]},{col[1]},{col[col.Length - 1]}");
                    }
                }
            }

            File.WriteAllLines(CsvFileFiltered, filteredLines);
            Console.WriteLine($"Saved filtered CSV to {CsvFileFiltered}");
        }

        static void PrepareCsvFile()
        {
            if (!File.Exists(CsvFile))
            {
                string content = @"sepal.length,sepal.width,petal.length,petal.width,variety
5.1,3.5,1.4,0.2,Setosa
4.9,3.0,1.4,0.2,Setosa
4.7,3.2,1.3,0.2,Setosa
4.6,3.1,1.5,0.2,Setosa
5.0,3.6,1.4,0.2,Setosa
7.0,3.2,4.7,1.4,Versicolor
6.4,3.2,4.5,1.5,Versicolor
6.9,3.1,4.9,1.5,Versicolor
6.3,3.3,6.0,2.5,Virginica
5.8,2.7,5.1,1.9,Virginica";

                File.WriteAllText(CsvFile, content);
                Console.WriteLine("[INFO] Created iris.csv");
            }
        }
    }
}
