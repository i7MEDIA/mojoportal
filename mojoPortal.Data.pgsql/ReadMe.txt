1. Create your database and set the conneciton string in Web.config, as long as the user in your connection string has permission to create database objects, they will be created automatically.
2. Make sure the Data folder beneath the web site (and all its children) are writable by the web process.
3. Navigate to yoursiteroot/Setup/Default.aspx and setup will automatically setup or upgrade your database.



