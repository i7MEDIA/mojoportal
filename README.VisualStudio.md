# General Notes for developers working with the Visual Studio Solutions

Clone the repository as described [here](https://www.mojoportal.com/getting-the-code).

Using the mojoPortal Source Code requires Visual Studio 2017 and .NET 4.6.2, look under Help > About in VS to find the version.

Although we recommend for developers to use the source code, we do not recommend for anyone to modify the mojoPortal source code. All of your custom code should be in your own custom projects. As soon as you modify the mojoPortal code, you will be cutting  yourself off from future improvements and bug fixes and it will be diffiuclt or impossible to upgrade without losing your modifications.
http://www.mojoportal.com/avoid-forking-the-code.aspx


## About the .sln files:
- mojoportal.sln has all the features and is targeting .NET 4.6.2
- mojoportal-core.sln is just the core web content management system with minimal features and targets .NET 4.6.2
- Open the .sln file in VS 2017 and choose Rebuild Solution from the Build menu.

Note that we have data layer projects in the solutions for the supported databases, but you can safely remove the data layer projects that you are not interested in using from the solution to reduce the build time.

## More documentation is available here:
- http://www.mojoportal.com/docs
- http://www.mojoportal.com/dev-docs
- http://www.mojoportal.com/dev-docs/hello-world-quick-start
- http://www.mojoportal.com/dev-docs/visual-studio
- http://www.mojoportal.com/dev-docs/building-a-guestbook-video-series

## Steps for working in VS Web Server

1) Setup your db according the the instructions here: http://www.mojoportal.com/docs/database-configuration
2) Put your database connection string for your chosen db in the user.config.   
The data layer that is used is determined by the selected build profile. ie Debug and Release are for MS SQL but we also have Debug - MySql, Release - MySql, etc. So you choose the build profile for the database you are using and then rebuild the solution. 
3) Choose the build profile for your chosen database platform. Release and Debug are for MS SQL, for other databases there are Debug - MySql, Release -MySql, etc
4) Build the entire Solution - choose Rebuild Solution from the Build menu in VS
5) Make sure mojoPortal.Web project is set as the sartup project (the startup project is bold in solution explorer, right click it to set it as the startup project if needed) the Launch the VS Web server and visit /Setup/Default.aspx to complete setup. You could launch it by running the debugger (Play Button) and starting the mojoPortal.Web project, but its easier if you just rebuild the solution then right click the /Default.aspx file in the root of mojoPortal.Web project from Solution Explorer and choose View In Web Browser. If you do run it from the debugger, realize that in debug mode it is going to break on all errors and there will be lots of errors at first because the database objects don't exist until after the setup page runs. You can just click continue on every error and it will redirect to the Setup/Default.aspx page automatically and run the scripts and setup the first site. By just viewing it in the browser instead of debugging you don't have to deal directly with the errors. Debugging is really for when you want to step through the code to resolve some problem or find out what it is doing or what some variable is in some particular place in the code. You can set break points in any of the projects but you always need to debug it by launching the mojoPortal.Web project. 
6) Enjoy :)
7) Spread the word and support the project by purchasing add on features(http://www.mojoportal.com/add-ons) or donating (http://www.mojoportal.com/donate)!

## Steps for IIS setup

1) You must right click the Visual Studio icon and choose run as Administrator in order to be able to debug using IIS
2) complete steps 1 through 5 as above 
3) Right click the Web project in VS and choose properties, then click the Web tab and select "Use IIS Web Server" and enter the url for the project so that 
4) complete steps 6 through 8 as above

## Additional info:

You may notice when debugging that multiple web servers are spawned. This is because there are multiple web applications in the solution as features are split into separate projects. All the files for features get copied up to the main mojoPortal.Web project ie Web folder by post build events, so the feature projects are not meant to be run directly, the mojoPortal.Web project must always be the startup project, though you can set breakpoints and debug any of the code in any of the projects, but the main web project always has to be the startup project. You can disable those extra web apps from launching a web server as described here http://stackoverflow.com/questions/16363/how-do-you-configure-vs2008-to-only-open-one-webserver-in-a-solution-with-multi#16390

Just click the project node in Solution Explorer and then click the Properties tab on the right and you will see where to disable it. 


