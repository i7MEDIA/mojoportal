/// Author:					Joe Audette
/// Created:				2007-02-28
/// Last Modified:			2009-02-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents a file associated with a download product
    /// </summary>
    public class ProductFile
    {

        #region Constructors

        //public ProductFile()
        //{ }


        public ProductFile(Guid productGuid)
        {
            this.productGuid = productGuid;
            GetProductFile(productGuid);
        }

        #endregion

        #region Private Properties

        private Guid productGuid = Guid.Empty;
        private string fileName;
        private byte[] fileImage = null;
        private string serverFileName;
        private int byteLength = 0;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public byte[] FileImage
        {
            get { return fileImage; }
            set { fileImage = value; }
        }
        public string ServerFileName
        {
            get { return serverFileName; }
            set { serverFileName = value; }
        }
        public int ByteLength
        {
            get { return byteLength; }
            set { byteLength = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        #endregion

        #region Private Methods

        private void GetProductFile(Guid productGuid) 
		{
            using (IDataReader reader = DBProductFile.Get(productGuid))
            {
                if (reader.Read())
                {
                    this.productGuid = new Guid(reader["ProductGuid"].ToString());
                    this.fileName = reader["FileName"].ToString();
                    //this.fileImage = Byte[]
                    this.serverFileName = reader["ServerFileName"].ToString();
                    this.byteLength = Convert.ToInt32(reader["ByteLength"]);
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());

                }

            }
		
		}

        private bool Create()
        {
            //Guid newID = Guid.NewGuid();

            //this.productGuid = newID;

            int rowsAffected = DBProductFile.Add(
                this.productGuid,
                this.fileName,
                this.fileImage,
                this.serverFileName,
                this.byteLength,
                this.created,
                this.createdBy);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBProductFile.Update(
                this.productGuid,
                this.fileName,
                this.fileImage,
                this.serverFileName,
                this.byteLength,
                this.created,
                this.createdBy);

        }

        private bool Exists()
        {
            bool result = false;
            using (IDataReader reader = DBProductFile.Get(productGuid))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region Public Methods


        public bool Save()
        {
            if (Exists())
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

        public static bool Delete(Guid productGuid)
        {
            return DBProductFile.Delete(productGuid);
        }


        

        #endregion


    }

}
