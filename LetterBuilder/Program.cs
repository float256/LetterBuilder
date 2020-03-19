using System;
using System.Data.SqlClient;

namespace LetterBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectorySystem dirSystem = new DirectorySystem(@"Data Source=DESKTOP-2U3C1KN\SQLEXPRESS;Initial Catalog=DirectoryStructure;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            if ((args.Length == 1) && (args[0] == "--help"))
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
            else if ((args.Length == 1) && (args[0] == "--showtree"))
            {
                Console.Write(dirSystem.GetStructure());
            }
            else if ((args.Length == 2) && (args[0] == "--add") && (args[1] == "catalog"))
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
                    Console.WriteLine("Incorrect Input");
                }
            }
            else if ((args.Length == 2) && (args[0] == "--add") && (args[1] == "text"))
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
                    Console.WriteLine("Incorrect Input");
                }
            }
            else if ((args.Length == 3) && (args[0] == "--update") && (args[1] == "catalog"))
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
                    Console.WriteLine("Incorrect Input");
                }
            }
            else if ((args.Length == 3) && (args[0] == "--update") && (args[1] == "text"))
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
            else if ((args.Length == 3) && (args[0] == "--delete"))
            {
                int id;
                Console.Write("Are you sure? yes/no: ");
                string answer = Console.ReadLine();
                if (answer == "yes")
                {
                    if (int.TryParse(args[2], out id))
                    {
                        switch (args[1])
                        {
                            case "text":
                                dirSystem.DeleteTextBlock(id);
                                break;
                            case "catalog":
                                dirSystem.DeleteCatalog(id);
                                break;
                            default:
                                Console.WriteLine("Incorrect Input");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Input");
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
