using AutoMapper;
using Careblock.Model.Database;
using Careblock.Model.Web.Account;

namespace Careblock.Service.Helper.Common;

public class AutoMapperProfile : Profile
{
    // Hạn chế sử dụng automapper
    public AutoMapperProfile()
    {
        CreateMap<AccountFormDto, Account>();
    }
}