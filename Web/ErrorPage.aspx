<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>


<script runat="server">

// http://www.microsoft.com/technet/security/advisory/2416728.mspx

        void Page_Load() {
        byte[] delay = new byte[1];
        RandomNumberGenerator prng = new RNGCryptoServiceProvider();

        prng.GetBytes(delay);
        Thread.Sleep((int)delay[0]);
        
        IDisposable disposable = prng as IDisposable;
        if (disposable != null) { disposable.Dispose(); }

        
    }
	
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Oops!</title>
</head>
<body>
    <div>
        <p style="font-size: large">
					We're sorry but a server error has occurred while trying to process your request.
					</p>
					<p style="font-size: large">
					The error has been logged and will be reviewed by our staff as soon as possible.
					It is possible that the error was just a momentary hiccup and you may wish to use the back button and try
					again or go back to the <a href="Default.aspx">home page</a>.
					</p>
    </div>
</body>
</html>
