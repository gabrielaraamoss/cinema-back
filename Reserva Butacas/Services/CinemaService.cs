using Reserva_Butacas.Data;
using Reserva_Butacas.Dtos; 
using Reserva_Butacas.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IBillboardRepository _billboardRepository;
        private readonly ILogger<CinemaService> _logger;
        private readonly Context _context;

        public CinemaService(
            IBookingRepository bookingRepository,
            ISeatRepository seatRepository,
            IBillboardRepository billboardRepository,
            Context context,
            ILogger<CinemaService> logger)
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _billboardRepository = billboardRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CancelSeatAndBooking(SeatDTO cancellationDTO)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var seat = await _seatRepository.GetById(cancellationDTO.SeatId);
                if (seat == null)
                {
                    _logger.LogWarning($"Seat with id {cancellationDTO.SeatId} not found.");
                    return false;
                }

                var booking = await _bookingRepository.GetById(cancellationDTO.BookingId);
                if (booking == null || booking.SeatId != cancellationDTO.SeatId)
                {
                    _logger.LogWarning($"Booking with id {cancellationDTO.BookingId} not found for seat {cancellationDTO.SeatId}.");
                    return false;
                }

                await _bookingRepository.Delete(cancellationDTO.BookingId);

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cancelling seat and booking: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelBillboardAndBookings(BillboardDTO cancellationDTO)
        {
            try
            {
                if (cancellationDTO.CancellationDate.Date < DateTime.Now.Date)
                {
                    throw new InvalidOperationException("No se puede cancelar funciones de la cartelera con fecha anterior a la actual.");
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                var billboard = await _billboardRepository.GetById(cancellationDTO.BillboardId);
                if (billboard == null)
                {
                    _logger.LogWarning($"Billboard with id {cancellationDTO.BillboardId} not found.");
                    return false;
                }

                var affectedBookings = await _bookingRepository.GetByBillboardId(billboard.Id);
                await _billboardRepository.Delete(cancellationDTO.BillboardId);

                foreach (var booking in affectedBookings)
                {
                    var seat = await _seatRepository.GetById(booking.SeatId);
                    if (seat != null)
                    {
                        seat.Status = true;
                        await _seatRepository.Update(seat);
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cancelling billboard and bookings: {ex.Message}");
                return false;
            }
        }
        
        public async Task<List<DatabaseEntities.BookingEntity>> GetMovieReservations(DatabaseEntities.MovieGenreEnum genre, DateTime startDate, DateTime endDate)
        {
            try
            {
                var reservations = await _bookingRepository.GetReservationsByGenreAndDateRangeAsync(genre, startDate, endDate);
                return reservations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<SeatStatusDTO> GetSeatStatusByRoom(int roomId)
        {
            try
            {
                var seats = await _seatRepository.GetByRoomId(roomId);

                if (seats == null || !seats.Any())
                {
                    throw new InvalidOperationException($"No seats found for room with id {roomId}");
                }

                var totalSeats = seats.Count();
                var availableSeats = seats.Count(s => s.Status); 
                var occupiedSeats = totalSeats - availableSeats;

                var seatStatusDto = new SeatStatusDTO
                {
                    RoomId = roomId,
                    TotalSeats = totalSeats,
                    AvailableSeats = availableSeats,
                    OccupiedSeats = occupiedSeats
                };

                return seatStatusDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving seat status for room {roomId}: {ex.Message}");
                throw; 
            }
        }
        
        public async Task<List<SeatStatusDTO>> GetSeatStatusByRoomForDate(DateTime currentDate)
        {
            try
            {
                var billboards = await _billboardRepository.GetByDate(currentDate);

                if (billboards == null || !billboards.Any())
                {
                    throw new InvalidOperationException($"No billboards found for date {currentDate.ToShortDateString()}");
                }

                var roomIdList = billboards.Select(b => b.RoomId).Distinct().ToList();
                var seatStatusList = new List<SeatStatusDTO>();

                foreach (var roomId in roomIdList)
                {
                    var seats = await _seatRepository.GetByRoomId(roomId);
                    var totalSeats = seats.Count();
                    var availableSeats = seats.Count(s => s.Status);
                    var occupiedSeats = totalSeats - availableSeats;

                    var seatStatusDto = new SeatStatusDTO
                    {
                        RoomId = roomId,
                        TotalSeats = totalSeats,
                        AvailableSeats = availableSeats,
                        OccupiedSeats = occupiedSeats
                    };

                    seatStatusList.Add(seatStatusDto);
                }

                return seatStatusList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving seat status for date {currentDate.ToShortDateString()}: {ex.Message}");
                throw;
            }
        }


    }
    
}
