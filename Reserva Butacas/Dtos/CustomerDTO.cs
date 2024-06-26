namespace Reserva_Butacas.Dtos;

public class CustomerDTO
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public short Age { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}