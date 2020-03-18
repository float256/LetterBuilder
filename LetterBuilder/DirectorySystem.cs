using System.Collections.Generic;
using System.Data.SqlClient;

namespace LetterBuilder
{
    class DirectorySystem
    {
        public string ConnectionString { get; }
        struct CatalogTableRow
        {
            public int id;
            public string name;
            public int idParentCatalog;

            public CatalogTableRow(int id = 0, string name = null, int idParentCatalog = 0)
            {
                this.id = id;
                this.name = name;
                this.idParentCatalog = idParentCatalog;
            }
        };

        struct TextBlockTableRow
        {
            public int id;
            public string name;
            public string text;
            public int idParentCatalog;

            public TextBlockTableRow(int id = 0, string name = null, string text = null, int idParentCatalog = 0)
            {
                this.id = id;
                this.name = name;
                this.text = text;
                this.idParentCatalog = idParentCatalog;
            }
        };

        public DirectorySystem(string connectionString)
        {
            ConnectionString = connectionString;
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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"INSERT INTO catalog VALUES ('{name}', {parentCatalogID})";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateCatalog(int id, string name, int parentCatalogID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"UPDATE catalog SET name='{name}', id_parent_catalog={parentCatalogID} WHERE id_catalog={id}";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCatalog(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"DELETE FROM catalog WHERE id_catalog={id}";
                command.Connection = connection;
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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"INSERT INTO text_block VALUES ('{name}', '{text}', {parentCatalogID})";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateTextBlock(int id, string name, string text, int parentCatalogID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"UPDATE text_block SET name='{name}', text='{text}', id_parent_catalog={parentCatalogID} WHERE id_text_block={id}";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteTextBlock(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"DELETE  FROM text_block WHERE id_text_block={id}";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Этот метод возвращает структуру системы каталогов
        /// </summary>
        /// <returns>
        ///     Строка, содержащая структуру системы
        /// </returns>
        public string GetStructure()
        {
            Dictionary<int, CatalogTableRow> allCatalogInfo = new Dictionary<int, CatalogTableRow>();
            Dictionary<int, List<int>> directoryChilds = new Dictionary<int, List<int>>();
            Dictionary<int, TextBlockTableRow> allTextBlockInfo = new Dictionary<int, TextBlockTableRow>();
            Dictionary<int, List<int>> directoryTextBlockAttachments = new Dictionary<int, List<int>>();

            // Получение данных о каталогах и их запись в allCatalogInfo и directoryChilds.

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"SELECT * FROM catalog";
                command.Connection = connection;
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CatalogTableRow currRow = new CatalogTableRow(
                        (int)reader.GetValue(0), (string)reader.GetValue(1), (int)reader.GetValue(2));
                    allCatalogInfo[currRow.id] = currRow;
                    if (directoryChilds.ContainsKey(currRow.idParentCatalog))
                    {
                        directoryChilds[currRow.idParentCatalog].Add(currRow.id);
                    }
                    else
                    {
                        directoryChilds[currRow.idParentCatalog] = new List<int> { currRow.id };
                    }
                }
            }

            // Получение данных о текстовых файлах их запись в directoryTextBlockAttachments.

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    allTextBlockInfo[currRow.id] = currRow;
                    if (directoryTextBlockAttachments.ContainsKey(currRow.idParentCatalog))
                    {
                        directoryTextBlockAttachments[currRow.idParentCatalog].Add(currRow.id);
                    }
                    else
                    {
                        directoryTextBlockAttachments[currRow.idParentCatalog] = new List<int> { currRow.id };
                    }
                }
            }

            // Составление результирующей строки.

            if (allCatalogInfo.Count == 0)
            {
                return "";
            }

            // folderIndicesProcessingOrder - стек, содержащий порядок обработки папок.
            // parentDirectoryIndices - стек, содержащий индексы всех родительских каталогов относительно текущей обрабатываемой папки.
            Stack<int> folderIndicesProcessingOrder = new Stack<int>();
            Stack<int> parentDirectoryIndices = new Stack<int>();
            int depthLevel = 0;
            string result = "";

            // Вывод информации о файлах, находящихся в корневом каталоге
            if (directoryTextBlockAttachments.ContainsKey(0))
            {
                foreach (int textBlockIdx in directoryTextBlockAttachments[0])
                {
                    result += $"{allTextBlockInfo[textBlockIdx].name} (text: {textBlockIdx})\n";
                }
            }

            foreach (int item in directoryChilds[0])
            {
                folderIndicesProcessingOrder.Push(item);
            }
            while (folderIndicesProcessingOrder.Count != 0)
            {
                int currIdx = folderIndicesProcessingOrder.Peek();

                // Если текущий обрабатываемый каталог находится в parentDirectoryIndices, то содержимое
                // этой папки уже обработано и добавлено в result. Поэтому нужно удалить его из parentDirectoryIndices
                // и folderIndicesProcessingOrder, и понизить уровень глубины папок.
                if ((parentDirectoryIndices.Count != 0) && (parentDirectoryIndices.Peek() == currIdx))
                {
                    parentDirectoryIndices.Pop();
                    folderIndicesProcessingOrder.Pop();
                    depthLevel--;
                }
                else
                {
                    result += $"{new string(' ', depthLevel * 2)}{allCatalogInfo[currIdx].name} (catalog: {currIdx})\n";

                    // Запись в результирующую строку информации о вложенных файлах текущей папки
                    if (directoryTextBlockAttachments.ContainsKey(currIdx) && (directoryTextBlockAttachments[currIdx].Count != 0))
                    {
                        foreach (int textBlockIdx in directoryTextBlockAttachments[currIdx])
                        {
                            result += $"{new string(' ', depthLevel * 2 + 2)}{allTextBlockInfo[textBlockIdx].name} (text: {textBlockIdx})\n";
                        }
                    }

                    // Если у папки есть вложенные папки, то они добавляются в parentDirectoryIndices,
                    // т.е. их обработка начнется сразу после текущей итерации
                    if (directoryChilds.ContainsKey(currIdx) && (directoryChilds[currIdx].Count != 0))
                    {
                        parentDirectoryIndices.Push(currIdx);
                        foreach (int item in directoryChilds[currIdx])
                        {
                            folderIndicesProcessingOrder.Push(item);
                        }
                        depthLevel++;
                    }
                    else
                    {
                        folderIndicesProcessingOrder.Pop();
                    }
                }
            }
            return result;
        }
    }
}
