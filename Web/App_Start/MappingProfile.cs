using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using mojoPortal.FileSystem;
using mojoPortal.Web.Dtos;

namespace mojoPortal.Web.App_Start
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<WebFile, FileServiceDto>();
			CreateMap<WebFolder, FileServiceDto>();

			CreateMap<FileServiceDto, WebFile>();
			CreateMap<FileServiceDto, WebFolder>();
		}
	}
}