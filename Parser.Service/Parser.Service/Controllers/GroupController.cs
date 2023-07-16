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
    public class GroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetSqlManager _getSqlManager;

        public GroupController(IGetSqlManager getSqlManager, IMapper mapper)
        {
            _mapper = mapper;
            _getSqlManager = getSqlManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GroupDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGroups(int complectationId)
        {
            if (complectationId == default)
            {
                throw new ValidException("One or more parameters are not valid");
            }

            return Ok(_mapper.Map<List<GroupDto>>(await _getSqlManager.GetGroups(complectationId)));
        }
    }
}
