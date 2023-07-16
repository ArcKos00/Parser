using System.Net;
using Parser.Common.Exceptions;
using Parser.Common.SqlManager;
using Parser.Common.SqlManager.Models;
using Parser.Service.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Parser.Service.Controllers
{
    [ApiController]
    [Route(DefaultRoute.Route)]
    public class ComplectationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetSqlManager _getSqlManager;

        public ComplectationController(IGetSqlManager getSqlManager, IMapper mapper)
        {
            _mapper = mapper;
            _getSqlManager = getSqlManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ComplectationDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComplectations(int modelId)
        {
            if (modelId == default)
            {
                throw new ValidException("One or more parameters are not valid");
            }

            return Ok(_mapper.Map<List<ComplectationDto>>(await _getSqlManager.GetComplectations(modelId)));
        }

        /// <summary>
        /// Зробив ліміт, бо свагер невивозить.
        /// </summary>
        /// <param name="complectationId">айді комплектації.</param>
        /// <param name="pageIndex">індекс сторінки.</param>
        /// <param name="pageSize">розмір сторінки.</param>
        /// <returns>відображення.</returns>
        /// <exception cref="ValidException">робить валідацію вхідних даних.</exception>
        [HttpGet]
        [ProducesResponseType(typeof(List<GlobalDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComlectationGlobalView(int complectationId, int pageIndex = 0, int pageSize = 100)
        {
            if (complectationId == default)
            {
                throw new ValidException("One or more parameters are not valid");
            }

            var result = _mapper.Map<List<GlobalDto>>(await _getSqlManager.GetFullDependDataForComplectation(complectationId));
            return Ok(result.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }
    }
}
