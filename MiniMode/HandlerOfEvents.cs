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
#if DEBUG
			this._plugin.Debug("HandlerOfEvents(MiniModePlugin plugin) fired successfully");
#endif
			this._plugin = plugin;
			this._utilities = new Utilities(this._plugin);
#if DEBUG
			this._plugin.Debug("HandlerOfEvents(MiniModePlugin plugin) ended successfully");
#endif
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
#if DEBUG
			this._plugin.Debug("void OnWaitingForPlayers(WaitingForPlayersEvent ev) fired successfully");
#endif
			_plugin.ReloadConfig();
			
			if (!_plugin.Enabled)
				_plugin.PluginManager.DisablePlugin(_plugin);
#if DEBUG
			this._plugin.Debug("void OnWaitingForPlayers(WaitingForPlayersEvent ev) ended successfully");
#endif
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
#if DEBUG
			this._plugin.Debug("void OnRoundStart(RoundStartEvent ev) fired successfully");
#endif
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

#if DEBUG
			this._plugin.Debug("void OnRoundStart(RoundStartEvent ev) ended successfully");
#endif
		}

		public void OnUpdate(UpdateEvent ev)
		{
#if DEBUG
			this._plugin.Debug("void OnUpdate(UpdateEvent ev) fired successfully");
#endif
			if (_utilities.Active)
				_utilities.CheckForEscapees();
#if DEBUG
			this._plugin.Debug("void OnUpdate(UpdateEvent ev) ended successfully");
#endif
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
#if DEBUG
			this._plugin.Debug("void OnSetRole(PlayerSetRoleEvent ev) fired successfully");
#endif

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

#if DEBUG
			this._plugin.Debug("void OnSetRole(PlayerSetRoleEvent ev) ended successfully");
#endif
		}
	}
}