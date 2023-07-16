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
    public class SubGroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetSqlManager _getSqlManager;

        public SubGroupController(IGetSqlManager getSqlManager, IMapper mapper)
        {
            _mapper = mapper;
            _getSqlManager = getSqlManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SubGroupDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubGroups(int complectationId, int groupId)
        {
            if (complectationId == default && groupId == default)
            {
                throw new ValidException("One or more parameters are not valid");
            }

            return Ok(_mapper.Map<List<SubGroupDto>>(await _getSqlManager.GetSubGroups(complectationId, groupId)));
        }
    }
}
