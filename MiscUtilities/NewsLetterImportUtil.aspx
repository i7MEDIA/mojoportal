<%@ Page Language="C#" ClassName="NewsletterImporter.aspx" Inherits="System.Web.UI.Page"   %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Web" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="mojoPortal.Web.Controls" %>
<%@ Import Namespace="mojoPortal.Web.UI" %>
<%@ Import Namespace="mojoPortal.Web.Editor" %>
<%@ Import Namespace="mojoPortal.Net" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="log4net" %>

<script runat="server">
	// Author:					Steve Railsback with modifications by 
	// Created:					2012-06-24
	// Last Modified:			2012-06-24
	// 
	// This is example code for importing email addresses into a mojoPortal newsletter
    // you may need to modify this code if your file is in a different format.

    /// <summary>
    /// 
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    private static readonly ILog log = LogManager.GetLogger(typeof(Page));
    private SubscriberRepository subscriptions = new SubscriberRepository();
    private SiteSettings siteSettings = null;
    private Guid letterInfoGuid = Guid.Empty;
    private LetterInfo letterInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUser.IsAdmin)
        {
            SiteUtils.RedirectToAccessDeniedPage(this);
            return;
        }
        
        LoadSettings();
        PopulateControls();
		
        
    }

    private void EnsureLetterInfo()
    {
        if (letterInfo != null) { return; }
        if (ddNewsLetter.SelectedIndex == -1) { return; }
        if (ddNewsLetter.SelectedValue.Length != 36) { return; }

        letterInfo = new LetterInfo(new Guid(ddNewsLetter.SelectedValue));
        letterInfoGuid = letterInfo.LetterInfoGuid;

    }
           

    /// <summary>
    /// Steve Railsback btnBatchProcessSubscribers_Click
    /// Batch update subscriber list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btnBatchProcessSubscribers_Click(object sender, EventArgs e)
    {
        EnsureLetterInfo();
        if (letterInfo == null) 
        {
            lblBatchProcessSubscribers.Text = "No newlsetter selected.";
            return; 
        }

        List<string[]> data = new List<string[]>();

        if (letterInfo.LetterInfoGuid != null)
        {
            // is there a file to process?
            if (fileBatchProcessSubscribers.HasFile)
            {
                // validate the file type
                if (fileBatchProcessSubscribers.FileName.EndsWith(".csv"))
                {
                    try
                    {
                        using (StreamReader streamReader = new StreamReader(fileBatchProcessSubscribers.PostedFile.InputStream))
                        {
                            // read csv into memory and build collection of valid emails to add or remove
                            string line;
                            string[] row;

                            while (((line = streamReader.ReadLine()) != string.Empty) && (line != null))
                            {
                                // validate the line, skip false
                                if (isValidSubscriberLine(line))
                                {
                                    row = line.Split(',');
                                    data.Add(row);
                                }
                            }
                        }


                        // ints to squirrel the total adds, removes and fails
                        int subscribersAdded = 0;
                        int subscribersRemoved = 0;
                        int subscribersFailed = 0;

                        // loop through data collection and do it!
                        for (int i = 0; i < data.Count; i++)
                        {
                            string result = DoSubscription(data[i], letterInfo);
                            if (result == "added")
                            {
                                subscribersAdded += 1;
                            }

                            if (result == "deleted")
                            {
                                subscribersRemoved += 1;
                            }

                            if (result == "failed")
                            {
                                subscribersFailed += 1;
                            }

                        }

                        lblBatchProcessSubscribers.Text = String.Format("Done! {0} Added, {1} Removed, {2} Failed (check the log)", subscribersAdded, subscribersRemoved, subscribersFailed);

                        LetterInfo.UpdateSubscriberCount(letterInfo.LetterInfoGuid);

                        
                    }
                    catch (Exception ex)
                    {
                        lblBatchProcessSubscribers.Text = "Error: " + ex.Message.ToString();
                    }

                }
                else
                {
                    lblBatchProcessSubscribers.Text = "Invalid file format.";
                }

            }
            else
            {
                lblBatchProcessSubscribers.Text = "You have not specified a file.";
            }

        }
        else
        {
            lblBatchProcessSubscribers.Text = "Newsletter not specified.";
        }


    }



    /// <summary>
    /// Determines whether subscriber line is valid
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>
    ///   <c>true</c> if [is valid subscriber line] [the specified data]; otherwise, <c>false</c>.
    /// </returns>
    private bool isValidSubscriberLine(string data)
    {
        bool isValid = true;
        string[] arr;
        arr = data.Split(',');

        if (arr.Length == 2)
        {
            // validate first field, should match a + or -
            if (!Regex.IsMatch(arr[0], @"^[\+|\-]{1}$"))
            {
                isValid = false;
            }

            // validate email address
            if (!RegexHelper.IsValidEmailAddressSyntax(arr[1]))
            {
                isValid = false;
            }



            // check for blacklisted emails
            // to check against a email blacklist web service
            if (arr[1] == "email@gmail.com")
            {
                isValid = false;
            }



        }
        else
        {
            isValid = false;
        }

        return isValid;

    }





    /// <summary>
    /// Steve Railsback DoSubscription adds or deletes a subscriber
    /// </summary>
    /// <param name="data"></param>
    /// <param name="letterInfo"></param>
    private string DoSubscription(string[] data, LetterInfo letterInfo)
    {
        string action = data[0];
        string email = data[1];

        // get subscriber using email address
        LetterSubscriber subscriber = subscriptions.Fetch(siteSettings.SiteGuid, letterInfoGuid, email);

        if (action == "+")
        {
            return AddSubscriber(subscriber, email);
        }
        else
        {
            return DeleteSubscriber(subscriber);
        }

    }



    /// <summary>
    /// Steve Railsback Adds the subscriber.
    /// </summary>
    /// <param name="subscriber">The subscriber.</param>
    /// <param name="email">The email.</param>
    /// <returns></returns>
    private string AddSubscriber(LetterSubscriber subscriber, string email)
    {
        // get site user
        SiteUser siteUser = SiteUser.GetByEmail(siteSettings, email);

        // Only concerned about new subscribers
        if (subscriber == null)
        {

            try
            {
                subscriptions.Save(
                    new LetterSubscriber
                    {
                        SiteGuid = siteSettings.SiteGuid,
                        EmailAddress = email,
                        LetterInfoGuid = letterInfoGuid,
                        UseHtml = true,

                        UserGuid = siteUser != null ? siteUser.UserGuid : new Guid("00000000-0000-0000-0000-000000000000"),
                        IsVerified = true,
                        BeginUtc = DateTime.UtcNow,
                        IpAddress = SiteUtils.GetIP4Address()
                    }

                );

                return "added";
            }
            catch (Exception ex)
            {
                log.Info(letterInfo.Title + ": Error: could not add " + email + ", " + ex.Message);
                return "failed";

            }

        }

        return "";

    }



    /// <summary>
    /// Steve Railsback Deletes the subscriber.
    /// </summary>
    /// <param name="subscriber">The subscriber.</param>
    /// <returns></returns>
    private string DeleteSubscriber(LetterSubscriber subscriber)
    {
        // only concerned about existing subscribers
        if (subscriber != null)
        {
            try
            {
                subscriptions.Delete(subscriber);
                return "deleted";
            }
            catch (Exception ex)
            {

                log.Info(letterInfo.Title + ": Error: " + ex.Message);
                return "failed";
            }
        }
        return "";

    }

    
	
	private void PopulateControls()
    {
        if (IsPostBack) { return; }
        List<LetterInfo> letterInfoList = LetterInfo.GetAll(siteSettings.SiteGuid);
        ddNewsLetter.DataSource = letterInfoList;
        ddNewsLetter.DataBind();
    }


    
    private void LoadSettings()
    {

        siteSettings = CacheHelper.GetCurrentSiteSettings();

    }

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>mojoPortal Newsletter Subscriber Import Utility</title> 
</head>
<body>
 <form id="form1" runat="server">
