using System.Linq;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace MiniMode
{
	public class HandlerOfEvents : IEventHandlerWaitingForPlayers,
		IEventHandlerRoundStart,
		IEventHandlerUpdate,
		IEventHandlerDoorAccess
	{
		private readonly MiniModePlugin _plugin;
		private bool _fixedGuardSpawns = false;
		
		public HandlerOfEvents(MiniModePlugin plugin) => this._plugin = plugin;

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			_plugin.ReloadConfig();
			
			if (!_plugin.Enabled)
				_plugin.PluginManager.DisablePlugin(_plugin);
			
			if (!this._fixedGuardSpawns)
			{
				// fix guard spawns

				this._fixedGuardSpawns = true;
			}
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

		public void OnDoorAccess(PlayerDoorAccessEvent ev)
		{
			if (ev.Player.TeamRole.Team == Team.NINETAILFOX ||
			    ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY &&
			    ev.Door == _plugin.Server.Map.GetDoors().Where(x => x.Position.y < 7979).First()) // pseudo code
				ev.Allow = false;
		}
	}
}