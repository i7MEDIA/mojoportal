#nullable enable
using mojoPortal.SearchIndex;
using mojoPortal.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace mojoPortal.Web.Models;

public record SearchResultsViewModel(
	Results Results,
	PagerInfo Pagination,
	List<Item> Items
)
{
	public static SearchResultsViewModel Create(
		int pageNumber,
		int pageSize,
		int totalItemCount,
		bool showModuleTitle,
		string pagerUrlFormat,
		string queryText,
		TimeZoneInfo timeZone,
		IndexItemCollection items,
		SearchResultsDisplaySettings displaySettings
	)
	{
		var from = ((pageNumber - 1) * pageSize) + 1;
		var to = Math.Min(from + pageSize - 1, totalItemCount);

		return new(
			new Results(
				from,
				to,
				totalItemCount,
				HttpUtility.HtmlEncode(queryText)
			),
			new PagerInfo(
				pageNumber,
				pageSize,
				(int)Math.Ceiling((double)totalItemCount / pageSize),
				pagerUrlFormat
			),
			[.. items
				.Cast<IndexItem>()
				.Select(x => new Item(
					new Link(x, showModuleTitle),
					new Content(x.Intro),
					new Author(x.Author, displaySettings.ShowAuthor),
					new Date(x.CreatedUtc, timeZone, displaySettings.ShowCreatedDate),
					new Date(x.LastModUtc, timeZone, displaySettings.ShowLastModDate),
					!string.IsNullOrWhiteSpace(x.ItemImage) ? new Image(x.ItemImage, x.Title) : null
				))]
		);
	}
}


public record Results(
	int From,
	int To,
	int Total,
	string Query
);


public record Title
{
	private readonly bool _showModuleTitle;

	public string PageTitle { get; private set; }
	public string ModuleTitle { get; private set; }
	public string ItemTitle { get; private set; }

	public Title(
		string pageTitle,
		string moduleTitle,
		string itemTitle,
		bool showModuleTitle
	)
	{
		PageTitle = pageTitle;
		ModuleTitle = moduleTitle;
		ItemTitle = itemTitle;

		_showModuleTitle = showModuleTitle;
	}


	public override string ToString()
	{
		if (!string.IsNullOrWhiteSpace(ItemTitle))
		{
			if (_showModuleTitle)
			{
				return $"{PageTitle} > {ModuleTitle} > {ItemTitle}";
			}
			else
			{
				return $"{PageTitle} > {ItemTitle}";
			}
		}
		else
		{
			return PageTitle;
		}
	}
}


public record Link
{
	public Title Text { get; private set; }
	public string Href { get; private set; }


	public Link(IndexItem item, bool showModuleTitle)
	{
		if (item.UseQueryStringParams)
		{
			Href = item.ViewPage
				.ToLinkBuilder()
				.PageId(item.PageId)
				.ModuleId(item.ModuleId)
				.ItemId(item.ItemId)
				.ToString() + item.QueryStringAddendum;
		}
		else
		{
			Href = item.ViewPage.ToLinkBuilder().ToString();
		}

		Text = new Title(item.PageName, item.ModuleTitle, item.Title, showModuleTitle);
	}
}


public record Content(string Text)
{
	public override string ToString()
	{
		return Text;
	}
}


public record Author(string Name, bool ShowAuthor)
{
	public override string ToString()
	{
		return Name;
	}
}


public record Date
{
	private readonly DateTime? _date = null;

	public bool ShowDate { get; private set; }


	public Date(DateTime date, TimeZoneInfo? timeZoneInfo = null, bool showDate = false)
	{
		if (date.Date != DateTime.MinValue.Date)
		{
			if (timeZoneInfo is not null)
			{
				_date = TimeZoneInfo.ConvertTimeFromUtc(date, timeZoneInfo);
			}
			else
			{
				_date = date;
			}

			ShowDate = true;
		}
		else
		{
			ShowDate = false;
		}

		if (showDate == false)
		{
			ShowDate = false;
		}
	}


	public override string ToString() => ToString("d");
	public string ToString(string format) => ShowDate && _date.HasValue ? _date.Value.ToString(format) : string.Empty;
}


public record Image(string Href, string Alt);


public record Item(
	Link Link,
	Content Content,
	Author Author,
	Date CreatedDate,
	Date ModifiedDate,
	Image? Image = null
);
