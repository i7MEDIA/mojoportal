// Author:					
// Created:					2009-12-24
// Last Modified:			2009-12-24
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

    public class SavedQueryRespository
    {

        public SavedQueryRespository()
        { }

        /// <summary>
        /// Persists a new instance of SavedQuery.
        /// </summary>
        /// <returns></returns>
        public void Save(SavedQuery savedQuery)
        {
            if (savedQuery == null) { return; }

            if (savedQuery.Id == Guid.Empty)
            {
                savedQuery.Id = Guid.NewGuid();

                if (savedQuery.CreatedBy == Guid.Empty) { savedQuery.CreatedBy = savedQuery.LastModBy; }

                DBSavedQuery.Create(
                    savedQuery.Id,
                    savedQuery.Name,
                    savedQuery.Statement,
                    savedQuery.CreatedUtc,
                    savedQuery.CreatedBy);
            }
            else
            {
                DBSavedQuery.Update(
                    savedQuery.Id,
                    savedQuery.Statement,
                    savedQuery.LastModUtc,
                    savedQuery.LastModBy);

            }
        }


        /// <param name="id"> id </param>
        public SavedQuery Fetch(Guid id)
        {
            using (IDataReader reader = DBSavedQuery.GetOne(id))
            {
                if (reader.Read())
                {
                    SavedQuery savedQuery = new SavedQuery();
                    savedQuery.Id = new Guid(reader["Id"].ToString());
                    savedQuery.Name = reader["Name"].ToString();
                    savedQuery.Statement = reader["Statement"].ToString();
                    savedQuery.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    savedQuery.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    savedQuery.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    savedQuery.LastModBy = new Guid(reader["LastModBy"].ToString());

                    return savedQuery;

                }
            }

            return null;
        }

        public SavedQuery Fetch(string name)
        {
            using (IDataReader reader = DBSavedQuery.GetOne(name))
            {
                if (reader.Read())
                {
                    SavedQuery savedQuery = new SavedQuery();
                    savedQuery.Id = new Guid(reader["Id"].ToString());
                    savedQuery.Name = reader["Name"].ToString();
                    savedQuery.Statement = reader["Statement"].ToString();
                    savedQuery.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    savedQuery.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    savedQuery.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    savedQuery.LastModBy = new Guid(reader["LastModBy"].ToString());

                    return savedQuery;

                }
            }

            return null;
        }


        /// <summary>
        /// Deletes an instance of SavedQuery. Returns true on success.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public bool Delete(Guid id)
        {
            return DBSavedQuery.Delete(id);
        }


        


        /// <summary>
        /// Gets an IList with all instances of SavedQuery.
        /// </summary>
        public List<SavedQuery> GetAll()
        {
            IDataReader reader = DBSavedQuery.GetAll();
            return LoadListFromReader(reader);

        }

        


        private List<SavedQuery> LoadListFromReader(IDataReader reader)
        {
            List<SavedQuery> savedQueryList = new List<SavedQuery>();

            try
            {
                while (reader.Read())
                {
                    SavedQuery savedQuery = new SavedQuery();
                    savedQuery.Id = new Guid(reader["Id"].ToString());
                    savedQuery.Name = reader["Name"].ToString();
                    savedQuery.Statement = reader["Statement"].ToString();
                    savedQuery.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    savedQuery.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    savedQuery.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    savedQuery.LastModBy = new Guid(reader["LastModBy"].ToString());
                    savedQueryList.Add(savedQuery);

                }
            }
            finally
            {
                reader.Close();
            }

            return savedQueryList;

        }


    }

}
