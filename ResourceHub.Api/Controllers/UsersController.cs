using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.QueryParams;
using AutoMapper;
using ResourceHub.Shared.DTOs;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UsersController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserQueryParams query)
        {
            var pagedUsers = await _service.GetUsersAsync(query);

            return Ok(new
            {
                pagedUsers.PageNumber,
                pagedUsers.PageSize,
                pagedUsers.TotalCount,
                pagedUsers.TotalPages,
                Data = _mapper.Map<IEnumerable<UserDto>>(pagedUsers.Data)
            });
        }
    }
}