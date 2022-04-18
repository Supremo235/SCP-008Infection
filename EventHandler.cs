using Exiled.Events.Extensions;
using static Exiled.Events.Events;
using CustomPlayerEffects;
using MEC;
using Respawning;
using Exiled.API.Interfaces;
using GameCore;
using HarmonyLib;
using Log = Exiled.API.Features.Log;
using Random = System.Random;
using System.IO;
using Exiled.API.Structs;
using Exiled.API.Enums;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API.Features;
using Exiled.CustomRoles.Events;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace SCP_008Infection
{
    public class EventHandler
    {
        public Plugin plugin;

        private Random Gen = new Random();
        public EventHandler(Plugin plugin) => this.plugin = plugin;

        private System.Random rand = new System.Random();
        public Vector3 InfectedPosition = Vector3.zero;
        public RoleType PacienteRole = RoleType.ClassD;
        public void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (ev.Player.Role.Team == Team.SCP)
            {
                ev.Player.DisableEffect(EffectType.Poisoned);
            }
        }
        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Handler.Type == DamageType.Scp0492)
            {
                if (ev.Target != ev.Attacker)
                {


                    if (Plugin.Instance.Config.ZombieDamage >= 0)
                    {
                        ev.Amount = Plugin.Instance.Config.ZombieDamage;
                    }
                }
                int chance = Gen.Next(1, 100);
                if (chance <= Plugin.Instance.Config.SCP008InfecionChance)

                {
                    ev.Target.DisplayNickname = $"SCP-008-1({ev.Target.Nickname})";
                    ev.Target.EnableEffect(EffectType.Poisoned, 99999f);
                    ev.Target.ShowHint($"{Plugin.Instance.Translation.InfectionHint}", Plugin.Instance.Config.HintTime1);
                    ev.Attacker.ShowHint(Plugin.Instance.Translation.AttackerHintInfecting.Replace("{TargetName}", ev.Target.Nickname), Plugin.Instance.Config.HintTime2);
                }
            }
            if (ev.Handler.Type == DamageType.Poison)
            {

            }
        }
        public void OnDying(DyingEventArgs ev)
        {
            if (Plugin.Instance.Config.Transformation)
            {
                if (ev.Handler.Type == DamageType.Poison)
                {

                    Timing.CallDelayed(0.5f, 0f, () =>
                    { CustomRole.Get("Scp049-2").AddRole(ev.Target); });

                    int chance = Gen.Next(1, 100);
                    Timing.CallDelayed(2.5f, 0f, () =>
                    {
                        ev.Target.Position = InfectedPosition;
                        Timing.CallDelayed(2.6f, () =>
                        {
                            if (chance <= Plugin.Instance.Config.SCP0081Chance)
                            {
                                if (Plugin.Instance.Config.Scp0081 == true)
                                {
                                    CustomRole.Get("008-1").AddRole(ev.Target);
                                }
                            }

                            if (chance <= Plugin.Instance.Config.TankChance)
                            {
                                if (Plugin.Instance.Config.Tank == true)
                                {
                                    CustomRole.Get("Tank").AddRole(ev.Target);
                                }
                            }
                        });


                    });

                }

                if (ev.Handler.Type == DamageType.Scp0492)
                {
                    if (Plugin.Instance.Config.ZombieDamageTransformation == true)
                    {
                        Timing.CallDelayed(0.5f, 0f, () =>
                        { CustomRole.Get("Scp049-2").AddRole(ev.Target); });
                        int chance = Gen.Next(1, 100);
                        Timing.CallDelayed(2.5f, 0f, () =>
                        {
                            ev.Target.Position = InfectedPosition;
                            Timing.CallDelayed(2.6f, () =>
                            {
                                if (chance <= Plugin.Instance.Config.SCP0081Chance)
                                {
                                    if (Plugin.Instance.Config.Scp0081 == true)
                                    {
                                        CustomRole.Get("008-1").AddRole(ev.Target);
                                    }
                                }
                                if (chance <= Plugin.Instance.Config.TankChance)
                                {
                                    if (Plugin.Instance.Config.Tank == true)
                                    {
                                        CustomRole.Get("Tank").AddRole(ev.Target);
                                    }
                                }
                            });


                        });
                    }
                }
            }
        }
    }
}