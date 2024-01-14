using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace mojoPortal.Web.UI;
//todo: remove SlidePanel
/// <summary>
/// A wrapper control for http://malsup.com/jquery/cycle/
/// Using a panel which renders as a div any first child elements of the panel can be cycled
/// </summary>
[Obsolete()]
public class SlidePanel : BasePanel
{
	/// <summary>
	/// true by default, set to false and the slideshow is disabled and behavior is like a normal <asp:Panel
	/// </summary>
	public bool EnableSlideShow { get; set; } = true;

	/// <summary>
	/// name of transition effect (or comma separated names, ex: fade,scrollUp,scrollDown,scrollRight,scrollLeft,turnUp,turnDown,turnRight,turnLeft,curtainX,curtainY,slideX,slideY,shuffle,pinch,zoom) 
	/// </summary>
	public string TransitionEffect { get; set; } = "fade";

	/// <summary>
	/// speed of the transition (any valid fx speed value) 
	/// </summary>
	public int Speed { get; set; } = 1000;

	/// <summary>
	/// speed of the 'in' transition 
	/// </summary>
	public int SpeedIn { get; set; } = 0;

	/// <summary>
	/// speed of the 'out' transition 
	/// </summary>
	public int SpeedOut { get; set; } = 0;

	/// <summary>
	/// milliseconds between slide transitions (0 to disable auto advance) 
	/// </summary>
	public int TransitionInterval { get; set; } = 3000;

	/// <summary>
	/// callback for determining per-slide timeout value:  function(currSlideElement, nextSlideElement, options, forwardFlag)
	/// </summary>
	public string TransitionIntervalCallback { get; set; } = string.Empty;

	/// <summary>
	/// true to enable "pause on hover" 
	/// </summary>
	public bool PauseOnHover { get; set; } = false;

	/// <summary>
	/// true to start next transition immediately after current one completes 
	/// </summary>
	public bool Continuous { get; set; } = false;

	/// <summary>
	/// selector for element to use as click trigger for next slide 
	/// </summary>
	public string NextSelector { get; set; } = string.Empty;

	/// <summary>
	/// selector for element to use as click trigger for previous slide
	/// </summary>
	public string PreviousSelector { get; set; } = string.Empty;

	/// <summary>
	/// callback fn for prev/next clicks:  function(isNext, zeroBasedSlideIndex, slideElement) 
	/// </summary>
	public string PrevNextClick { get; set; } = string.Empty;

	/// <summary>
	/// selector for element to use as pager container 
	/// </summary>
	public string Pager { get; set; } = string.Empty;

	public bool PagerBefore { get; set; } = false;

	/// <summary>
	/// callback fn for pager clicks:  function(zeroBasedSlideIndex, slideElement)
	/// </summary>
	public string PagerClick { get; set; } = string.Empty;

	/// <summary>
	/// name of event which drives the pager navigation, default is click
	/// </summary>
	public string PagerEvent { get; set; } = "click";

	/// <summary>
	/// callback fn for building anchor links:  function(index, DOMelement)
	/// </summary>
	public string PagerAnchorBuilder { get; set; } = string.Empty;

	/// <summary>
	/// transition callback (scope set to element to be shown):     function(currSlideElement, nextSlideElement, options, forwardFlag) 
	/// </summary>
	public string Before { get; set; } = string.Empty;

	/// <summary>
	/// transition callback (scope set to element that was shown):  function(currSlideElement, nextSlideElement, options, forwardFlag)
	/// </summary>
	public string After { get; set; } = string.Empty;

	/// <summary>
	/// callback invoked when the slideshow terminates (use with autostop or nowrap options): function(options) 
	/// </summary>
	public string End { get; set; } = string.Empty;

	// easing method for both in and out transitions 
	private string easing = string.Empty;
	// easing for "in" transition 
	private string easeIn = string.Empty;
	// easing for "out" transition
	private string easeOut = string.Empty;

	/// <summary>
	/// coords for shuffle animation, ex: { top:15, left: 200 } 
	/// </summary>
	public string Shuffle { get; set; } = string.Empty;

	/// <summary>
	/// properties that define how the slide animates in
	/// </summary>
	public string AnimIn { get; set; } = string.Empty;

	/// <summary>
	/// properties that define how the slide animates out
	/// </summary>
	public string AnimOut { get; set; } = string.Empty;

	/// <summary>
	/// properties that define the initial state of the slide before transitioning in
	/// </summary>
	public string CssBefore { get; set; } = string.Empty;

	/// <summary>
	/// properties that defined the state of the slide after transitioning out
	/// </summary>
	public string CssAfter { get; set; } = string.Empty;

	/// <summary>
	/// function used to control the transition: function(currSlideElement, nextSlideElement, options, afterCalback, forwardFlag) 
	/// </summary>
	public string FxFn { get; set; } = string.Empty;

	/// <summary>
	/// container height default is auto
	/// </summary>
	public string ContainerHeight { get; set; } = "auto";

	/// <summary>
	/// zero-based index of the first slide to be displayed
	/// </summary>
	public int StartingSlide { get; set; } = 0;

