using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class CatalogRepository : IRepository<Catalog>
    {
        private string _connectionString;
        public int ParentCatalogId { get; set; }
        public List<Catalog> RepositoryContent { get; } = new List<Catalog>();

        public CatalogRepository(string connectionString)
        {
            _connectionString = connectionString;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM catalog", connection);
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Catalog currCatalog = new Catalog
                    {
                        Id = (int)reader.GetValue(0),
                        Name = (string)reader.GetValue(1),
                        ParentCatalogId = (int)reader.GetValue(2)
                    };
                    RepositoryContent.Add(currCatalog);
                }
            }
        }

        /// <summary>
        /// Метод добавляет в базу данных и в RepositoryContent каталог с указанными в entity значениями
        /// </summary>
        /// <param name="entity">Объект типа Catalog</param>
        public void Add(Catalog entity)
        {
            RepositoryContent.Add(entity);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO catalog VALUES (@name, @parentCatalogId)", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters["@name"].Value = entity.Name;
                command.Parameters["@parentCatalogId"].Value = entity.ParentCatalogId;
                RepositoryContent[RepositoryContent.Count - 1].Id = (int)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Данный метод обновляет значение в базе данных и в RepositoryContent. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса Catalog, значения которого будут использоваться для обновления записи</param>
        public void Update(Catalog entity)
        {
            Catalog catalogFromRepository = RepositoryContent.Find(item => item.Id == entity.Id);
            catalogFromRepository.Name = entity.Name;
            catalogFromRepository.ParentCatalogId = entity.ParentCatalogId;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE catalog SET name=@name, id_parent_catalog=@parentCatalogId WHERE id_catalog=@id", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@name"].Value = entity.Name;
                command.Parameters["@parentCatalogId"].Value = entity.ParentCatalogId;
                command.Parameters["@id"].Value = entity.Id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Данный метод удаляет значение из базы данных и из RepositoryContent
        /// </summary>
        /// <param name="id">Id значения, которое требуется удалить</param>
        public void Delete(int id)
        {
            RepositoryContent.Remove(RepositoryContent.Find(item => item.Id == id));
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
        /// Данный метод возвращает все каталоги, находящиеся в директории
        /// </summary>
        /// <param name="id">Id каталога</param>
        public List<Catalog> GetCatalogContent(int id)
        {
            return RepositoryContent.FindAll(item => item.ParentCatalogId == id);
        }
    }
}
