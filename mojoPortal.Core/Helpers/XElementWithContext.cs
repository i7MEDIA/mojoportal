using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Core.Helpers;

public class XElementWithContext<TContext>(XElement element, TContext context)
{
	public XElement Element { get; private set; } = element;
	public TContext Context { get; private set; } = context;

	public XElementWithContext<TNewContext> With<TNewContext>(TNewContext context)
	{
		return new XElementWithContext<TNewContext>(Element, context);
	}

	public XElementWithContext<TContext> ToAttr<TProperty>(Expression<Func<TContext, TProperty>> targetExpression)
	{
		Element.ToAttr(Context, targetExpression);
		return this;
	}

	public XElementWithContext<TContext> FromAttr<TProperty>(Expression<Func<TContext, TProperty>> targetExpression)
	{
		Element.FromAttr(Context, targetExpression);
		return this;
	}
}
