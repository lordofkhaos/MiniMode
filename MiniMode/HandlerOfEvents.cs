using System;
using System.Collections.Generic;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using SmodTeam = Smod2.API.Team;

namespace MiniMode
{
	public class HandlerOfEvents : IEventHandlerWaitingForPlayers,
		IEventHandlerRoundStart,
		IEventHandlerUpdate,
	//	IEventHandlerDoorAccess,
		IEventHandlerSetRole
	{
		private readonly MiniModePlugin _plugin;

		public HandlerOfEvents(MiniModePlugin plugin)
		{
			this._plugin = plugin;
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			_plugin.ReloadConfig();
			
			if (!_plugin.Enabled)
				_plugin.PluginManager.DisablePlugin(_plugin);
		}

		public void OnRoundStart(RoundStartEvent ev) =>
			_plugin.Info(ev.Server.NumPlayers > _plugin.MaxPlayers
				? "Max players threshold reached, resuming normal rounds"
				: "Beginning mini round");

		public void OnUpdate(UpdateEvent ev)
		{
			Utilities utilities = new Utilities(_plugin);
			utilities.CheckForEscapees();
		}

		/*public void OnDoorAccess(PlayerDoorAccessEvent ev)
		{
			if (ev.Player.TeamRole.Team == SmodTeam.NINETAILFOX ||
			    ev.Player.TeamRole.Team == SmodTeam.CHAOS_INSURGENCY &&
			    ev.Door == _plugin.Server.Map.GetDoors().Where(x => x.Position.y < 7979).First()) // pseudo code
			{
				ev.Allow = false;
			}
		}*/

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (ev.Role != Role.FACILITY_GUARD) return;
			Random rnd = new Random();
			DateTime toTime = DateTime.UtcNow.AddSeconds(3);
			while (true)
			{
				if (DateTime.UtcNow < toTime)
					continue;
				List<Vector> scientistSpawns = _plugin.Server.Map.GetSpawnPoints(Role.SCIENTIST);
				ev.Player.Teleport(scientistSpawns[rnd.Next(0, scientistSpawns.Count)]);
				break;
			}
		}
	}
}