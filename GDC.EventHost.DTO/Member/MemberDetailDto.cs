using GDC.EventHost.DTO.MemberAsset;
using GDC.EventHost.DTO.ShoppingCart;

namespace GDC.EventHost.DTO.Member
{
    public class MemberDetailDto
    {
        public Guid Id { get; set; }

        public required string UserId { get; set; }

        public required string Username { get; set; }

        public required string FullName { get; set; }

        public required string EmailAddress { get; set; }

        public bool IsActive { get; set; } = false;

        public ShoppingCartDto? ShoppingCart { get; set; }

        public List<MemberAssetDto> MemberAssets { get; set; } = [];
    }
}