#!/bin/sh
echo "Copying files..."

config="$1"

cp bin/$config/mojoPortal.Features*.dll ../Web/bin/;
cp bin/$config/Argotic*.dll ../Web/bin/;
cp bin/$config/CookComputing.XmlRpcV2.dll ../Web/bin/;
cp App_GlobalResources/*.resx ../Web/App_GlobalResources/;
cp -f -r Setup/* ../Web/Setup/;

if [ -d ../Web/Poll ]; then
	rm -rf ../Web/Poll
fi
mkdir ../Web/Poll;
cp Poll/*.aspx ../Web/Poll/;
cp Poll/*.ascx ../Web/Poll/;

if [ -d ../Web/Survey ]; then
	rm -rf ../Web/Survey
fi
mkdir ../Web/Survey;
cp Survey/*.aspx ../Web/Survey/;
cp Survey/*.ascx ../Web/Survey/;

if [ -d ../Web/FeedManager ]; then
	rm -rf ../Web/FeedManager
fi
mkdir ../Web/FeedManager;
mkdir ../Web/FeedManager/Controls;
cp FeedManager/*.aspx ../Web/FeedManager/;
cp FeedManager/*.ascx ../Web/FeedManager/;
cp FeedManager/Controls/*.ascx ../Web/FeedManager/Controls/;

# blog
if [ -d ../Web/Blog ]; then
	rm -rf ../Web/Blog
fi
mkdir ../Web/Blog;
mkdir ../Web/Blog/Controls;
cp Blog/*.aspx ../Web/Blog/;
cp Blog/*.ashx ../Web/Blog/;
cp Blog/*.ascx ../Web/Blog/;
cp Blog/Controls/*.ascx ../Web/Blog/Controls/;

# contactform
if [ -d ../Web/ContactForm ]; then
	rm -rf ../Web/ContactForm
fi
mkdir ../Web/ContactForm;
cp ContactForm/*.aspx ../Web/ContactForm/;
cp ContactForm/*.ascx ../Web/ContactForm/;

# eventcalendar
if [ -d ../Web/EventCalendar ]; then
	rm -rf ../Web/EventCalendar
fi
mkdir ../Web/EventCalendar;
cp EventCalendar/*.aspx ../Web/EventCalendar/;
cp EventCalendar/*.ascx ../Web/EventCalendar/;

# foldergallery
if [ -d ../Web/FolderGallery ]; then
	rm -rf ../Web/FolderGallery
fi
mkdir ../Web/FolderGallery;
cp FolderGallery/*.aspx ../Web/FolderGallery/;
cp FolderGallery/*.ascx ../Web/FolderGallery/;

# forums
if [ -d ../Web/Forums ]; then
	rm -rf ../Web/Forums
fi
mkdir ../Web/Forums;
cp Forums/*.aspx ../Web/Forums/;
cp Forums/*.ascx ../Web/Forums/;

# google map
if [ -d ../Web/GoogleMap ]; then
	rm -rf ../Web/GoogleMap
fi
mkdir ../Web/GoogleMap;
# cp GoogleMap/*.aspx ../Web/GoogleMap/;
cp GoogleMap/*.ascx ../Web/GoogleMap/;

# htmlinclude
if [ -d ../Web/HtmlInclude ]; then
	rm -rf ../Web/HtmlInclude
fi
mkdir ../Web/HtmlInclude;
cp HtmlInclude/*.aspx ../Web/HtmlInclude/;
cp HtmlInclude/*.ascx ../Web/HtmlInclude/;

# image gallery
if [ -d ../Web/ImageGallery ]; then
	rm -rf ../Web/ImageGallery
fi
mkdir ../Web/ImageGallery;
cp ImageGallery/*.aspx ../Web/ImageGallery/;
cp ImageGallery/*.ascx ../Web/ImageGallery/;
cp ImageGallery/*.xsl ../Web/ImageGallery/;

# link module
if [ -d ../Web/List ]; then
	rm -rf ../Web/List
fi
mkdir ../Web/List;
cp List/*.aspx ../Web/List/;
cp List/*.ascx ../Web/List/;

# shared files
if [ -d ../Web/SharedFiles ]; then
	rm -rf ../Web/SharedFiles
fi
mkdir ../Web/SharedFiles;
cp SharedFiles/*.aspx ../Web/SharedFiles/;
cp SharedFiles/*.ascx ../Web/SharedFiles/;

# xml/xsl
if [ -d ../Web/XmlXsl ]; then
	rm -rf ../Web/XmlXsl
fi
mkdir ../Web/XmlXsl;
cp XmlXsl/*.aspx ../Web/XmlXsl/;
cp XmlXsl/*.ascx ../Web/XmlXsl/;

# live messenger
if [ -d ../Web/LiveMessenger ]; then
	rm -rf ../Web/LiveMessenger
fi
mkdir ../Web/LiveMessenger;
# cp LiveMessenger/*.aspx ../Web/LiveMessenger/;
cp LiveMessenger/*.ascx ../Web/LiveMessenger/;

# Twitter
if [ -d ../Web/Twitter ]; then
	rm -rf ../Web/Twitter
fi
mkdir ../Web/Twitter;
# cp Twitter/*.aspx ../Web/Twitter/;
cp Twitter/*.ascx ../Web/Twitter/;

# Flickr
if [ -d ../Web/Flickr ]; then
	rm -rf ../Web/Flickr
fi
mkdir ../Web/Flickr;
# cp Flickr/*.aspx ../Web/Flickr/;
cp Flickr/*.ascx ../Web/Flickr/;


# IFrame
if [ -d ../Web/IFrame ]; then
	rm -rf ../Web/IFrame
fi
mkdir ../Web/IFrame;
# cp IFrame/*.aspx ../Web/IFrame/;
cp IFrame/*.ascx ../Web/IFrame/;

# GoogleTranslate
if [ -d ../Web/GoogleTranslate ]; then
	rm -rf ../Web/GoogleTranslate
fi
mkdir ../Web/GoogleTranslate;
# cp GoogleTranslate/*.aspx ../Web/GoogleTranslate/;
cp GoogleTranslate/*.ascx ../Web/GoogleTranslate/;

# BingMap
if [ -d ../Web/BingMap ]; then
	rm -rf ../Web/BingMap
fi
mkdir ../Web/BingMap;
# cp BingMap/*.aspx ../Web/BingMap/;
cp BingMap/*.ascx ../Web/BingMap/;




echo "Finished Copying Files"



