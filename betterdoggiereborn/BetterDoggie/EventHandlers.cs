using System;
using UnityEngine;
using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MEC;
using PlayerRoles;

namespace BetterDoggie
{

    public static class EventHandlers
    {
        private static Config _config = BetterDoggie.Singleton.Config;
        private static Dictionary<Player, CoroutineHandle?> _activeAbilities = BetterDoggie.Singleton.ActiveAbilities;

        public static void OnChangingRoles(ChangingRoleEventArgs ev)
        {
            var player = ev.Player;

            // When 939 dies change the size back to normal
            if (ev.NewRole == RoleTypeId.Scp939)
                player.Scale = new Vector3(1, 1, 1);

            Timing.CallDelayed(2f, () =>
            {
                if (player == null || !Is939(player.Role)) return;

                player.Broadcast(_config.SpawnBroadcast);
                player.Broadcast(new Exiled.API.Features.Broadcast("<color=red>Remember to set your ability keybind! (.doggiehelp for help)</color>"));
                player.ShowHint(_config.KeybindHint, _config.KeybindHintShowDuration);

                player.Health = _config.DoggieHealth;
                player.MaxHealth = _config.DoggieHealth;
                player.ArtificialHealth = _config.DoggieAhp;
                player.MaxArtificialHealth = _config.DoggieAhp;

                player.Scale = _config.DoggieScale;

                if (_config.ColaSpeedBoost <= 0) return;
                player.EnableEffect<MovementBoost>();
                player.ChangeEffectIntensity<MovementBoost>(_config.ColaSpeedBoost);
            });
        }

        public static void OnHurtingPlayer(HurtingEventArgs ev)
        {
            var attacker = ev.Attacker;

            if (attacker == null || ev.Player == null || attacker == ev.Player || !Is939(attacker.Role.Type))
                return;

            var maxHume = BetterDoggie.Singleton.Config.DoggieAhp;
            ev.Amount = BetterDoggie.Singleton.Config.BaseDamage +
                        Math.Abs(ev.Attacker.ArtificialHealth - maxHume) / maxHume * BetterDoggie.Singleton.Config.MaxDamageBoost;

            attacker.EnableEffect<Sinkhole>(_config.SlowdownDuration, _config.ShouldSlowdownStack);
            attacker.ChangeEffectIntensity<Sinkhole>(2);
        }

        public static void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            var player = ev.Player;

            if (!_config.EnableDogDoorBusting)
                return;

            if (!Is939(player.Role)
                || (ev.Door is Exiled.API.Interfaces.IDamageableDoor door && door.IsDestroyed)
                || (ev.Door.Base is PryableDoor gate && gate.IsConsideredOpen()))
                return;

            if (_activeAbilities.ContainsKey(player) && _activeAbilities[player] == null)
            {
                _activeAbilities[player] = Timing.RunCoroutine(DoorBustingCooldown(player));

                BustDoor(ev.Door, player, _config.EnableBustSpeedBoost);
            }
        }

        public static IEnumerator<float> DoorBustingCooldown(Player player)
        {
            for(int i = _config.DoorBustingCooldown; i > 0; i--)
            {
                yield return Timing.WaitForSeconds(1f);

                player.ShowHint($"Door bust on cooldown for {i} more seconds.", 1);
            }

            _activeAbilities[player] = null;
        }

        /// <summary>
        /// Busts down a door and applies effect
        /// </summary>
        /// <param name="door"></param>
        /// <param name="ply"></param>
        /// <param name="speedBoost"></param>
        private static void BustDoor(Door door, Player ply, bool speedBoost)
        {
            switch (door)
            {
                case Exiled.API.Interfaces.IDamageableDoor damage:
                    damage.Damage(damage.Health, DoorDamageType.Scp096);
                    break;
        
                case Gate gate:
                    gate.TryPry();
                    break;
            }

            if (!speedBoost)
                return;
    
            ply.ChangeEffectIntensity<MovementBoost>(_config.BustBoostAmount);
            Timing.CallDelayed(2f, () => ply.ChangeEffectIntensity<MovementBoost>(_config.ColaSpeedBoost));
        }


        
        /// <summary>
        /// Check if player is 939
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private static bool Is939(RoleTypeId role)
        {
            return role == RoleTypeId.Scp939;
        }
    }
}