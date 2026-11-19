using AngleSharp.Dom;
using mojoPortal.Data;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace mojoPortal.Business;

/// <summary>
/// Represents a State within a Country
/// </summary>
public class GeoZone
{
	#region Constructors

	public GeoZone()
	{ }


	public GeoZone(Guid guid)
	{
		GetGeoZone(guid);
	}

	#endregion


	#region Public Properties

	public Guid Guid { get; set; } = Guid.Empty;
	public Guid CountryGuid { get; set; } = Guid.Empty;
	public string Name { get; set; }
	public string Code { get; set; }

	#endregion


	#region Private Methods

	private void GetGeoZone(Guid guid)
	{
		using IDataReader reader = DBGeoZone.GetOne(guid);

		LoadFromReader(this, reader);
	}


	private static void LoadFromReader(GeoZone geoZone, IDataReader reader)
	{
		if (reader.Read())
		{
			geoZone.Guid = new Guid(reader["Guid"].ToString());
			geoZone.CountryGuid = new Guid(reader["CountryGuid"].ToString());
			geoZone.Name = reader["Name"].ToString();
			geoZone.Code = reader["Code"].ToString();
		}
	}


	private bool Create()
	{
		Guid = Guid.NewGuid();

		var rowsAffected = DBGeoZone.Create(
			Guid,
			CountryGuid,
			Name,
			Code);

		return rowsAffected > 0;
	}


	private bool Update()
	{
		return DBGeoZone.Update(
			Guid,
			CountryGuid,
			Name,
			Code);
	}


	#endregion


	#region Public Methods

	public bool Save()
	{
		if (Guid != null && Guid != Guid.Empty)
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
		return DBGeoZone.Delete(guid);
	}


	public static IDataReader GetByCountry(Guid countryGuid)
	{
		return DBGeoZone.GetByCountry(countryGuid);
	}


	public static GeoZone GetByCode(Guid countryGuid, string code)
	{
		var geoZone = new GeoZone();

		using (var reader = DBGeoZone.GetByCode(countryGuid, code))
		{
			LoadFromReader(geoZone, reader);
		}

		if (geoZone.Guid == Guid.Empty)
		{
			return null;
		}

		return geoZone;
	}


	public static IDataReader GetListByName(string name) => DBGeoZone.GetByName(name);

	public static DataTable GetPage(
		Guid countryGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		var dataTable = new DataTable();

		dataTable.Columns.Add("Guid", typeof(Guid));
		dataTable.Columns.Add("CountryGuid", typeof(Guid));
		dataTable.Columns.Add("CountryName", typeof(string));
		dataTable.Columns.Add("Name", typeof(string));
		dataTable.Columns.Add("Code", typeof(string));

		using var reader = DBGeoZone.GetPage(countryGuid, pageNumber, pageSize, out totalPages);

		while (reader.Read())
		{
			DataRow row = dataTable.NewRow();
			row["Guid"] = new Guid(reader["Guid"].ToString());
			row["CountryGuid"] = reader["CountryGuid"];
			row["CountryName"] = reader["CountryName"];
			row["Name"] = reader["Name"];
			row["Code"] = reader["Code"];
			dataTable.Rows.Add(row);
		}

		return dataTable;
	}

	#endregion
}
