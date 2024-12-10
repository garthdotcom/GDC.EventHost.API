using System;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Order
{
    public class OrderForPatchDto
    {
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }

        public OrderStatusEnum OrderStatusId { get; set; }

        public DateTime Date { get; set; }
    }
}
