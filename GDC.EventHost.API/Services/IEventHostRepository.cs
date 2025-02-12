﻿using GDC.EventHost.DAL.Entities;
using GDC.EventHost.API.ResourceParameters;
using GDC.EventHost.Shared;
using GDC.EventHost.Shared.Performance;

namespace GDC.EventHost.API.Services
{
    public interface IEventHostRepository
    {
        Task AddAssetAsync(Asset asset);
        Task AddEventAssetAsync(EventAsset eventAsset);
        Task AddEventAsync(Event evt);
        Task AddMemberAssetAsync(MemberAsset memberAsset);
        Task AddMemberAsync(Member member);
        Task AddOrderAsync(Order order);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task AddPerformanceAssetAsync(PerformanceAsset performanceAsset);
        Task AddPerformanceAsync(Performance performance);
        Task AddPerformanceTypeAsync(PerformanceType performanceType);
        Task AddSeatAsync(Seat seat);
        Task AddSeatingPlanAsync(SeatingPlan seatingPlan);
        Task AddSeriesAssetAsync(SeriesAsset seriesAsset);
        Task AddSeriesAsync(Series series);
        Task AddShoppingCartAsync(ShoppingCart shoppingCart);
        Task AddShoppingCartItemAsync(ShoppingCartItem shoppingCartItem);
        Task AddTicketAsync(Ticket ticket);
        Task AddVenueAssetAsync(VenueAsset venueAsset);
        Task AddVenueAsync(Venue venue);
        Task<bool> AssetExistsAsync(Guid assetId);
        void DeleteAsset(Asset asset);
        void DeleteEvent(Event evt);
        void DeleteEventAsset(EventAsset eventAsset);
        void DeleteMember(Member member);
        void DeleteMemberAsset(MemberAsset memberAsset);
        void DeleteOrder(Order order);
        void DeleteOrderItem(OrderItem orderItem);
        void DeletePerformance(Performance performance);
        void DeletePerformanceAsset(PerformanceAsset performanceAsset);
        void DeleteSeat(Seat seat);
        void DeleteSeatingPlan(SeatingPlan seatingPlan);
        void DeleteSeries(Series series);
        void DeleteSeriesAsset(SeriesAsset seriesAsset);
        void DeleteShoppingCart(ShoppingCart shoppingCart);
        void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem);
        void DeleteTicket(Ticket ticket);
        void DeleteVenue(Venue venue);
        void DeleteVenueAsset(VenueAsset venueAsset);
        Task<bool> EventAssetExistsAsync(Guid eventId, Enums.AssetTypeEnum assetType);
        Task<bool> EventExistsAsync(Guid eventId);
        Task<Asset?> GetAssetByIdAsync(Guid assetId);
        Task<IEnumerable<Asset>> GetAssetsAsync();
        Task<IEnumerable<Asset>> GetAssetsAsync(AssetResourceParameters resourceParms);
        Task<EventAsset?> GetEventAssetByIdAsync(Guid eventAssetId);
        Task<List<EventAsset>> GetEventAssetsAsync(Guid eventId);
        Task<IEnumerable<EventAsset>> GetEventAssetsAsync(Guid eventId, AssetResourceParameters resourceParms);
        Task<Event?> GetEventByIdAsync(Guid eventId, bool includePast = false);
        Task<List<Performance>> GetEventPerformancesAsync(Guid eventId, bool includePast = false);
        Task<IEnumerable<Event>> GetEventsAsync(bool includePast = false);
        Task<IEnumerable<Event>> GetEventsAsync(EventResourceParameters resourceParms, bool includePast = false);
        Task<MemberAsset?> GetMemberAssetByIdAsync(Guid memberAssetId);
        Task<List<MemberAsset>> GetMemberAssetsAsync(Guid memberId);
        Task<IEnumerable<MemberAsset>> GetMemberAssetsAsync(Guid memberId, AssetResourceParameters resourceParms);
        Task<Member?> GetMemberByIdAsync(Guid memberId);
        Task<Member?> GetMemberByMembershipNumberAsync(string membershipNumber);
        Task<IEnumerable<Member>> GetMembersAsync();
        Task<IEnumerable<Member>> GetMembersAsync(MemberResourceParameters resourceParms);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersAsync(OrderResourceParameters resourceParms);
        Task<IEnumerable<Order>> GetOrdersForMemberAsync(Guid memberId);
        Task<PerformanceAsset?> GetPerformanceAssetByIdAsync(Guid performanceAssetId);
        Task<List<PerformanceAsset>> GetPerformanceAssetsAsync(Guid performanceId);
        Task<IEnumerable<PerformanceAsset>> GetPerformanceAssetsAsync(Guid performanceId, AssetResourceParameters resourceParms);
        Task<Performance?> GetPerformanceByIdAsync(Guid performanceId);
        Task<IEnumerable<Performance>> GetPerformancesAsync(bool includePast = false);
        Task<IEnumerable<Performance>> GetPerformancesAsync(PerformanceResourceParameters resourceParms, bool includePast = false);
        PerformanceTicketCount GetPerformanceTicketCount(Guid performanceId);
        Task<List<Ticket>> GetPerformanceTicketsAsync(Guid performanceId);
        Task<PerformanceType?> GetPerformanceTypeByIdAsync(Guid performanceTypeId, bool includePast = false);
        Task<List<Performance>> GetPerformanceTypePerformancesAsync(Guid performanceTypeId, bool includePast = false);
        Task<IEnumerable<PerformanceType>> GetPerformanceTypesAsync(bool includePast = false);
        IEnumerable<SeatPosition> GetRootSeatPositions(Guid seatingPlanId);
        Task<Seat?> GetSeatByIdAsync(Guid seatId);
        Task<SeatingPlan?> GetSeatingPlanByIdAsync(Guid seatingPlanId, bool includePast = false);
        Task<List<Performance>> GetSeatingPlanPerformancesAsync(Guid seatingPlanId, bool includePast = false);
        Task<IEnumerable<SeatingPlan>> GetSeatingPlansAsync(bool includePast = false);
        Task<IEnumerable<SeatingPlan>> GetSeatingPlansAsync(SeatingPlanResourceParameters resourceParms, bool includePast = false);
        Task<SeatPosition?> GetSeatPositionByIdAsync(Guid seatPositionId);
        Task<IEnumerable<SeatPosition>> GetSeatPositionsAsync();
        IEnumerable<SeatPosition> GetSeatPositionsForParent(Guid seatPositionId);
        IEnumerable<SeatPosition> GetSeatPositionsForSeat(Guid parentId);
        Task<IEnumerable<Seat>> GetSeatsAsync();
        List<Seat> GetSeatsForParent(Guid seatPositionId);
        Task<SeriesAsset?> GetSeriesAssetByIdAsync(Guid seriesAssetId);
        Task<List<SeriesAsset>> GetSeriesAssetsAsync(Guid seriesId);
        Task<List<SeriesAsset>> GetSeriesAssetsAsync(Guid seriesId, AssetResourceParameters resourceParms);
        Task<IEnumerable<Series>> GetSeriesAsync(bool includePast = false);
        Task<IEnumerable<Series>> GetSeriesAsync(SeriesResourceParameters resourceParms, bool includePast = false);
        Task<Series?> GetSeriesByIdAsync(Guid seriesId, bool includePast = false);
        Task<List<Event>> GetSeriesEventsAsync(Guid seriesId, bool includePast = false);
        Task<ShoppingCart?> GetShoppingCartByIdAsync(Guid shoppingCartId);
        Task<ShoppingCart?> GetShoppingCartByMemberIdAsync(Guid memberId);
        Task<ShoppingCartItem?> GetShoppingCartItemByIdAsync(Guid shoppingCartItemId);
        Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync();
        Task<Ticket?> GetTicketByIdAsync(Guid ticketId);
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<IEnumerable<Ticket>> GetTicketsAsync(TicketResourceParameters resourceParms);
        Task<VenueAsset?> GetVenueAssetByIdAsync(Guid venueAssetId);
        Task<List<VenueAsset>> GetVenueAssetsAsync(Guid venueId);
        Task<IEnumerable<VenueAsset>> GetVenueAssetsAsync(Guid venueId, AssetResourceParameters resourceParms);
        Task<Venue?> GetVenueByIdAsync(Guid venueId, bool includePast = false);
        Task<List<Performance>> GetVenuePerformancesAsync(Guid venueId, bool includePast = false);
        Task<IEnumerable<Venue>> GetVenuesAsync(bool includePast = false);
        Task<IEnumerable<Venue>> GetVenuesAsync(VenueResourceParameters resourceParms, bool includePast = false);
        Task<List<SeatingPlan>> GetVenueSeatingPlansAsync(Guid venueId);
        Task<bool> MemberAssetExistsAsync(Guid memberId, Enums.AssetTypeEnum assetType);
        Task<bool> MemberExistsAsync(Guid memberId);
        Task<bool> OrderExistsAsync(Guid orderId);
        Task<bool> PerformanceAssetExistsAsync(Guid performanceId, Enums.AssetTypeEnum assetType);
        Task<bool> PerformanceExistsAsync(Guid performanceId);
        Task<bool> PerformanceTicketsExistAsync(Guid performanceId);
        Task<bool> PerformanceTypeExistsAsync(Guid performanceTypeId);
        Task<bool> SaveChangesAsync();
        Task<bool> SeatExistsAsync(Guid seatId);
        Task<bool> SeatingPlanExistsAsync(Guid seatingPlanId);
        Task<bool> SeriesAssetExistsAsync(Guid seriesId, Enums.AssetTypeEnum assetType);
        Task<bool> SeriesExistsAsync(Guid seriesId);
        Task<bool> ShoppingCartExistsAsync(Guid shoppingCartId);
        void SoftDeleteEvent(Event evt);
        void SoftDeleteMember(Member member);
        void SoftDeletePerformance(Performance performance);
        void SoftDeleteSeatingPlan(SeatingPlan seatingPlan);
        void SoftDeleteSeries(Series series);
        void SoftDeleteVenue(Venue venue);
        Task<bool> VenueAssetExistsAsync(Guid venueId, Enums.AssetTypeEnum assetType);
        Task<bool> VenueExistsAsync(Guid venueId);
    }
}