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
    public class DeskController : ControllerBase
    {
        private readonly IDeskService _deskService;
        private readonly ILocationService _locationService;

        public DeskController(IDeskService deskService, ILocationService locationService)
        {
            this._deskService = deskService;
            this._locationService = locationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DeskResponseDTO>>> getDesks()
        {
            var desks = await _deskService.GetDesks();
            Console.WriteLine("desks: ");
            Console.WriteLine(desks);

            return Ok(desks.Select(d => new DeskResponseDTO()
            {
                Id = d.Id,
                Capacity = d.Capacity,
                IsAvailiable = d.IsAvailiable,
                locationName = d.DeskLocation.Name
            }).ToList());
        }

        [HttpGet("{locationName}")]
        public async Task<ActionResult<List<DeskResponseDTO>>> GetDesksByLocation(string locationName)
        {
            var desks = await _deskService.GetDesksByLocation(locationName);
            return Ok(desks.Select(d => new DeskResponseDTO()
            {
                Id = d.Id,
                Capacity = d.Capacity,
                IsAvailiable = d.IsAvailiable,
                locationName = d.DeskLocation.Name
            }).ToList());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AddDeskToLocation(DeskDTO desk)
        {
            Location location = await _locationService.GetLocationByName(desk.locationName);
            if (location == null)
            {
                return BadRequest("No such location");
            }
            else
            {
                var newDesk = new Desk() { DeskLocation = location, Capacity = desk.Capacity };
                await _deskService.AddDesk(newDesk);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteDesk(int id)
        {
            var res = await _deskService.DeleteDesk(id);
            if (res.IsSuccess)
                return Ok();
            return BadRequest(res.Messsage);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ChangeDeskAvailiability(int id, bool availiability)
        {
            var res = await _deskService.ChangeDeskAvailiability(id, availiability);
            if (res.IsSuccess)
                return Ok();
            return BadRequest(res.Messsage);
        }

    }
}