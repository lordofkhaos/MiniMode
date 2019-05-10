using System.Collections.Generic;
using Smod2.API;
using UnityEngine;
using SmodTeam = Smod2.API.Team;
using Random = System.Random;

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
			Random rnd = new Random();
			List<Vector> scientistSpawns = _plugin.Server.Map.GetSpawnPoints(Role.SCIENTIST);
			foreach (Player player in this._plugin.Server.GetPlayers())
			{
				float yAxisLocation = player.GetPosition().y;
				if (yAxisLocation < 0 || yAxisLocation > 900 ||
				    player.TeamRole.Team != (SmodTeam.CLASSD | SmodTeam.SCIENTIST)) player.Teleport(scientistSpawns[rnd.Next(0, scientistSpawns.Count)]);
				GameObject ply = (GameObject) player.GetGameObject();
				ply.GetComponent<CharacterClassManager>().RegisterEscape();
			}
		}
	}
}