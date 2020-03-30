using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LetterBuilder
{
    public class DirectorySystem
    {
        private readonly string _connectionString;

        public DirectorySystem(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        ///     Добавление каталога в систему
        /// </summary>
        /// <param name="name">Имя каталога</param>
        /// <param name="parentCatalogId">
        ///     Индекс родительского каталога. В случае, если он не указан, ставится
        ///     значение 0. Это означает, что каталог находится в корневой директории
        /// </param>
        public void AddCatalog(string name, int parentCatalogId = 0)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO catalog VALUES (@name, @parentCatalogId)", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@parentCatalogId"].Value = parentCatalogId;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateCatalog(int id, string name, int parentCatalogId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE catalog SET name=@name, id_parent_catalog=@parentCatalogId WHERE id_catalog=@id", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@parentCatalogId"].Value = parentCatalogId;
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCatalog(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM catalog WHERE id_catalog=@id", connection);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Добавление текстового файла в систему в систему
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="text">Содержимое файла</param>
        /// <param name="parentCatalogId">
        ///     Индекс родительского каталога. В случае, если он не указан, ставится
        ///     значение 0. Это означает, что файл находится в корневой директории
        /// </param>
        public void AddText(string name, string text, int parentCatalogId = 0)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO text_block VALUES (@name, @text, @parentCatalogId)", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@text", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@text"].Value = text;
                command.Parameters["@parentCatalogId"].Value = parentCatalogId;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateTextBlock(int id, string name, string text, int parentCatalogId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE text_block SET name=@name, text=@text, id_parent_catalog=@parentCatalogId WHERE id_text_block=@id", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@text", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@text"].Value = text;
                command.Parameters["@parentCatalogId"].Value = parentCatalogId;
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteTextBlock(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM text_block WHERE id_text_block=@id", connection);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Этот метод выводит в консоль структуру системы каталогов
        /// </summary>
        public void PrintStructure()
        {
            Dictionary<int, CatalogTableRow> catalogTableInfo;
            Dictionary<int, List<int>> catalogToChildCatalogsMap;
            Dictionary<int, TextBlockTableRow> textBlockTableInfo;
            Dictionary<int, List<int>> catalogToChildTextBlocksMap;
            string result = string.Empty;

            // Получение данных о каталогах и их запись в CatalogTableInfo и CatalogToChildCatalogsMap.
            CatalogsReadingResult catalogsReadingResult = GetCatalogTableInfo();
            catalogTableInfo = catalogsReadingResult.CatalogTableInfo;
            catalogToChildCatalogsMap = catalogsReadingResult.CatalogToChildCatalogsMap;

            // Получение данных о текстовых файлах их запись в CatalogToChildTextBlocksMap.
            TextBlocksReadingResult textBlockReadingResult = GetTextBlockTableInfo();
            textBlockTableInfo = textBlockReadingResult.TextBlockTableInfo;
            catalogToChildTextBlocksMap = textBlockReadingResult.CatalogToChildTextBlocksMap;

            // Создание дерева файловой системы
            Node tree = CreateTree(catalogToChildCatalogsMap, catalogToChildTextBlocksMap);

            // Печать результата.
            PrintTree(tree, catalogTableInfo, textBlockTableInfo);
        }

        private void PrintTree(Node tree, Dictionary<int, CatalogTableRow> catalogTableInfo,
            Dictionary<int, TextBlockTableRow> textBlockTableInfo, int indentWidth = 2, int depthLevel = 0)
        {
            string blankIndent = new string(' ', depthLevel * indentWidth);
            foreach (Node node in tree.ChildrenNodes)
            {
                if (node.Type == NodeType.Catalog)
                {
                    int id = catalogTableInfo[node.ElementId].Id;
                    string name = catalogTableInfo[node.ElementId].Name;
                    Console.WriteLine($"{blankIndent}{name}(catalog:{id})");
                    PrintTree(node, catalogTableInfo, textBlockTableInfo, depthLevel: depthLevel + 1);
                }
                else
                {
                    int id = textBlockTableInfo[node.ElementId].Id;
                    string name = textBlockTableInfo[node.ElementId].Name;
                    Console.WriteLine($"{blankIndent}{name}(text:{id})");
                }
            }
        }

        private Node CreateTree(Dictionary<int, List<int>> catalogToChildCatalogsMap,
            Dictionary<int, List<int>> catalogToChildTextBlocksMap,
            int currentCatalogId = 0)
        {
            Node result = new Node(currentCatalogId, NodeType.Catalog);
            if (catalogToChildTextBlocksMap.ContainsKey(currentCatalogId))
            {
                foreach (int id in catalogToChildTextBlocksMap[currentCatalogId])
                {
                    result.AddChild(id, NodeType.TextBlock);
                }
            }
            if (catalogToChildCatalogsMap.ContainsKey(currentCatalogId))
            {
                foreach (int id in catalogToChildCatalogsMap[currentCatalogId])
                {
                    result.AddChild(CreateTree(catalogToChildCatalogsMap, catalogToChildTextBlocksMap, id));
                }
            }
            return result;
        }

        /// <summary>
        /// Получение данных из таблицы text_block
        /// </summary>
        /// <returns>
        /// Функция возвращает структуру, содержащую два значения:
        /// TextBlockTableInfo: Словарь содержащий всю информацию из таблицы text_block. Значение строк записывается в 
        /// переменные класса TextBlockTableRow. В качестве ключа для словаря используется id каталога
        /// CatalogToChildTextBlocksMap: Словарь содержащий id всех текстовых файлов в каталоге. В качестве ключа
        /// для словаря используется id каталога
        /// </returns>
        private TextBlocksReadingResult GetTextBlockTableInfo()
        {
            Dictionary<int, TextBlockTableRow> textBlockTableInfo = new Dictionary<int, TextBlockTableRow>();
            Dictionary<int, List<int>> catalogToChildTextBlocksMap = new Dictionary<int, List<int>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"SELECT * FROM text_block";
                command.Connection = connection;
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBlockTableRow currRow = new TextBlockTableRow(
                        (int)reader.GetValue(0), (string)reader.GetValue(1), (string)reader.GetValue(2), (int)reader.GetValue(3));
                    textBlockTableInfo[currRow.Id] = currRow;
                    if (catalogToChildTextBlocksMap.ContainsKey(currRow.ParentCatalogId))
                    {
                        catalogToChildTextBlocksMap[currRow.ParentCatalogId].Add(currRow.Id);
                    }
                    else
                    {
                        catalogToChildTextBlocksMap[currRow.ParentCatalogId] = new List<int> { currRow.Id };
                    }
                }
            }
            TextBlocksReadingResult result = new TextBlocksReadingResult();
            result.TextBlockTableInfo = textBlockTableInfo;
            result.CatalogToChildTextBlocksMap = catalogToChildTextBlocksMap;
            return result;
        }

        /// <summary>
        /// Получение данных из таблицы catalog
        /// </summary>
        /// <returns>
        /// Функция возвращает структуру, содержащую два значения:
        /// CatalogTableInfo: Словарь содержащий всю информацию из таблицы catalog. Значение строк записывается в 
        /// переменные класса CatalogTableRow. В качестве ключа для словаря используется id каталога
        /// CatalogToChildCatalogsMap: Словарь содержащий id всех дочерних каталогов для каждого каталога. В качестве ключа
        /// для словаря используется id каталога
        /// </returns>
        private CatalogsReadingResult GetCatalogTableInfo()
        {
            Dictionary<int, CatalogTableRow> catalogTableInfo = new Dictionary<int, CatalogTableRow>();
            Dictionary<int, List<int>> catalogToChildCatalogsMap = new Dictionary<int, List<int>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM catalog", connection);
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CatalogTableRow currRow = new CatalogTableRow(
                        (int)reader.GetValue(0), (string)reader.GetValue(1), (int)reader.GetValue(2));
                    catalogTableInfo[currRow.Id] = currRow;
                    if (catalogToChildCatalogsMap.ContainsKey(currRow.ParentCatalogId))
                    {
                        catalogToChildCatalogsMap[currRow.ParentCatalogId].Add(currRow.Id);
                    }
                    else
                    {
                        catalogToChildCatalogsMap[currRow.ParentCatalogId] = new List<int> { currRow.Id };
                    }
                }
            }
            CatalogsReadingResult result = new CatalogsReadingResult();
            result.CatalogTableInfo = catalogTableInfo;
            result.CatalogToChildCatalogsMap = catalogToChildCatalogsMap;
            return result;
        }
    }
}
