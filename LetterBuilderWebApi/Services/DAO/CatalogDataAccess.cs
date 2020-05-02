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
    public class CatalogDataAccess : ICatalogDataAccess
    {
        private string _connectionString;

        public CatalogDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Данный метод возвращает все каталоги, находящиеся в указанной директории.
        /// Если подкаталогов нет, то возвращается пустой список
        /// </summary>
        /// <param name="parentCatalogId">Id родительского каталога</param>
        public List<Catalog> GetSubcatalogsByParentCatalogId(int parentCatalogId)
        {
            List<Catalog> subcatalogs = new List<Catalog>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM catalog WHERE id_parent_catalog=@parentCatalogId", connection))
                {
                    command.Parameters.Add("@parentCatalogId", SqlDbType.Int).Value = parentCatalogId;
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Catalog currCatalog = new Catalog
                        {
                            Id = (int)reader.GetValue(0),
                            Name = (string)reader.GetValue(1),
                            ParentCatalogId = (int)reader.GetValue(2),
                            OrderInParentCatalog = (int)reader.GetValue(3)
                        };
                        subcatalogs.Add(currCatalog);
                    }
                }
            }
            return subcatalogs;
        }

        /// <summary>
        /// Данный метод возвращает все каталоги, находящиеся в базе данных
        /// </summary>
        /// <returns>Объект типа List, содержащий все каталоги</returns>
        public List<Catalog> GetAll()
        {
            List<Catalog> repositoryContent = new List<Catalog>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM catalog", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    command.ExecuteNonQuery();
                    while (reader.Read())
                    {
                        Catalog currCatalog = new Catalog
                        {
                            Id = (int)reader.GetValue(0),
                            Name = (string)reader.GetValue(1),
                            ParentCatalogId = (int)reader.GetValue(2),
                            OrderInParentCatalog = (int)reader.GetValue(3)
                        };
                        repositoryContent.Add(currCatalog);
                    }
                }
            }
            return repositoryContent;
        }


        /// <summary>
        /// Данный метод возвращает запись из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса Catalog, содержащий значения для указанного
        /// каталога из базы данных. Если записи нет, то возвращается объект со значениями по-умолчанию</returns>
        public Catalog GetById(int id)
        {
            Catalog catalog = new Catalog();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM catalog WHERE id_catalog = @id", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    catalog.Id = (int)reader.GetValue(0);
                    catalog.Name = (string)reader.GetValue(1);
                    catalog.ParentCatalogId = (int)reader.GetValue(2);
                    catalog.OrderInParentCatalog = (int)reader.GetValue(3);
                }
            }
            return catalog;
        }
    }
}
