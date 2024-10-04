#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.Core.Helpers;

public class RoleStringHelper
{
	private const char DELIMTER = ';';
	private readonly HashSet<string> _roles;

	public RoleStringHelper(string? rolesString = null)
	{
		_roles = (rolesString ?? string.Empty)
		.Split([DELIMTER], StringSplitOptions.RemoveEmptyEntries)
		.Select(x => x.Trim())
		.ToHashSet();
	}


	public static RoleStringHelper Parse(string? rolesString = null)
	{
		return new RoleStringHelper(rolesString);
	}


	public IReadOnlyList<string> Roles => _roles.ToList();


	public bool Add(string role)
	{
		if (string.IsNullOrWhiteSpace(role))
		{
			throw new ArgumentException($"{nameof(role)} cannot be null or empty string.", role);
		}

		return _roles.Add(role);
	}


	public void AddRange(IEnumerable<string> roles)
	{
		foreach (string role in roles)
		{
			_roles.Add(role);
		}
	}


	public bool Remove(string role)
	{
		if (string.IsNullOrWhiteSpace(role))
		{
			throw new ArgumentException($"{nameof(role)} cannot be null or empty string.", role);
		}

		return _roles.Remove(role);
	}


	public bool Contains(string role)
	{
		if (string.IsNullOrWhiteSpace(role))
		{
			throw new ArgumentException($"{nameof(role)} cannot be null or empty string.", role);
		}

		return _roles.Contains(role);
	}


	public override string ToString()
	{
		return string.Join(DELIMTER.ToString(), _roles) + ";";
	}
}
