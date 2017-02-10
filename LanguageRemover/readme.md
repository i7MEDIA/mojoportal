#LanguageRemover


The purpose of this console app is to improve performance when working with mojoPortal in Visual Studio by removing the extra languages .resx files from the Web/App_GlobalResources folder.

Each of those extra languages adds to the time it takes for the ASP.NET compiler to compile the app on the first web request. Since we now have so many translations it has become a major slow down.

So by building this project after building the other projects in the solution we can remove all except for the English resource files.

__DO NOT RUN__ this manually ie using the debugger, it will just throw errors

To run it just right click it and choose build. A post build event will execute it and pass in the file system path to Web/App_GlobalResources
