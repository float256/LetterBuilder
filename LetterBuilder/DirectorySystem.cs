using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LetterBuilder
{
    class DirectorySystem
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
        /// <param name="parentCatalogID">
        ///     Индекс родительского каталога. В случае, если он не указан, ставится
        ///     значение 0. Это означает, что каталог находится в корневой директории
        /// </param>
        public void AddCatalog(string name, int parentCatalogID = 0)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO catalog VALUES (@name, @parentCatalogID)", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogID", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@parentCatalogID"].Value = parentCatalogID;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateCatalog(int id, string name, int parentCatalogID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE catalog SET name=@name, id_parent_catalog=@parentCatalogID WHERE id_catalog=@ID", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogID", SqlDbType.Int);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@parentCatalogID"].Value = parentCatalogID;
                command.Parameters["@ID"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCatalog(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM catalog WHERE id_catalog=@ID", connection);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@ID"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Добавление текстового файла в систему в систему
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="text">Содержимое файла</param>
        /// <param name="parentCatalogID">
        ///     Индекс родительского каталога. В случае, если он не указан, ставится
        ///     значение 0. Это означает, что файл находится в корневой директории
        /// </param>
        public void AddText(string name, string text, int parentCatalogID = 0)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO text_block VALUES (@name, @text, @parentCatalogID)", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@text", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogID", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@text"].Value = text;
                command.Parameters["@parentCatalogID"].Value = parentCatalogID;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateTextBlock(int id, string name, string text, int parentCatalogID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE text_block SET name=@name, text=@text, id_parent_catalog=@parentCatalogID WHERE id_text_block=@ID", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@text", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogID", SqlDbType.Int);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@name"].Value = name;
                command.Parameters["@text"].Value = text;
                command.Parameters["@parentCatalogID"].Value = parentCatalogID;
                command.Parameters["@ID"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteTextBlock(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM text_block WHERE id_text_block=@ID", connection);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@ID"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Получение данных из таблицы catalog
        /// </summary>
        /// <returns>
        /// Функция возвращает два значения:
        /// 1) Словарь содержащий всю информацию из таблицы catalog. Значение строк записывается в 
        /// переменные класса CatalogTableRow. В качестве ключа для словаря используется id каталога
        /// 2) Словарь содержащий id всех дочерних каталогов для каждого каталога. В качестве ключа
        /// для словаря используется id каталога
        /// </returns>
        private (Dictionary<int, CatalogTableRow> catalogTableInfo, Dictionary<int, List<int>> directoryChilds) GetCatalogInfo()
        {
            Dictionary<int, CatalogTableRow> catalogTableInfo = new Dictionary<int, CatalogTableRow>();
            Dictionary<int, List<int>> directoryChilds = new Dictionary<int, List<int>>();

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
                    catalogTableInfo[currRow.ID] = currRow;
                    if (directoryChilds.ContainsKey(currRow.IDParentCatalog))
                    {
                        directoryChilds[currRow.IDParentCatalog].Add(currRow.ID);
                    }
                    else
                    {
                        directoryChilds[currRow.IDParentCatalog] = new List<int> { currRow.ID };
                    }
                }
            }
            return (catalogTableInfo, directoryChilds);
        }

        /// <summary>
        /// Получение данных из таблицы text_block
        /// </summary>
        /// <returns>
        /// Функция возвращает два значения:
        /// 1) Словарь содержащий всю информацию из таблицы text_block. Значение строк записывается в 
        /// переменные класса TextBlockTableRow. В качестве ключа для словаря используется id каталога
        /// 2) Словарь содержащий id всех текстовых файлов в каталоге. В качестве ключа
        /// для словаря используется id каталога
        /// </returns>
        private (Dictionary<int, TextBlockTableRow> textBlockTableInfo, Dictionary<int, List<int>> directoryTextBlockAttachments)  GetTextBlockTableInfo()
        {
            Dictionary<int, TextBlockTableRow> textBlockTableInfo = new Dictionary<int, TextBlockTableRow>();
            Dictionary<int, List<int>> directoryTextBlockAttachments = new Dictionary<int, List<int>>();

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
                    textBlockTableInfo[currRow.ID] = currRow;
                    if (directoryTextBlockAttachments.ContainsKey(currRow.IDParentCatalog))
                    {
                        directoryTextBlockAttachments[currRow.IDParentCatalog].Add(currRow.ID);
                    }
                    else
                    {
                        directoryTextBlockAttachments[currRow.IDParentCatalog] = new List<int> { currRow.ID };
                    }
                }
            }
            return (textBlockTableInfo, directoryTextBlockAttachments);
        }

        private Node CreateTree(Dictionary<int, List<int>> directoryChilds,
            Dictionary<int, List<int>> directoryTextBlockAttachments,
            int currentCatalog = 0)
        {
            Node result = new Node(currentCatalog, NodeType.Catalog);
            if (directoryTextBlockAttachments.ContainsKey(currentCatalog))
            {
                foreach (int id in directoryTextBlockAttachments[currentCatalog])
                {
                    result.AddChild(id, NodeType.TextBlock);
                }
            }
            if (directoryChilds.ContainsKey(currentCatalog))
            {
                foreach (int id in directoryChilds[currentCatalog])
                {
                    result.AddChild(CreateTree(directoryChilds, directoryTextBlockAttachments, id));
                }
            }
            return result;
        }

        private void PrintTree(Node tree, Dictionary<int, CatalogTableRow> catalogTableInfo,
            Dictionary<int, TextBlockTableRow> textBlockTableInfo, int indentWidth = 2, int depthLevel = 0)
        {
            string blankIndent = new string(' ', depthLevel * indentWidth);
            foreach (Node node in tree.NodeChilds)
            {
                if (node.Type == NodeType.Catalog)
                {
                    int id = catalogTableInfo[node.NodeData].ID;
                    string name = catalogTableInfo[node.NodeData].Name;
                    Console.WriteLine($"{blankIndent}{name}(catalog:{id})");
                    PrintTree(node, catalogTableInfo, textBlockTableInfo, depthLevel: depthLevel + 1);
                }
                else
                {
                    int id = textBlockTableInfo[node.NodeData].ID;
                    string name = textBlockTableInfo[node.NodeData].Name;
                    Console.WriteLine($"{blankIndent}{name}(catalog:{id})");
                }
            }
        }

        /// <summary>
        ///     Этот метод выводит в консоль структуру системы каталогов
        /// </summary>
        public void PrintStructure()
        {
            Dictionary<int, CatalogTableRow> catalogTableInfo;
            Dictionary<int, List<int>> directoryChilds;
            Dictionary<int, TextBlockTableRow> textBlockTableInfo;
            Dictionary<int, List<int>> directoryTextBlockAttachments;
            string result = string.Empty;

            // Получение данных о каталогах и их запись в catalogTableInfo и directoryChilds.
            (catalogTableInfo, directoryChilds) = GetCatalogInfo();

            // Получение данных о текстовых файлах их запись в directoryTextBlockAttachments.
            (textBlockTableInfo, directoryTextBlockAttachments) = GetTextBlockTableInfo();

            // Создание дерева файловой системы
            Node tree = CreateTree(directoryChilds, directoryTextBlockAttachments);

            // Печать результата.
            PrintTree(tree, catalogTableInfo, textBlockTableInfo);
        }
    }
}
