using System;

namespace mojoPortal.Web.UI
{
    public interface IUpdateCommentStats
    {
        void UpdateCommentStats(Guid contentGuid, int commentCount);
    }
}
