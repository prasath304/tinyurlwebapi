using tinyurlwebapi.Data;
using AutoMapper;
using tinyurlwebapi.Model;
namespace tinyurlwebapi.Mappings
{
    public class UrlProfile: Profile
    {
        public UrlProfile()
        {
            // Entity → DTO
            CreateMap<Urls, UrlsDto>().ReverseMap();

        }
    }

}
