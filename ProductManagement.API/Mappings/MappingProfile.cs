using AutoMapper;
using ProductManagement.API.Dtos;
using ProductManagement.API.Models;

namespace ProductManagement.API.Mappings
{
    // The "settings" of the printing machine
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // FROM entity  →  TO dto      (for GET: HR file → ID card)
            CreateMap<Supplier, SupplierDto>();

            // FROM dto     →  TO entity   (for POST/PUT: joining form → HR file)
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();

            // ===== Product =====
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.StockStatus,
                           opt => opt.MapFrom(src => src.StockQuantity > 0 ? "In stock" : "Out of stock"));
            // ===== Supplier card (custom names) =====
            CreateMap<Supplier, SupplierCardDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Stars, opt => opt.MapFrom(src => new string('★', src.Rating)))
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.ContactPerson ?? "Not available"));
        }
    }
}