using System;
namespace SuperFlexiUI;

public class MarkupDefinition : SuperFlexiDisplaySettings, ICloneable
{
	/// <summary>
	/// Creates a shallow copy of MarkupDefinition
	/// </summary>
	/// <returns>object</returns>
	public object Clone()
	{
		MarkupDefinition defintion = (MarkupDefinition)this.MemberwiseClone();
		return defintion;
	}
}