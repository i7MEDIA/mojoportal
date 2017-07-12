// Author:					
// Created:				    2011-06-19
// Last Modified:			2011-06-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://www.jplayer.org/
    /// http://www.jplayer.org/latest/developer-guide/
    /// 
    /// this panel is designed to find any links for mp3 files inside it and wire them up as a playlist using jPlayer 
    /// currently it is only for audio but could be expanded with configurable properties to enable other scenarios.
    /// 
    /// at the moment this is just some preliminary proof of concept mainly developed becuase I needed a media player that supports
    /// mobile devices for the webstore product list when selling mp3 files and showing teaser files.
    /// </summary>
    public class jPlayerPanel : Panel
    {
        private string element = "div";

        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        private bool renderPlayer = true;

        public bool RenderPlayer
        {
            get { return renderPlayer; }
            set { renderPlayer = value; }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (renderPlayer) { SetupScript(); }

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            writer.Write("<" + element + " id='" + this.ClientID + "' class='" + CssClass + "'>\n");

            base.RenderContents(writer);

            if (renderPlayer) { RenderPlayerMarkup(writer); }

            writer.Write("\n</" + element + ">");

        }

        private void RenderPlayerMarkup(HtmlTextWriter writer)
        {
            writer.Write("<div id=\"jquery_jplayer_" + ClientID + "\" class=\"jp-jplayer\"></div>");

            writer.Write("<div class=\"jp-audio\">");

            writer.Write("<div class=\"jp-type-playlist\">");

            writer.Write("<div id=\"jp_interface_" + ClientID + "\" class=\"jp-interface\">");

            writer.Write("<ul class=\"jp-controls\">");

            writer.Write("<li><a href=\"#\" class=\"jp-play\" tabindex=\"1\">play</a></li>");
            writer.Write("<li><a href=\"#\" class=\"jp-pause\" tabindex=\"1\">pause</a></li>");
            writer.Write("<li><a href=\"#\" class=\"jp-stop\" tabindex=\"1\">stop</a></li>");
            writer.Write("<li><a href=\"#\" class=\"jp-mute\" tabindex=\"1\">mute</a></li>");
            writer.Write("<li><a href=\"#\" class=\"jp-unmute\" tabindex=\"1\">unmute</a></li>");
            writer.Write("<li><a href=\"#\" class=\"jp-previous\" tabindex=\"1\">previous</a></li>");
            writer.Write("<li><a href=\"#\" class=\"jp-next\" tabindex=\"1\">next</a></li>");

            writer.Write("</ul>");

            writer.Write("<div class=\"jp-progress\">");
            writer.Write("<div class=\"jp-seek-bar\">");
            writer.Write("<div class=\"jp-play-bar\"></div>");
            writer.Write("</div>"); 
            writer.Write("</div>"); 

            writer.Write("<div class=\"jp-volume-bar\">");
            writer.Write("<div class=\"jp-volume-bar-value\"></div>");
            writer.Write("</div>"); 

            writer.Write("<div class=\"jp-current-time\"></div>");
            writer.Write("<div class=\"jp-duration\"></div>");

            writer.Write("</div>"); // end interface

            writer.Write("<div id=\"jp_playlist_" + ClientID + "\" class=\"jp-playlist\">");
            //The method Playlist.displayPlaylist() uses this unordered list
            writer.Write("<ul><li></li></ul>");
            writer.Write("</div>");

            writer.Write("</div>"); // end jp-type-playlist

            writer.Write("</div>"); // end jp-audio
        }

        private void SetupScript()
        {
            //TODO: cleanup, probably can isolate some of this so it doesn't have to be repeated if there are multiple panels on the page

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(document).ready(function(){");

            script.Append("var Playlist = function(instance, playlist, options) {");

            script.Append("var self = this; ");

            script.Append("this.instance = instance; "); // String: To associate specific HTML with this playlist
            script.Append("this.playlist = playlist; "); // Array of Objects: The playlist
            script.Append("this.options = options; "); // Object: The jPlayer constructor options for this playlist

            script.Append("this.current = 0; ");

            script.Append("this.cssId = { ");
            script.Append("jPlayer: \"jquery_jplayer_\", ");
            script.Append("interface: \"jp_interface_\", ");
            script.Append("playlist: \"jp_playlist_\" ");
            script.Append("}; ");

            script.Append("this.cssSelector = {}; ");

            script.Append("$.each(this.cssId, function(entity, id) {");
            script.Append("self.cssSelector[entity] = \"#\" + id + self.instance; ");
            script.Append("}); ");

            script.Append("if(!this.options.cssSelectorAncestor) {");
            script.Append("this.options.cssSelectorAncestor = this.cssSelector.interface; ");
            script.Append("} ");

            script.Append("$(this.cssSelector.jPlayer).jPlayer(this.options); ");

            script.Append("$(this.cssSelector.interface + \" .jp-previous\").click(function() {");
            script.Append("self.playlistPrev(); ");
            script.Append("$(this).blur(); ");
            script.Append("return false; ");
            script.Append("}); ");

            script.Append("$(this.cssSelector.interface + \" .jp-next\").click(function() {");
            script.Append("self.playlistNext(); ");
            script.Append("$(this).blur(); ");
            script.Append("return false; ");
            script.Append("}); ");

            script.Append("}; ");

            // begin prototype
            script.Append("Playlist.prototype = {");
            script.Append("displayPlaylist: function() {");
            script.Append("var self = this; ");
            script.Append("$(this.cssSelector.playlist + \" ul\").empty(); ");
            script.Append("for (i=0; i < this.playlist.length; i++) {");
            script.Append("var listItem = (i === this.playlist.length-1) ? \"<li class='jp-playlist-last'>\" : \"<li>\"; ");
            script.Append("listItem += \"<a href='#' id='\" + this.cssId.playlist + this.instance + \"_item_\" + i +\"' tabindex='1'>\"+ this.playlist[i].name +\"</a>\"; ");

            // Create links to free media
            script.Append("if(this.playlist[i].free) {");
            script.Append("var first = true; ");
            script.Append("listItem += \"<div class='jp-free-media'>(\"; ");
            script.Append("$.each(this.playlist[i], function(property,value) {");
            script.Append("if($.jPlayer.prototype.format[property]) {"); // Check property is a media format.
            script.Append("if(first) {");
            script.Append("first = false; ");
            script.Append("} else {");
            script.Append("listItem += \" | \"; ");
            script.Append("} ");
            script.Append("listItem += \"<a id='\" + self.cssId.playlist + self.instance + \"_item_\" + i + \"_\" + property + \"' href='\" + value + \"' tabindex='1'>\" + property + \"</a>\"; ");
            script.Append("} ");
            script.Append("}); ");
            script.Append("listItem += \")</span>\"; ");
            script.Append("} ");
            script.Append("listItem += \"</li>\"; ");

            // Associate playlist items with their media
            script.Append("$(this.cssSelector.playlist + \" ul\").append(listItem); ");
            script.Append("$(this.cssSelector.playlist + \"_item_\" + i).data(\"index\", i).click(function() {");
            script.Append("var index = $(this).data(\"index\"); ");
            script.Append("if(self.current !== index) {");
            script.Append("self.playlistChange(index); ");
            script.Append("} else {");
            script.Append("$(self.cssSelector.jPlayer).jPlayer(\"play\"); ");
            script.Append("} ");
            script.Append("$(this).blur(); ");
            script.Append("return false; ");
            script.Append("}); ");

            // Disable free media links to force access via right click
            script.Append("if(this.playlist[i].free) {");
            script.Append("$.each(this.playlist[i], function(property,value) {");
            script.Append("if($.jPlayer.prototype.format[property]) {"); // Check property is a media format.
            script.Append("$(self.cssSelector.playlist + \"_item_\" + i + \"_\" + property).data(\"index\", i).click(function() {");
            script.Append("var index = $(this).data(\"index\"); ");
            script.Append("$(self.cssSelector.playlist + \"_item_\" + index).click(); ");
            script.Append("$(this).blur(); ");
            script.Append("return false; ");
            script.Append("}); ");
            script.Append("} ");
            script.Append("}); ");
            script.Append("} ");
            script.Append("} ");
            script.Append("}, ");

            script.Append("playlistInit: function(autoplay) {");
            script.Append("if(autoplay) {");
            script.Append("this.playlistChange(this.current); ");
            script.Append("} else {");
            script.Append("this.playlistConfig(this.current); ");
            script.Append("} ");
            script.Append("}, ");
            script.Append("playlistConfig: function(index) {");
            script.Append("$(this.cssSelector.playlist + \"_item_\" + this.current).removeClass(\"jp-playlist-current\").parent().removeClass(\"jp-playlist-current\"); ");
            script.Append("$(this.cssSelector.playlist + \"_item_\" + index).addClass(\"jp-playlist-current\").parent().addClass(\"jp-playlist-current\"); ");
            script.Append("this.current = index; ");
            script.Append("$(this.cssSelector.jPlayer).jPlayer(\"setMedia\", this.playlist[this.current]); ");
            script.Append("}, ");
            script.Append("playlistChange: function(index) {");
            script.Append("this.playlistConfig(index); ");
            script.Append("$(this.cssSelector.jPlayer).jPlayer(\"play\"); ");
            script.Append("}, ");
            script.Append("playlistNext: function() {");
            script.Append("var index = (this.current + 1 < this.playlist.length) ? this.current + 1 : 0; ");
            script.Append("this.playlistChange(index); ");
            script.Append("}, ");
            script.Append("playlistPrev: function() { ");
            script.Append("var index = (this.current - 1 >= 0) ? this.current - 1 : this.playlist.length - 1; ");
            script.Append("this.playlistChange(index); ");
            script.Append("} ");
            script.Append("}; ");

            //end protoype

            //script.Append("}); ");

            //script.Append("</script>");

            //this.Page.ClientScript.RegisterStartupScript(
            //    typeof(Page),
            //    "jplayerplaylist",
            //    script.ToString());

            //script = new StringBuilder();
            //script.Append("\n<script type=\"text/javascript\">");

            //script.Append("$(document).ready(function(){");

            // find all the links for .mp3 files inside this panel and add them to the playlist
            script.Append("var list" + ClientID + " =  [];");
            script.Append("$('#" + ClientID + " a[href$=\".mp3\"]').each(function(index, value){");
            script.Append("var item = {name:\"\",mp3:\"\"}; ");
            script.Append("item.name = $(this).text(); ");
            script.Append("item.mp3 = $(this).attr('href'); ");
            script.Append("list" + ClientID + ".push(item); ");

            script.Append("}); ");
            
         
            script.Append("var playlist" + ClientID + "  = new Playlist(\"" + ClientID + "\", list" + ClientID +", ");
            script.Append("{ ");
            script.Append("ready: function() { ");
            script.Append("playlist" + ClientID + ".displayPlaylist(); ");
            script.Append("playlist" + ClientID + ".playlistInit(false); "); // Parameter is a boolean for autoplay.
            script.Append("}, ");
            script.Append("ended: function() {");
            script.Append("playlist" + ClientID + ".playlistNext(); ");
            script.Append("}, ");
            script.Append("play: function() {");
            script.Append("$(this).jPlayer(\"pauseOthers\"); ");
            script.Append("}, ");
            script.Append("swfPath: \"/ClientScript/jplayer\", ");
            script.Append("supplied: \"mp3\" ");
            script.Append("}); ");

            script.Append("}); ");

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());



        }

    }
}