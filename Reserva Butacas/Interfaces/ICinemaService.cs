using System;
using System.Threading.Tasks;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Services;
    public interface ICinemaService
    {
        Task<bool> CancelSeatAndBooking(SeatDTO cancellationDTO);
        Task<bool> CancelBillboardAndBookings(BillboardDTO cancellationDTO);
        Task<List<DatabaseEntities.BookingEntity>> GetMovieReservations(DatabaseEntities.MovieGenreEnum genre, DateTime startDate, DateTime endDate);
        Task<SeatStatusDTO> GetSeatStatusByRoom(int roomId);
        Task<List<SeatStatusDTO>> GetSeatStatusByRoomForDate(DateTime currentDate); 
    }
