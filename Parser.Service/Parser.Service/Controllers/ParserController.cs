using System.Net;
using Parser.Common.ParserManager;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Parser.Service.Controllers
{
    [ApiController]
    [Route(DefaultRoute.Route)]
    public class ParserController : ControllerBase
    {
        private readonly IParserManager _parserManager;

        public ParserController(IParserManager parser)
        {
            _parserManager = parser;
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Start(int count)
        {
            await _parserManager.Start(count);
            return Ok(true);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ParseCarPage(string url, int previousId)
        {
            await _parserManager.HandleCarPage(url, previousId);
            return Ok(true);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ParseComplectationPage(string url, int previousId)
        {
            await _parserManager.HandleComplectationPage(url, previousId);
            return Ok(true);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ParseGroupPage(string url, int previousId)
        {
            await _parserManager.HandleGroupPage(url, previousId);
            return Ok(true);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ParseSubGroupPage(string url, int previousId, int complectationId)
        {
            await _parserManager.HandleSubGroupPage(url, previousId, complectationId);
            return Ok(true);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ParseDetailPage(string url, int previousId)
        {
            await _parserManager.HandleDetailPage(url, previousId);
            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> SavePageAsStaticHtml(string url)
        {
            await _parserManager.SavePageAsStaticHtml(url);
            return Ok();
        }
    }
}
