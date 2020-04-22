﻿using LetterBuilderWebAdmin.Models;
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
        ///  Метод добавляет в базу данных текстовый файл с указанными в entity значениями
        /// </summary>
        /// <param name="entity">Объект типа TextBlock</param>
        public void Add(TextBlock entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO text_block VALUES (@name, @text, @parentCatalogId, @order) SELECT SCOPE_IDENTITY()", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = entity.Name;
                command.Parameters.Add("@text", SqlDbType.NVarChar).Value = entity.Text;
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int).Value = entity.ParentCatalogId;
                command.Parameters.Add("@order", SqlDbType.Int).Value = entity.OrderInParentCatalog;
                entity.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Данный метод обновляет имя и содержание текстового файла в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса TextBlock, значения которого будут использоваться для обновления записи></param>
        public void UpdateNameAndText(TextBlock entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE text_block SET name=@name, text=@text WHERE id_text_block=@id", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = entity.Name;
                command.Parameters.Add("@text", SqlDbType.NVarChar).Value = entity.Text;
                command.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Данный метод обновляет поле OrderInParentCatalog данного элемента,
        /// но он не меняет порядок элемента, имеющего такой же порядковый номер
        /// и такой же родительский каталог, что и данный элемент.
        /// Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса TextBlock, значения которого будут использоваться для обновления записи</param>
        public void UpdateOrder(TextBlock entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE text_block SET order_in_parent_directory=@order WHERE id_text_block=@id", connection);
                command.Parameters.Add("@order", SqlDbType.Int).Value = entity.OrderInParentCatalog;
                command.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///  Данный метод удаляет текстовый файл из базы данных
        /// </summary>
        /// <param name="id">Id текстового каталога, которого требуется удалить</param>
        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM text_block WHERE id_text_block=@id", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.ExecuteNonQuery();
            }
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