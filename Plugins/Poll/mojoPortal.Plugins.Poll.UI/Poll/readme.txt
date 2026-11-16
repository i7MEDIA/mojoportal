The image file for the result should be placed in the skin folder and be 
named pollresult.jpg. If no image is found it then looks in the Web.config 
for the setting PollResultColorIfNoImage:

<add key="PollResultColorIfNoImage" value="Green" />

If not found or not a legal color, it defaults to Blue. If you for some 
reason don't want to show the result this way, add a tranparent image or 
remove it and type Transparent for the setting value.

-----------------------------------------------------------------------------

The Web.config setting PollShowMyPollHistoryButtonSetting_Default is a 
true/false which will be used if the setting PollShowMyPollHistoryButtonSetting
hasn't been added. If none of this is found it will default to false.

<add key="PollShowMyPollHistoryButtonSetting_Default" value="false" />
