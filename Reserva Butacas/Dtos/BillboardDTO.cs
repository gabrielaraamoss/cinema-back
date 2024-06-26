namespace Reserva_Butacas.Dtos;

public class BillboardDTO
{
    public int BillboardId { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int MovieId { get; set; }
    public int RoomId { get; set; }
    public DateTime CancellationDate { get; set; }
}