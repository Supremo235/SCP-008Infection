using System.ComponentModel;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace SCP_008Infection
{
    public sealed class Config : IConfig
    {
        [Description("The plugin is enabled?.")]
        public bool IsEnabled { get; set; } = true;

        [Description("if he can cast the role of SCP-008-1 when transformed by SCP-008 or killed by a zombie with a slim chance?.")]
        public bool Scp0081 { get; set; } = true;

        [Description("if he can play the role of TANK when transformed by SCP-008 or killed by a zombie with a small chance?.")]
        public bool Tank { get; set; } = true;

        [Description("transform if killed by a zombie.")]
        public bool ZombieDamageTransformation { get; set; } = true;
        [Description("If you transform people into SCP-049-2 after dying from SCP-008 or SCP-049-2.")]
        public bool Transformation { get; set; } = true;
        [Description("if it will transform people into SCP-008-1 when it dies by SCP-008 with a slim chance.")]
        public bool Transform0081bySCP008 { get; set; } = true;

        [Description("SCP-008-1 Chance.")]
        public int SCP0081Chance { get; set; } = 20;

        [Description("Tank Chance.")]
        public int TankChance { get; set; } = 10;

        [Description("SCP-049-2 Damage.")]
        public ushort ZombieDamage { get; set; } = 40;

        [Description("Probability of being infected from SCP-008 by an SCP-049-2.")]
        public ushort SCP008InfecionChance { get; set; } = 40;

        [Description("hint time that appears when you get infected.")]
        public int HintTime1 { get; set; } = 5;

        [Description("Time of the hint that appears when I infect someone.")]
        public ushort HintTime2 { get; set; } = 7;

        [Description("Hint Time of Died by SCP-008.")]
        public ushort HintTime3 { get; set; } = 7;
    }
}