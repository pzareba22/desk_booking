using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tables.Services;

namespace Tables.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {

        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            this._locationService = locationService;
        }


        [HttpGet]
        public async Task<ActionResult<List<LocationResponseDTO>>> GetLocations()
        {
            var locations = await _locationService.GetLocations();
            return Ok(locations.Select(l => new LocationResponseDTO()
            {
                Id = l.Id,
                Name = l.Name,
                Desks = l.Desks != null ? l.Desks.Select(d => new DeskDTO()
                {
                    Capacity = d.Capacity,
                    IsAvailiable = d.IsAvailiable,
                    locationName = d.DeskLocation.Name
                }).ToArray() : new List<DeskDTO>(),
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationResponseDTO>> GetLocation(int id)
        {
            var location = await _locationService.GetLocationById(id);
            if (location == null) return BadRequest("No such Location");
            return Ok(new LocationResponseDTO()
            {
                Id = location.Id,
                Name = location.Name,
                Desks = location.Desks.Select(d => new DeskDTO()
                {
                    Capacity = d.Capacity,
                    IsAvailiable = d.IsAvailiable,
                    locationName = d.DeskLocation.Name
                }).ToArray()
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AddLocation(Location location)
        {
            var res = await _locationService.AddLocation(location);
            if (res.IsSuccess) return Ok();
            return BadRequest(res.Messsage);
        }

        // [HttpPut("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateLocation(Location location)
        {
            var res = await _locationService.UpdateLocation(location);

            if (res.IsSuccess) return Ok();

            return BadRequest(res.Messsage);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var res = await _locationService.DeleteLocation(id);
            if (res.IsSuccess) return Ok();
            return BadRequest(res.Messsage);
        }

    }
}