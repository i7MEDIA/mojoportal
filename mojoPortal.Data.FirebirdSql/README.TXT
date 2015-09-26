Installation
  
  Install the firebird server software, the easiest way is to install the executable installation downdload.
    Firebird server:
      Firebird V2.0 Downloads
      12th Aug 2006 Official Windows Setup and Installer For Classic and SuperServer Release Candidate 4
      LINK: http://www.ibphoenix.com/main.nfs?a=ibphoenix&page=ibp_download_20
      If you get -904 errors, run the uninstall.bat and then the install-super.bat files in the firebird/bin directory.
  
  The firebird data client is included in the mojoportal distribution.
  
  OPTIONAL GUI DATABASE EXPLORER
    http://www.flamerobin.org/
  
  Open the file "C:\Program Files\Firebird\aliases.conf" and add the following line,
        mojoportal = C:\MOJOPORTAL.FDB (put your local path to your .fdb file here)
  
  
  The firebird connection string needs to be added or configured in the web\Web.config file,
        <add key="FirebirdConnectionString" value="Data Source=localhost;Server Type=0;Port Number=3050;Database=MOJOPORTAL104TEST;Dialect=3;Charset=UNICODE_FSS;Pooling=True;Min Pool Size=0;Max Pool Size=200;Connection Timeout=10;Connection Lifetime=60;Fetch Size=200;User Id=SYSDBA;Password=masterkey" />
  
  
1. Create your database and set the conneciton string in Web.config, as long as the user in your connection string has permission to create database objects, they will be created automatically.
2. Make sure the Data folder beneath the web site (and all its children) are writable by the web process.
3. Navigate to yoursiteroot/Setup/Default.aspx and setup will automatically setup or upgrade your database.

