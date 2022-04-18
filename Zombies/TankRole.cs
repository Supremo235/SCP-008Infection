using Assets._Scripts.Dissonance;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;
using MEC;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using Random = System.Random;
using Exiled.API.Enums;
using InventorySystem.Items.ThrowableProjectiles;
using System.ComponentModel;

namespace SCP_008Infection
{
    [CustomRole(RoleType.Scp0492)]
    public class TankRole : CustomRole
    {
        private Random Gen = new Random();

        public override uint Id { get; set; } = 102;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 1000;
        [Description("DON'T TOUCH NAME CONFIGS")]
        public override string Name { get; set; } = "Tank";
        public override string Description { get; set; } = "A zombie specialized in damage, with an exaggerated damage";
        public override string CustomInfo { get; set; } = "Tank";
        public override SpawnProperties SpawnProperties { get; set; }
        public override bool KeepInventoryOnSpawn { get; set; } = false;

        public float MovementMultiplier { get; set; } = 0.8f;
        public int Damage = 75;
        public string SpawnHint = "You are an improved version of SCP-049-2, you do brutal damage in exchange for slowness";

        protected override void RoleAdded(Exiled.API.Features.Player player)
        {
            Timing.CallDelayed(5.5f, (Action)(() =>
            {
                player.Health = MaxHealth;
                player.MaxHealth = MaxHealth;
                player.ShowHint($"{SpawnHint}", 14);
                player.ChangeWalkingSpeed(MovementMultiplier);
                player.ChangeRunningSpeed(MovementMultiplier);
                player.IsGodModeEnabled = false;
            }));
            player.Scale = base.Scale;

        }
        protected override void RoleRemoved(Exiled.API.Features.Player player)
        {
            player.Scale = Vector3.one;
            Timing.CallDelayed(1.5f, (Action)(() =>
            {
                player.ChangeWalkingSpeed(1f);
                player.ChangeRunningSpeed(1f);
            }));
            base.RoleRemoved(player);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.UnsubscribeEvents();
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                ev.Amount = 75;
                ev.Target.EnableEffect(EffectType.MovementBoost, 0.5f);
                ev.Target.ChangeEffectIntensity(EffectType.MovementBoost, 255);
            }
        }
    }
}