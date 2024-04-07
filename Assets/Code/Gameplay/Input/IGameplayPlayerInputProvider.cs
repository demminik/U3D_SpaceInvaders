using System;

namespace SpaceInvaders.Gameplay.Input {

    public interface IGameplayPlayerInputProvider {

        public event Action<PlayerActionData> OnPlayerAction;
    }
}