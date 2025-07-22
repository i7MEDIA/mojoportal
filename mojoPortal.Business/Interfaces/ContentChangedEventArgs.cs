namespace mojoPortal.Business;

public delegate void ContentChangedEventHandler(object sender, ContentChangedEventArgs e);

public class ContentChangedEventArgs : EventArgs
{
	public bool IsDeleted { get; set; } = false;
}
