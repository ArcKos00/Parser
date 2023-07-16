using System.Net;
using Parser.Common.Exceptions;
using Parser.Common.SqlManager;
using Parser.Service.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Parser.Service.Controllers
{
    [ApiController]
    [Route(DefaultRoute.Route)]
    public class SpareController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetSqlManager _getSqlManager;

        public SpareController(IGetSqlManager getSqlManager, IMapper mapper)
        {
            _mapper = mapper;
            _getSqlManager = getSqlManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SchemaDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSpares(int complectationId, int groupId, int subGroupId)
        {
            if (complectationId == default && groupId == default && subGroupId == default)
            {
                throw new ValidException("One or more parameters are not valid");
            }

            return Ok(_mapper.Map<SchemaDto>(await _getSqlManager.GetSpares(complectationId, groupId, subGroupId)));
        }
    }
}
