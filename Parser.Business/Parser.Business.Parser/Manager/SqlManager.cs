using System.Text;
using Parser.Common.BlobRepository;
using Parser.Common.Parser.ParsedPages;
using Parser.Common.SqlManager;
using Parser.Common.SqlManager.Models;
using Parser.Common.SqlRepository;

namespace Parser.Business.Parser.Manager
{
    public class SqlManager : ISqlManager, IGetSqlManager
    {
        private readonly ICarRepository _carRepository;
        private readonly IComplectationRepository _complectationRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISubGroupRepository _subGroupRepository;
        private readonly IDetailRepository _detailRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IImageRepository _imageRepository;
        public SqlManager(
            ICarRepository carRepository,
            IComplectationRepository complectationRepository,
            IGroupRepository groupRepository,
            ISubGroupRepository subGroupRepository,
            IDetailRepository detailRepository,
            IBlobRepository blobRepository,
            IBrandRepository brandRepository,
            IImageRepository imageRepository,
            ITableInfoRepository tableInfoRepository)
        {
            _carRepository = carRepository;
            _complectationRepository = complectationRepository;
            _groupRepository = groupRepository;
            _subGroupRepository = subGroupRepository;
            _detailRepository = detailRepository;
            _blobRepository = blobRepository;
            _brandRepository = brandRepository;
            _imageRepository = imageRepository;
        }

        /// <summary>
        /// Надсилає сутність сторінки з автівками до бази даних.
        /// </summary>
        /// <param name="cars">обєкт черги з сутностями з сторінки з автівками.</param>
        /// <param name="previousId">айді попередьньї сутності з брендом.</param>
        /// <returns>список айді доданих сутностей.</returns>
        public async Task<List<int>> SendCarPage(Queue<CarPage> cars, int previousId)
        {
            var listId = new List<int>();
            while (cars.Count > 0)
            {
                var car = cars.Dequeue();
                foreach (var assembly in car.Cars)
                {
                    var item = await _carRepository.SendCar(car.Name, assembly.Code, assembly.StartDate, assembly.EndDate, assembly.Assembly);
                    listId.Add(item.Id);
                }
            }

            return listId;
        }

        /// <summary>
        /// Надсилає сутність сторінки з комплектаціями до бази даних.
        /// </summary>
        /// <param name="complectations">обєкт черги з сутностями з сторінки з комплектаціями.</param>
        /// <param name="previousId">айді попереднього каталогу.</param>
        /// <returns>список айді доданих сутностей.</returns>
        public async Task<List<int>> SendComplectationPage(Queue<ComplectationPage> complectations, int previousId)
        {
            var listId = new List<int>();
            while (complectations.Count > 0)
            {
                var complectation = complectations.Dequeue();
                var complectationItem = await _complectationRepository.SendComplectation(complectation?.Complectation?.Complectation!, complectation?.Complectation?.StartDate!, complectation?.Complectation?.EndDate!, previousId);

                var attributesId = await SendAttributePair(complectation!);

                await SendAttributeComplectationPair(attributesId, complectationItem.Id);

                listId.Add(complectationItem.Id);
            }

            return listId;
        }

        /// <summary>
        /// Надсилає сутність сторінки з групами до бази даних.
        /// </summary>
        /// <param name="groups">обєкт черги з сутностями з сторінки з групами.</param>
        /// <returns>список айді створени записів.</returns>
        public async Task<List<int>> SendGroupPage(Queue<GroupPage> groups)
        {
            var listId = new List<int>();
            while (groups.Count > 0)
            {
                var group = groups.Dequeue();
                var item = await _groupRepository.SendGroup(group.Name);
                listId.Add(item.Id);
            }

            return listId;
        }

        /// <summary>
        /// Надсилає сутність сторінки з підгрупами до бази даних.
        /// </summary>
        /// <param name="subGroups">обєкт черги з сутностями з сторінки з підгрупами.</param>
        /// <param name="previousId">айді попередньої сутності.</param>
        /// <param name="complectationId">айді комплектації до якої відноситься підгрупа.</param>
        /// <returns>список айді створени записів.</returns>
        public async Task<List<int>> SendSubGroupPage(Queue<SubGroupPage> subGroups, int previousId, int complectationId)
        {
            var listId = new List<int>();
            while (subGroups.Count > 0)
            {
                var subgroup = subGroups.Dequeue();
                var imageId = await _imageRepository.SendImage(await _blobRepository.AddBlob(subgroup.ImageUrl));
                var item = await _subGroupRepository.SendSubGroup(subgroup.Name, previousId, complectationId, imageId);
                listId.Add(item.Id);
            }

            return listId;
        }

