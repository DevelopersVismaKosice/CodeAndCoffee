using AngleSharp;
using System;

namespace VismaDocker.ScraperWorker
{
	public class HtmlScraper
	{
		readonly IBrowsingContext _browsingContext;

		public HtmlScraper(IBrowsingContext browsingContext)
		{
			this._browsingContext = browsingContext;
		}
  	}
}