	/// <summary>
	/// true if in/out transitions should occur simultaneously default is true
	/// </summary>
	public bool Sync { get; set; } = true;

	/// <summary>
	/// true for random, false for sequence (not applicable to shuffle fx) 
	/// </summary>
	public bool Random { get; set; } = false;

	/// <summary>
	/// force slides to fit container default is false
	/// </summary>
	public bool Fit { get; set; } = false;

	/// <summary>
	/// resize container to fit largest slide default is true
	/// </summary>
	public bool ContainerResize { get; set; } = true;

	/// <summary>
	/// true to pause when hovering over pager link default is false
	/// </summary>
	public bool PauseOnPagerHover { get; set; } = false;

	/// <summary>
	/// true to end slideshow after X transitions (where X == slide count), default is false
	/// </summary>
	public bool Autostop { get; set; } = false;

	/// <summary>
	/// number of transitions (optionally used with autostop to define X)
	/// </summary>
	public int AutostopCount { get; set; } = 0;

	/// <summary>
	/// additional delay (in ms) for first transition (hint: can be negative)
	/// </summary>
	public int Delay { get; set; } = 0;

	/// <summary>
	/// expression for selecting slides (if something other than all children is required) 
	/// </summary>
	public string SlideExpr { get; set; } = string.Empty;

	/// <summary>
	/// set to true to disable extra cleartype fixing (leave false to force background color setting on slides)
	/// </summary>
	public bool CleartypeNoBg { get; set; } = false;

	/// <summary>
	/// true to prevent slideshow from wrapping
	/// </summary>
	public bool Nowrap { get; set; } = false;

	/// <summary>
	/// force fast transitions when triggered manually (via pager or prev/next); value == time in ms 
	/// </summary>
	public int FastOnEvent { get; set; } = 0;

	/// <summary>
	/// valid when multiple effects are used; true to make the effect sequence random 
	/// </summary>
	public bool RandomizeEffects { get; set; } = true;

	/// <summary>
	/// causes animations to transition in reverse 
	/// </summary>
	public bool ReverseTransitions { get; set; } = false;

	/// <summary>
	/// causes manual transition to stop an active transition instead of being ignored
	/// </summary>
	public bool ManualTrump { get; set; } = true;

	/// <summary>
	/// requeue the slideshow if any image slides are not yet loaded default is true
	/// </summary>
	public bool RequeueOnImageNotLoaded { get; set; } = true;

	public int RequeueTimeout { get; set; } = 250;

	/// <summary>
	/// this is if you want to supplement the id selector so the slide container is not the outer div but any div inside the outer div with a particular css class.
	/// </summary>
	public string SlideContainerClass { get; set; } = string.Empty;

	/// <summary>
	/// true is the default behavior but it interferes with clicking links within the slide
	/// added 2013-07-04 based on this forum thread: https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11870~1#post49373
	/// </summary>
	public bool EnableSlideClick { get; set; } = true;


