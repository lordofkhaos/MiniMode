using Smod2.API;
using SmodTeam = Smod2.API.Team;
using UnityEngine;

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
				if (!(yAxisLocation > 0) || !(yAxisLocation < 900) ||
				    player.TeamRole.Team != (SmodTeam.CLASSD | SmodTeam.SCIENTIST)) continue;
				GameObject ply = (GameObject) player.GetGameObject();
				ply.GetComponent<CharacterClassManager>().RegisterEscape();
			}
		}
	}
}