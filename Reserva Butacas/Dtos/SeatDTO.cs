namespace Reserva_Butacas.Dtos;

public class SeatDTO
{
    public int SeatId { get; set; }
    public int BookingId { get; set; }
    public short Number { get; set; }
    public short RowNumber { get; set; }
    public int RoomId { get; set; }
    public bool Status { get; set; }
}