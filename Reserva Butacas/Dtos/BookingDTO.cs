namespace Reserva_Butacas.Dtos;

public class BookingDTO
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int SeatId { get; set; }
    public int BillboardId { get; set; }
}