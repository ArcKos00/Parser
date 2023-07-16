using System.Net;
using Parser.Common.SqlManager;
using Parser.Service.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Parser.Service.Controllers
{
    [ApiController]
    [Route(DefaultRoute.Route)]
    public class BrandController : ControllerBase
    {
        private readonly IGetSqlManager _getSqlManager;
        private readonly IMapper _mapper;

        public BrandController(IGetSqlManager getSqlManager, IMapper mapper)
        {
            _mapper = mapper;
            _getSqlManager = getSqlManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<BrandDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBrands()
        {
            return Ok(_mapper.Map<List<BrandDto>>(await _getSqlManager.GetBrands()));
        }
    }
}
