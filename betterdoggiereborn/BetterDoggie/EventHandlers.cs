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
        private static Config _config => BetterDoggie.Singleton.Config;
        private static Dictionary<Player, CoroutineHandle?> _activeAbilities = BetterDoggie.Singleton.ActiveAbilities;

        public static void OnChangingRoles(ChangingRoleEventArgs ev)
        {
            // When 939 dies change the size back to normal
            if (ev.Player.Role.Type == RoleTypeId.Scp939)
                ev.Player.Scale = Vector3.one;

            Timing.CallDelayed(2f, () =>
            {
                if (ev.Player == null || !API.is939(ev.Player.Role)) return;
                ev.Player.Broadcast(_config.SpawnBroadcast);
                ev.Player.Broadcast(_config.abilitykeybind);
                ev.Player.ShowHint(_config.KeybindHint, _config.KeybindHintShowDuration);
                ev.Player.Health = _config.DoggieHealth;
                ev.Player.MaxHealth = _config.DoggieHealth;
                ev.Player.ArtificialHealth = _config.DoggieAhp;
                ev.Player.MaxArtificialHealth = _config.DoggieAhp;
                ev.Player.Scale = _config.DoggieScale;
                if (_config.ColaSpeedBoost <= 0) return;
                ev.Player.EnableEffect<MovementBoost>();
                ev.Player.ChangeEffectIntensity<MovementBoost>(_config.ColaSpeedBoost);
            });
        }

        public static void OnHurtingPlayer(HurtingEventArgs ev)
        {
            var attacker = ev.Attacker;
            if (attacker == null || ev.Player == null || attacker == ev.Player || !API.is939(attacker.Role.Type))
                return;
            var maxHume = BetterDoggie.Singleton.Config.DoggieAhp;
            ev.Amount = BetterDoggie.Singleton.Config.BaseDamage +
                        Math.Abs(ev.Attacker.ArtificialHealth - maxHume) / maxHume *
                        BetterDoggie.Singleton.Config.MaxDamageBoost;
            attacker.EnableEffect<Sinkhole>(_config.SlowdownDuration, _config.ShouldSlowdownStack);
            attacker.ChangeEffectIntensity<Sinkhole>(2);
        }

        public static void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            var player = ev.Player;
            if (!_config.EnableDogDoorBusting)
                return;
            if (!API.is939(player.Role.Type)
                || (ev.Door is Exiled.API.Interfaces.IDamageableDoor door && door.IsDestroyed)
                || (ev.Door.Base is PryableDoor gate && gate.IsConsideredOpen())) return;
            if (_activeAbilities.ContainsKey(player) && _activeAbilities[player] == null)
            {
                _activeAbilities[player] = Timing.RunCoroutine(DoorBustingCooldown(player));
                API.BustDoor(ev.Door, player, _config.EnableBustSpeedBoost);
            }
        }

        public static IEnumerator<float> DoorBustingCooldown(Player player)
        {
            for (int i = _config.DoorBustingCooldown; i > 0; i--)
            {
                yield return Timing.WaitForSeconds(1f);
                player.ShowHint($"{_config.cooldowntext.Replace("%i%", i.ToString())}", 1);
            }

            _activeAbilities[player] = null;
        }
    }
}