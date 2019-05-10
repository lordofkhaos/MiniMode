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
		private readonly Utilities _utilities;

		public HandlerOfEvents(MiniModePlugin plugin)
		{
			this._plugin = plugin;
			this._utilities = new Utilities(this._plugin);
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			_plugin.ReloadConfig();
			
			if (!_plugin.Enabled)
				_plugin.PluginManager.DisablePlugin(_plugin);
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			if (ev.Server.NumPlayers > _plugin.MaxPlayers)
			{
				_plugin.Info("Max players threshold reached, resuming normal rounds");
				_utilities.Active = false;
			}
			else
			{
				_plugin.Info("Beginning mini round");
				_utilities.Active = true;
			}
		}

		public void OnUpdate(UpdateEvent ev)
		{
			if (_utilities.Active)
				_utilities.CheckForEscapees();
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
			if (!_utilities.Active || ev.Role != Role.FACILITY_GUARD) return;
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