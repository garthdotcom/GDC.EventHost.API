using GDC.EventHost.DTO.Asset;
using GDC.EventHost.DTO.ShoppingCart;
using System;
using System.Collections.Generic;

namespace GDC.EventHost.DTO.Member
{
    public class MemberDetailDto
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public ShoppingCartDto ShoppingCart { get; set; }

        public List<MemberAssetDto> MemberAssets { get; set; }
    }
}