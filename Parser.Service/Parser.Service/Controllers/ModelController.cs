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
    public class ModelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetSqlManager _getSqlManager;

        public ModelController(IGetSqlManager getSqlManager, IMapper mapper)
        {
            _mapper = mapper;
            _getSqlManager = getSqlManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ModelDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetModels(int brandId)
        {
            if (brandId == default)
            {
                throw new ValidException("One or more parameters are not valid");
            }

            return Ok(_mapper.Map<List<ModelDto>>(await _getSqlManager.GetModels(brandId)));
        }
    }
}
