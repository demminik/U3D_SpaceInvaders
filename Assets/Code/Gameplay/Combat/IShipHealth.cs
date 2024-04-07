using SpaceInvaders.Gameplay.Entities;
using System;

namespace SpaceInvaders.Gameplay.Combat {

    public interface IShipHealth {

        public event Action<Ship> OnDeath;
    }
}