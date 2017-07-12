// Author:					
// Created:					2009-02-22
// Last Modified:			2012-08-30
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

    public class EmailTemplate
    {

        #region Constructors

        public EmailTemplate()
        { }


        public EmailTemplate(
            Guid guid)
        {
            GetEmailTemplate(
                guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid specialGuid1 = Guid.Empty;
        private Guid specialGuid2 = Guid.Empty;
        private string name = string.Empty;
        private string subject = string.Empty;
        private string textBody = string.Empty;
        private string htmlBody = string.Empty;
        private bool hasHtml;
        private bool isEditable;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        private Guid lastModBy = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public Guid SpecialGuid1
        {
            get { return specialGuid1; }
            set { specialGuid1 = value; }
        }
        public Guid SpecialGuid2
        {
            get { return specialGuid2; }
            set { specialGuid2 = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string TextBody
        {
            get { return textBody; }
            set { textBody = value; }
        }
        public string HtmlBody
        {
            get { return htmlBody; }
            set { htmlBody = value; }
        }
        public bool HasHtml
        {
            get { return hasHtml; }
            set { hasHtml = value; }
        }
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }
        public Guid LastModBy
        {
            get { return lastModBy; }
            set { lastModBy = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of EmailTemplate.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetEmailTemplate(Guid guid)
        {
            using (IDataReader reader = DBEmailTemplate.GetOne(guid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.specialGuid1 = new Guid(reader["SpecialGuid1"].ToString());
                this.specialGuid2 = new Guid(reader["SpecialGuid2"].ToString());
                this.name = reader["Name"].ToString();
                this.subject = reader["Subject"].ToString();
                this.textBody = reader["TextBody"].ToString();
                this.htmlBody = reader["HtmlBody"].ToString();
                this.hasHtml = Convert.ToBoolean(reader["HasHtml"]);
                this.isEditable = Convert.ToBoolean(reader["IsEditable"]);
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                this.lastModBy = new Guid(reader["LastModBy"].ToString());

            }
            
        }

        /// <summary>
        /// Persists a new instance of EmailTemplate. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBEmailTemplate.Create(
                this.guid,
                this.siteGuid,
                this.featureGuid,
                this.moduleGuid,
                this.specialGuid1,
                this.specialGuid2,
                this.name,
                this.subject,
                this.textBody,
                this.htmlBody,
                this.hasHtml,
                this.isEditable,
                this.createdUtc,
                this.lastModBy);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of EmailTemplate. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBEmailTemplate.Update(
                this.guid,
                this.name,
                this.subject,
                this.textBody,
                this.htmlBody,
                this.hasHtml,
                this.isEditable,
                this.lastModUtc,
                this.lastModBy);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of EmailTemplate. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.guid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        public static bool Exists(Guid moduleGuid, string name)
        {
            int count = DBEmailTemplate.GetCountByModuleAndName(moduleGuid, name);
            return (count > 0);
        }

        public static bool Exists(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2, string name)
        {
            int count = DBEmailTemplate.GetCountByModuleSpecialAndName(moduleGuid, specialGuid1, specialGuid2, name);
            return (count > 0);
        }

        public static int GetCount(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            return DBEmailTemplate.GetCount(siteGuid, featureGuid, moduleGuid, specialGuid1, specialGuid2);
        }

        /// <summary>
        /// Deletes an instance of EmailTemplate. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBEmailTemplate.Delete(guid);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBEmailTemplate.DeleteBySite(siteGuid);
        }

        public static bool DeleteByFeature(Guid featureGuid)
        {
            return DBEmailTemplate.DeleteByFeature(featureGuid);
        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBEmailTemplate.DeleteByModule(moduleGuid);
        }

        public static bool DeleteBySpecial1(Guid specialGuid1)
        {
            return DBEmailTemplate.DeleteBySpecial1(specialGuid1);
        }

        public static bool DeleteBySpecial2(Guid specialGuid2)
        {
            return DBEmailTemplate.DeleteBySpecial2(specialGuid2);
        }

        public static IDataReader GetPageByFeature(
            Guid siteGuid,
            Guid featureGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBEmailTemplate.GetPageByFeature(
                siteGuid,
                featureGuid,
                pageNumber,
                pageSize,
                out totalPages);

        }
       

        private static List<EmailTemplate> LoadListFromReader(IDataReader reader)
        {
            List<EmailTemplate> emailTemplateList = new List<EmailTemplate>();
            try
            {
                while (reader.Read())
                {
                    EmailTemplate emailTemplate = new EmailTemplate();
                    emailTemplate.guid = new Guid(reader["Guid"].ToString());
                    emailTemplate.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    emailTemplate.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                    emailTemplate.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    emailTemplate.specialGuid1 = new Guid(reader["SpecialGuid1"].ToString());
                    emailTemplate.specialGuid2 = new Guid(reader["SpecialGuid2"].ToString());
                    emailTemplate.name = reader["Name"].ToString();
                    emailTemplate.subject = reader["Subject"].ToString();
                    emailTemplate.textBody = reader["TextBody"].ToString();
                    emailTemplate.htmlBody = reader["HtmlBody"].ToString();
                    emailTemplate.hasHtml = Convert.ToBoolean(reader["HasHtml"]);
                    emailTemplate.isEditable = Convert.ToBoolean(reader["IsEditable"]);
                    emailTemplate.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    emailTemplate.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    emailTemplate.lastModBy = new Guid(reader["LastModBy"].ToString());
                    emailTemplateList.Add(emailTemplate);

                }
            }
            finally
            {
                reader.Close();
            }

            return emailTemplateList;

        }

        public static IDataReader Get(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            return DBEmailTemplate.Get(siteGuid, featureGuid, moduleGuid, specialGuid1, specialGuid2);
        }

        public static IDataReader GetByModule(Guid moduleGuid)
        {
            return DBEmailTemplate.GetByModule(moduleGuid);
        }

        public static IDataReader GetByModule(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            return DBEmailTemplate.GetByModule(moduleGuid, specialGuid1, specialGuid2);
        }

        public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
        {
            return DBEmailTemplate.GetByFeature(siteGuid, featureGuid);
        }
        


        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of EmailTemplate.
        /// </summary>
        public static int CompareByName(EmailTemplate emailTemplate1, EmailTemplate emailTemplate2)
        {
            return emailTemplate1.Name.CompareTo(emailTemplate2.Name);
        }
        /// <summary>
        /// Compares 2 instances of EmailTemplate.
        /// </summary>
        public static int CompareBySubject(EmailTemplate emailTemplate1, EmailTemplate emailTemplate2)
        {
            return emailTemplate1.Subject.CompareTo(emailTemplate2.Subject);
        }
        /// <summary>
        /// Compares 2 instances of EmailTemplate.
        /// </summary>
        public static int CompareByTextBody(EmailTemplate emailTemplate1, EmailTemplate emailTemplate2)
        {
            return emailTemplate1.TextBody.CompareTo(emailTemplate2.TextBody);
        }
        /// <summary>
        /// Compares 2 instances of EmailTemplate.
        /// </summary>
        public static int CompareByHtmlBody(EmailTemplate emailTemplate1, EmailTemplate emailTemplate2)
        {
            return emailTemplate1.HtmlBody.CompareTo(emailTemplate2.HtmlBody);
        }
        /// <summary>
        /// Compares 2 instances of EmailTemplate.
        /// </summary>
        public static int CompareByCreatedUtc(EmailTemplate emailTemplate1, EmailTemplate emailTemplate2)
        {
            return emailTemplate1.CreatedUtc.CompareTo(emailTemplate2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of EmailTemplate.
        /// </summary>
        public static int CompareByLastModUtc(EmailTemplate emailTemplate1, EmailTemplate emailTemplate2)
        {
            return emailTemplate1.LastModUtc.CompareTo(emailTemplate2.LastModUtc);
        }

        #endregion


    }

}
