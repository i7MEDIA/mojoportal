using System.Collections;

namespace mojoPortal.SearchIndex;

public class IndexItemCollection : CollectionBase
{
	public int ItemCount { get; set; }

	public int PageIndex { get; set; }

	public long ExecutionTime { get; set; }

	public IndexItem this[int index] => (IndexItem)this.List[index];

	public IndexItemCollection() {}

	public void Add(IndexItem item) => this.List.Add(item);

	public void Remove(IndexItem item) => this.List.Remove(item);
}