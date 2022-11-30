using AutoMapper;
using mojoPortal.Core.Mappers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Dtos;

namespace mojoPortal.Web.App_Start
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<WebFile, FileServiceDto>()
				.ForMember(x => x.Rights, y => y.Ignore());

			CreateMap<WebFolder, FileServiceDto>()
				.ForMember(x => x.Rights, y => y.Ignore())
				.ForMember(x => x.Size, y => y.Ignore())
				.ForMember(x => x.ContentType, y => y.Ignore());

			CreateMap<FileServiceDto, WebFile>()
				.ForMember(x => x.Path, y => y.Ignore())
				.ForMember(x => x.FolderVirtualPath, y => y.Ignore())
				.ForMember(x => x.Data, y => y.Ignore())
				.ForMember(x => x.VirtualPath, y => y.Ignore())
				.ForMember(x => x.Created, y => y.Ignore());

			CreateMap<FileServiceDto, WebFolder>()
				.ForMember(x => x.Path, y => y.Ignore())
				.ForMember(x => x.VirtualPath, y => y.Ignore())
				.ForMember(x => x.Created, y => y.Ignore());
		}
	}

	public class AutoMapperConfig
	{
		public static void Configure()
		{
			AutoMapperAdapter.Initialize(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});

			AutoMapperAdapter.AssertConfigurationIsValid();
		}
	}
}