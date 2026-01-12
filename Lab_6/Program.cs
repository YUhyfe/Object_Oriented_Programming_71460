using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace App
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Imie { get; set; } = "";
        public string Nazwisko { get; set; } = "";
        public List<Ocena> Oceny { get; set; } = new();
    }

    public class Ocena
    {
        public int OcenaId { get; set; }
        public double Wartosc { get; set; }
        public string Przedmiot { get; set; } = "";
        public int StudentId { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string connectionString =
                "Data Source=localhost\\SQLEXPRESS;" +
                "Initial Catalog=Programowanie;" +
                "Integrated Security=True;" +
                "Encrypt=True;" +
                "TrustServerCertificate=True";

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Połączono z bazą danych.");
                Console.WriteLine("------------------------------------------------");

                WypiszStudentow(connection);

                WypiszStudentaPoId(connection, 1);

                var studenci = PobierzStudentowZOcenami(connection);
                foreach (var s in studenci)
                {
                    Console.WriteLine($"Student: {s.Imie} {s.Nazwisko}");
                    foreach (var o in s.Oceny)
                    {
                        Console.WriteLine($" - {o.Przedmiot}: {o.Wartosc}");
                    }
                }
                
                DodajStudenta(connection, new Student { Imie = "Testowy", Nazwisko = "Janusz" });

                DodajOcene(connection, new Ocena { Wartosc = 4.5, Przedmiot = "W-F", StudentId = 1 });

                UsunOcenyZPrzedmiotu(connection, "geografia");

                ZaktualizujOcene(connection, 1, 5.0);

            }
            catch (Exception exc)
            {
                Console.WriteLine("Wystąpił błąd: " + exc.Message);
            }
        }

        static void WypiszStudentow(SqlConnection connection)
        {
            string sql = "SELECT student_id, imie, nazwisko FROM student";
            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Lista studentów:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["student_id"]}: {reader["imie"]} {reader["nazwisko"]}");
            }
        }

        static void WypiszStudentaPoId(SqlConnection connection, int id)
        {
            string sql = "SELECT imie, nazwisko FROM student WHERE student_id = @id";
            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"Znaleziono: {reader["imie"]} {reader["nazwisko"]}");
            }
            else
            {
                Console.WriteLine($"Nie znaleziono studenta o ID: {id}");
            }
        }

        static List<Student> PobierzStudentowZOcenami(SqlConnection connection)
        {
            var studentDictionary = new Dictionary<int, Student>();

            string sql = @"
                SELECT s.student_id, s.imie, s.nazwisko, o.ocena_id, o.wartosc, o.przedmiot 
                FROM student s 
                LEFT JOIN ocena o ON s.student_id = o.student_id";

            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int sId = (int)reader["student_id"];

                if (!studentDictionary.TryGetValue(sId, out Student currentStudent))
                {
                    currentStudent = new Student
                    {
                        StudentId = sId,
                        Imie = (string)reader["imie"],
                        Nazwisko = (string)reader["nazwisko"],
                        Oceny = new List<Ocena>()
                    };
                    studentDictionary.Add(sId, currentStudent);
                }

                if (reader["ocena_id"] != DBNull.Value)
                {
                    currentStudent.Oceny.Add(new Ocena
                    {
                        OcenaId = (int)reader["ocena_id"],
                        Wartosc = (double)reader["wartosc"],
                        Przedmiot = (string)reader["przedmiot"],
                        StudentId = sId
                    });
                }
            }

            return new List<Student>(studentDictionary.Values);
        }

        static void DodajStudenta(SqlConnection connection, Student student)
        {
            string sql = "INSERT INTO student (imie, nazwisko) VALUES (@imie, @nazwisko)";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@imie", student.Imie);
            command.Parameters.AddWithValue("@nazwisko", student.Nazwisko);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"Dodano studenta. Zmieniono wierszy: {rowsAffected}");
        }

        static void DodajOcene(SqlConnection connection, Ocena ocena)
        {
            if (!CzyOcenaPoprawna(ocena.Wartosc))
            {
                Console.WriteLine($"Błąd: Wartość {ocena.Wartosc} jest niepoprawna. Ocena nie została dodana.");
                return;
            }

            string sql = "INSERT INTO ocena (wartosc, przedmiot, student_id) VALUES (@val, @sub, @sid)";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@val", ocena.Wartosc);
            command.Parameters.AddWithValue("@sub", ocena.Przedmiot);
            command.Parameters.AddWithValue("@sid", ocena.StudentId);

            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Dodano ocenę.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Błąd SQL (np. brak studenta): " + ex.Message);
            }
        }

        static void UsunOcenyZPrzedmiotu(SqlConnection connection, string przedmiot)
        {
            string sql = "DELETE FROM ocena WHERE przedmiot = @przedmiot";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@przedmiot", przedmiot);

            int rows = command.ExecuteNonQuery();
            Console.WriteLine($"Usunięto ocen z przedmiotu '{przedmiot}': {rows}");
        }

        static void ZaktualizujOcene(SqlConnection connection, int ocenaId, double nowaWartosc)
        {
            if (!CzyOcenaPoprawna(nowaWartosc))
            {
                Console.WriteLine($"Błąd: Wartość {nowaWartosc} jest niepoprawna.");
                return;
            }

            string sql = "UPDATE ocena SET wartosc = @val WHERE ocena_id = @id";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@val", nowaWartosc);
            command.Parameters.AddWithValue("@id", ocenaId);

            int rows = command.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine("Zaktualizowano ocenę.");
            else
                Console.WriteLine("Nie znaleziono oceny o podanym ID.");
        }

        static bool CzyOcenaPoprawna(double w)
        {
            if (w < 2.0 || w > 5.0) return false;

            if (w == 2.5) return false;

            if (w % 0.5 != 0) return false;

            return true;
        }
    }
}