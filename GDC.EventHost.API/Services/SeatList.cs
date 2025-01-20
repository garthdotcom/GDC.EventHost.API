using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.Services;
using GDC.EventHost.Shared.Seat;
using GDC.EventHost.Shared.SeatPosition;
using GDC.EventHost.Shared.Ticket;

namespace GHC.EventHost.API.Services
{
    public class SeatList
    {
        private readonly IEventHostRepository _eventHostRepository;

        public SeatList(IEventHostRepository eventHostRepository, Guid layoutId)
        {
            _eventHostRepository = eventHostRepository ??
                throw new ArgumentNullException(nameof(eventHostRepository));

            _eventHostRepository = eventHostRepository;

            SeatPositions = GetSeatPositions(layoutId);

            SeatDetails = GetSeatsToDisplay();
        }


        // list of entities
        public IEnumerable<SeatPosition> SeatPositions { get; }


        // detailed list of composite seat dtos
        public IEnumerable<SeatDisplayDto> SeatDetails { get; }


        // list of detailed tickets
        public async Task<IEnumerable<TicketDetailDto>> GetPerformanceTicketsAsync(Guid performanceId)
        {
            var detailedTicketList = new List<TicketDetailDto>();

            var performanceDetail = await _eventHostRepository.GetPerformanceByIdAsync(performanceId);

            var performanceTickets = performanceDetail.Tickets;

            foreach (var seatDetail in SeatDetails)
            {
                var ticket = performanceTickets.FirstOrDefault(t => t.SeatId == seatDetail.Id); 

                if (ticket != null)
                {
                    detailedTicketList.Add(new TicketDetailDto()
                    {
                        Id = ticket.Id,
                        Number = ticket.Number,
                        Price = ticket.Price,
                        TicketStatusId = ticket.TicketStatusId,
                        TicketStatusName = ticket.TicketStatusId.ToString(),
                        PerformanceId = performanceId,
                        PerformanceTitle = performanceDetail.Event.Title,
                        PerformanceDate = performanceDetail.Date,
                        VenueName = performanceDetail.Venue.Name,
                        SeatId = seatDetail.Id,
                        Seat = seatDetail
                    });
                }
                //todo: alert case where seat has no ticket
            }

            return detailedTicketList;
        }


        private void GetRoot(SeatPosition node, ref List<SeatPosition> seatPositionList)
        {
            GetNode(node, ref seatPositionList);
        }

        private void GetNode(SeatPosition node, ref List<SeatPosition> seatPositionList)
        {
            node.Seats = _eventHostRepository.GetSeatsForParent(node.Id);

            seatPositionList.Add(node);

            var children = _eventHostRepository.GetSeatPositionsForParent(node.Id);

            foreach (var child in children)
            {
                GetNode(child, ref seatPositionList);
            }
        }

        private IEnumerable<SeatPosition> GetSeatPositions(Guid layoutId)
        {
            var seatPositionList = new List<SeatPosition>();

            var rootPositions = _eventHostRepository.GetRootSeatPositions(layoutId);

            foreach (var rootPosition in rootPositions)
            {
                GetRoot(rootPosition, ref seatPositionList);
            }

            return seatPositionList;
        }

        private IEnumerable<SeatDisplayDto> GetSeatsToDisplay()
        {
            var positionStack = new Stack<SeatPosition>();
            var seatDisplayList = new List<SeatDisplayDto>();

            foreach (var seatPosition in SeatPositions)
            {
                if (positionStack.Count() > 0 &&
                    positionStack.Peek().Level == seatPosition.Level &&
                    positionStack.Peek().Seats.Count() == 0)
                {
                    positionStack.Pop();
                }

                positionStack.Push(seatPosition);

                if (seatPosition.Seats.Count() > 0)
                {
                    var posnDisplayList = new List<SeatPositionDisplayDto>();

                    // make a list of the current stack contents

                    var posnSourceList = positionStack
                        .OrderBy(p => p.Level)
                        .ToList();

                    foreach (var posn in posnSourceList)
                    {
                        posnDisplayList.Add(new SeatPositionDisplayDto()
                        {
                            SeatPositionTypeId = posn.SeatPositionTypeId,
                            SeatPositionTypeName = posn.SeatPositionTypeId.ToString(),
                            OrdinalValue = posn.OrdinalValue,
                            DisplayValue = posn.DisplayValue,
                            Level = posn.Level
                        });
                    }

                    // use the stack contents list for the position list on each seat

                    foreach (var seat in seatPosition.Seats)
                    {
                        seatDisplayList.Add(new SeatDisplayDto()
                        {
                            Id = seat.Id,
                            SeatTypeId = seat.SeatTypeId,
                            SeatTypeName = seat.SeatTypeId.ToString(),
                            OrdinalValue = seat.OrdinalValue,
                            DisplayValue = seat.DisplayValue,
                            ParentId = seat.ParentId,
                            SeatPositions = posnDisplayList
                        });
                    }

                    positionStack.Pop();
                }
            }

            return seatDisplayList;
        }
    }
}