	private void SetupInstanceScript()
	{
		StringBuilder script = new StringBuilder();
		script.Append("\n<script type=\"text/javascript\">");

		//this CDATA section prevents html validation errors from the markup in the javascript
		script.Append("\n //<![CDATA[ \n");

		if (SlideContainerClass.Length > 0)
		{
			script.Append("$('#" + this.ClientID + "  div." + SlideContainerClass + "')");

			if (Pager.Length > 0 && PagerBefore)
			{
				script.Append(".before('<div id=\"" + this.ClientID + Pager + "\" class=\"cyclenav\"></div>').cycle({");
			}
			else if (Pager.Length > 0)
			{
				script.Append(".after('<div id=\"" + this.ClientID + Pager + "\" class=\"cyclenav\"></div>').cycle({");
			}
			else
			{
				script.Append(".cycle({");
			}
		}
		else
		{
			script.Append("$('#" + this.ClientID + "')");

			if (Pager.Length > 0 && PagerBefore)
			{
				script.Append(".before('<div id=\"" + this.ClientID + Pager + "\" class=\"cyclenav\"></div>').cycle({");
			}
			else if (Pager.Length > 0)
			{
				script.Append(".after('<div id=\"" + this.ClientID + Pager + "\" class=\"cyclenav\"></div>').cycle({");
			}
			else
			{
				script.Append(".cycle({");
			}
		}

		script.Append("fx:'" + TransitionEffect + "'");
		script.Append(",speed:" + Speed.ToInvariantString());
		if (SpeedIn > 0) { script.Append(",speedIn:" + SpeedIn.ToInvariantString()); }
		if (SpeedOut > 0) { script.Append(",speedOut:" + SpeedOut.ToInvariantString()); }
		script.Append(",timeout:" + TransitionInterval.ToInvariantString());
		if (NextSelector.Length > 0)
		{
			script.Append(",next:'" + NextSelector + "'");
		}
		else
		{
			if (SlideContainerClass.Length > 0)
			{
				script.Append(",next:'#" + this.ClientID + " > div." + SlideContainerClass + "'");
			}
			else
			{
				script.Append(",next:'#" + this.ClientID + "'");
			}
		}
		if (PreviousSelector.Length > 0) { script.Append(",prev:'" + PreviousSelector + "'"); }
		if (PrevNextClick.Length > 0) { script.Append(",prevNextClick:" + PrevNextClick); }
		if (Pager.Length > 0) { script.Append(",pager:'#" + this.ClientID + Pager + "'"); }
		if (PagerClick.Length > 0) { script.Append(",pagerClick:" + PagerClick); }
		if (PagerEvent != "click") { script.Append(",pagerEvent:'" + PagerEvent + "'"); }
		if (PagerAnchorBuilder.Length > 0) { script.Append(",pagerAnchorBuilder:" + PagerAnchorBuilder); }
		if (Before.Length > 0) { script.Append(",before:" + Before); }
		if (After.Length > 0) { script.Append(",after:" + After); }
		if (End.Length > 0) { script.Append(",end:" + End); }
		if (Shuffle.Length > 0) { script.Append(",shuffle:" + Shuffle); }
		if (AnimIn.Length > 0) { script.Append(",animIn:" + AnimIn); }
		if (AnimOut.Length > 0) { script.Append(",animOut:" + AnimOut); }
		if (CssBefore.Length > 0) { script.Append(",cssBefore:" + CssBefore); }
		if (CssAfter.Length > 0) { script.Append(",cssAfter:" + CssAfter); }
		if (FxFn.Length > 0) { script.Append(",fxFn:" + FxFn); }
		if (ContainerHeight != "auto") { script.Append(",height:'" + ContainerHeight + "'"); }
		if (StartingSlide > 0) { script.Append(",startingSlide:" + StartingSlide.ToInvariantString()); }
		if (!Sync) { script.Append(",sync:0"); }
		if (Random) { script.Append(",random:1"); }
		if (Fit) { script.Append(",fit:1"); }
		if (!ContainerResize) { script.Append(",containerResize:0"); }
		if (PauseOnHover) { script.Append(",pause:1"); }
		if (PauseOnPagerHover) { script.Append(",pauseOnPagerHover:1"); }
		if (Autostop) { script.Append(",pauseOnPagerHover:1"); }
		if (AutostopCount > 0) { script.Append(",autostopCount:" + AutostopCount.ToInvariantString()); }
		if (Delay != 0) { script.Append(",delay:" + Delay.ToInvariantString()); }
		if (Continuous) { script.Append(",autostop:1"); }
		if (SlideExpr.Length > 0) { script.Append(",slideExpr:'" + SlideExpr + "'"); }
		if (CleartypeNoBg) { script.Append(",cleartypeNoBg:true"); }
		if (Nowrap) { script.Append(",nowrap:1"); }
		if (FastOnEvent > 0) { script.Append(",fastOnEvent:" + FastOnEvent.ToInvariantString()); }
		if (!RandomizeEffects) { script.Append(",randomizeEffects:0"); }
		if (ReverseTransitions) { script.Append(",rev:1"); }
		if (!ManualTrump) { script.Append(",manualTrump:0"); }
		if (!RequeueOnImageNotLoaded) { script.Append(",requeueOnImageNotLoaded:false"); }
		if (RequeueTimeout != 250) { script.Append(",requeueTimeout:" + RequeueTimeout.ToInvariantString()); }
		if (TransitionIntervalCallback.Length > 0) { script.Append(",timeoutFn:" + TransitionIntervalCallback); }
		//script.Append("})");
		script.Append("});");

		if (!EnableSlideClick)
		{

			script.Append("$('#" + this.ClientID + "').off('click');");

		}

		script.Append("  \n //]]> \n");
		script.Append(" </script>");

		this.Page.ClientScript.RegisterStartupScript(
			this.GetType(),
			this.UniqueID,
			script.ToString());

	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (!EnableSlideShow) { return; }

		ValidateSettings();
		SetupMainScript();
		SetupInstanceScript();
	}

	private void ValidateSettings()
	{
		string allowedTransitions = "fade,scrollUp,scrollDown,scrollRight,scrollLeft,turnUp,turnDown,turnRight,turnLeft,curtainX,curtainY,slideX,slideY,shuffle,pinch,zoom";
		List<string> allowed = allowedTransitions.SplitOnChar(',');
		List<string> supplied = TransitionEffect.SplitOnCharAndTrim(',');
		StringBuilder cleanTranstions = new StringBuilder();
		string comma = string.Empty;
		foreach (string t in supplied)
		{
			if (allowed.Contains(t))
			{
				cleanTranstions.Append(comma + t);
				comma = ",";
			}
		}

		TransitionEffect = cleanTranstions.ToString();
		if (TransitionEffect.Length == 0) { TransitionEffect = "fade"; }

	}


	private void SetupMainScript()
	{

		//Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
		//        "jqcycle", "\n<script  src=\""
		//        + Page.ResolveUrl("~/ClientScript/jqmojo/cycle.js") + "\" type=\"text/javascript\" ></script>");

		Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
				"jqcycle", "\n<script src=\""
				+ Page.ResolveUrl("~/ClientScript/jqmojo/jquery.cycle.all.min.js") + "\" type=\"text/javascript\"></script>");

	}
}
