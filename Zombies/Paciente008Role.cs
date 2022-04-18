namespace SCP_008Infection
{
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using PlayerStatsSystem;
using UnityEngine;
using YamlDotNet.Serialization;
using RoleType = RoleType;
using Random = System.Random;
    using System.ComponentModel;

    [CustomRole(RoleType.Scp0492)]

    public class Paciente008Role : CustomRole
    {
        private Random Gen = new Random();

        public override uint Id { get; set; } = 101;
        public override RoleType Role { get; set; } = RoleType.Tutorial;
        public override int MaxHealth { get; set; } = 250;
        [Description("DON'T TOUCH NAME CONFIGS")]
        public override string Name { get; set; } = "008-1";
        public override string Description { get; set; } = "Zombie capable of picking up objects and behaving like a human";
        public override string CustomInfo { get; set; } = "SCP-008-1";
        public RoleType VisibleRole { get; set; } = (RoleType)1;
        public override SpawnProperties SpawnProperties { get; set; }
        public override bool KeepInventoryOnSpawn { get; set; } = true;
        [Description("Chance of infecting someone with 008 by shooting them.")]
        public int ShotInfectionChance = 20;
        [Description("Tickets the Chaos Insurgency gain when SCP-008-1 dies.")]
        public int GainTickesForDiedCI = 20;
        [Description("Tickets the Mobile Task Force gain when SCP-008-1 dies.")]
        public int GainTickesForDiedMTF = 20;
        [Description("Infect chance to the killer.")]
        public int DiedInfectionChance = 20;
        [Description("Cassie on SCP-008-1 died.")]
        public bool CassieEnabled = true;
        [Description("Hint that will come up when you try to use an item you can't, {Object} is the object.")]
        public string DontUsingObject = "You can't use an {Object} because you're an SCP-008-1";
        [Description("Hint that will appear to SCP-008-1 when it spawns.")]
        public string SpawnHint = "You have spawned as an enhanced version of SCP-049-2, your shots can infect humans, do not attempt to attack SCPs they are on your team";
        [Description("Hint that will come out when you get infected by death of SCP-008-1.")]
        public string DiedInfectionHint = "You have been infected with SCP-008 by coming into contact with an SCP-008-1";
        [Description("Hint that appears when infected by SCP-008-1.")]
        public string InfectionWhitShotHint = "You have been infected with SCP-008 by SCP-008-1";
        public float MovementMultiplier { get; set; } = 1.0f;

        protected override void RoleAdded(Exiled.API.Features.Player player)
        {
            player.UnitName = "SCP-008";
            Timing.CallDelayed(5.5f, (() =>
            {
                VisibleRole = Plugin.Instance.Handler.PacienteRole;
                player.ShowHint($"{SpawnHint}", 14);
                player.ChangeAppearance(VisibleRole);
                player.ChangeWalkingSpeed(MovementMultiplier);
                player.ChangeRunningSpeed(MovementMultiplier);
                player.IsGodModeEnabled = false;
            }));
            player.Scale = base.Scale;

        }
        protected override void RoleRemoved(Exiled.API.Features.Player player)
        {
            player.Scale = Vector3.one;
            Timing.CallDelayed(1.5f, (() =>
            {
                player.ChangeWalkingSpeed(1f);
                player.ChangeRunningSpeed(1f);
            }));
            base.RoleRemoved(player);
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.Escaping += OnEscaped;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaped;
            base.UnsubscribeEvents();
        }
        private void OnHurting(HurtingEventArgs ev)
        {

            if (ev.Attacker == null)
            {
                return;
            }


            if (Check(ev.Target))
            { 
                if (ev.Attacker.Role.Team == Team.SCP)
                {
                    ev.IsAllowed = false;
                }

                if (ev.Handler.Type == DamageType.Poison)
                {
                    ev.IsAllowed = false;
                }
            
            }
                if (Check(ev.Attacker))
                {
                    ev.Attacker.Health += 5;

                    if (ev.Target.Role.Team == Team.SCP)
                    {
                    ev.IsAllowed = false;
                    }
                }
        }
        private void OnUsingItem(UsingItemEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (ev.Item.Type == ItemType.SCP500)
                {
                    string message = DontUsingObject.Replace("{Object}", ev.Item.Type.ToString());
                    ev.IsAllowed = false;
                    ev.Player.ShowHint($"{DontUsingObject}", 5);
                }

                if (ev.Item.Type == ItemType.Medkit)
                {
                    string message = DontUsingObject.Replace("{Object}", ev.Item.Type.ToString());
                    ev.IsAllowed = false;
                    ev.Player.ShowHint($"{DontUsingObject}", 5);
                }

                if (ev.Item.Type == ItemType.Adrenaline)
                {
                    string message = DontUsingObject.Replace("{Object}", ev.Item.Type.ToString());
                    ev.IsAllowed = false;
                    ev.Player.ShowHint($"{DontUsingObject}", 5);
                }
            }
        }
        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Target == null && ev.Killer == null)
            {
                return;
            }

            if (Check(ev.Target))
            {

                Respawn.GrantTickets(Respawning.SpawnableTeamType.NineTailedFox ,GainTickesForDiedMTF);
                Respawn.GrantTickets(Respawning.SpawnableTeamType.ChaosInsurgency, GainTickesForDiedCI);
                int chance = Gen.Next(1, 100);
                if (chance <= DiedInfectionChance)
                {
                    ev.Killer.EnableEffect(EffectType.Poisoned, 100000, true);
                    ev.Killer.ShowHint($"{DiedInfectionHint}", 7);
                }
                if (CassieEnabled == true)
                {
                    if (ev.Killer.Role == RoleType.Scientist)
                    {
                        Cassie.Message($"SCP 0 0 8 . 1 Terminated by scientist personnel", true, true, true);
                    }
                    if (ev.Killer.Role == RoleType.ClassD)
                    {
                        Cassie.Message($"SCP 0 0 8 . 1 Terminated by class d personnel", true, true, true);
                    }
                    if (SerpentsHand.API.IsSerpent(ev.Killer))
                    {
                        Cassie.Message($"SCP 0 0 8 . 1 Terminated by serpents hand personnel", true, true, true);
                    }
                    if (UIURescueSquad.API.IsUiu(ev.Killer))
                    {
                        Cassie.Message($"SCP 0 0 8 . 1 contained successfully Containment U I U personnel unit {ev.Killer.UnitName}", true, true, true);
                    }
                    if (ev.Killer.Role.Team == Team.MTF)
                    {
                        Cassie.Message($"SCP 0 0 8 . 1 contained successfully Containment MtfUnit {ev.Killer.UnitName}", true, true, true);
                    }
                    if (ev.Killer.Role.Team == Team.CHI)
                    {
                        Cassie.Message($"SCP 0 0 8 . 1 Terminated by Chaos Insurgency Personnel", true, true, true);
                    }
                }
            }
        }
        private void OnShot(ShotEventArgs ev)
        {
            if (ev.Target == null && ev.Shooter == null)
            {
                return;
            }

            if (Check(ev.Shooter))
            {
                if (ev.Target.IsHuman)
                {

                
                int chance = Gen.Next(1, 100);
                    if (chance <= ShotInfectionChance)
                    {
                        ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Poisoned, 9999999f, true);
                        ev.Target.ShowHint($"<color=red>Te infecto con el SCP-008 {ev.Shooter.Nickname}\n¡CURATE CON UN SCP-500 RAPIDO!</color>", 10);
                        ev.Shooter.ShowHint($"<color=mint>Infectaste con el SCP-008 a {ev.Target.Nickname}</color>", 8);
                    }
                }
            }
        
        }
        private void OnEscaped(EscapingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }
    }
}


