using LetterBuilderWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Services
{
    public class FileSystemRepository : IFileSystemRepository
    {
        private ITextBlockRepository _textRepository;
        private ICatalogRepository _catalogRepository;

        public FileSystemRepository(ITextBlockRepository textRepository, ICatalogRepository catalogRepository)
        {
            _textRepository = textRepository;
            _catalogRepository = catalogRepository;
        }

        /// <summary>
        /// Данный метод возвращает все текстовые файлы, находящиеся в базе данных
        /// </summary>
        /// <returns>Объект типа List, содержащий все текстовые файлы</returns>
        public List<TextBlock> GetAllTextBlocks() => _textRepository.GetAll();

        /// <summary>
        /// Данный метод возвращает все каталоги, находящиеся в базе данных
        /// </summary>
        /// <returns>Объект типа List, содержащий все каталоги</returns>
        public List<Catalog> GetAllCatalogs() => _catalogRepository.GetAll();

        /// <summary>
        /// Данный метод возвращает запись текстового файла из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса TextBlock, содержащий значения для указанного
        /// текстового файла из базы данных. Если записи нет, то возвращается объект со значениями по-умолчанию</returns>
        public TextBlock GetTextBlockById(int id) => _textRepository.GetById(id);

        /// <summary>
        /// Данный метод возвращает запись каталога из базы данных по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Объект класса Catalog, содержащий значения для указанного
        /// каталога из базы данных. Если записи нет, то возвращается объект со значениями по-умолчанию</returns>
        public Catalog GetCatalogById(int id) => _catalogRepository.GetById(id);

        /// <summary>
        /// Метод добавляет в базу данных каталог с указанными в catalog значениями. Порядковый номер элемента вычисляется внутри метода
        /// </summary>
        /// <param name="catalog">Объект типа Catalog</param>
        public void Add(Catalog catalog)
        {
            // Элементы в базу данных добавляются в конец, т.е. их порядковый номер равен 
            // количеству всех элементов находящихся в том же каталоге, что и заданный + 1
            catalog.OrderInParentCatalog = GetSubcatalogs(catalog.ParentCatalogId).Count + GetCatalogAttachments(catalog.ParentCatalogId).Count + 1;

            _catalogRepository.Add(catalog);
        }

        /// <summary>
        /// Метод добавляет в базу данных текстовый файл с указанными в textBlock значениями. Порядковый номер элемента вычисляется внутри метода
        /// </summary>
        /// <param name="textBlock">Объект типа TextBlock</param>
        public void Add(TextBlock textBlock)
        {
            // Элементы в базу данных добавляются в конец, т.е. их порядковый номер равен 
            // количеству всех элементов находящихся в том же каталоге, что и заданный + 1
            textBlock.OrderInParentCatalog = GetSubcatalogs(textBlock.ParentCatalogId).Count + GetCatalogAttachments(textBlock.ParentCatalogId).Count + 1;

            _textRepository.Add(textBlock);
        }

        /// <summary>
        /// Данный метод обновляет имя и содержание текстового файла в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="textBlock">Объект класса TextBlock, значения которого будут использоваться для обновления записи></param>
        public void UpdateValue(TextBlock textBlock) => _textRepository.UpdateNameAndText(textBlock);

        /// <summary>
        /// Данный метод обновляет имя каталога в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса Catalog, значения которого будут использоваться для обновления записи</param>
        public void UpdateValue(Catalog catalog) => _catalogRepository.UpdateName(catalog);

        /// <summary>
        /// Данный метод получает подкаталоги для входного каталога
        /// </summary>
        /// <param name="id">Id каталога, в котором будет вестись поиск подкаталогов</param>
        /// <returns>Список подкаталогов, состоящий из объектов типа Catalog</returns>
        public List<Catalog> GetSubcatalogs(int id) => _catalogRepository.GetSubcatalogsByParentCatalogId(id);

        /// <summary>
        /// Данный метод получает файлы, находящиеся в каталоге
        /// </summary>
        /// <param name="id">>Id каталога, в котором будет вестись поиск файлов</param>
        /// <returns>Список текстовых файлов, состоящий из объектов типа TextBlock</returns>
        public List<TextBlock> GetCatalogAttachments(int id) => _textRepository.GetTextBlocksByParentCatalogId(id);


        /// <summary>
        /// Данный метод удаляет текстовый файл из базы данных. Также,в случае, если
        /// isRestoreItemOrder==true все элементы, находящиеся ниже по порядку декрементируют 
        /// значение OrderInParentCatalog
        /// </summary>
        /// <param name="id">Id значения, которое нужно удалить</param>
        /// <param name="isRestoreItemOrder">Если true, то кроме удаления тааже будет восстановлен правильный 
        /// порядок элементов, находящихся в том же каталоге, что и данный каталог</param>
        public void DeleteTextBlock(int id, bool isRestoreItemOrder=true)
        {
            // Уменьшение OrderInParentCatalog у соседних элементов
            if (isRestoreItemOrder)
            {
                TextBlock textBlock = GetTextBlockById(id);
                foreach (Catalog item in GetSubcatalogs(textBlock.ParentCatalogId))
                {
                    if (item.OrderInParentCatalog > textBlock.OrderInParentCatalog)
                    {
                        item.OrderInParentCatalog--;
                        _catalogRepository.UpdateOrder(item);
                    }
                }
                foreach (TextBlock item in GetCatalogAttachments(textBlock.ParentCatalogId))
                {
                    if (item.OrderInParentCatalog > textBlock.OrderInParentCatalog)
                    {
                        item.OrderInParentCatalog--;
                        _textRepository.UpdateOrder(item);
                    }
                }
            }
            _textRepository.Delete(id);
        }

        /// <summary>
        /// Данный метод удаляет каталог и все фийлы и подкаталоги, содержащиеся в нем из базы данных. Также,в случае, если
        /// isRestoreItemOrder==true все элементы, находящиеся ниже по порядку декрементируют значение OrderInParentCatalog
        /// </summary>
        /// <param name="id">Id каталога, который нужно удалить</param>
        /// <param name="isRestoreItemOrder">Если true, то кроме удаления тааже будет восстановлен правильный 
        /// порядок элементов, находящихся в том же каталоге, что и данный каталог</param>
        public void DeleteCatalog(int id, bool isRestoreItemOrder=true)
        {
            // Удаление файлов в каталоге
            foreach (TextBlock item in _textRepository.GetTextBlocksByParentCatalogId(id))
            {
                DeleteTextBlock(item.Id, false);
            }

            // Удаление подкаталогов
            foreach (Catalog item in _catalogRepository.GetSubcatalogsByParentCatalogId(id))
            {
                DeleteCatalog(item.Id, false);
            }

            // Уменьшение OrderInParentCatalog у соседних элементов
            if (isRestoreItemOrder)
            {
                Catalog catalog = GetCatalogById(id);
                foreach (Catalog item in GetSubcatalogs(catalog.ParentCatalogId))
                {
                    if (item.OrderInParentCatalog > catalog.OrderInParentCatalog)
                    {
                        item.OrderInParentCatalog--;
                        _catalogRepository.UpdateOrder(item);
                    }
                }
                foreach (TextBlock item in GetCatalogAttachments(catalog.ParentCatalogId))
                {
                    if (item.OrderInParentCatalog > catalog.OrderInParentCatalog)
                    {
                        item.OrderInParentCatalog--;
                        _textRepository.UpdateOrder(item);
                    }
                }
            }
            
            // Удаление каталога
            _catalogRepository.Delete(id);
        }

        /// <summary>
        /// Данный метод меняет порядок элемента. Если существует элемент, 
        /// имеющий такой же порядок и находящийся в том же каталоге, он меняется местами с входным элементом
        /// </summary>
        /// <param name="textBlock">Объект типа TextBlock, который должен иметь Id и Order элемента, у которого нужно сменить порядковый номер</param>
        /// <param name="order">Новый порядковый номер элемента</param>
        public void UpdateOrder(TextBlock textBlock, int order)
        {
            int initalOrder = textBlock.OrderInParentCatalog;

            // Поиск каталога, находящегося в том же каталоге, что и textBlock и имеющего
            // порядковый номер, указанный в order, и замена его порядкового номера
            Catalog catalogWithSameOrder = _catalogRepository.GetSubcatalogsByParentCatalogId(textBlock.ParentCatalogId).Find(
                item => item.OrderInParentCatalog == order);
            if (catalogWithSameOrder != null )
            {
                catalogWithSameOrder.OrderInParentCatalog = initalOrder;
                _catalogRepository.UpdateOrder(catalogWithSameOrder);
            }

            // Поиск текстового блока, находящегося в том же каталоге, что и textBlock и имеющего
            // порядковый номер, указанный в order, и замена его порядкового номера
            TextBlock textBlockWithSameOrder = _textRepository.GetTextBlocksByParentCatalogId(textBlock.ParentCatalogId).Find(
                item => item.OrderInParentCatalog == order);
            if (textBlockWithSameOrder != null)
            {
                textBlockWithSameOrder.OrderInParentCatalog = initalOrder;
                _catalogRepository.UpdateOrder(catalogWithSameOrder);
            }

            // Замена порядкового номера у textBlock
            textBlock.OrderInParentCatalog = order;
            _textRepository.UpdateOrder(textBlock);
        }

        /// <summary>
        /// Данный метод меняет порядок элемента. Если существует элемент, 
        /// имеющий такой же порядок и находящийся в том же каталоге, он меняется местами с входным элементом
        /// </summary>
        /// <param name="catalog">Объект типа Catalog, который должен иметь Id и Order элемента, у которого нужно сменить порядковый номер</param>
        /// <param name="order">Новый порядковый номер элемента</param>
        public void UpdateOrder(Catalog catalog, int order)
        {
            int initalOrder = catalog.OrderInParentCatalog;

            // Поиск каталога, находящегося в том же каталоге, что и textBlock и имеющего
            // порядковый номер, указанный в order, и замена его порядкового номера
            Catalog catalogWithSameOrder = _catalogRepository.GetSubcatalogsByParentCatalogId(catalog.ParentCatalogId).Find(
                item => item.OrderInParentCatalog == order);
            if (catalogWithSameOrder != null)
            {
                catalogWithSameOrder.OrderInParentCatalog = initalOrder;
                _catalogRepository.UpdateOrder(catalogWithSameOrder);
            }

            // Поиск текстового блока, находящегося в том же каталоге, что и textBlock и имеющего
            // порядковый номер, указанный в order, и замена его порядкового номера
            TextBlock textBlockWithSameOrder = _textRepository.GetTextBlocksByParentCatalogId(catalog.ParentCatalogId).Find(
                item => item.OrderInParentCatalog == order);
            if (textBlockWithSameOrder != null)
            {
                textBlockWithSameOrder.OrderInParentCatalog = initalOrder;
                _textRepository.UpdateOrder(textBlockWithSameOrder);
            }

            // Замена порядкового номера у textBlock
            catalog.OrderInParentCatalog = order;
            _catalogRepository.UpdateOrder(catalog);
        }
    }
}
