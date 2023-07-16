using Parser.Common.Parser.ParsedPages;

namespace Parser.Common.SqlManager
{
    public interface ISqlManager
    {
        /// <summary>
        /// Надсилає сутність сторінки з автівками до бази даних.
        /// </summary>
        /// <param name="cars">обєкт черги з сутностями з сторінки з автівками.</param>
        /// <param name="previousId">айді попередьньї сутності з брендом.</param>
        /// <returns>список айді доданих сутностей.</returns>
        public Task<List<int>> SendCarPage(Queue<CarPage> cars, int previousId);

        /// <summary>
        /// Надсилає сутність сторінки з комплектаціями до бази даних.
        /// </summary>
        /// <param name="complectations">обєкт черги з сутностями з сторінки з комплектаціями.</param>
        /// <param name="previousId">айді попереднього каталогу.</param>
        /// <returns>список айді доданих сутностей.</returns>
        public Task<List<int>> SendComplectationPage(Queue<ComplectationPage> complectations, int previousId);

        /// <summary>
        /// Надсилає сутність сторінки з групами до бази даних.
        /// </summary>
        /// <param name="groups">обєкт черги з сутностями з сторінки з групами.</param>
        /// <returns>список айді створени записів.</returns>
        public Task<List<int>> SendGroupPage(Queue<GroupPage> groups);

        /// <summary>
        /// Надсилає сутність сторінки з підгрупами до бази даних.
        /// </summary>
        /// <param name="subGroups">обєкт черги з сутностями з сторінки з підгрупами.</param>
        /// <param name="previousId">айді попередньої сутності.</param>
        /// <param name="complectationId">айді комплектації до якої відноситься підгрупа.</param>
        /// <returns>список айді створени записів.</returns>
        public Task<List<int>> SendSubGroupPage(Queue<SubGroupPage> subGroups, int previousId, int complectationId);

        /// <summary>
        /// Надсилає сутність сторінки з деталями до бази даних.
        /// </summary>
        /// <param name="details">обєкт черги з сутностями з сторінки з деталями.</param>
        /// <param name="previousId">айді попередньої сутності.</param>
        /// <returns>Task.</returns>
        public Task SendDetailPage(Queue<DetailPage> details, int previousId);
    }
}