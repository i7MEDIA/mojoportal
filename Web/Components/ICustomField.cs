using System.Collections.Generic;

namespace mojoPortal.Web.UI;

public interface ICustomField
{
	void SetValue(string val);
	string GetValue();

	//void Attributes(string attribs);
	void Attributes(IDictionary<string, string> attribs);
}