using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperFlexiUI.Controls;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperFlexiUI
{
    public class SampleSuperFlexi : ISuperFlexiBase
    {
        Guid ISuperFlexiBase.DefGuid { get { return Guid.Parse("9a0be965-4019-4dc5-9464-1c4f623012ef"); } }
        String ISuperFlexiBase.Name { get { return "My Cool SuperFlexi Thing"; } }

        [Display(Order = 200, Name = "The Title", Prompt = "Fill me Up Buttercup", GroupName = "Group1")]
        public TextBox Title { get; set; }

        [Display(Order = 201, Name = "Subtitles are spiffy", Prompt = "I prefer monogamy", GroupName = "Group1")]
        public TextBox SubTitle { get; set; }

        [Display(Order = 500, Name = "Item Sort Rank")]
        int ISuperFlexiBase.SortOrder { get; set; }

    }
}