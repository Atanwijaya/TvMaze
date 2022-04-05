using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TVScapper.Interfaces;
using TVScapper.Models;

namespace TVScapper.APIControllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class TVController : ControllerBase
    {
        ITVMazeService _tvMazeService;
        public TVController(ITVMazeService tvMazeService)
        {
            _tvMazeService = tvMazeService;
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> GetTVShows([FromRoute]int page, [FromQuery]int? limit)
        {
            var result = await _tvMazeService.GetTVShowsWithCastAsync(page, limit);
            return Ok(result);
        }

        [HttpGet("{ID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "", typeof(TVShowVM))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Constant.ExceptionMessage.IDNotFoundException, typeof(string))]
        public async Task<IActionResult> GetTVShowByID([FromRoute] int ID)
        {
            var result = await _tvMazeService.GetTVShowsWithCastByIDAsync(ID);
            return Ok(result);
        }
    }
}
