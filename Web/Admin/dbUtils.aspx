<%@ Page language="c#" Codebehind="dbUtils.aspx.cs" AutoEventWireup="True" Inherits="mojoPortal.Web.AdminUI.DBUtils" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>dbUtils</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h3>HtmlDecode/HtmlEncode Utility</h3>
			<table>
				<tr>
					<th colspan="2" align="left">
						<font color="red" size="medium">DO NOT SKIP THIS STEP: Backup your database before using this utility. 
						<br />It is possible that this will mess up your data, I do not warrant that this page 
							will fix your data. Use this page at your own risk. It is likely that content 
							that has been intentionally HtmlEncoded like if you blogged about html and 
							intentionally encoded the tags to show what the markup looked like, this page 
							probably won't fix it correctly.
							<br />Although this page is designed to be secure and will use SSL if it is available,
							it might not be a bad idea to remove the Admin/dbUtils.aspx file after you are sure your
							database has upgraded as best as possible.</font>
					</th>
				</tr>
				<tr>
					<td colspan="2" align="left">
						<p><b>Background</b> - our cross site scripting (xss) prevention strategy has changed 
						since early releases. </p>
						<p>Thanks to input from Dean Brettle we have changed our strategy to one of storing 
							the data raw and applying anti xss strategies on the way out of the db while 
							still enabling allowed markup. Currently we use <a href="http://www.brettle.com/neathtml">NeatHtml</a> to process the data on 
							the way out and encode any potentially dangerous markup. 
							 </p>
					
						<table border="1">
							<tr>
								<th align="left">
									Database Connection String:
								</th>
								<td colspan="2">
									<asp:TextBox ID="txtConnectionString" Runat="server" Columns="80"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td colspan="3">
								Here is how it breaks down historically on a module by module basis. You can try these buttons
								and see if they fix your backed up data. If you have a lot of content in your site(s) you should 
								have a good book and a pot of coffee ready when you click one of these buttons. You must enter the
								correct connection string above before clicking any of the buttons.
								</td>
							</tr>
							<tr>
								<th align="left">
									HtmlContent Module:
								</th>
								<td>
									Has been HtmlEncoded on the way into the db and HtmlDecoded on the way out 
									since the beginning of the project and was the poorly concieved implementation 
									later copied in other modules leading to our current situation.
								</td>
								<td>
									<asp:Button ID="btnDecodeHtml" Runat="server" Text="Decode All Content In This Module" onclick="btnDecodeHtml_Click"></asp:Button>
									<br>
									<asp:Label ID="lblHtmlResults" Runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<th align="left">
									Blog Module:
								</th>
								<td>
									Has always been HtmlDecoded on the way out but has only been HtmlEncoded on the 
									way in since the 1.0/2.0 release
								</td>
								<td>
									<asp:Button ID="btnDecodeBlog" Runat="server" Text="Decode All Content In This Module" onclick="btnDecodeBlog_Click"></asp:Button>
									<br>
									<asp:Label ID="lblBlogResults" Runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<th align="left">
									Gallery:
								</th>
								<td>
									Has always been stored raw but has been HtmlEncoded on the way in and 
									HtmlDecoded on the way out since the 1.0/2.0 release
								</td>
								<td>
									<asp:Button ID="btnDecodeGallery" Runat="server" Text="Decode All Content In This Module" onclick="btnDecodeGallery_Click"></asp:Button>
									<br>
									<asp:Label ID="lblGalleryResults" Runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<th align="left">
									Event Calendar:
								</th>
								<td>
									Has always been stored raw but has been HtmlEncoded on the way in and 
									HtmlDecoded on the way out since the 1.0/2.0 release
								</td>
								<td>
									<asp:Button ID="btnDecodeEvents" Runat="server" Text="Decode All Content In This Module" onclick="btnDecodeEvents_Click"></asp:Button>
									<br>
									<asp:Label ID="lblEventsResults" Runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<th align="left">
									Forums Module:
								</th>
								<td>
									Has always been stored raw but has been HtmlEncoded on the way in and 
									HtmlDecoded on the way out since the 1.0/2.0 release
								</td>
								<td>
									<asp:Button ID="btnDecodeForums" Runat="server" Text="Decode All Content In This Module" onclick="btnDecodeForums_Click"></asp:Button>
									<br>
									<asp:Label ID="lblForumResults" Runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<th align="left">
									Links Module:
								</th>
								<td>
									Description for links has always been stored raw but has been HtmlEncoded on 
									the way in and HtmlDecoded on the way out since the 1.0/2.0 release
								</td>
								<td>
									<asp:Button ID="btnDecodeLinks" Runat="server" Text="Decode All Content In This Module" onclick="btnDecodeLinks_Click"></asp:Button>
									<br>
									<asp:Label ID="lblLinksREsults" Runat="server"></asp:Label>
								</td>
							</tr>
						</table>
						<br>
						<b>(Experts only)</b> You can more granularly HtmlDecode or HtmlEncode Content 
						in any table in your site below if you know the correct table names, field 
						names, and the connection string and optionally a where clause to filter which 
						rows get updated. This is for experts only.
						<br>
						You must be a site administrator to use this page, but for security reasons you 
						must also know the correct connection string for the database.
					</td>
				</tr>
				
				<tr>
					<th align="left">
						Table Name:
					</th>
					<td>
						<asp:TextBox ID="txtTableName" Runat="server" Columns="80"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<th align="left">
						Name of Field to Decode/Encode:<br />
						<asp:RadioButton ID="rbHtmlDecode" Text="HtmlDecode" Runat="server" GroupName="EncodeDecode" Checked="True"></asp:RadioButton>
						<asp:RadioButton ID="rbHtmlEncode" Text="HtmlEncode" Runat="server" GroupName="EncodeDecode"></asp:RadioButton>
						
					</th>
					<td>
						<asp:TextBox ID="txtFieldName" Runat="server" Columns="80"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<th align="left">
						Primary Key Field Name:
					</th>
					<td>
						<asp:TextBox ID="txtPKField" Runat="server" Columns="80"></asp:TextBox>
					</td>
				<tr>
					<th align="left">
						<i>Optional</i> Where:
					</th>
					<td>
						<asp:TextBox ID="txtWhereClause" Runat="server" Columns="80" Rows="3"></asp:TextBox>
						<br />(i.e)where itemid > 20 and itemid < 41 
					</td>
				<tr>
					<th>
					</th>
					<td>
						<asp:Button ID="btnEncodeDecode" Runat="server" Text="Update Table" onclick="btnEncodeDecode_Click"></asp:Button>
						&nbsp;
					</td>
				</tr>
				<tr>
					<th align="left">
						Results:
					</th>
					<td>
						<asp:Label ID="lblResults" ForeColor="Red" Runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<th>
					</th>
					<td>
						<asp:Button ID="btnSetGuids" Runat="server" Text="Set Guids" onclick="btnSetGuids_Click"></asp:Button>
						&nbsp;
					</td>
				</tr>
			</table>
			<portal:mojoGoogleAnalyticsScript ID="mojoGoogleAnalyticsScript1" runat="server" />
		</form>
	</body>
</HTML>
