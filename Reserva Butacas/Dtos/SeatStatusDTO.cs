namespace Reserva_Butacas.Dtos;

public class SeatStatusDTO
{
    public int RoomId { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int OccupiedSeats { get; set; }
}