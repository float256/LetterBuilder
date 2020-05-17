using LetterBuilderCore.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderCore.Services.DAO
{
    public class CatalogDataAccess : ICatalogDataAccess
    {
        private string _connectionString;

        public CatalogDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Метод добавляет в базу данных каталог с указанными в entity значениями
        /// </summary>
        /// <param name="entity">Объект типа Catalog</param>
        public void Add(Catalog entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO catalog VALUES (@name, @parentCatalogId, @order) SELECT SCOPE_IDENTITY()", connection))
                {
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = entity.Name;
                    command.Parameters.Add("@parentCatalogId", SqlDbType.Int).Value = entity.ParentCatalogId;
                    command.Parameters.Add("@order", SqlDbType.Int).Value = entity.OrderInParentCatalog;
                    entity.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Данный метод обновляет имя каталога в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса Catalog, значения которого будут использоваться для обновления записи</param>
        public void UpdateName(Catalog entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE catalog SET name=@name WHERE id_catalog=@id", connection))
                {
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = entity.Name;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Данный метод обновляет поле OrderInParentCatalog данного элемента,
        /// но он не меняет порядок элемента, имеющего такой же порядковый номер
        /// и такой же родительский каталог, что и данный элемент.
        /// Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса TextBlock, значения которого будут использоваться для обновления записи</param>
        public void UpdateOrder(Catalog entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE catalog SET order_in_parent_directory=@order WHERE id_catalog=@id", connection))
                {
                    command.Parameters.Add("@order", SqlDbType.Int).Value = entity.OrderInParentCatalog;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateParentCatalog(Catalog entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE catalog SET id_parent_catalog=@parentCatalogId WHERE id_catalog=@id", connection))
                {
                    command.Parameters.Add("@parentCatalogId", SqlDbType.Int).Value = entity.ParentCatalogId;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Данный метод удаляет каталог из базы данных, но он не удаляет записи о вложенных файлах и папках
        /// </summary>
        /// <param name="id">Id каталога, которого требуется удалить</param>
        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM catalog WHERE id_catalog=@id", connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
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
                            Id = Convert.ToInt32(reader["id_catalog"]),
                            Name = Convert.ToString(reader["name"]),
                            ParentCatalogId = Convert.ToInt32(reader["id_parent_catalog"]),
                            OrderInParentCatalog = Convert.ToInt32(reader["order_in_parent_directory"])
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
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Catalog currCatalog = new Catalog
                        {
                            Id = Convert.ToInt32(reader["id_catalog"]),
                            Name = Convert.ToString(reader["name"]),
                            ParentCatalogId = Convert.ToInt32(reader["id_parent_catalog"]),
                            OrderInParentCatalog = Convert.ToInt32(reader["order_in_parent_directory"])
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
        /// каталога из базы данных. Если id == 0, то возвращается объект с значениями по-умолчанию.
        /// Если записи нет, то возвращается null</returns>
        public Catalog GetById(int id)
        {
            if (id == Constants.RootCatalogId)
            {
                return new Catalog { Id = 0 };
            }
            Catalog catalog = default;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM catalog WHERE id_catalog = @id", connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        catalog = new Catalog
                        {
                            Id = Convert.ToInt32(reader["id_catalog"]),
                            Name = Convert.ToString(reader["name"]),
                            ParentCatalogId = Convert.ToInt32(reader["id_parent_catalog"]),
                            OrderInParentCatalog = Convert.ToInt32(reader["order_in_parent_directory"])
                        };
                    }
                }
            }
            return catalog;
        }
    }
}
