using System;
using System.Web;

namespace mojoPortal.Web.Controls
{
	/// <summary>
    /// http://www.codeproject.com/aspnet/graphicalcontrols.asp
	/// Class that allows developer to simply set same images 
	/// to all Graphical controls in project
	/// </summary>
	public class GraphicalControlsSettings
	{
		#region Local variables

		private string c_checkImg,c_uncheckImg,c_checkImgOver,
			c_uncheckImgOver,c_checkImgDis,c_uncheckImgDis,c_threeState,c_threeStateOver;

		private string r_checkImg,r_uncheckImg,r_checkImgOver,
			r_uncheckImgOver,r_checkImgDis,r_uncheckImgDis;

		#endregion
		#region Checkbox

		/// <summary>
		/// Custom image url for indeterminate state of checkbox
		/// </summary>
		public string CheckboxIndeterminateImg
		{
			get { return c_threeState; }
			set { c_threeState=value; }
		}


		/// <summary>
		/// Custom image url for indeterminate state of active checkbox
		/// </summary>
		public string CheckboxIndeterminateOverImg
		{
			get { return c_threeStateOver; }
			set { c_threeStateOver=value; }
		}


		/// <summary>
		/// Custom image url for checked checkbox
		/// </summary>
		public string CheckboxCheckedImg
		{
			get { return c_checkImg; }
			set { c_checkImg=value; }
		}


		/// <summary>
		/// Custom image url for unchecked checkbox
		/// </summary>
		public string CheckboxUncheckedImg
		{
			get { return c_uncheckImg; }
			set { c_uncheckImg=value; }
		}


		/// <summary>
		/// Custom image url for checked checkbox
		/// </summary>
		public string CheckboxCheckedOverImg
		{
			get { return c_checkImgOver; }
			set { c_checkImgOver=value; }
		}


		/// <summary>
		/// Custom image url for unchecked checkbox
		/// </summary>
		public string CheckboxUncheckedOverImg
		{
			get { return c_uncheckImgOver; }
			set { c_uncheckImgOver=value; }
		}


		/// <summary>
		/// Custom image url for checked disabled checkbox
		/// </summary>
		public string CheckboxCheckedDisImg
		{
			get { return c_checkImgDis; }
			set { c_checkImgDis=value; }
		}


		/// <summary>
		/// Custom image url for unchecked disabled checkbox
		/// </summary>
		public string CheckboxUncheckedDisImg
		{
			get { return c_uncheckImgDis; }
			set { c_uncheckImgDis=value; }
		}

		#endregion
		#region Radio button

		/// <summary>
		/// Custom image url for checked radiobutton
		/// </summary>
		public string RadioCheckedImg
		{
			get { return r_checkImg; }
			set { r_checkImg=value; }
		}


		/// <summary>
		/// Custom image url for unchecked radiobutton
		/// </summary>
		public string RadioUncheckedImg
		{
			get { return r_uncheckImg; }
			set { r_uncheckImg=value; }
		}


		/// <summary>
		/// Custom image url for checked radiobutton
		/// </summary>
		public string RadioCheckedOverImg
		{
			get { return r_checkImgOver; }
			set { r_checkImgOver=value; }
		}


		/// <summary>
		/// Custom image url for unchecked radiobutton
		/// </summary>
		public string RadioUncheckedOverImg
		{
			get { return r_uncheckImgOver; }
			set { r_uncheckImgOver=value; }
		}


		/// <summary>
		/// Custom image url for checked disabled radiobutton
		/// </summary>
		public string RadioCheckedDisImg
		{
			get { return r_checkImgDis; }
			set { r_checkImgDis=value; }
		}


		/// <summary>
		/// Custom image url for unchecked disabled radiobutton
		/// </summary>
		public string RadioUncheckedDisImg
		{
			get { return r_uncheckImgDis; }
			set { r_uncheckImgDis=value; }
		}

		#endregion
		#region Public methods

		/// <summary>
		/// Save settings to Application
		/// </summary>
		public void Save()
		{
			HttpContext.Current.Application["GraphicalControlsSettings"]=this;
		}

		#endregion
		#region Internal

		/// <summary>
		/// Load saved global settings
		/// </summary>
		/// <returns>Object with settings or null </returns>
		internal static GraphicalControlsSettings Load()
		{
			return (GraphicalControlsSettings)
				HttpContext.Current.Application["GraphicalControlsSettings"];
		}

		#endregion
	}
}
