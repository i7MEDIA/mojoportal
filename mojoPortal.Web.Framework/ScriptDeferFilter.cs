using System;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;

//namespace Dropthings.Web.Util
namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// Summary description for ScriptDeferFilter
    /// </summary>
    public class ScriptDeferFilter : Stream
    {
        Stream responseStream;
        long position;

        /// <summary>
        /// When this is true, script blocks are suppressed and captured for 
        /// later rendering
        /// </summary>
        bool captureScripts;

        /// <summary>
        /// Holds all script blocks that are injected by the controls
        /// The script blocks will be moved after the form tag renders
        /// </summary>
        StringBuilder scriptBlocks;

        Encoding encoding;

        /// <summary>
        /// Holds characters from last Write(...) call where the start tag did not
        /// end and thus the remaining characters need to be preserved in a buffer so 
        /// that a complete tag can be parsed
        /// </summary>
        char[] pendingBuffer = null;

        /// <summary>
        /// When this is true, it means the last script tag tag started from a Write(...) call
        /// was marked as pinned, which means it must not be moved and must be rendered
        /// exactly where it is.
        /// </summary>
        bool lastScriptTagIsPinned = false;

        /// <summary>
        /// If this is true, then it means a script tag started, but did not end
        /// </summary>
        bool scriptTagStarted = false;

        /// <summary>
        /// Added 2008-11-04 by 
        /// Omar implemented a method to mark some scripts so they don't get moved to the bottom by adding a "pin" attribute
        /// to the script element like this script pin src="...
        /// however this is not xhtml compliant and is therefore not a good solution even though its more efficient than my hacky solution.
        /// this bool determines whether to use my hacky solution, it will come from Web.config UseMojoScriptFilterExceptionsWhenCombiningJavaScript
        /// </summary>
        bool useHackyScriptExceptions = false;

        public ScriptDeferFilter(HttpResponse response)
        {
            this.encoding = response.Output.Encoding;
            this.responseStream = response.Filter;

            this.scriptBlocks = new StringBuilder(5000);
            // When this is on, script blocks are captured and not written to output
            this.captureScripts = true;
            this.useHackyScriptExceptions = ConfigHelper.GetBoolProperty("UseMojoScriptFilterExceptionsWhenCombiningJavaScript", this.useHackyScriptExceptions);
        }

        #region Filter overrides
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Close()
        {
            this.FlushPendingBuffer();
            responseStream.Close();
        }

        private void FlushPendingBuffer()
        {
            // Some characters were left in the buffer 
            if (null != this.pendingBuffer)
            {
                this.WriteOutput(this.pendingBuffer, 0, this.pendingBuffer.Length);
                this.pendingBuffer = null;
            }

        }

        public override void Flush()
        {
            this.FlushPendingBuffer();
            responseStream.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return position; }
            set { position = value; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            responseStream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read(buffer, offset, count);
        }
        #endregion

        public override void Write(byte[] buffer, int offset, int count)
        {
            // If we are not capturing script blocks anymore, just redirect to response stream
            if (!this.captureScripts)
            {
                this.responseStream.Write(buffer, offset, count);
                return;
            }

            /* 
             * Script and HTML can be in one of the following combinations in the specified buffer:          
             * .....<script ....>.....</script>.....
             * <script ....>.....</script>.....
             * <script ....>.....</script>
             * <script ....>.....</script> .....
             * ....<script ....>..... 
             * <script ....>..... 
             * .....</script>.....
             * .....</script>
             * <script>.....
             * .... </script>
             * ......
             * Here, "...." means html content between and outside script tags
            */

            char[] content;
            char[] charBuffer = this.encoding.GetChars(buffer, offset, count);

           

            // If some bytes were left for processing during last Write call
            // then consider those into the current buffer
            if (null != this.pendingBuffer)
            {
                content = new char[charBuffer.Length + this.pendingBuffer.Length];
                Array.Copy(this.pendingBuffer, 0, content, 0, this.pendingBuffer.Length);
                Array.Copy(charBuffer, 0, content, this.pendingBuffer.Length, charBuffer.Length);
                this.pendingBuffer = null;
            }
            else
            {
                content = charBuffer;
            }

            int scriptTagStart = 0;
            int lastScriptTagEnd = 0;

            int pos;
            for (pos = 0; pos < content.Length; pos++)
            {
                // See if tag start
                char c = content[pos];
                if (c == '<')
                {
                    /*
                        Make sure there are enough characters available in the buffer to finish 
                        tag start. This will happen when a tag partially starts but does not end
                        For example, a partial script tag
                        <script
                        Or it's the ending html tag or some tag closing that ends the whole response
                        </html>
                    */
                    if (pos + "script pin".Length > content.Length)
                    {
                        // a tag started but there are less than 10 characters available. So, let's
                        // store the remaining content in a buffer and wait for another Write(...) or
                        // flush call.
                        this.pendingBuffer = new char[content.Length - pos];
                        Array.Copy(content, pos, this.pendingBuffer, 0, content.Length - pos);
                        break;
                    }

                    int tagStart = pos;

                    // Check if it's a tag ending
                    if (content[pos + 1] == '/')
                    {
                        pos += 2; // go past the </ 

                        // See if script tag is ending
                        if (isScriptTag(content, pos))
                        {
                            if (this.lastScriptTagIsPinned)
                            {
                                // The last script tag was pinned. So, it will not be moved
                                this.lastScriptTagIsPinned = false;

                                // This this tag as just another tag has just closed
                                pos++;
                            }
                            else
                            {
                                // Script tag just ended. Two scenarios can happend:
                                // This can be a partial buffer where the script beginning tag is not present
                                // This can be a partial buffer of a pinned script tag
                                pos = pos + "script>".Length;

                                scriptBlocks.Append(content, scriptTagStart, pos - scriptTagStart);
                                scriptBlocks.Append(Environment.NewLine);

                                lastScriptTagEnd = pos;
                                scriptTagStarted = false;

                                pos--; // continue will increase pos by one again
                                continue;
                            }
                        }
                        else if (isBodyTag(content, pos))
                        {
                            // body tag has just end. Time for rendering all the script
                            // blocks we have suppressed so far and stop capturing script blocks

                            if (this.scriptBlocks.Length > 0)
                            {
                                // Render all pending html output till now
                                this.WriteOutput(content, lastScriptTagEnd, tagStart - lastScriptTagEnd);

                                // Render the script blocks
                                this.RenderAllScriptBlocks();

                                // Stop capturing for script blocks
                                this.captureScripts = false;

                                // Write from the body tag start to the end of the inut buffer and return
                                // from the function. We are done.
                                this.WriteOutput(content, tagStart, content.Length - tagStart);
                                return;
                            }
                        }
                        else
                        {
                            // some other tag's closing. safely skip one character as smallest
                            // html tag is one character e.g. <b>. just an optimization to save one loop
                            pos++;
                        }
                    }
                    else
                    { //(content[pos + 1] != '/') we are not at a tag ending 

                        if (isScriptTag(content, pos + 1))
                        {
                            //current position is at <script

                            if (useHackyScriptExceptions)
                            {
                                // 2008-11-04 
                                // some scripts must be at the top (example neathtml.js)
                                // some scripts must be allowed to render inline (example neathtml)

                                // this is our opportunity to pin by moving the position past the scripts
                                // we don't want to move to the bottom 
                                int charsToSkip = 0;
                                if (ShouldNotMoveScript(content, pos, out charsToSkip))
                                {
                                    lastScriptTagIsPinned = true;
                                    pos += charsToSkip;
                                    continue;
                                }
                            }

                            
                            // If the script tag is marked to be pinned, then it won't be moved.
                            // it will be considered as a regular html tag
                            this.lastScriptTagIsPinned = isPinned(content, pos + 1);

                            if (!this.lastScriptTagIsPinned)
                            {
                                // Script tag started. Record the position as we will 
                                // capture the whole script tag including its content
                                // and store in an internal buffer.
                                scriptTagStart = pos;

                                // Write html content since last script tag closing upto this script tag 
                                this.WriteOutput(content, lastScriptTagEnd, scriptTagStart - lastScriptTagEnd);

                                // Skip the tag start to save some loops
                                //pos += "<script".Length;
                                pos += 7;

                                scriptTagStarted = true;
                            }
                            else
                            {
                                pos++;
                            }
                        }
                        else
                        {
                            // some other tag started
                            // safely skip 2 character because the smallest tag is one character e.g. <b>
                            // just an optimization to eliminate one loop 
                            pos++;
                        }
                    }
                }

            } //for (pos = 0; pos < content.Length; pos++)

            // If a script tag is partially sent to buffer, then the remaining content
            // is part of the last script block
            if (scriptTagStarted)
            {
                this.scriptBlocks.Append(content, scriptTagStart, pos - scriptTagStart);
            }
            else
            {
                // Render the characters since the last script tag ending
                this.WriteOutput(content, lastScriptTagEnd, pos - lastScriptTagEnd);
            }
        }

        /// <summary>
        /// Render collected scripts blocks all together
        /// </summary>
        private void RenderAllScriptBlocks()
        {
            string output = CombineScripts.CombineScriptBlocks(this.scriptBlocks.ToString());
            byte[] scriptBytes = this.encoding.GetBytes(output);
            this.responseStream.Write(scriptBytes, 0, scriptBytes.Length);
        }

        private void WriteOutput(char[] content, int pos, int length)
        {
            if (length == 0) return;

            byte[] buffer = this.encoding.GetBytes(content, pos, length);
            this.responseStream.Write(buffer, 0, buffer.Length);
        }
        private void WriteOutput(string content)
        {
            byte[] buffer = this.encoding.GetBytes(content);
            this.responseStream.Write(buffer, 0, buffer.Length);
        }

        

        private bool isPinned(char[] content, int pos)
        {
            // we are not using this invalid attribute so just return false;
            //if (pos + 5 + 3 < content.Length)
            //    return ((content[pos + 7] == 'p' || content[pos + 7] == 'P')
            //        && (content[pos + 8] == 'i' || content[pos + 8] == 'I')
            //        && (content[pos + 9] == 'n' || content[pos + 9] == 'N'));
            //else
                return false;
        }

        private bool isBodyTag(char[] content, int pos)
        {
            if (pos + 3 < content.Length)
                return ((content[pos] == 'b' || content[pos] == 'B')
                    && (content[pos + 1] == 'o' || content[pos + 1] == 'O')
                    && (content[pos + 2] == 'd' || content[pos + 2] == 'D')
                    && (content[pos + 3] == 'y' || content[pos + 3] == 'Y'));
            else
                return false;
        }

        /// <summary>
        /// TODO: we need this to only return true for external script tags, ie with src= ?
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool isScriptTag(char[] content, int pos)
        {
            if (pos + 5 < content.Length)
                return ((content[pos] == 's' || content[pos] == 'S')
                    && (content[pos + 1] == 'c' || content[pos + 1] == 'C')
                    && (content[pos + 2] == 'r' || content[pos + 2] == 'R')
                    && (content[pos + 3] == 'i' || content[pos + 3] == 'I')
                    && (content[pos + 4] == 'p' || content[pos + 4] == 'P')
                    && (content[pos + 5] == 't' || content[pos + 5] == 'T'));
            else
                return false;

        }

        /// <summary>
        /// added by  2008-11-04
        /// this function should only be called right after isScript returns true
        /// its a very crappy hack to solve a few problems caused by moving the javascript to the bottom
        /// neathtml.js must be at the top, its a very important script for preventing xss
        /// it also wraps a lof things inside script tags inside the content and we don't want to move those script blocks to the bottom either 
        /// 
        /// strategy: detect scripts we don't want to move to bottom and when found determine # of chars to move ahead in order to move past the
        /// script in processing the output stream
        /// 
        /// sub strategy if the script is an inline block with no src=, don't move it to the bottom (assume its neathtml)
        /// problem is some inline script blocks do need to be moved to the bottom so we hard code some exceptions to ignore
        /// 
        /// if anyone has ideas about refactoring this to make it more maintainable and/or more efficient
        /// please give it a try and submit the change to joe.audette@gmail.com
        /// </summary>
        private bool ShouldNotMoveScript(char[] content, int pos, out int charsToSkip)
        {
            // initialize the out param
            charsToSkip = 0;

            int closingScriptIndex = GetOffsetOfNextClosingScript(content, pos);
            int srcIndex = GetOffsetNextIndexOfSrc(content, pos, closingScriptIndex);

            //need to ignore things with inline script created by ajax, so it will go to the bottom
            //int indexOfInitScript = scr.IndexOf("Sys.");
            int indexOfInitScript = GetOffsetNextIndexOfSysDot(content, pos, closingScriptIndex);
            if ((indexOfInitScript > -1) && (indexOfInitScript < closingScriptIndex)) { return false; }

            // another ajax exception
            //indexOfInitScript = scr.IndexOf("WebForm_InitCallback");
            indexOfInitScript = GetOffsetNextIndexOfWebFormInitCallback(content, pos, closingScriptIndex);

            if ((indexOfInitScript > -1) && (indexOfInitScript < closingScriptIndex)) { return false; }

            // since we do move the main yahoo scripts to bottom, must also move inline tab declarations below it
            /* Example, this can be entered right in the content editor to create tabs so it may appear anywhere in a page:
             <script type="text/javascript">
                var mainTabs = new YAHOO.widget.TabView('divtabs');
            </script>

             */
            //int indexOfYahoo = scr.IndexOf("YAHOO");
            int indexOfException = GetOffsetNextIndexOfYAHOO(content, pos, closingScriptIndex);

            if ((indexOfException > -1) && (indexOfException < closingScriptIndex)) { return false; }

            //gaJsHost
            indexOfException = GetOffsetNextIndexOfGA(content, pos, closingScriptIndex);
            if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
            {
                charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                return true;
            }

            if (
                (closingScriptIndex > -1)
                && ((srcIndex == -1) || (srcIndex > closingScriptIndex))
                )
            {
                // script has no src so leave it alone
                charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                return true;

            }

            if (
                (closingScriptIndex > -1)
                && ((srcIndex > -1) && (srcIndex < closingScriptIndex))
                )
            {
                // script has a src=

                // neathtml.js must be at the top
                //int indexOfExceptionIdentifier = scr.IndexOf("neathtml", StringComparison.InvariantCultureIgnoreCase);
                indexOfException = GetOffsetNextIndexOfNeatHtml(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }

                indexOfException = GetOffsetNextIndexOfFriendlyUrlSuggest(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }

                indexOfException = GetOffsetNextIndexOfDatePicker(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }

                indexOfException = GetOffsetNextIndexOfGoogleMap(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }

                indexOfException = GetOffsetNextIndexOfGoogleMapUds(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }

                indexOfException = GetOffsetNextIndexOfmojoMap(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }

               


                

                // odiogo must also be at the top
                //indexOfExceptionIdentifier = scr.IndexOf("odiogo", StringComparison.InvariantCultureIgnoreCase);
                indexOfException = GetOffsetNextIndexOfOdiogo(content, pos, closingScriptIndex);

                if ((indexOfException > -1) && (indexOfException < closingScriptIndex))
                {
                    charsToSkip = closingScriptIndex + 9; // </script> = 9 chars
                    return true;
                }



            }




            return false;
        }

        /// <summary>
        /// returns the number of characters from pos to the beginning of the next closing script element
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int GetOffsetOfNextClosingScript(char[] content, int pos)
        {
            // looking for </script>
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;
            int maxSteps = 500;
            while (
                (!found) 
                && (currentPosition < content.Length - 10)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == '<')
                    && (content[currentPosition + 1] == '/')
                    && (content[currentPosition + 2] == 's' || content[currentPosition + 2] == 'S')
                    && (content[currentPosition + 3] == 'c' || content[currentPosition + 3] == 'C')
                    && (content[currentPosition + 4] == 'r' || content[currentPosition + 4] == 'R')
                    && (content[currentPosition + 5] == 'i' || content[currentPosition + 5] == 'I')
                    && (content[currentPosition + 6] == 'p' || content[currentPosition + 6] == 'P')
                    && (content[currentPosition + 7] == 't' || content[currentPosition + 7] == 'T')
                    && (content[currentPosition + 8] == '>')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;
        }

        /// <summary>
        /// returns the number of characters from pos to the beginning of the next src= in the char array
        /// </summary>
        private int GetOffsetNextIndexOfSrc(char[] content, int pos, int maxSteps)
        {
            // looking for src=
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;
           
            while (
                (!found)
                && (currentPosition < content.Length - 10)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 's' || content[currentPosition] == 'S')
                    && (content[currentPosition + 1] == 'r' || content[currentPosition + 1] == 'R')
                    && (content[currentPosition + 2] == 'c' || content[currentPosition + 2] == 'C')
                    && (content[currentPosition + 3] == '=')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        /// <summary>
        /// returns the number of characters from pos to the beginning of the next Sys. in the char array
        /// </summary>
        private int GetOffsetNextIndexOfSysDot(char[] content, int pos, int maxSteps)
        {
            // looking for Sys.
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 10)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'S')
                    && (content[currentPosition + 1] == 'y')
                    &&  (content[currentPosition + 2] == 's')
                    && (content[currentPosition + 3] == '.')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        /// <summary>
        /// returns the number of characters from pos to the beginning of the next WebForm_InitCallback in the char array
        /// </summary>
        private int GetOffsetNextIndexOfWebFormInitCallback(char[] content, int pos, int maxSteps)
        {
            // looking for WebForm_InitCallback
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 18)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'W')
                    && (content[currentPosition + 1] == 'e')
                    && (content[currentPosition + 2] == 'b')
                    && (content[currentPosition + 3] == 'F')
                    && (content[currentPosition + 4] == 'o')
                    && (content[currentPosition + 5] == 'r')
                    && (content[currentPosition + 6] == 'm')
                    && (content[currentPosition + 7] == '_')
                    && (content[currentPosition + 8] == 'I')
                    && (content[currentPosition + 9] == 'n')
                    && (content[currentPosition + 10] == 'i')
                    && (content[currentPosition + 11] == 't')
                    && (content[currentPosition + 12] == 'C')
                    && (content[currentPosition + 13] == 'a')
                    && (content[currentPosition + 14] == 'l')
                    && (content[currentPosition + 15] == 'l')
                    && (content[currentPosition + 16] == 'b')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfYAHOO(char[] content, int pos, int maxSteps)
        {
            // looking for YAHOO
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 10)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'Y')
                    && (content[currentPosition + 1] == 'A')
                    && (content[currentPosition + 2] == 'H')
                    && (content[currentPosition + 3] == 'O')
                    && (content[currentPosition + 4] == 'O')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }


        private int GetOffsetNextIndexOfNeatHtml(char[] content, int pos, int maxSteps)
        {
            // looking for NeatHtml.js
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 12)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'N')
                    && (content[currentPosition + 1] == 'e')
                    && (content[currentPosition + 2] == 'a')
                    && (content[currentPosition + 3] == 't')
                    && (content[currentPosition + 4] == 'H')
                    && (content[currentPosition + 5] == 't')
                    && (content[currentPosition + 6] == 'm')
                    && (content[currentPosition + 7] == 'l')
                    //&& (content[currentPosition + 8] == '.')
                    //&& (content[currentPosition + 9] == 'j')
                    //&& (content[currentPosition + 10] == 's')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfFriendlyUrlSuggest(char[] content, int pos, int maxSteps)
        {
            // looking for friendlyurlsuggest.js
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 12)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'f')
                    && (content[currentPosition + 1] == 'r')
                    && (content[currentPosition + 2] == 'i')
                    && (content[currentPosition + 3] == 'e')
                    && (content[currentPosition + 4] == 'n')
                    && (content[currentPosition + 5] == 'd')
                    && (content[currentPosition + 6] == 'l')
                    && (content[currentPosition + 7] == 'y')
                    && (content[currentPosition + 8] == 'u')
                    && (content[currentPosition + 9] == 'r')
                    && (content[currentPosition + 10] == 'l')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfDatePicker(char[] content, int pos, int maxSteps)
        {
            // looking for DatePicker/
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 12)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'D')
                    && (content[currentPosition + 1] == 'a')
                    && (content[currentPosition + 2] == 't')
                    && (content[currentPosition + 3] == 'e')
                    && (content[currentPosition + 4] == 'P')
                    && (content[currentPosition + 5] == 'i')
                    && (content[currentPosition + 6] == 'c')
                    && (content[currentPosition + 7] == 'k')
                    && (content[currentPosition + 8] == 'e')
                    && (content[currentPosition + 9] == 'r')
                    && (content[currentPosition + 10] == '/')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfOdiogo(char[] content, int pos, int maxSteps)
        {
            // looking for odiogo
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 10)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'o')
                    && (content[currentPosition + 1] == 'd')
                    && (content[currentPosition + 2] == 'i')
                    && (content[currentPosition + 3] == 'o')
                    && (content[currentPosition + 4] == 'g')
                    && (content[currentPosition + 5] == 'o')
                    
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfGA(char[] content, int pos, int maxSteps)
        {
            // looking for gaJsHost
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 10)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'g')
                    && (content[currentPosition + 1] == 'a')
                    && (content[currentPosition + 2] == 'J')
                    && (content[currentPosition + 3] == 's')
                    && (content[currentPosition + 4] == 'H')
                    && (content[currentPosition + 5] == 'o')
                    && (content[currentPosition + 6] == 's')
                    && (content[currentPosition + 7] == 't')

                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfGoogleMap(char[] content, int pos, int maxSteps)
        {
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 12)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'm')
                    && (content[currentPosition + 1] == 'a')
                    && (content[currentPosition + 2] == 'p')
                    && (content[currentPosition + 3] == 's')
                    && (content[currentPosition + 4] == '.')
                    && (content[currentPosition + 5] == 'g')
                    && (content[currentPosition + 6] == 'o')
                    && (content[currentPosition + 7] == 'o')
                    && (content[currentPosition + 8] == 'g')
                    && (content[currentPosition + 9] == 'l')
                    && (content[currentPosition + 10] == 'e')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfGoogleMapUds(char[] content, int pos, int maxSteps)
        {
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 12)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'g')
                    && (content[currentPosition + 1] == 'o')
                    && (content[currentPosition + 2] == 'o')
                    && (content[currentPosition + 3] == 'g')
                    && (content[currentPosition + 4] == 'l')
                    && (content[currentPosition + 5] == 'e')
                    && (content[currentPosition + 6] == '.')
                    && (content[currentPosition + 7] == 'c')
                    && (content[currentPosition + 8] == 'o')
                    && (content[currentPosition + 9] == 'm')
                    && (content[currentPosition + 10] == '/')
                    && (content[currentPosition + 11] == 'u')
                    && (content[currentPosition + 12] == 'd')
                    && (content[currentPosition + 13] == 's')
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }

        private int GetOffsetNextIndexOfmojoMap(char[] content, int pos, int maxSteps)
        {
            bool found = false;
            int currentPosition = pos;
            int stepsAdded = 0;

            while (
                (!found)
                && (currentPosition < content.Length - 12)
                && (stepsAdded <= maxSteps)
                )
            {
                currentPosition += 1;
                stepsAdded += 1;

                if (
                    (content[currentPosition] == 'm')
                    && (content[currentPosition + 1] == 'o')
                    && (content[currentPosition + 2] == 'j')
                    && (content[currentPosition + 3] == 'o')
                    && (content[currentPosition + 4] == 'g')
                    && (content[currentPosition + 5] == 'm')
                    && (content[currentPosition + 6] == 'a')
                    && (content[currentPosition + 7] == 'p')
                    && (content[currentPosition + 8] == 'u')
                    && (content[currentPosition + 9] == 't')
                    && (content[currentPosition + 10] == 'i')
                    
                    )
                {
                    found = true;
                }

            }

            if (found) { return stepsAdded; }

            return -1;

        }
        
        

    }
}
