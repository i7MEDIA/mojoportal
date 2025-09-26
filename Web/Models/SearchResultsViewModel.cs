#nullable enable
using mojoPortal.SearchIndex;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web.Models;

public record SearchResultsViewModel(
	string PageTitle,
	Controls Controls,
	ResultsMessage ResultsMessage,
	Results Results,
	PagerInfo Pagination,
	HelpLink HelpLink,
	Filters Filters
)
{
	public static SearchResultsViewModel Create(
		string pageTitle,
		int pageNumber,
		int pageSize,
		int totalItemCount,
		bool showModuleTitleInResultLink,
		string pageUrlFormat,
		string queryText,
		TimeZoneInfo? timeZone,
		IndexItemCollection? items,
		SearchResultsDisplaySettings displaySettings,
		float? duration,
		List<SelectListItem> featureList,
		DateTime dateStart,
		DateTime dateEnd,
		bool queryErrorOccurred
	)
	{
		var from = ((pageNumber - 1) * pageSize) + 1;
		var to = Math.Min(from + pageSize - 1, totalItemCount);

		return new(
			pageTitle,
			new Controls(
				new SearchInput(queryText),
				new SearchButton(Resource.SearchButtonText)
			),
			new ResultsMessage(
				Preamble: Resource.SearchResultsLabel,
				From: from,
				ToText: Resource.SearchResultsOfLabel,
				To: to,
				ForText: Resource.SearchResultsForLabel,
				Total: totalItemCount,
				Query: queryText,
				Duration: duration,
				DurationText: Resource.SearchResultsSecondsLabel
			),
			new Results(
				Items: [.. items?
					.Cast<IndexItem>()
					.Select(x => new Item(
						x.ItemId,
						new Link(x, showModuleTitleInResultLink),
						new Content(x.Intro),
						new Author(x.Author, displaySettings.ShowAuthor),
						new Date(x.CreatedUtc, timeZone, displaySettings.ShowCreatedDate),
						new Date(x.LastModUtc, timeZone, displaySettings.ShowLastModDate),
						!string.IsNullOrWhiteSpace(x.ItemImage) ? new Image(x.ItemImage, x.Title) : null,
						!string.IsNullOrWhiteSpace(x.Sku) ? x.Sku : null,
						!string.IsNullOrWhiteSpace(x.FeatureId) ? x.FeatureId : null,
						!string.IsNullOrWhiteSpace(x.FeatureResourceFile) && !string.IsNullOrWhiteSpace(x.FeatureName) ?
							HttpContext.GetGlobalResourceObject(x.FeatureResourceFile, x.FeatureName) as string :
							null
					)) ?? []],
				ShowResults: items is not null && totalItemCount > 0,
				ShowNoResults: queryErrorOccurred || !string.IsNullOrWhiteSpace(queryText) && totalItemCount == 0,
				NoResultsText: queryErrorOccurred ? Resource.SearchQueryInvalid : Resource.SearchResultsNotFound
			),
			new PagerInfo(
				pageNumber,
				pageSize,
				(int)Math.Ceiling((double)totalItemCount / pageSize),
				pageUrlFormat
			),
			new HelpLink(
				Resource.HelpLink,
				"Help.aspx".ToLinkBuilder().AddParam("helpkey", "search-help").ToString(),
				Resource.CloseDialogButton
			),
			new Filters(
				new Feature(
					featureList,
					!WebConfigSettings.DisableSearchFeatureFilters && displaySettings.ShowFeatureFilter && featureList.Count > 1
				),
				new DateRange(
					Resource.SearchDateFilterPreamble,
					dateStart.Date > DateTime.MinValue.Date ? dateStart.ToShortDateString() : string.Empty,
					Resource.and,
					dateEnd.Date < DateTime.MaxValue.Date ? dateEnd.ToShortDateString() : string.Empty,
					!WebConfigSettings.DisableSearchFeatureFilters && displaySettings.ShowModifiedDateFilters
				)
			)
		);
	}
}


public record SearchInput(string Value);
public record SearchButton(string Text);
public record Controls(SearchInput SearchInput, SearchButton SearchButton);


public record Filters(
	Feature Features,
	DateRange DateRange
);


public record Feature(
	List<SelectListItem> Options,
	bool Show
);


public record DateRange(
	string Preamble,
	string DateStart,
	string AndText,
	string DateEnd,
	bool Show
);


public record ResultsMessage(
	string Preamble,
	int From,
	string ToText,
	int To,
	string ForText,
	int Total,
	string Query,
	float? Duration,
	string DurationText
);

public record Results(
	List<Item> Items,
	bool ShowResults,
	bool ShowNoResults,
	string NoResultsText
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


public record HelpLink(
	string Text,
	string Href,
	string CloseButtonText
);


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
	int? ItemId,
	Link Link,
	Content Content,
	Author Author,
	Date CreatedDate,
	Date ModifiedDate,
	Image? Image = null,
	string? Sku = null,
	string? FeatureId = null,
	string? FeatureName = null
);
