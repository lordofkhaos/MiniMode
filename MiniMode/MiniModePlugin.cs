using Smod2;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.EventHandlers;

namespace MiniMode
{
	[PluginDetails(
		author = "lordofkhaos",
		name = "MiniMode",
		configPrefix = "kmm",
		description = "Create a miniature mode locked down into LCZ",
		id = "com.lordofkhaos.minimode",
		version = "1.0.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
	public class MiniModePlugin : Plugin
	{
		private HandlerOfEvents _eventHandler;
		
		public bool Enabled { get; private set; }
		public int MaxPlayers { get; private set; }
		
		public override void Register()
		{
			this.AddConfig(new ConfigSetting("enable", true, true, 
				"To enable this plugin"));
			this.AddConfig(new ConfigSetting("max_players", 13, true,
				"Up to this number of players for the plugin to be active"));
			
			_eventHandler = new HandlerOfEvents(this);
			this.AddEventHandler(typeof(IEventHandlerWaitingForPlayers), _eventHandler);
			this.AddEventHandler(typeof(IEventHandlerRoundStart), _eventHandler);
			this.AddEventHandler(typeof(IEventHandlerUpdate), _eventHandler);
		}

		public override void OnEnable()
		{
			this.Info(this.Details.name + " has been enabled!");
		}

		public override void OnDisable()
		{
			this.Info(this.Details.name + " has been disabled!");
		}

		public void ReloadConfig()
		{
			this.Enabled = this.GetConfigBool("kmm_enabled");
			this.MaxPlayers = this.GetConfigInt("kmm_max_players");
		}
	}
}