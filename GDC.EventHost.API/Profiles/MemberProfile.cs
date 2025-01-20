using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Member;

namespace GDC.EventHost.API.Profiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>();

            CreateMap<Member, MemberDetailDto>();

            CreateMap<MemberForUpdateDto, Member>();

            CreateMap<Member, MemberForUpdateDto>();
        }
    }
}
