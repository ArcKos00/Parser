namespace Parser.Common.ParserManager
{
    public interface IParserManager
    {
        /// <summary>
        /// початок парсингу починаючи з першої сторінки за посиланням на першу сторінку.
        /// </summary>
        /// <returns>await.</returns>
        public Task Start(int count);

        /// <summary>
        /// Обробляє спаршені дані з першої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public Task HandleCarPage(string url, int previousId);

        /// <summary>
        /// Обробляє спаршені дані з другої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public Task HandleComplectationPage(string url, int previousId);

        /// <summary>
        /// Обробляє спаршені дані з третьої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public Task HandleGroupPage(string url, int previousId);

        /// <summary>
        /// Обробляє спаршені дані з четвертої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public Task HandleSubGroupPage(string url, int previousId, int complectationId);

        /// <summary>
        /// Обробляє спаршені дані з пятої сторінки з машинами.
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>await.</returns>
        public Task HandleDetailPage(string url, int previousId);

        /// <summary>
        /// метод для збереження статичного html коду без javascript у поточну директорію (для тесту).
        /// </summary>
        /// <param name="url">посилання на сторінку.</param>
        /// <returns>Task.</returns>
        public Task SavePageAsStaticHtml(string url);
    }
}
