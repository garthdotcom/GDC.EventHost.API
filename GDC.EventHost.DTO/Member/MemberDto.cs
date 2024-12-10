using GDC.EventHost.DTO.ShoppingCart;
using System;

namespace GDC.EventHost.DTO.Member
{
    public class MemberDto
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; } 

        public bool IsActive { get; set; }

        public ShoppingCartDto ShoppingCart { get; set; }
    }
}