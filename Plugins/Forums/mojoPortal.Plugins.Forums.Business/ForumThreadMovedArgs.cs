namespace mojoPortal.Business
{
	public class ForumThreadMovedArgs
	{
		private int forumID;
		private int originalForumID;

		public int ForumId
		{
			get { return forumID; }
			set { forumID = value; }
		}

		public int OriginalForumId
		{
			get { return originalForumID; }
			set { originalForumID = value; }
		}

	}
}
