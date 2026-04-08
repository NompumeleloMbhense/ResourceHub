using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Entities;
using ResourceHub.Api.DTOs;
using AutoMapper;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;
        private readonly IMapper _mapper;

        public ResourceController(IResourceService resourceService, IMapper mapper)
        {
            _resourceService = resourceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _resourceService.GetAllResourcesAsync();
            var result = _mapper.Map<IEnumerable<ResourceDto>>(resources);

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resourceService.DeleteResourceAsync(id);
            return NoContent();
        }
    }
}