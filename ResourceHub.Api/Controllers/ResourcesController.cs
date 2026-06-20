using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.QueryParams;
using ResourceHub.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ResourceHub.Core.Entities;

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

        // GET: api/resources
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ResourceQueryParams query)
        {
            var pagedResources = await _resourceService.GetAllResourcesAsync(query);

            return Ok(new
            {
                pagedResources.PageNumber,
                pagedResources.PageSize,
                pagedResources.TotalCount,
                pagedResources.TotalPages,
                Data = _mapper.Map<IEnumerable<ResourceDto>>(pagedResources.Data)
            });
        }

        // GET: api/resources/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resource = await _resourceService.GetResourceByIdAsync(id);

            if (resource == null)
                return NotFound();

            return Ok(_mapper.Map<ResourceDto>(resource));
        }

        // POST: api/resources
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateResourceDto dto)
        {
            var resource = _mapper.Map<Resource>(dto);

            await _resourceService.CreateResourceAsync(resource);

            return CreatedAtAction(
                nameof(GetById),
                new { id = resource.Id },
                _mapper.Map<ResourceDto>(resource)
            );
        }

        // PUT: api/resources/1
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateResourceDto dto)
        {
            await _resourceService.UpdateResourceAsync(id,
                new Resource(
                    dto.Name,
                    dto.Description,
                    dto.Location,
                    dto.Capacity
                )
            );

            return NoContent();
        }

        // DELETE: api/resources/1
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resourceService.DeleteResourceAsync(id);

            return NoContent();
        }
    }
}