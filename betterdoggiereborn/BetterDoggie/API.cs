using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using MEC;
using PlayerRoles;

namespace BetterDoggie
{
    public static class API
    {
        
        /// <summary>
        /// Check if player is 939
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool is939(RoleTypeId role)
        {
            return role == RoleTypeId.Scp939;
        }
        
        /// <summary>
        /// Busts down a door and applies effect
        /// </summary>
        /// <param name="door"></param>
        /// <param name="ply"></param>
        /// <param name="speedBoost"></param>
        
        public static void BustDoor(Door door, Player ply, bool speedBoost)
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
            ply.ChangeEffectIntensity<MovementBoost>(BetterDoggie.Singleton.Config.BustBoostAmount);
            Timing.CallDelayed(2f, () => ply.ChangeEffectIntensity<MovementBoost>(BetterDoggie.Singleton.Config.ColaSpeedBoost));
        }
        
    }
}