using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
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
using Random = System.Random;
using Exiled.API.Enums;
using System.ComponentModel;

namespace SCP_008Infection
{
    [CustomRole(RoleType.Scp0492)]
    public class Scp0492por008Role : CustomRole
    {
        private Random Gen = new Random();

        public override uint Id { get; set; } = 100;
        public override RoleType Role { get; set; } = RoleType.Scp0492;
        public override int MaxHealth { get; set; } = 750;
        [Description("DON'T TOUCH NAME CONFIGS")]
        public override string Name { get; set; } = "Scp049-2";
        public override string Description { get; set; } = "A person transformed into SCP-049-2 due to SCP-008";
        public override string CustomInfo { get; set; } = "Scp049-2";
        public override SpawnProperties SpawnProperties { get; set; }
        public override bool KeepInventoryOnSpawn { get; set; } = false;

        public float MovementMultiplier { get; set; } = 1.0f;

        protected override void RoleAdded(Exiled.API.Features.Player player)
        {
            Timing.CallDelayed(5.5f, (Action)(() =>
            {
                CustomInfo = $"AHP: {player.ArtificialHealth}/{player.MaxArtificialHealth}\nSCP-008-1";
                player.ShowHint($"{Plugin.Instance.Translation.MessageWhenItInfectsYou}", Plugin.Instance.Config.HintTime3);
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
    }
}