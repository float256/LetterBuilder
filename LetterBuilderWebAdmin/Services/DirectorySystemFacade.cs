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
        /// Метод добавляет в базу данных каталог с указанными в catalog значениями. Порядковый номер элемента вычисляется внутри метода
        /// </summary>
        /// <param name="catalog">Объект типа Catalog</param>
        public void Add(Catalog catalog)
        {
            // Элементы в базу данных добавляются в конец, т.е. их порядковый номер равен 
            // максимальному номеру из элементов, находящихся в том же каталоге + 1
            List<Catalog> neighboringCatalogs = GetSubcatalogs(catalog.ParentCatalogId);
            List<TextBlock> neighboringTextBlocks = GetCatalogAttachments(catalog.ParentCatalogId);
            int maxCatalogOrder = (neighboringCatalogs.Count > 0) ?
                neighboringCatalogs.Max(item => item.OrderInParentCatalog) : 0;
            int maxTextBlockOrder = (neighboringTextBlocks.Count > 0) ?
                neighboringTextBlocks.Max(item => item.OrderInParentCatalog) : 0;
            catalog.OrderInParentCatalog = Math.Max(maxCatalogOrder, maxTextBlockOrder) + 1;

            _catalogDataAccess.Add(catalog);
        }

        /// <summary>
        /// Метод добавляет в базу данных текстовый файл с указанными в textBlock значениями. Порядковый номер элемента вычисляется внутри метода
        /// </summary>
        /// <param name="textBlock">Объект типа TextBlock</param>
        public void Add(TextBlock textBlock)
        {
            // Элементы в базу данных добавляются в конец, т.е. их порядковый номер равен 
            // максимальному номеру из элементов, находящихся в том же каталоге + 1
            List<Catalog> neighboringCatalogs = GetSubcatalogs(textBlock.ParentCatalogId);
            List<TextBlock> neighboringTextBlocks = GetCatalogAttachments(textBlock.ParentCatalogId);
            int maxCatalogOrder = (neighboringCatalogs.Count > 0) ?
                neighboringCatalogs.Max(item => item.OrderInParentCatalog) : 0;
            int maxTextBlockOrder = (neighboringTextBlocks.Count > 0) ?
                neighboringTextBlocks.Max(item => item.OrderInParentCatalog) : 0;
            textBlock.OrderInParentCatalog = Math.Max(maxCatalogOrder, maxTextBlockOrder) + 1;

            _textDataAccess.Add(textBlock);
        }

        /// <summary>
        /// Данный метод обновляет имя и содержание текстового файла в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="textBlock">Объект класса TextBlock, значения которого будут использоваться для обновления записи</param>
        public void UpdateValue(TextBlock textBlock)
        {
            _textDataAccess.UpdateNameAndText(textBlock);
        }

        /// <summary>
        /// Данный метод обновляет родительский каталог текстового файла в базе данных. Id записи берется из поля
        /// Id передаваемого объекта. Также в объекте должен быть указан новый ParentCatalogId
        /// </summary>
        /// <param name="textBlock">Объект класса TextBlock, значения которого будут использоваться для обновления записи</param>
        public void UpdateParentCatalog(TextBlock textBlock)
        {
            int maxOrder = 0;
            foreach (TextBlock item in GetCatalogAttachments(textBlock.ParentCatalogId))
            {
                if (maxOrder < item.OrderInParentCatalog)
                {
                    maxOrder = item.OrderInParentCatalog;
                }
            }
            foreach (Catalog item in GetSubcatalogs(textBlock.ParentCatalogId))
            {
                if (maxOrder < item.OrderInParentCatalog)
                {
                    maxOrder = item.OrderInParentCatalog;
                }
            }
            textBlock.OrderInParentCatalog = maxOrder + 1;
            _textDataAccess.UpdateParentCatalog(textBlock);
            _textDataAccess.UpdateOrder(textBlock);
        }

        /// <summary>
        /// Данный метод обновляет имя каталога в базе данных. Id записи берется из поля Id передаваемого объекта
        /// </summary>
        /// <param name="entity">Объект класса Catalog, значения которого будут использоваться для обновления записи</param>
        public void UpdateValue(Catalog catalog)
        {
            _catalogDataAccess.UpdateName(catalog);
        }

        /// <summary>
        /// Данный метод обновляет родительский каталог каталога в базе данных. Id записи берется из поля
        /// Id передаваемого объекта. Также в объекте должен быть указан новый ParentCatalogId
        /// </summary>
        /// <param name="catalog">Объект класса Catalog, значения которого будут использоваться для обновления записи</param>
        public void UpdateParentCatalog(Catalog catalog)
        {
            int maxOrder = 0;
            foreach (TextBlock item in GetCatalogAttachments(catalog.ParentCatalogId))
            {
                if (maxOrder < item.OrderInParentCatalog)
                {
                    maxOrder = item.OrderInParentCatalog;
                }
            }
            foreach (Catalog item in GetSubcatalogs(catalog.ParentCatalogId))
            {
                if (maxOrder < item.OrderInParentCatalog)
                {
                    maxOrder = item.OrderInParentCatalog;
                }
            }
            catalog.OrderInParentCatalog = maxOrder + 1;
            _catalogDataAccess.UpdateParentCatalog(catalog);
            _catalogDataAccess.UpdateOrder(catalog);
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


        /// <summary>
        /// Данный метод удаляет текстовый файл из базы данных.
        /// значение OrderInParentCatalog
        /// </summary>
        /// <param name="id">Id значения, которое нужно удалить</param>
        public void DeleteTextBlock(int id)
        {
            _textDataAccess.Delete(id);
        }

        /// <summary>
        /// Данный метод удаляет каталог и все фийлы и подкаталоги, содержащиеся в нем из базы данных. 
        /// </summary>
        /// <param name="id">Id каталога, который нужно удалить</param>
        public void DeleteCatalog(int id)
        {
            // Удаление файлов в каталоге
            foreach (TextBlock item in _textDataAccess.GetTextBlocksByParentCatalogId(id))
            {
                DeleteTextBlock(item.Id);
            }

            // Удаление подкаталогов
            foreach (Catalog item in _catalogDataAccess.GetSubcatalogsByParentCatalogId(id))
            {
                DeleteCatalog(item.Id);
            }

            // Удаление каталога
            _catalogDataAccess.Delete(id);
        }

        /// <summary>
        /// Данный метод меняет порядок элемента. Если существует элемент, 
        /// имеющий такой же порядок и находящийся в том же каталоге, он меняется местами с входным элементом
        /// </summary>
        /// <param name="textBlock">Объект типа TextBlock, который должен иметь Id и Order элемента, у которого нужно сменить порядковый номер</param>
        /// <param name="orderAction">Действие, которое нужно совершить: перемещение вверх или перемещение вни</param>
        public void UpdateOrder(TextBlock textBlock, OrderAction orderAction)
        {

            int initalOrder = textBlock.OrderInParentCatalog;

            // Поиск каталога, находящегося в том же каталоге, что и 
            // textBlock и находящегося ближе всех к данному элементу
            Catalog nearestCatalog;
            List<Catalog> neighboringCatalogs = GetSubcatalogs(textBlock.ParentCatalogId);
            if (orderAction == OrderAction.MoveUp)
            {
                nearestCatalog = neighboringCatalogs.FindLast(item => item.OrderInParentCatalog < initalOrder);
            }
            else
            {
                nearestCatalog = neighboringCatalogs.Find(item => item.OrderInParentCatalog > initalOrder);
            }

            // Поиск текстового блока, находящегося в том же каталоге, что и 
            // textBlock и находящегося ближе всех к данному элементу
            TextBlock nearestTextBlock;
            List<TextBlock> neighboringTextBlocks = GetCatalogAttachments(textBlock.ParentCatalogId);
            if (orderAction == OrderAction.MoveUp)
            {
                nearestTextBlock = neighboringTextBlocks.FindLast(item => item.OrderInParentCatalog < initalOrder);
            }
            else
            {
                nearestTextBlock = neighboringTextBlocks.Find(item => item.OrderInParentCatalog > initalOrder);
            }

            // Замена порядкового номера у textBlock. Также заменяется порядковый номер ближайшего элемента
            int distanceToNearestCatalog = (nearestCatalog != null) ? Math.Abs(nearestCatalog.OrderInParentCatalog - initalOrder) : 0;
            int distanceToNearestTextBlock = (nearestTextBlock != null) ? Math.Abs(nearestTextBlock.OrderInParentCatalog - initalOrder) : 0;
            using (TransactionScope scope = new TransactionScope())
            {
                if (((nearestCatalog == null) && (nearestTextBlock != null)) ||
                    ((distanceToNearestCatalog >= distanceToNearestTextBlock) && nearestTextBlock != null))
                {
                    textBlock.OrderInParentCatalog = nearestTextBlock.OrderInParentCatalog;
                    nearestTextBlock.OrderInParentCatalog = initalOrder;
                    _textDataAccess.UpdateOrder(nearestTextBlock);
                }
                else if (((nearestCatalog != null) && (nearestTextBlock == null)) ||
                    ((distanceToNearestCatalog <= distanceToNearestTextBlock) && (nearestCatalog != null)))
                {
                    textBlock.OrderInParentCatalog = nearestCatalog.OrderInParentCatalog;
                    nearestCatalog.OrderInParentCatalog = initalOrder;
                    _catalogDataAccess.UpdateOrder(nearestCatalog);
                }
                _textDataAccess.UpdateOrder(textBlock);
                scope.Complete();
            }
        }

        /// <summary>
        /// Данный метод меняет порядок элемента. Если существует элемент, 
        /// имеющий такой же порядок и находящийся в том же каталоге, он меняется местами с входным элементом
        /// </summary>
        /// <param name="catalog">Объект типа Catalog, который должен иметь Id и Order элемента, у которого нужно сменить порядковый номер</param>
        /// <param name="order">Новый порядковый номер элемента</param>
        public void UpdateOrder(Catalog catalog, OrderAction orderAction)
        {
            int initalOrder = catalog.OrderInParentCatalog;

            // Поиск каталога, находящегося в том же каталоге, что и 
            // textBlock и находящегося ближе всех к данному элементу
            Catalog nearestCatalog;
            List<Catalog> neighboringCatalogs = GetSubcatalogs(catalog.ParentCatalogId);
            if (orderAction == OrderAction.MoveUp)
            {
                nearestCatalog = neighboringCatalogs.FindLast(item => item.OrderInParentCatalog < initalOrder);
            }
            else
            {
                nearestCatalog = neighboringCatalogs.Find(item => item.OrderInParentCatalog > initalOrder);
            }

            // Поиск текстового блока, находящегося в том же каталоге, что и 
            // textBlock и находящегося ближе всех к данному элементу
            TextBlock nearestTextBlock;
            List<TextBlock> neighboringTextBlocks = GetCatalogAttachments(catalog.ParentCatalogId);
            if (orderAction == OrderAction.MoveUp)
            {
                nearestTextBlock = neighboringTextBlocks.FindLast(item => item.OrderInParentCatalog < initalOrder);
            }
            else
            {
                nearestTextBlock = neighboringTextBlocks.Find(item => item.OrderInParentCatalog > initalOrder);
            }

            // Замена порядкового номера у textBlock. Также заменяется порядковый номер ближайшего элемента
            int distanceToNearestCatalog = (nearestCatalog != null) ? Math.Abs(nearestCatalog.OrderInParentCatalog - initalOrder) : 0;
            int distanceToNearestTextBlock = (nearestTextBlock != null) ? Math.Abs(nearestTextBlock.OrderInParentCatalog - initalOrder) : 0;
            using (TransactionScope scope = new TransactionScope())
            {
                if (((nearestCatalog == null) && (nearestTextBlock != null)) ||
                    ((distanceToNearestCatalog >= distanceToNearestTextBlock) && (nearestTextBlock != null)))
                {
                    catalog.OrderInParentCatalog = nearestTextBlock.OrderInParentCatalog;
                    nearestTextBlock.OrderInParentCatalog = initalOrder;
                    _textDataAccess.UpdateOrder(nearestTextBlock);
                }
                else if (((nearestCatalog != null) && (nearestTextBlock == null)) ||
                    (distanceToNearestCatalog <= distanceToNearestTextBlock) && (nearestCatalog != null))
                {
                    catalog.OrderInParentCatalog = nearestCatalog.OrderInParentCatalog;
                    nearestCatalog.OrderInParentCatalog = initalOrder;
                    _catalogDataAccess.UpdateOrder(nearestCatalog);
                }
                _catalogDataAccess.UpdateOrder(catalog);
                scope.Complete();
            }
        }
    }
}
