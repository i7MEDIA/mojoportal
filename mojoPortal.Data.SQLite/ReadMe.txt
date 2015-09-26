Since SQLite is a file based database, we have already confgured a database and populated it with default content.
You don't have to do anything to setup the db unless you want to use a different database than the one provided.
You do still need to make the Data folder beneath the web writable by the web process.

If you want to use a different db, the script for table creation is all you need. Follow the instructions form here:
http://www.mojoportal.com/sqlitesetup.aspx

1. Create your database and set the conneciton string in Web.config, as long as the user in your connection string has permission to create database objects, they will be created automatically.
2. Make sure the Data folder beneath the web site (and all its children) are writable by the web process.
3. Navigate to yoursiteroot/Setup/Default.aspx and setup will automatically setup or upgrade your database.