        /// <summary>
        /// Надсилає сутність сторінки з деталями до бази даних.
        /// </summary>
        /// <param name="details">обєкт черги з сутностями з сторінки з деталями.</param>
        /// <param name="previousId">айді попередньої сутності.</param>
        /// <returns>Task.</returns>
        public async Task SendDetailPage(Queue<DetailPage> details, int previousId)
        {
            while (details.Count > 0)
            {
                var detail = details.Dequeue();

                await SendDetailInfo(detail, previousId);
            }
        }

        /// <summary>
        /// Отримати усі бренди.
        /// </summary>
        /// <returns>список брендів.</returns>
        public async Task<List<Brand>> GetBrands()
        {
            var result = await _brandRepository.GetBrands();
            return result.ToList();
        }

        /// <summary>
        /// Отримати усі моделі стосовно бренда.
        /// </summary>
        /// <returns>список моделей.</returns>
        public async Task<List<Model>> GetModels(int id)
        {
            var result = await _carRepository.GetModels(id);
            return result.ToList();
        }

        /// <summary>
        /// Отримати усі комплектації стосовно моделі.
        /// </summary>
        /// <returns>список комплектацій.</returns>
        public async Task<List<Complectation>> GetComplectations(int id)
        {
            var items = await _complectationRepository.GetComplectations(id);

            return items.ToList();
        }

        /// <summary>
        /// Отримати усі групи стосовно комплектації.
        /// </summary>
        /// <returns>список груп.</returns>
        public async Task<List<Group>> GetGroups(int id)
        {
            var result = await _groupRepository.GetGroups(id);
            return result.ToList();
        }

        /// <summary>
        /// Отримати усі підгрупи стосовно групи.
        /// </summary>
        /// <returns>список підгруп.</returns>
        public async Task<List<SubGroup>> GetSubGroups(int complectationId, int groupId)
        {
            var result = await _subGroupRepository.GetSubGroups(complectationId, groupId);
            return result.ToList();
        }

        /// <summary>
        /// Отримати усі схеми деталей стосовно підгрупи.
        /// </summary>
        /// <returns>список схем деталей.</returns>
        public async Task<Schema> GetSpares(int complectationId, int groupId, int subGroupId)
        {
            return await _detailRepository.GetSchema(complectationId, groupId, subGroupId);
        }

        /// <summary>
        /// Отримати усі повязані дані до комплектаціх.
        /// </summary>
        /// <param name="id">айді комплектації.</param>
        /// <returns>список повязаних даних.</returns>
        public async Task<List<Global>> GetFullDependDataForComplectation(int id)
        {
            var items = await _complectationRepository.GetFullInfoForComplectation(id);

            return items.ToList();
        }

        private async Task SendDetailInfo(DetailPage detail, int previousId)
        {
            foreach (var detailInfo in detail.Details)
            {
                var detailIdObject = await _detailRepository.SendDetail(detail.DetailCode, detail.DetailName, previousId);

                var infoIdObject = await _detailRepository.SendDetailInfo(detailInfo.Count, detailInfo.StartDate, detailInfo.EndDate, detailInfo.Usings);

                await SendDetailCode(detailInfo.Codes, detailIdObject.Id, infoIdObject.Id);
            }
        }

        private async Task SendDetailCode(List<string> codes, int detailId, int infoId)
        {
            foreach (var code in codes)
            {
                await _detailRepository.SendDetailCode(code, detailId, infoId);
            }
        }

        private async Task<List<int>> SendAttributePair(ComplectationPage complectation)
        {
            var listId = new List<int>();
            foreach (var attribute in complectation?.Attributes!)
            {
                var item = await _complectationRepository.SendAttributes(attribute.Key, attribute.Value);
                listId.Add(item.Id);
            }

            return listId;
        }

        private async Task SendAttributeComplectationPair(List<int> attributesId, int complectationId)
        {
            foreach (var id in attributesId)
            {
                await _complectationRepository.SendPairAttribute_Complectation(
                    id,
                    complectationId);
            }
        }
    }
}
