<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload-blueimp.aspx.cs" Inherits="PrototypeWeb.upload_blueimp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel='stylesheet' type='text/css' href='//ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/themes/sunny/jquery-ui.css' />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/jquery-ui.min.js" type="text/javascript" ></script>
    
    <%-- 
        <script src="ClientScript/jqfileupload/jquery.iframe-transport.js"></script>

        <script src="ClientScript/jqfileupload/jquery.fileupload.js"></script>

    <script src="http://blueimp.github.com/JavaScript-Load-Image/load-image.min.js"></script>
<script src="http://blueimp.github.com/JavaScript-Canvas-to-Blob/canvas-to-blob.min.js"></script>
       --%>

    
    <%-- 
    <script src="ClientScript/jqfileupload/jquery.fileupload-fp.js"></script>
        --%>

    <style>
        
        .fileinput-button {
          position: relative;
          overflow: hidden;
          float: left;
          margin-right: 4px;
        }
        .fileinput-button input {
          position: absolute;
          top: 0;
          right: 0;
          margin: 0;
          opacity: 0;
          filter: alpha(opacity=0);
          transform: translate(-300px, 0) scale(4);
          font-size: 23px;
          direction: ltr;
          cursor: pointer;
        }

        div.uploadfilelist {
            padding-top: 10px;
            clear: both;
        }

        div.uploadfilelist .ui-button,
        .uploadcontainer .ui-button {
            padding: 7px;
        }

        ul.filelist {
            
            list-style: none;
        }

        ul.filelist .ui-icon-trash {
            display: inline-block;
            cursor: hand;
            cursor: pointer;
            
        }

        div.fileupload-dropzone {
            padding: 50px 0 50px 0;
            width: 100%;
            border:dashed thin black;
            margin: 4px 4px 4px 4px;
            text-align:center;
            vertical-align:middle;
        }

    </style>
    <script>
        function refreshPanel() {
            $('#btnRefresh').click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sc1" runat="server" EnablePageMethods="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
        <div>
            <p>Goals/Requirements:</p>
                <ul>
                    <li>
                        File Upload Based On BlueImp jQuery File Upload <a href="https://github.com/blueimp/jQuery-File-Upload/wiki/Basic-plugin">https://github.com/blueimp/jQuery-File-Upload/wiki/Basic-plugin</a>
                    </li>
                    <li>
                        Fallback to normal postback single file upload if javascript is disabled or not available
                    </li>
                    <li>
                        Modern browsers get multi file selection if MaxFilesAllowed > 1 on the control
                    </li>
                    <li>
                        Modern browsers get a progress bar during file upload
                    </li>
                    <li>
                        Support drag and drop of files in modern browsers
                    </li>
                    <li>
                        Support more than one instance on a page
                    </li>
                    <li>
                        Other form fields are also submitted with the file
                    </li>
                    <li>
                        Works inside UpdatePanel?
                    </li>
                </ul>
                
            <input id="hdnFoo" name="hdnFoo" type="hidden" value="myfoo" />

            <portal:jQueryFileUpload ID="up1" runat="server" AcceptFileTypes="/(\.|\/)(gif|jpe?g|png)$/i" MaxFilesAllowed="5" ServiceUrl="~/Services/upload-service-blueimp.ashx" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />

            <p>&nbsp;</p>
            

            <asp:UpdatePanel ID="pnlUpldate" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <portal:jQueryFileUpload ID="up2" runat="server" MaxFilesAllowed="5" ServiceUrl="~/Services/upload-service-blueimp.ashx?sub=1" 
                        UploadCompleteCallback="refreshPanel"  />
                    <asp:Button ID="btnUpload2" runat="server" Text="Upload" OnClick="btnUpload2_Click" />

                    <asp:Label ID="lblTime" runat="server" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            

        </div>
    </form>
</body>
</html>
