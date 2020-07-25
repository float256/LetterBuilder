using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public class PictureDataAccess: IPictureDataAccess
    {
        private string _connectionString;

        public PictureDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Метод добавляет в базу данных изображение с указанными в entity значениями. 
        /// В поле Picture.Id записывается id созданной строки в БД
        /// </summary>
        /// <param name="entity">Объект типа Picture</param>
        public void Add(Picture entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO picture VALUES (@binaryData) SELECT SCOPE_IDENTITY()", connection))
                {
                    command.Parameters.Add("@binaryData", SqlDbType.VarBinary, -1).Value = entity.BinaryData;
                    entity.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Данный метод возвращает запись из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса Picture, содержащий значения для указанного изображения из базы данных.</returns>
        public Picture GetById(int id)
        {
            Picture picture = new Picture { Id = -1 };
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM picture WHERE id_picture = @id", connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        picture.Id = Convert.ToInt32(reader["id_picture"]);
                        picture.BinaryData = (byte[])(reader["binary_data"]);
                    }
                }
            }
            if (picture.Id == -1)
            {
                return null;
            }
            else
            {
                return picture;
            }
        }

        /// <summary>
        /// Данный метод удаляет изображение из базы данных
        /// </summary>
        /// <param name="id">Id изображения, которое требуется удалить</param>
        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM picture WHERE id_catalog=@id", connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
