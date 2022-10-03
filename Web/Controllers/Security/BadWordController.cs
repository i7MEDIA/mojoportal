using System.Web.Http;

namespace mojoPortal.Web.Controllers
{
	public class ReturnObject
	{
		public bool Found { get; set; } = false;
	}

	public class RequestObject
	{
		public string StringToCheck { get; set; }
	}
	public class BadWordController : ApiController
    {
		[HttpPost]
		[Route("BadWord/CheckString")]
		public bool CheckString([FromBody] RequestObject r)
		{
			//if (r.StringToCheck.ContainsBadWords())
			//{
			//	return new ReturnObject
			//	{
			//		Found = true
			//	};
			//}

			//return new ReturnObject();
			return r.StringToCheck.ContainsBadWords();
		}
	}
}
