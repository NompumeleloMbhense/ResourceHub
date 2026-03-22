using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Entities;
using ResourceHub.Api.DTOs;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _resourceService.GetAllResourcesAsync();

            var result = resources.Select(r => new ResourceDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Location = r.Location,
                Capacity = r.Capacity,
                IsAvailable = r.IsAvailable
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resource = await _resourceService.GetResourceByIdAsync(id);

            if (resource == null)
                return NotFound();

            var result = new ResourceDto
            {
                Id = resource.Id,
                Name = resource.Name,
                Description = resource.Description,
                Location = resource.Location,
                Capacity = resource.Capacity,
                IsAvailable = resource.IsAvailable
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateResourceDto dto)
        {
            var resource = new Resource(dto.Name, dto.Description, dto.Location, dto.Capacity);

            var created = await _resourceService.CreateResourceAsync(resource);

            var result = new ResourceDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Location = created.Location,
                Capacity = created.Capacity,
                IsAvailable = created.IsAvailable
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateResourceDto dto)
        {
            var updated = await _resourceService.UpdateResourceAsync(
                id, dto.Name, dto.Description, dto.Location, dto.Capacity, dto.IsAvailable
            );

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _resourceService.DeleteResourceAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}