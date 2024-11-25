using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Xml.Linq;

//USE [StudentGrades]
//GO

//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[Grades] (
//    [Id]           INT            NOT NULL,
//    [FullName]     NVARCHAR (100) NOT NULL,
//    [GroupName]    NVARCHAR (50)  NOT NULL,
//    [AvgYearGrade] FLOAT (53)     NOT NULL,
//    [MinSubject]   NVARCHAR (50)  NOT NULL,
//    [MaxSubject]   NVARCHAR (50)  NOT NULL
//);

namespace ADO_ClassWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudentGrades;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))

                Console.WriteLine("1. Connect to Database\n" +
                "2. Display all records\n" +
                "3. Display all student names\n" +
                "4. Display all average grades\n" +
                "5. Display students with min grade above specified\n" +
                "6. Display unique subjects with minimum grades\n" +
                "7. Display minimum average grade\n" +
                "8. Display maximum average grade\n" +
                "9. Display count of students with min grade in Math\n" +
                "10. Display count of students with max grade in Math\n" +
                "11. Display student count in each group\n" +
                "12. Display average grade per group\n" +
                "0. Exit");
            int MenyChoice;
            do
            {
                Console.Write("\nEnter your choice: ");
                MenyChoice = int.Parse(Console.ReadLine());

                switch (MenyChoice)
                {
                    case 1:
                        {
                            try
                            {
                                using (SqlConnection connection = new SqlConnection(ConnectionString))
                                {
                                    connection.Open();
                                    Console.WriteLine("Successfully connected to the database.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        break;
                    case 2:
                            
                        string query = "SELECT * FROM Grades";
 
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("\nAll Records:");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"Id: {reader["Id"]}, FullName: {reader["FullName"]}, GroupName: {reader["GroupName"]}, AvgYearGrade: {reader["AvgYearGrade"]}, MinSubject: {reader["MinSubject"]}, MaxSubject: {reader["MaxSubject"]}");
                                }
                            }
                        }
                        break;
                    case 3:
                        string column = "FullName";
                        query = $"SELECT DISTINCT {column} FROM Grades";

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            Console.WriteLine($"\n{column}s:");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader[column]);
                                }
                            }
                        }
                        break;
                    case 4:
                        column = "AvgYearGrade";
                        query = $"SELECT DISTINCT {column} FROM Grades";
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            Console.WriteLine($"\n{column}s:");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader[column]);
                                }
                            }
                        }
                        break;
                    case 5:
                        Console.Write("Enter minimum grade threshold: ");
                        float minGrade = float.Parse(Console.ReadLine());
                        query = "SELECT FullName FROM Grades WHERE AvgYearGrade > @MinGrade";

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MinGrade", minGrade);
                            connection.Open();

                            Console.WriteLine($"Students with average grade above {minGrade}:");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader["FullName"]);
                                }
                            }
                        }
                        break;
                    case 6:
                        query = "SELECT DISTINCT MinSubject FROM Grades";

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            Console.WriteLine("\nUnique subjects with minimum grades:");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader["MinSubject"]);
                                }
                            }
                        }
                        break;
                    case 7:

                        ExecuteScalarQuery("SELECT MIN(AvgYearGrade) FROM Grades", "Minimum Average Grade");
                        break;
                    case 8:
                        ExecuteScalarQuery("SELECT MAX(AvgYearGrade) FROM Grades", "Maximum Average Grade");
                        break;
                    case 9:
                        DisplayCountBySubject("Math", "MinSubject");
                        break;
                    case 10:
                        DisplayCountBySubject("Math", "MaxSubject");
                        break;
                    case 11:
                        query = "SELECT GroupName, COUNT(*) AS StudentCount FROM Grades GROUP BY GroupName";

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            Console.WriteLine("\nStudent count per group:");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"GroupName: {reader["GroupName"]}, StudentCount: {reader["StudentCount"]}");
                                }
                            }
                        }
                        break;
                    case 12:
                        query = "SELECT GroupName, AVG(AvgYearGrade) AS AvgGroupGrade FROM Grades GROUP BY GroupName";

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            Console.WriteLine("\nAverage grade per group:");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"GroupName: {reader["GroupName"]}, AvgGroupGrade: {reader["AvgGroupGrade"]}");
                                }
                            }
                        }
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            } while (MenyChoice != 0);
        }

        static void DisplayCountBySubject(string subject, string column)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudentGrades;Integrated Security=True;Connect Timeout=30;";

            string query = $"SELECT COUNT(*) FROM Grades WHERE {column} = @Subject";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Subject", subject);
                connection.Open();

                int count = (int)command.ExecuteScalar();
                Console.WriteLine($"Number of students with {subject} as {column}: {count}");
            }
        }

        static void ExecuteScalarQuery(string query, string label)
        {
            string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StudentGrades;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                Console.WriteLine($"{label}: {result}");
            }
        }
    }
}

