/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2012-08-12
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
using System.Globalization;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
    public static class DBWebPartContent
    {
        public static int AddWebPart(
            Guid webPartId,
            Guid siteGuid,
            int siteId,
            string title,
            string description,
            string imageUrl,
            string className,
            string assemblyName,
            bool availableForMyPage,
            bool allowMultipleInstancesOnMyPage,
            bool availableForContentSystem)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];
            
            arParams[0] = new NpgsqlParameter(":webpartid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            arParams[1] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new NpgsqlParameter(":description", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new NpgsqlParameter(":imageurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageUrl;

            arParams[5] = new NpgsqlParameter(":classname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = className;

            arParams[6] = new NpgsqlParameter(":assemblyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = assemblyName;

            arParams[7] = new NpgsqlParameter(":availableformypage", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = availableForMyPage;

            arParams[8] = new NpgsqlParameter(":allowmultipleinstancesonmypage", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowMultipleInstancesOnMyPage;

            arParams[9] = new NpgsqlParameter(":availableforcontentsystem", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = availableForContentSystem;

            arParams[10] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = siteGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_insert(:webpartid,:siteid,:title,:description,:imageurl,:classname,:assemblyname,:availableformypage,:allowmultipleinstancesonmypage,:availableforcontentsystem,:siteguid)",
                arParams);

            return rowsAffected;

        }




        public static bool UpdateWebPart(
            Guid webPartId,
            int siteId,
            string title,
            string description,
            string imageUrl,
            string className,
            string assemblyName,
            bool availableForMyPage,
            bool allowMultipleInstancesOnMyPage,
            bool availableForContentSystem)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[10];
            
            arParams[0] = new NpgsqlParameter(":webpartid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            arParams[1] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new NpgsqlParameter(":description", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new NpgsqlParameter(":imageurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageUrl;

            arParams[5] = new NpgsqlParameter(":classname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = className;

            arParams[6] = new NpgsqlParameter(":assemblyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = assemblyName;

            arParams[7] = new NpgsqlParameter(":availableformypage", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = availableForMyPage;

            arParams[8] = new NpgsqlParameter(":allowmultipleinstancesonmypage", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowMultipleInstancesOnMyPage;

            arParams[9] = new NpgsqlParameter(":availableforcontentsystem", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = availableForContentSystem;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_update(:webpartid,:siteid,:title,:description,:imageurl,:classname,:assemblyname,:availableformypage,:allowmultipleinstancesonmypage,:availableforcontentsystem)",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateCountOfUseOnMyPage(Guid webPartId, int increment)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":webpartid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            arParams[1] = new NpgsqlParameter(":increment", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = increment;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_updatecountofuseonmypage(:webpartid,:increment)",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteWebPart(Guid webPartId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":webpartid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_delete(:webpartid)",
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetWebPart(Guid webPartId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":webpartid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_select_onev2(:webpartid)",
                arParams);

        }

        public static IDataReader SelectBySite(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_select_bysite(:siteid)",
                arParams);

        }

        public static DataTable SelectPage(
            int siteId,
            int pageNumber,
            int pageSize,
            bool sortByClassName,
            bool sortByAssemblyName)
        {

            int totalRows = Count(siteId);
            int totalPages = totalRows / pageSize;
            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder = 0;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }


            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":sortbyclassname", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortByClassName;

            arParams[4] = new NpgsqlParameter(":sortbyassemblyname", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = sortByAssemblyName;

            DataTable dt = new DataTable();
            dt.Columns.Add("WebPartID", typeof(String));
            dt.Columns.Add("Title", typeof(String));
            dt.Columns.Add("Description", typeof(String));
            dt.Columns.Add("ImageUrl", typeof(String));
            dt.Columns.Add("ClassName", typeof(String));
            dt.Columns.Add("AssemblyName", typeof(String));
            dt.Columns.Add("AvailableForMyPage", typeof(bool));
            dt.Columns.Add("AllowMultipleInstancesOnMyPage", typeof(bool));
            dt.Columns.Add("AvailableForContentSystem", typeof(bool));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_selectpage(:siteid,:pagenumber,:pagesize,:sortbyclassname,:sortbyassemblyname)",
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["WebPartID"] = reader["WebPartID"].ToString();
                    row["Title"] = reader["Title"];
                    row["Description"] = reader["Description"];
                    row["ImageUrl"] = reader["ImageUrl"];
                    row["ClassName"] = reader["ClassName"];
                    row["AssemblyName"] = reader["AssemblyName"];
                    row["AvailableForMyPage"] = reader["AvailableForMyPage"];
                    row["AllowMultipleInstancesOnMyPage"] = reader["AllowMultipleInstancesOnMyPage"];
                    row["AvailableForContentSystem"] = reader["AvailableForContentSystem"];
                    row["TotalPages"] = totalPages;

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }



        public static int Count(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;
            
            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_count(:siteid)", arParams));

            return count;

        }

        public static bool Exists(Int32 siteId, String className, String assemblyName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":classname", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = className;

            arParams[2] = new NpgsqlParameter(":assemblyname", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = assemblyName;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_webparts_exists(:siteid,:classname,:assemblyname)",
                arParams));

            return (count > 0);

        }

        public static IDataReader GetWebPartsForMyPage(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.moduleid, ");
            sqlCommand.Append("m.siteid, ");
            sqlCommand.Append("m.moduledefid, ");
            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.allowmultipleinstancesonmypage, ");
            sqlCommand.Append("m.countofuseonmypage , ");
            sqlCommand.Append("m.icon As moduleicon, ");
            sqlCommand.Append("md.icon As featureicon, ");
            sqlCommand.Append("md.featurename, ");
            sqlCommand.Append("md.resourcefile, ");
            sqlCommand.Append("false As isassembly, ");
            sqlCommand.Append("'' As webpartid ");

            sqlCommand.Append("FROM	mp_modules m ");
            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

            sqlCommand.Append("WHERE m.siteid = " + siteId.ToString(CultureInfo.InvariantCulture) 
                + " AND m.availableformypage = true ");

            sqlCommand.Append(" UNION ");

            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("-1 As moduleid, ");
            sqlCommand.Append("w.siteid, ");
            sqlCommand.Append("0 As moduledefid, ");
            sqlCommand.Append("w.title As moduletitle, ");
            sqlCommand.Append("w.allowmultipleinstancesonmypage, ");
            sqlCommand.Append("w.countofuseonmypage , ");
            sqlCommand.Append("w.imageurl As moduleicon, ");
            sqlCommand.Append("w.imageUrl As featureicon, ");
            sqlCommand.Append("w.description As featurename, ");
            sqlCommand.Append("'Resource' As resourcefile, ");
            sqlCommand.Append("true As isassembly, ");
            sqlCommand.Append("w.webpartid ");

            sqlCommand.Append("FROM	mp_webparts w ");

            sqlCommand.Append("WHERE w.siteid = " + siteId.ToString(CultureInfo.InvariantCulture) 
                + " AND w.availableformypage = true ");

            sqlCommand.Append("ORDER BY moduletitle ");

            sqlCommand.Append("; ");


            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            //arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = siteID;


            return NpgsqlHelper.ExecuteReader(
                 ConnectionString.GetReadConnectionString(),
                 CommandType.Text,
                 sqlCommand.ToString());

        }

        public static IDataReader GetMostPopular(int siteId, int numberToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.moduleid, ");
            sqlCommand.Append("m.siteid, ");
            sqlCommand.Append("m.moduledefid, ");
            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.allowmultipleinstancesonmypage, ");
            sqlCommand.Append("m.countofuseonmypage , ");
            sqlCommand.Append("m.icon As moduleicon, ");
            sqlCommand.Append("md.icon As featureicon, ");
            sqlCommand.Append("md.featurename, ");
            sqlCommand.Append("md.resourcefile, ");
            sqlCommand.Append("false As isassembly, ");
            sqlCommand.Append("'' As webpartid ");

            sqlCommand.Append("FROM	mp_modules m ");
            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

            sqlCommand.Append("WHERE m.siteid = " + siteId.ToString(CultureInfo.InvariantCulture) 
                + " AND m.availableformypage = true ");

            sqlCommand.Append(" UNION ");

            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("-1 As moduleid, ");
            sqlCommand.Append("w.siteid, ");
            sqlCommand.Append("0 As moduledefid, ");
            sqlCommand.Append("w.title As moduletitle, ");
            sqlCommand.Append("w.allowmultipleinstancesonmypage, ");
            sqlCommand.Append("w.countofuseonmypage , ");
            sqlCommand.Append("w.imageurl As moduleicon, ");
            sqlCommand.Append("w.imageUrl As featureicon, ");
            sqlCommand.Append("w.description As featurename, ");
            sqlCommand.Append("'Resource' As resourcefile, ");
            sqlCommand.Append("true As isassembly, ");
            sqlCommand.Append("w.webpartid ");

            sqlCommand.Append("FROM	mp_webparts w ");

            sqlCommand.Append("WHERE w.siteid = " + siteId.ToString(CultureInfo.InvariantCulture) 
                + " AND w.availableformypage = true ");

            sqlCommand.Append("ORDER BY countofuseonmypage DESC, moduletitle ");

            sqlCommand.Append("LIMIT " + numberToGet.ToString(CultureInfo.InvariantCulture) + "; ");


            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            //arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = siteID;


            return NpgsqlHelper.ExecuteReader(
                 ConnectionString.GetReadConnectionString(),
                 CommandType.Text,
                 sqlCommand.ToString());

        }

    }
}
