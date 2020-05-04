using LetterBuilderWebAdmin.Models;
using LetterBuilderWebAdmin.Services.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace LetterBuilderWebAdmin.Services
{
    public class DirectorySystemFacade : IDirectorySystemFacade
    {
        private ITextBlockDataAccess _textDataAccess;
        private ICatalogDataAccess _catalogDataAccess;

        public DirectorySystemFacade(ITextBlockDataAccess textDataAccess, ICatalogDataAccess catalogDataAccess)
        {
            _textDataAccess = textDataAccess;
            _catalogDataAccess = catalogDataAccess;
        }

        /// <summary>
        /// Данный метод возвращает все текстовые файлы, находящиеся в базе данных
        /// </summary>
        /// <returns>Объект типа List, содержащий все текстовые файлы</returns>
        public List<TextBlock> GetAllTextBlocks()
        {
            return _textDataAccess.GetAll();
        }

        /// <summary>
        /// Данный метод возвращает все каталоги, находящиеся в базе данных
        /// </summary>
        /// <returns>Объект типа List, содержащий все каталоги</returns>
        public List<Catalog> GetAllCatalogs()
        {
            return _catalogDataAccess.GetAll();
        }

        /// <summary>
        /// Данный метод возвращает запись текстового файла из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса TextBlock, содержащий значения для указанного
        /// текстового файла из базы данных. Если записи нет, то возвращается объект со значениями по-умолчанию</returns>
        public TextBlock GetTextBlockById(int id)
        {
            return _textDataAccess.GetById(id);
        }

        /// <summary>
        /// Данный метод возвращает запись каталога из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса Catalog, содержащий значения для указанного
        /// каталога из базы данных. Если записи нет, то возвращается объект со значениями по-умолчанию</returns>
        public Catalog GetCatalogById(int id)
        {
            return _catalogDataAccess.GetById(id);
        }

        /// <summary>
        /// Данный метод получает подкаталоги для входного каталога
        /// </summary>
        /// <param name="id">Id каталога, в котором будет вестись поиск подкаталогов</param>
        /// <returns>Список подкаталогов, состоящий из объектов типа Catalog</returns>
        public List<Catalog> GetSubcatalogs(int id)
        {
            return _catalogDataAccess.GetSubcatalogsByParentCatalogId(id).OrderBy(item => item.OrderInParentCatalog).ToList();
        }

        /// <summary>
        /// Данный метод получает файлы, находящиеся в каталоге
        /// </summary>
        /// <param name="id">>Id каталога, в котором будет вестись поиск файлов</param>
        /// <returns>Список текстовых файлов, состоящий из объектов типа TextBlock</returns>
        public List<TextBlock> GetCatalogAttachments(int id)
        {
            return _textDataAccess.GetTextBlocksByParentCatalogId(id).OrderBy(item => item.OrderInParentCatalog).ToList();
        }
    }
}
