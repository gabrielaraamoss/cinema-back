using Reserva_Butacas.Models;

namespace Reserva_Butacas.Dtos;

public class MovieDTO
{
    public string Name { get; set; }
    public DatabaseEntities.MovieGenreEnum Genre { get; set; }
    public short AllowedAge { get; set; }
    public short LengthMinutes { get; set; }
}