using System.Text.RegularExpressions;

// ── helpers ──────────────────────────────────────────────────────────────────

static void PrintLine(string text = "", ConsoleColor color = ConsoleColor.White)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}

static void PrintCentered(string text, ConsoleColor color = ConsoleColor.White, int width = 70)
{
    int padding = Math.Max(0, (width - text.Length) / 2);
    Console.ForegroundColor = color;
    Console.WriteLine(new string(' ', padding) + text);
    Console.ResetColor();
}

static void PrintDivider(char ch = '─', int width = 70, ConsoleColor color = ConsoleColor.DarkGray)
{
    Console.ForegroundColor = color;
    Console.WriteLine(new string(ch, width));
    Console.ResetColor();
}

static bool IsValidEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email)) return false;
    return Regex.IsMatch(email,
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.IgnoreCase);
}

static string GetEmailsFilePath()
{
    // Save next to the executable
    string dir = AppContext.BaseDirectory;
    return Path.Combine(dir, "registered_students.csv");
}

static bool EmailAlreadyRegistered(string path, string email)
{
    if (!File.Exists(path)) return false;
    foreach (string line in File.ReadLines(path))
    {
        string[] parts = line.Split(',');
        if (parts.Length >= 2 && parts[1].Trim().Equals(email, StringComparison.OrdinalIgnoreCase))
            return true;
    }
    return false;
}

static void SaveEmail(string path, string email)
{
    bool fileExists = File.Exists(path);
    using StreamWriter sw = File.AppendText(path);
    if (!fileExists)
        sw.WriteLine("Timestamp,Email");                          // header row
    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{email}");
}

// ── banner ───────────────────────────────────────────────────────────────────

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Clear();

PrintDivider('═', 70, ConsoleColor.Cyan);
PrintCentered("", ConsoleColor.White);
PrintCentered("  ACSC 330  ·  Interactive Design", ConsoleColor.Cyan);
PrintCentered("C# & Unity Laboratory — Spring 2026", ConsoleColor.White);
PrintCentered("", ConsoleColor.White);
PrintDivider('═', 70, ConsoleColor.Cyan);
Console.WriteLine();

// ── course overview ───────────────────────────────────────────────────────────

PrintLine("  Welcome to ACSC 330!", ConsoleColor.Yellow);
Console.WriteLine();
PrintLine("  This course introduces you to interactive application development", ConsoleColor.Gray);
PrintLine("  using C# and the Unity game engine. By the end of the semester", ConsoleColor.Gray);
PrintLine("  you will design, build, and publish your own interactive projects.", ConsoleColor.Gray);
Console.WriteLine();

PrintDivider(color: ConsoleColor.DarkGray);

PrintLine();
PrintLine("  Lab Schedule Overview", ConsoleColor.Yellow);
PrintLine();

var labs = new (int No, string Topic)[]
{
    (1,  "C# Fundamentals & .NET Environment Setup"),
    (2,  "Object-Oriented Programming in C#"),
    (3,  "Unity Interface, GameObjects & Components"),
    (4,  "Scripting with C# in Unity"),
    (5,  "Physics, Colliders & Rigidbody"),
    (6,  "UI System — Canvas, Buttons & Events"),
    (7,  "Scene Management & Game Flow"),
    (8,  "Audio, Animation & Particle Systems"),
    (9,  "Mid-term Project Review"),
    (10, "Advanced Interactions & Custom Input"),
    (11, "Lighting, Shaders & Visual Polish"),
    (12, "Optimization & Build Pipeline"),
    (13, "Publishing & Deployment"),
    (14, "Final Project Presentations"),
};

foreach (var (no, topic) in labs)
{
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write($"    Lab {no,2}  ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(topic);
}

Console.ResetColor();
Console.WriteLine();
PrintDivider(color: ConsoleColor.DarkGray);

// ── assessment breakdown ──────────────────────────────────────────────────────

PrintLine();
PrintLine("  Assessment Breakdown", ConsoleColor.Yellow);
PrintLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.Write("    Lab Work       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("40 %");
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.Write("    Mid-term       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("25 %");
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.Write("    Final Project  "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("35 %");
Console.ResetColor();
Console.WriteLine();
PrintDivider(color: ConsoleColor.DarkGray);

// ── email registration ────────────────────────────────────────────────────────

PrintLine();
PrintLine("  Student Registration", ConsoleColor.Yellow);
PrintLine();
PrintLine("  Please enter your university e-mail address to register", ConsoleColor.Gray);
PrintLine("  your attendance and receive course updates.", ConsoleColor.Gray);
PrintLine();

string csvPath = GetEmailsFilePath();

while (true)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("  Email > ");
    Console.ForegroundColor = ConsoleColor.Green;
    string? input = Console.ReadLine()?.Trim();
    Console.ResetColor();

    if (string.IsNullOrWhiteSpace(input))
    {
        PrintLine("  Please enter your email address.", ConsoleColor.Red);
        continue;
    }

    if (!IsValidEmail(input))
    {
        PrintLine($"  '{input}' does not look like a valid email. Try again.", ConsoleColor.Red);
        continue;
    }

    if (EmailAlreadyRegistered(csvPath, input))
    {
        PrintLine();
        PrintLine("  This email is already registered — you're all set!", ConsoleColor.Yellow);
        break;
    }

    SaveEmail(csvPath, input);

    PrintLine();
    PrintDivider('─', 70, ConsoleColor.DarkGreen);
    PrintCentered("Registration Successful", ConsoleColor.Green);
    PrintDivider('─', 70, ConsoleColor.DarkGreen);
    PrintLine();
    PrintLine($"  Registered: {input}", ConsoleColor.White);
    PrintLine($"  Saved to:   {csvPath}", ConsoleColor.DarkGray);
    PrintLine();
    PrintLine("  See you in class!  Good luck this semester.", ConsoleColor.Yellow);
    break;
}

PrintLine();
PrintDivider('═', 70, ConsoleColor.Cyan);
PrintLine();
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.Write("  Press any key to exit...");
Console.ResetColor();
Console.ReadKey(intercept: true);
Console.WriteLine();
