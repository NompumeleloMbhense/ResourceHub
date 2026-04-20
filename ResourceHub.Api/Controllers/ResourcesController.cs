using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Entities;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Pagination;
using ResourceHub.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly IMapper _mapper;

        public ResourcesController(IResourceService resourceService, IMapper mapper)
        {
            _resourceService = resourceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ResourceQueryParams query)
        {
            var pagedResources = await _resourceService.GetAllResourcesAsync(query);

            var result = new PagedResult<ResourceDto>
            {
                PageNumber = pagedResources.PageNumber,
                PageSize = pagedResources.PageSize,
                TotalCount = pagedResources.TotalCount,
                TotalPages = pagedResources.TotalPages,
                Data = _mapper.Map<IEnumerable<ResourceDto>>(pagedResources.Data)
            };

            return Ok(result);
        }

        // GET: api/resource/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resource = await _resourceService.GetResourceByIdAsync(id);

            if (resource == null)
                return NotFound();

            var result = _mapper.Map<ResourceDto>(resource);

            return Ok(result);
        }

        // POST: api/resource
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateResourceDto dto)
        {
            var resource = _mapper.Map<Resource>(dto);

            var created = await _resourceService.CreateResourceAsync(resource);

            return CreatedAtAction(nameof(GetById),
            new { id = created.Id },
            _mapper.Map<ResourceDto>(created));
        }

        // PUT: api/resource/1
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateResourceDto dto)
        {
            await _resourceService.UpdateResourceAsync(
                id,
                dto.Name,
                dto.Description,
                dto.Location,
                dto.Capacity,
                dto.IsAvailable
            );

            return NoContent();
        }

        // GET: api/resource/1
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resourceService.DeleteResourceAsync(id);
            return NoContent();
        }
    }
}