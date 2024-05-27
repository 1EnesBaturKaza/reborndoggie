namespace BetterDoggie
{
    using UnityEngine;
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using Exiled.API.Features;
    
    public class Config : IConfig
    {
        
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("debug?")]
        public bool Debug { get; set; }

        [Description("The max HP that dog will spawn with.")]
        public int DoggieHealth { get; set; } = 1500;
        
        [Description("The maximum AHP the dog can spawn with.")]
        public int DoggieAhp { get; set; } = 600;

        [Description("Should the dog get the a speed boost? (Set to 0 or less to disable)")]
        public byte ColaSpeedBoost { get; set; } = 20;

        [Description("The duration the dog should get slowed down when attacking.")]
        public float SlowdownDuration { get; set; } = 1.5f;

        [Description("Should the slowdown time stack for each attack the dog does? (Add X seconds to slowdown versus just resetting it to X seconds)")]
        public bool ShouldSlowdownStack { get; set; } = true;

        [Description("The size of the dog when it spawns.")]
        public Vector3 DoggieScale { get; set; } = new Vector3(.85f, .85f, .85f);

        [Description("The base amount of damage the dog will do.")]
        public float BaseDamage { get; set; } = 40f;

        [Description("The maximum amount of additional damage the dog can deal.")]
        public float MaxDamageBoost { get; set; } = 75f;

        [Description("Message to send to players when they spawn as the dog.")]
        public Broadcast SpawnBroadcast { get; set; } = new Broadcast(
            "<color=orange>You have spawned as an <color=red>upgraded</color> SCP-939! You run <color=red>faster</color> but slow down when you attack! " +
            "You can also bust down doors and pry gates when your Hume shield is below 50!</color>", 8);
        
        public Broadcast abilitykeybind { get; set; } = new Broadcast(
            "<color=orange>You have spawned as an <color=red>upgraded</color> SCP-939! You run <color=red>faster</color> but slow down when you attack! " +
            "You can also bust down doors and pry gates when your Hume shield is below 50!</color>", 8);

        [Description("Hint to show players to set their keybinds when they spawn.")]
        public string KeybindHint { get; set; } = "Upgraded SCP-939s have a boost ability that <color=orange>temporarily grants the ability to break down doors.</color>" +
            "To use this ability, you must set a <color=orange>keybind in your console (~ key) with the format: \"cmdbind <keycode> .doggieboost\"";

        [Description("door bust cooldown text text")]
        public string cooldowntext { get; set; } = $"Door bust on cooldown for %i% more seconds.";
        
        [Description("help command text")]
        public string helpcommandtext { get; set; } = "Upgraded SCP-939s have a boost ability that temporarily grants the ability to break down doors. \nTo use this ability, you must set a keybind in your console (~ key) with the format: cmdbind <keycode> .doggieboost \nFor example: cmdbind f .doggieboost will bind your F key to the .doggieboost command.";

        [Description("Door busting reactivation text")]
        public string doorreactivatetext { get; set; } = $"Door busting ability can be re-activated in %i% seconds.";

        public int KeybindHintShowDuration { get; set; } = 20;

        [Description("Can 939 bust open doors and gates if it is below a certain AHP?")]
        public bool EnableDogDoorBusting { get; set; } = true;

        [Description("The text when door bust ability is activated")]
        public string doorbusttextability { get; set; } = "<color=green>Door busting ability activated.";

        [Description("The cooldown between enabling / disabling the door busting ability.")]
        public int DoorBustingCooldown { get; set; } = 15;

        [Description("Gives 939 a speed boost when it busts down a door.")]
        public bool EnableBustSpeedBoost { get; set; } = true;

        [Description("The speed boost the dog gets when it busts down a door.")]
        public byte BustBoostAmount { get; set; } = 50;
    }
}
