using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;
using HarmonyLib;
using Map = Exiled.Events.Handlers.Map;
using ScpPlayer = Exiled.Events.Handlers.Scp079;
using ScpPlayer049 = Exiled.Events.Handlers.Scp049;
using ScpPlayer106 = Exiled.Events.Handlers.Scp106;
using ServerEvent = Exiled.Events.Handlers.Server;
using HServer = Exiled.Events.Handlers.Server;
using PlayerEvent = Exiled.Events.Handlers.Player;
using Scp079 = Exiled.Events.Handlers.Scp079;
using MEC;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
namespace SCP_008Infection
{
    public class Plugin : Plugin<Config, Translation>
    {
        public static Plugin Instance;

        private Harmony _harmony;
        private string _harmonyId;

        public override string Name => nameof(SCP_008Infection);
        public override string Author => "Supremo";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override string Prefix => "SCP-008Infection";
        public override Version RequiredExiledVersion => new Version(5, 0, 0);
        public EventHandler Handler { get; private set; }

        public override void OnEnabled()
        {
            PlayerEvent.Hurting += Handler.OnHurting;
            PlayerEvent.Dying += Handler.OnDying;
            PlayerEvent.ReceivingEffect += Handler.OnReceivingEffect;
            Handler = new EventHandler(this);
            Plugin.Instance = this;
            _harmony = new Harmony(this._harmonyId);
            _harmony.PatchAll();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvent.Hurting -= Handler.OnHurting;
            PlayerEvent.Dying -= Handler.OnDying;
            PlayerEvent.ReceivingEffect -= Handler.OnReceivingEffect;
            Handler = (EventHandler)null;
            _harmony.UnpatchAll(this._harmonyId);
            base.OnDisabled();
            Plugin.Instance = (Plugin)null;
            Handler = null;
        }
    }
}
