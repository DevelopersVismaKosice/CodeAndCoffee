using AngleSharp;
using Microsoft.Extensions.DependencyInjection;

namespace VismaDocker.ScraperWorker
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAngleSharp(this IServiceCollection services) =>
			services.AddSingleton(BrowsingContext.New(
				AngleSharp.Configuration.Default.WithDefaultLoader()));
	}
}