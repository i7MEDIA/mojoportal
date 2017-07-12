// Author:					
// Created:					2012-08-11
// Last Modified:			2012-08-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class CommentRepository
    {

        public CommentRepository()
        { }

        /// <summary>
        /// Persists a new instance of Comment.
        /// </summary>
        /// <returns></returns>
        public void Save(Comment comment)
        {
            if (comment == null) { return; }

            if (comment.Guid == Guid.Empty)
            {
                comment.Guid = Guid.NewGuid();

                DBComments.Create(
                    comment.Guid,
                    comment.ParentGuid,
                    comment.SiteGuid,
                    comment.FeatureGuid,
                    comment.ModuleGuid,
                    comment.ContentGuid,
                    comment.UserGuid,
                    comment.Title,
                    comment.UserComment,
                    comment.UserName,
                    comment.UserEmail,
                    comment.UserUrl,
                    comment.UserIp,
                    comment.CreatedUtc,
                    comment.ModerationStatus,
                    comment.ModeratedBy,
                    comment.ModerationReason);
            }
            else
            {
                DBComments.Update(
                    comment.Guid,
                    comment.UserGuid,
                    comment.Title,
                    comment.UserComment,
                    comment.UserName,
                    comment.UserEmail,
                    comment.UserUrl,
                    comment.UserIp,
                    comment.LastModUtc,
                    comment.ModerationStatus,
                    comment.ModeratedBy,
                    comment.ModerationReason);

            }
        }


        /// <param name="guid"> guid </param>
        public Comment Fetch(Guid guid)
        {
            using (IDataReader reader = DBComments.GetOne(guid))
            {
                if (reader.Read())
                {
                    Comment comment = new Comment();
                    comment.Guid = new Guid(reader["Guid"].ToString());
                    comment.ParentGuid = new Guid(reader["ParentGuid"].ToString());
                    comment.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    comment.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
                    comment.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
                    comment.ContentGuid = new Guid(reader["ContentGuid"].ToString());
                    comment.UserGuid = new Guid(reader["UserGuid"].ToString());
                    comment.Title = reader["Title"].ToString();
                    comment.UserComment = reader["UserComment"].ToString();
                    comment.UserName = reader["UserName"].ToString();
                    comment.UserEmail = reader["UserEmail"].ToString();
                    comment.UserUrl = reader["UserUrl"].ToString();
                    comment.UserIp = reader["UserIp"].ToString();
                    comment.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    comment.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    comment.ModerationStatus = Convert.ToByte(reader["ModerationStatus"]);
                    comment.ModeratedBy = new Guid(reader["ModeratedBy"].ToString());
                    comment.ModerationReason = reader["ModerationReason"].ToString();

                    //external properties not stored in mp_Comments
                    comment.UserId = Convert.ToInt32(reader["UserID"]);
                    comment.PostAuthor = reader["PostAuthor"].ToString();

                    if (comment.PostAuthor.Length == 0)
                    {
                        comment.PostAuthor = comment.UserName;
                    }

                    comment.AuthorEmail = reader["AuthorEmail"].ToString();
                    if (comment.AuthorEmail.Length == 0)
                    {
                        comment.AuthorEmail = comment.UserEmail;
                    }

                    comment.UserRevenue = Convert.ToDecimal(reader["UserRevenue"]);
                    comment.Trusted = Convert.ToBoolean(reader["Trusted"]);
                    comment.PostAuthorAvatar = reader["PostAuthorAvatar"].ToString();
                    comment.PostAuthorWebSiteUrl = reader["PostAuthorWebSiteUrl"].ToString();
                    if (comment.PostAuthorWebSiteUrl.Length == 0)
                    {
                        comment.PostAuthorWebSiteUrl = comment.UserUrl;
                    }

                    return comment;

                }
            }

            return null;
        }


        /// <summary>
        /// Deletes an instance of Comment. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public bool Delete(Guid guid)
        {
            return DBComments.Delete(guid);
        }

        public bool DeleteByContent(Guid contentGuid)
        {
            return DBComments.DeleteByContent(contentGuid);
        }

        public bool DeleteByParent(Guid parentGuid)
        {
            return DBComments.DeleteByParent(parentGuid);
        }

        public bool DeleteBySite(Guid siteGuid)
        {
            return DBComments.DeleteBySite(siteGuid);
        }

        public bool DeleteByFeature(Guid featureGuid)
        {
            return DBComments.DeleteBySite(featureGuid);
        }

        public bool DeleteByModule(Guid moduleGuid)
        {
            return DBComments.DeleteByModule(moduleGuid);
        }


        /// <summary>
        /// Gets a count of Comment. 
        /// </summary>
        public int GetCount(Guid contentGuid, int moderationStatus)
        {
            return DBComments.GetCount(contentGuid, moderationStatus);
        }

        public int GetCountByModule(Guid moduleGuid, int moderationStatus)
        {
            return DBComments.GetCountByModule(moduleGuid, moderationStatus);
        }

        public int GetCountBySite(Guid siteGuid)
        {
            return DBComments.GetCountBySite(siteGuid);
        }

        public List<Comment> GetByContentAsc(Guid contentGuid)
        {
            IDataReader reader = DBComments.GetByContentAsc(contentGuid);
            return LoadListFromReader(reader);
        }

        public List<Comment> GetByContentDesc(Guid contentGuid)
        {
            IDataReader reader = DBComments.GetByContentDesc(contentGuid);
            return LoadListFromReader(reader);
        }

        public List<Comment> GetByParentAsc(Guid parentGuid)
        {
            IDataReader reader = DBComments.GetByParentAsc(parentGuid);
            return LoadListFromReader(reader);
        }

        public List<Comment> GetByParentDesc(Guid parentGuid)
        {
            IDataReader reader = DBComments.GetByParentDesc(parentGuid);
            return LoadListFromReader(reader);
        }
        

        ///// <summary>
        ///// Gets an IList with page of instances of Comment.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public List<Comment> GetPage(int pageNumber, int pageSize, out int totalPages)
        //{
        //    totalPages = 1;
        //    IDataReader reader = DBComment.GetPage(pageNumber, pageSize, out totalPages);
        //    return LoadListFromReader(reader);
        //}


        private List<Comment> LoadListFromReader(IDataReader reader)
        {
            List<Comment> commentList = new List<Comment>();

            try
            {
                while (reader.Read())
                {
                    Comment comment = new Comment();
                    comment.Guid = new Guid(reader["Guid"].ToString());
                    comment.ParentGuid = new Guid(reader["ParentGuid"].ToString());
                    comment.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    comment.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
                    comment.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
                    comment.ContentGuid = new Guid(reader["ContentGuid"].ToString());
                    comment.UserGuid = new Guid(reader["UserGuid"].ToString());
                    comment.Title = reader["Title"].ToString();
                    comment.UserComment = reader["UserComment"].ToString();
                    comment.UserName = reader["UserName"].ToString();
                    comment.UserEmail = reader["UserEmail"].ToString();
                    comment.UserUrl = reader["UserUrl"].ToString();
                    comment.UserIp = reader["UserIp"].ToString();
                    comment.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    comment.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    comment.ModerationStatus = Convert.ToByte(reader["ModerationStatus"]);
                    comment.ModeratedBy = new Guid(reader["ModeratedBy"].ToString());
                    comment.ModerationReason = reader["ModerationReason"].ToString();

                    //external properties not stored in mp_Comments
                    comment.UserId = Convert.ToInt32(reader["UserID"]);
                    comment.PostAuthor = reader["PostAuthor"].ToString();

                    if (comment.PostAuthor.Length == 0)
                    {
                        comment.PostAuthor = comment.UserName;
                    }

                    comment.AuthorEmail = reader["AuthorEmail"].ToString();

                    if (comment.AuthorEmail.Length == 0)
                    {
                        comment.AuthorEmail = comment.UserEmail;
                    }

                    comment.UserRevenue = Convert.ToDecimal(reader["UserRevenue"]);
                    comment.Trusted = Convert.ToBoolean(reader["Trusted"]);
                    comment.PostAuthorAvatar = reader["PostAuthorAvatar"].ToString();
                    comment.PostAuthorWebSiteUrl = reader["PostAuthorWebSiteUrl"].ToString();
                    if (comment.PostAuthorWebSiteUrl.Length == 0)
                    {
                        comment.PostAuthorWebSiteUrl = comment.UserUrl;
                    }

                    commentList.Add(comment);

                }
            }
            finally
            {
                reader.Close();
            }

            return commentList;

        }


    }

}
