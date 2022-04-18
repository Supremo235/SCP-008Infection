namespace SCP_008Infection
{
    using Exiled.API.Interfaces;

    public class Translation : ITranslation
    {
        public string InfectionHint { get; set; } = "You are poisoned by SCP-008, FIND AN SCP-500 FAST";
        public string AttackerHintInfecting { get; set; } = "You Infected {TargetName} whit SCP-008";
        public string MessageWhenItInfectsYou { get; set; } = "You have died by SCP-008, now you are a zombie";
        public string TankMessage { get; set; } = "You are poisoned by SCP-008, FIND AN SCP-500 FAST";
        public string P0Message { get; set; } = "You are poisoned by SCP-008, FIND AN SCP-500 FAST";
    }
}