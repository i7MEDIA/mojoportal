// Author:					
// Created:				    2007-02-17
// Last Modified:			2009-02-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a language
    /// </summary>
    public class Language
    {

        #region Constructors

        public Language()
        { }


        public Language(Guid guid)
        {
            GetLanguage(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private string name;
        private string code;
        private int sort;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        #endregion

        #region Private Methods

        private void GetLanguage(Guid guid)
        {
            using (IDataReader reader = DBLanguage.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.name = reader["Name"].ToString();
                    this.code = reader["Code"].ToString();
                    this.sort = Convert.ToInt32(reader["Sort"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBLanguage.Create(
                this.guid,
                this.name,
                this.code,
                this.sort);

            return (rowsAffected > 0);

        }

        private bool Update()
        {

            return DBLanguage.Update(
                this.guid,
                this.name,
                this.code,
                this.sort);

        }


        #endregion

        #region Public Methods


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

        public static bool Delete(Guid guid)
        {
            return DBLanguage.Delete(guid);
        }

        public static IDataReader GetAll()
        {
            return DBLanguage.GetAll();
        }

        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            
            return DBLanguage.GetPage(pageNumber, pageSize, out totalPages);

            
        }



        #endregion

    }

}
