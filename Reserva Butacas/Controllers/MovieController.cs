// MovieController.cs

using Microsoft.AspNetCore.Mvc;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movie;

        public MovieController(IMovieRepository movie)
        {
            _movie = movie;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movie.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movie.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieDTO movieDto)
        {
            if (movieDto == null)
            {
                return BadRequest("Movie data is missing.");
            }

            var movieEntity = new DatabaseEntities.MovieEntity
            {
                Name = movieDto.Name,
                Genre = movieDto.Genre, 
                AllowedAge = movieDto.AllowedAge,
                LengthMinutes = movieDto.LengthMinutes
            };

            await _movie.AddAsync(movieEntity);

            return Ok("Movie added successfully.");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movieToDelete = await _movie.GetByIdAsync(id);
            if (movieToDelete == null)
            {
                return NotFound();
            }

            await _movie.DeleteAsync(movieToDelete);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieDTO movieDto)
        {
            var movieToUpdate = await _movie.GetByIdAsync(id);
            if (movieToUpdate == null)
            {
                return NotFound();
            }

            movieToUpdate.Name = movieDto.Name;
            movieToUpdate.Genre = movieDto.Genre;
            movieToUpdate.AllowedAge = movieDto.AllowedAge;
            movieToUpdate.LengthMinutes = movieDto.LengthMinutes;

            await _movie.UpdateAsync(movieToUpdate);

            return NoContent();
        }
    }
}