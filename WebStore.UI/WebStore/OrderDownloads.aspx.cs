/// Author:					Joe Audette
/// Created:				2007-02-15
/// Last Modified:		    2008-03-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Web;

namespace WebStore.UI
{
    public partial class OrderDownloadsPage : NonCmsBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {

        }

        private void LoadSettings()
        {
            AddClassToBody("webstoreOrderdownloads");

        }


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressPageMenu();
        }

        #endregion

    }
}
