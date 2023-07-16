using Parser.Common.Parser.ParsedPages;

namespace Parser.Common.Repos.Parser
{
    public interface IParser
    {
        public IAsyncEnumerable<CarPage> ParseCarPage(string url);

        /// <summary>
        /// Розбирає усі дані на другій сторінці сайту.
        /// </summary>
        /// <param name="url">посилання на другу сторінку.</param>
        /// <returns>Таск.</returns>
        public IAsyncEnumerable<ComplectationPage> ParseComplectationPage(string url);

        /// <summary>
        /// Парсинг третьої сторінки з групами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>список даних з третьої сторінки.</returns>
        public IAsyncEnumerable<GroupPage> ParseSpareGroupPage(string url);

        /// <summary>
        /// Парсить четверту сторінку сайту підгрупи.
        /// </summary>
        /// <param name="url">посилання на третю сторінку.</param>
        /// <returns>Список даних з четвертої сторінки.</returns>
        public IAsyncEnumerable<SubGroupPage> ParseSpareSubGroupPage(string url);

        /// <summary>
        /// Парсить пяту сторінку з деталями.
        /// </summary>
        /// <param name="url">посилання на пяту сторінку.</param>
        /// <returns>повертає дані з пятої сторінки.</returns>
        public IAsyncEnumerable<DetailPage> ParseDetailPage(string url);

        /// <summary>
        /// Збарігає статичний html текст.
        /// </summary>
        /// <param name="url">url адреса з якої скачати html.</param>
        /// <returns>Таск.</returns>
        public Task TestSaveSaticHtmlPage(string url);
    }
}
