using System;
using System.Data.SqlClient;

namespace LetterBuilder
{
    class Program
    {
        static bool IsHelpCommand(string[] args) => (args.Length == 1) && (args[0] == "--help");
        static bool IsShowtreeCommand(string[] args) => (args.Length == 1) && (args[0] == "--showtree");
        static bool IsAddCatalogCommand(string[] args) => (args.Length == 2) && (args[0] == "--add") && (args[1] == "catalog");
        static bool IsAddTextCommand(string[] args) => (args.Length == 2) && (args[0] == "--add") && (args[1] == "text");
        static bool IsUpdateCatalogCommand(string[] args) => (args.Length == 3) && (args[0] == "--update") && (args[1] == "catalog");
        static bool IsUpdateTextCommand(string[] args) => (args.Length == 3) && (args[0] == "--update") && (args[1] == "text");
        static bool IsDeleteCalalogCommand(string[] args) => (args.Length == 3) && (args[0] == "--delete") && (args[1] == "catalog");
        static bool IsDeleteTextCommand(string[] args) => (args.Length == 3) && (args[0] == "--delete") && (args[1] == "text");

        static void Main(string[] args)
        {
            DirectorySystem dirSystem = new DirectorySystem(@"Data Source=DESKTOP-2U3C1KN\SQLEXPRESS;Initial Catalog=DirectoryStructure;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            if (IsHelpCommand(args))
            {
                Console.WriteLine("Available commands:\n" +
                    "  --showtree - print all catalogs\n" +
                    "  --add catalog\n" +
                    "  --add text\n" +
                    "  --delete catalog <id>\n" +
                    "  --delete text <id>\n" +
                    "  --update catalog <id>\n" +
                    "  --update text <id>");
            }
            else if (IsShowtreeCommand(args))
            {
                dirSystem.PrintStructure();
            }
            else if (IsAddCatalogCommand(args))
            {
                Console.Write("Please enter catalog name: ");
                string catalogName = Console.ReadLine();
                Console.Write("Please enter parent catalog id: ");
                int catalogId;
                if (int.TryParse(Console.ReadLine(), out catalogId))
                {
                    dirSystem.AddCatalog(catalogName, catalogId);
                }
                else
                {
                    Console.WriteLine("Integer value required");
                }
            }
            else if (IsAddTextCommand(args))
            {
                Console.Write("Please enter text block name: ");
                string name = Console.ReadLine();

                Console.Write("Please enter text: ");
                string text = Console.ReadLine();

                Console.Write("Please enter parent catalog id: ");
                int catalogId;
                if (int.TryParse(Console.ReadLine(), out catalogId))
                {
                    dirSystem.AddText(name, text, catalogId);
                }
                else
                {
                    Console.WriteLine("Integer value required");
                }
            }
            else if (IsUpdateCatalogCommand(args))
            {
                int id, parentCatalogId;
                Console.Write("Please enter catalog name: ");
                string name = Console.ReadLine();
                Console.Write("Please enter parent catalog id: ");
                if (int.TryParse(Console.ReadLine(), out parentCatalogId) && int.TryParse(args[2], out id))
                {
                    dirSystem.UpdateCatalog(id, name, parentCatalogId);
                }
                else
                {
                    Console.WriteLine("Integer value required");
                }
            }
            else if (IsUpdateTextCommand(args))
            {
                int id, catalogId;
                Console.Write("Please enter block name: ");
                string name = Console.ReadLine();
                Console.Write("Please enter text: ");
                string text = Console.ReadLine();
                Console.Write("Please enter parent catalog id: ");
                if (int.TryParse(Console.ReadLine(), out catalogId) && int.TryParse(args[2], out id))
                {
                    dirSystem.UpdateTextBlock(id, name, text, catalogId);
                }
                else
                {
                    Console.WriteLine("Integer value required");
                }
            }
            else if (IsDeleteCalalogCommand(args))
            {
                int id;
                Console.Write("Are you sure? yes/no: ");
                string answer = Console.ReadLine();
                if (answer == "yes")
                {
                    if (int.TryParse(args[2], out id))
                    {
                        dirSystem.DeleteCatalog(id);
                    }
                    else
                    {
                        Console.WriteLine("Integer value required");
                    }
                }
            }
            else if (IsDeleteTextCommand(args))
            {
                int id;
                Console.Write("Are you sure? yes/no: ");
                string answer = Console.ReadLine();
                if (answer == "yes")
                {
                    if (int.TryParse(args[2], out id))
                    {
                        dirSystem.DeleteTextBlock(id);
                    }
                    else
                    {
                        Console.WriteLine("Integer value required");
                    }
                }
            }
            else
            {
                Console.WriteLine("Incorrect command");
            }
        }
    }
}
