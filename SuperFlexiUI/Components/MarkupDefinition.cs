// Author:					(i7MEDIA) Joe Davis
// Created:				    2014-01-06
// Last Modified:			2014-01-07
//
// You must not remove this notice, or any other, from this software.
//


using System;
namespace SuperFlexiUI
{
    public class MarkupDefinition : SuperFlexiDisplaySettings, ICloneable
    {
 
        /// <summary>
        /// Does a shallow copy of MarkupDefinition
        /// </summary>
        /// <returns>object</returns>
        public object Clone()
        {
            MarkupDefinition defintion = (MarkupDefinition)this.MemberwiseClone();
            return defintion;
        }
    }
}