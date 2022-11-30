using AutoMapper;
using System;

namespace mojoPortal.Core.Mappers
{
	public static class AutoMapperAdapter
	{
		private const string InvalidOperationMessage = "Mapper not initialized. Call Initialize with appropriate configuration. If you are trying to use mapper instances through a container or otherwise, make sure you do not have any calls to the static Mapper.Map methods, and if you're using ProjectTo or UseAsDataSource extension methods, make sure you pass in the appropriate IConfigurationProvider instance.";
		private const string AlreadyInitialized = "Mapper already initialized. You must call Initialize once per application domain/process.";

		private static IConfigurationProvider _configuration;
		private static IMapper _instance;

		private static IConfigurationProvider Configuration
		{
			get => _configuration ?? throw new InvalidOperationException(InvalidOperationMessage);
			set => _configuration = (_configuration == null) ? value : throw new InvalidOperationException(AlreadyInitialized);
		}

		public static IMapper Mapper
		{
			get => _instance ?? throw new InvalidOperationException(InvalidOperationMessage);
			private set => _instance = value;
		}

		public static void Initialize(Action<IMapperConfigurationExpression> config)
		{
			Initialize(new MapperConfiguration(config));
		}

		public static void Initialize(MapperConfiguration config)
		{
			Configuration = config;
			Mapper = Configuration.CreateMapper();
		}

		public static void AssertConfigurationIsValid() => Configuration.AssertConfigurationIsValid();
	}
}
