#nullable enable
using mojoPortal.Web.UI.Pages;
using System;

namespace mojoPortal.Web.Plugins;

public interface IPluginMigrationService
{
	/// <summary>
	/// Executes schema installation, version-aware upgrade scripts, and code-first ContentFeature registration
	/// for all enabled plugins in ~/Plugins/.
	/// </summary>
	/// <param name="writeOutput">Callback to write setup status messages to the Setup page.</param>
	/// <param name="canAlterSchema">Whether the database user can alter schema.</param>
	void RunMigrations(Action<SetupStatus, string> writeOutput, bool canAlterSchema);
}
