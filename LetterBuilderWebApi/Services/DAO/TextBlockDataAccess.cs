using LetterBuilderWebAdmin.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Services.DAO
{
    public class TextBlockDataAccess : ITextBlockDataAccess
    {
        private string _connectionString;

        public TextBlockDataAccess(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("default");
        }

        /// <summary>
        /// Данный метод возвращает все текстовые файлы, находящиеся в базе данных
        /// </summary>
        /// <returns>Объект типа List, содержащий все текстовые файлы</returns>
        public List<TextBlock> GetAll()
        {
            List<TextBlock> repositoryContent = new List<TextBlock>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM text_block", connection);
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock currTextBlock = new TextBlock
                    {
                        Id = (int)reader.GetValue(0),
                        Name = (string)reader.GetValue(1),
                        Text = (string)reader.GetValue(2),
                        ParentCatalogId = (int)reader.GetValue(3),
                        OrderInParentCatalog = (int)reader.GetValue(4)
                    };
                    repositoryContent.Add(currTextBlock);
                }
            }
            return repositoryContent;
        }

        /// <summary>
        /// Данный метод возвращает запись из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса TextBlock, содержащий значения для указанного
        /// текстового файла из базы данных. Если записи нет, то возвращается объект со значениями по-умолчанию</returns>
        public TextBlock GetById(int id)
        {
            TextBlock textBlock = new TextBlock();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM text_block WHERE id_text_block = @id", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    textBlock.Id = (int)reader.GetValue(0);
                    textBlock.Name = (string)reader.GetValue(1);
                    textBlock.Text = (string)reader.GetValue(2);
                    textBlock.ParentCatalogId = (int)reader.GetValue(3);
                    textBlock.OrderInParentCatalog = (int)reader.GetValue(4);
                }
            }
            return textBlock;
        }

        /// <summary>
        /// Данный метод возвращает все текстовые файлы, находящиеся в указанной директории.
        /// Если таких файлов нет, то возвращается пустой список
        /// </summary>
        /// <param name="parentCatalogId">Id родительского каталога</param>
        /// <returns></returns>
        public List<TextBlock> GetTextBlocksByParentCatalogId(int parentCatalogId)
        {
            List<TextBlock> textBlocks = new List<TextBlock>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM text_block WHERE id_parent_catalog=@parentCatalogId", connection);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int).Value = parentCatalogId;
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBlock currTextBlock = new TextBlock
                    {
                        Id = (int)reader.GetValue(0),
                        Name = (string)reader.GetValue(1),
                        Text = (string)reader.GetValue(2),
                        ParentCatalogId = (int)reader.GetValue(3),
                        OrderInParentCatalog = (int)reader.GetValue(4)
                    };
                    textBlocks.Add(currTextBlock);
                }
            }
            return textBlocks.FindAll(item => item.ParentCatalogId == parentCatalogId);
        }
    }
}
