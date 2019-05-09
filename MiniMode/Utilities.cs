using Smod2.API;
using SmodTeam = Smod2.API.Team;

namespace MiniMode
{
	public class Utilities
	{
		private readonly MiniModePlugin _plugin;
		
		public Utilities(MiniModePlugin plugin)
		{
			this._plugin = plugin;
		}
		
		public void CheckForEscapees()
		{
			foreach (Player player in this._plugin.Server.GetPlayers())
			{
				float yAxisLocation = player.GetPosition().y;
				if (yAxisLocation > 900 && player.TeamRole.Team == (SmodTeam.CLASSD | SmodTeam.SCIENTIST)) // or whatever -> in LCZ
					player.Teleport(player.TeamRole.Team == SmodTeam.CLASSD ? new Vector(0, 0, 0) : new Vector(1, 0, 0)); // placeholder
			}
			
			throw new System.NotImplementedException();
		}
	}
}