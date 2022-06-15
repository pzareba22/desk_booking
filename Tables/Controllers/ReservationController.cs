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
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;

        public ReservationController(IReservationService reservationService, IUserService userService)
        {
            this._reservationService = reservationService;
            this._userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> getReservations()
        {
            var reservations = await _reservationService.GetReservations();
            return Ok(reservations.Select(r => new ReservationDTO()
            {
                deskId = r.Desk.Id,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo
            }).ToList());
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<ReservationDTO>>> getReservationsAdmin()
        {
            var reservations = await _reservationService.GetReservations();
            return Ok(reservations.Select(r => new ReservationAdminDTO()
            {
                deskId = r.Desk.Id,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                username = r.User.Username
            }).ToList());
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> BookDesk(ReservationDTO reservationDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var res = await _userService.BookDesk(reservationDTO, identity);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.Messsage);
        }
    }

}