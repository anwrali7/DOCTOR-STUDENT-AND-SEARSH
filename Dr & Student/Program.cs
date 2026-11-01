using System;
using System.Collections.Generic;

namespace ExaminationSystem
{
    public enum QuestionLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }

    public enum ExamType
    {
        Practical = 1,
        Final = 2
    }

    public abstract class Question
    {
        public string? Header { get; set; }
        public int Marks { get; set; }
        public QuestionLevel Level { get; set; }

        public Question(string? header, int marks, QuestionLevel level)
        {
            Header = header;
            Marks = marks;
            Level = level;
        }

        public abstract void Display();
        public abstract bool CheckAnswer(string? answer);
    }

    public class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }

        public TrueFalseQuestion(string? header, int marks, QuestionLevel level, bool correctAnswer)
            : base(header, marks, level)
        {
            CorrectAnswer = correctAnswer;
        }

        public override void Display()
        {
            Console.WriteLine($"\n[True/False] {Header} ({Marks} Marks)");
            Console.WriteLine("1. True");
            Console.WriteLine("2. False");
        }

        public override bool CheckAnswer(string? answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                return false;
            }

            string clean = answer.Trim().ToLower();
            if (clean == "1" || clean == "true")
            {
                return CorrectAnswer == true;
            }
            if (clean == "2" || clean == "false")
            {
                return CorrectAnswer == false;
            }
            return false;
        }
    }

    public class ChooseOneQuestion : Question
    {
        public List<string?> Choices { get; set; }
        public int CorrectChoiceNumber { get; set; }

        public ChooseOneQuestion(string? header, int marks, QuestionLevel level, List<string?> choices, int correctChoiceNumber)
            : base(header, marks, level)
        {
            Choices = choices;
            CorrectChoiceNumber = correctChoiceNumber;
        }

        public override void Display()
        {
            Console.WriteLine($"\n[Choose One] {Header} ({Marks} Marks)");
            for (int i = 0; i < Choices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Choices[i]}");
            }
        }

        public override bool CheckAnswer(string? answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                return false;
            }

            return answer.Trim() == CorrectChoiceNumber.ToString();
        }
    }

    public class MultipleChoiceQuestion : Question
    {
        public List<string?> Choices { get; set; }
        public List<string> CorrectAnswers { get; set; }

        public MultipleChoiceQuestion(string? header, int marks, QuestionLevel level, List<string?> choices, List<string> correctAnswers)
            : base(header, marks, level)
        {
            Choices = choices;
            CorrectAnswers = correctAnswers;
        }

        public override void Display()
        {
            Console.WriteLine($"\n[Multiple Choice] {Header} ({Marks} Marks)");
            for (int i = 0; i < Choices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Choices[i]}");
            }
            Console.WriteLine("(Enter choice numbers separated by comma, 1,3)");
        }

        public override bool CheckAnswer(string? answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                return false;
            }

            string[] rawAnswers = answer.Split(',');
            List<string> cleanAnswers = new List<string>();

            foreach (var item in rawAnswers)
            {
                string token = item.Trim();
                if (!string.IsNullOrEmpty(token) && !cleanAnswers.Contains(token))
                {
                    cleanAnswers.Add(token);
                }
            }

            if (cleanAnswers.Count != CorrectAnswers.Count)
            {
                return false;
            }

            foreach (var correct in CorrectAnswers)
            {
                if (!cleanAnswers.Contains(correct))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Program
    {
        static List<Question> questionBank = new List<Question>();

        public static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== Main Menu ===");
                Console.WriteLine("1. Doctor Mode");
                Console.WriteLine("2. Student Mode");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice (1-3): ");

                string? option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        DoctorMode();
                        break;
                    case "2":
                        StudentMode();
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("Exiting program. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        public static void DoctorMode()
        {
            Console.WriteLine("\n--- DOCTOR MODE ---");
            
            int count = 0;
            while (true)
            {
                Console.Write("Enter number of questions to add: ");
                try
                {
                    count = Convert.ToInt32(Console.ReadLine());
                    if (count > 0) break;
                    Console.WriteLine("Please enter a positive number.");
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nSetting up Question #{i + 1}:");
                
                string? type = "";
                while (true)
                {
                    Console.Write("Choose Question Type (1. True/False, 2. Choose One, 3. Multiple Choice): ");
                    type = Console.ReadLine();
                    if (type == "1" || type == "2" || type == "3") break;
                    Console.WriteLine("Invalid type. Enter 1, 2, or 3 only.");
                }

                int levelInput = 0;
                while (true)
                {
                    Console.Write("Choose Level (1- Easy, 2- Medium, 3- Hard): ");
                    try
                    {
                        levelInput = Convert.ToInt32(Console.ReadLine());
                        if (levelInput >= 1 && levelInput <= 3) break;
                        Console.WriteLine("Invalid level. Enter 1, 2, or 3 only.");
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                }
                QuestionLevel level = (QuestionLevel)levelInput;

                Console.Write("Enter Question Header: ");
                string? header = Console.ReadLine();

                int marks = 0;
                while (true)
                {
                    Console.Write("Enter Marks: ");
                    try
                    {
                        marks = Convert.ToInt32(Console.ReadLine());
                        if (marks > 0) break;
                        Console.WriteLine("Marks must be greater than 0.");
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                }

                if (type == "1")
                {
                    string? ans = "";
                    while (true)
                    {
                        Console.Write("Enter Correct Answer (1 for True / 2 for False): ");
                        ans = Console.ReadLine();
                        if (ans == "1" || ans == "2") break;
                        Console.WriteLine("Invalid answer. Enter 1 or 2 only.");
                    }
                    bool isTrue = (ans == "1");
                    questionBank.Add(new TrueFalseQuestion(header, marks, level, isTrue));
                }
                else if (type == "2")
                {
                    List<string?> choices = new List<string?>();
                    for (int c = 1; c <= 4; c++)
                    {
                        Console.Write($"Enter Choice {c}: ");
                        choices.Add(Console.ReadLine());
                    }

                    int correctChoice = 0;
                    while (true)
                    {
                        Console.Write("Enter Correct Choice Number (1-4): ");
                        try
                        {
                            correctChoice = Convert.ToInt32(Console.ReadLine());
                            if (correctChoice >= 1 && correctChoice <= 4) break;
                            Console.WriteLine("Invalid choice. Enter a number between 1 and 4.");
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                        }
                    }
                    questionBank.Add(new ChooseOneQuestion(header, marks, level, choices, correctChoice));
                }
                else if (type == "3")
                {
                    List<string?> choices = new List<string?>();
                    for (int c = 1; c <= 4; c++)
                    {
                        Console.Write($"Enter Choice {c}: ");
                        choices.Add(Console.ReadLine());
                    }

                    Console.Write("Enter Correct Answers (comma separated, e.g., 1,3): ");
                    string? rawInput = Console.ReadLine();
                    string[] parts = (rawInput ?? "").Split(',');
                    List<string> correctAnswers = new List<string>();
                    foreach (var part in parts)
                    {
                        correctAnswers.Add(part.Trim());
                    }
                    questionBank.Add(new MultipleChoiceQuestion(header, marks, level, choices, correctAnswers));
                }
            }

            Console.WriteLine("\nQuestions added successfully to Question Bank!");
        }

        public static void StudentMode()
        {
            Console.WriteLine("\n--- STUDENT MODE ---");
            if (questionBank.Count == 0)
            {
                Console.WriteLine("Question Bank is empty! Please add questions in Doctor Mode first.");
                return;
            }

            string? examChoice = "";
            while (true)
            {
                Console.Write("Choose Exam Type (1. Practical, 2. Final): ");
                examChoice = Console.ReadLine();
                if (examChoice == "1" || examChoice == "2") break;
                Console.WriteLine("Invalid choice. Enter 1 or 2 only.");
            }

            int levelNum = 0;
            while (true)
            {
                Console.Write("Choose Level (1. Easy, 2. Medium, 3. Hard): ");
                try
                {
                    levelNum = Convert.ToInt32(Console.ReadLine());
                    if (levelNum >= 1 && levelNum <= 3) break;
                    Console.WriteLine("Invalid level. Enter 1, 2, or 3 only.");
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            QuestionLevel selectedLevel = (QuestionLevel)levelNum;

            List<Question> filteredQuestions = new List<Question>();
            foreach (var q in questionBank)
            {
                if (q.Level == selectedLevel)
                {
                    filteredQuestions.Add(q);
                }
            }

            if (filteredQuestions.Count == 0)
            {
                Console.WriteLine("No questions available for this level.");
                return;
            }

            int countToTake = filteredQuestions.Count;
            if (examChoice == "1")
            {
                countToTake = filteredQuestions.Count / 2;
                if (countToTake == 0)
                {
                    countToTake = 1;
                }
            }

            int score = 0;
            int totalMarks = 0;

            for (int i = 0; i < countToTake; i++)
            {
                Question currentQuestion = filteredQuestions[i];
                totalMarks += currentQuestion.Marks;

                currentQuestion.Display();

                Console.Write("Your Answer: ");
                string? studentAnswer = Console.ReadLine();

                if (currentQuestion.CheckAnswer(studentAnswer))
                {
                    score += currentQuestion.Marks;
                }
            }

            Console.WriteLine($"\nYour Result: {score}/{totalMarks}");
        }
    }
}