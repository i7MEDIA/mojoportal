#nullable enable
using mojoPortal.SearchIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
		IndexItemCollection items
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
					new Author(x.Author),
					new Date(x.CreatedUtc, timeZone),
					new Date(x.LastModUtc, timeZone)
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


public record Link
{
	public string Text { get; private set; }
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

		if (!string.IsNullOrWhiteSpace(item.Title))
		{
			if (showModuleTitle)
			{
				Text = $"{item.PageName} &gt; {item.ModuleTitle} &gt; {item.Title}";
			}
			else
			{
				Text = $"{item.PageName} &gt; {item.Title}";
			}
		}
		else
		{
			Text = item.PageName;
		}
	}
}


public record Content(string Text)
{
	public override string ToString()
	{
		return Text;
	}
}


public record Author(string Name)
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


	public Date(DateTime date, TimeZoneInfo? timeZoneInfo = null)
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
	}


	public override string ToString() => ToString("d");
	public string ToString(string format) => ShowDate && _date.HasValue ? _date.Value.ToString(format) : string.Empty;
}


public record Item(
	Link Link,
	Content Content,
	Author Author,
	Date CreatedDate,
	Date ModifiedDate
);
