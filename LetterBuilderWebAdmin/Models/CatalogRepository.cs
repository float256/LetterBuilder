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

        public CatalogRepository(string connectionString)
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
                SqlCommand command = new SqlCommand("INSERT INTO catalog VALUES (@name, @parentCatalogId)", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters["@name"].Value = entity.Name;
                command.Parameters["@parentCatalogId"].Value = entity.ParentCatalogId;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Данный метод обновляет имя каталога в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса Catalog, значения которого будут использоваться для обновления записи</param>
        public void Update(Catalog entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE catalog SET name=@name WHERE id_catalog=@id", connection);
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@name"].Value = entity.Name;
                command.Parameters["@id"].Value = entity.Id;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Данный метод удаляет каталог из базы данных и все вложенные в него каталоги и текстовые файлы
        /// </summary>
        /// <param name="id">Id каталога, которого требуется удалить</param>
        public void Delete(int id)
        {
            // Удаление вложенных файлов
            TextBlockRepository textBlockRepository = new TextBlockRepository(_connectionString);
            foreach (TextBlock item in textBlockRepository.GetTextBlocksByParentCatalogId(id))
            {
                textBlockRepository.Delete(item.Id);
            }

            // Удаление вложенных каталогов
            foreach (Catalog item in GetSubcatalogsByParentCatalogId(id))
            {
                Delete(item.Id);
            }

            // Удаление текущего каталога
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
                SqlCommand command = new SqlCommand("SELECT * FROM catalog WHERE id_parent_catalog=@parentCatalogId", connection);
                command.Parameters.Add("@parentCatalogId", SqlDbType.Int);
                command.Parameters["@parentCatalogId"].Value = parentCatalogId;
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
                    subcatalogs.Add(currCatalog);
                }
            }
            return subcatalogs;
        }

        /// <summary>
        /// Данный метод возвращает все каталоги, находящиеся в базе данных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Catalog> GetAll()
        {
            List<Catalog> repositoryContent = new List<Catalog>();
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
                    repositoryContent.Add(currCatalog);
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
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    catalog.Id = (int)reader.GetValue(0);
                    catalog.Name = (string)reader.GetValue(1);
                    catalog.ParentCatalogId = (int)reader.GetValue(2);
                }
            }
            return catalog;
        }
    }
}