<div>
<div>
Choose a Newsletter From the Dropdown List to Import users to that list:<br />
<asp:DropDownList ID="ddNewsLetter" runat="server" DataValueField="LetterInfoGuid" DataTextField="Title"></asp:DropDownList>
</div>
<asp:Panel ID="pnlBatchProcessSubscribers" runat="server">
<asp:FileUpload runat="server" ID="fileBatchProcessSubscribers" />
<asp:Button ID="btnImport" runat="server" OnClick="btnBatchProcessSubscribers_Click" Text="Import" />
<div>
<asp:Label runat="server" ID="lblBatchProcessSubscribers" />
</div>

</asp:Panel>
<p>
This little project came out of the need to import 6000+ subscribers from an email list. 
I wasn't about to enter all those emails by hand! So I wrote this little tool. 
The tool is very simple and easy to use. All a user needs to do is upload a csv file. 
Each line of the cvs files should include an action; a "+" to add a subscriber and an "-" to remove a subscriber, 
and the subscriber's email address.  
The tool accepts a mix of adds and remove actions. I provided examples below 
and a dummy csv file fakenames.csv to show the example file format.  
<br />-Steve Railsback
</p>
<p>
Add Subscribers
<br />+,ChristopherSHenson@trashymail.com
<br />+,AprilCMarcial@mailinator.com
<br />+,ScottRThornton@mailinator.com
<br />+,CurtisALe@trashymail.com
<br />+,MerissaMNichols@pookmail.com
<br />+,BrianSBates@mailinator.com
<br />+,JulesKMorris@pookmail.com
</p>
<p>
Remove Subscribers Example
<br />-,AprilCMarcial@mailinator.com
<br />-,ScottRThornton@mailinator.com
<br />-,CurtisALe@trashymail.com
<br />-,MerissaMNichols@pookmail.com
<br />-,BrianSBates@mailinator.com
<br />-,JulesKMorris@pookmail.com
</p>
<p>
Add / Remove Subscribers Example
<br />+,AprilCMarcial@mailinator.com
<br />-,ScottRThornton@mailinator.com
<br />+,CurtisALe@trashymail.com
<br />-,MerissaMNichols@pookmail.com
<br />+,BrianSBates@mailinator.com
<br />-,JulesKMorris@pookmail.com
<p>
Update Subscriber Email Example
<br />-,AprilCMarcial@mailinator.com
<br />+,AprilCMarcial@mailinator.org

</p>
</div>
</form>
</body>
</html>