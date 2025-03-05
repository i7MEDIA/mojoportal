using System;

namespace mojoPortal.SearchIndex;

public class IndexTerm : IComparable
{
	public string Term { get; set; } = string.Empty;

	public int Frequency { get; set; } = 1;

	public int CompareTo(object obj)
	{
		if (obj is not IndexTerm i)
		{
			return -1;
		}

		// sort descending on frequency
		return i.Frequency > this.Frequency ? 1 : -1;
	}
}