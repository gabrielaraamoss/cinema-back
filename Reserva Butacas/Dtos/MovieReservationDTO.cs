using Reserva_Butacas.Models;

namespace Reserva_Butacas.Dtos;

public class MovieReservationDTO
{
    public DatabaseEntities.MovieGenreEnum Genre { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}