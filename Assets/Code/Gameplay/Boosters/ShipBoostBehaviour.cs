using SpaceInvaders.Gameplay.Boosters;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Weapons;
using SpaceInvaders.Utils;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Combat {

    // TODO: this logic should be split by booster type to process each booster type separately (weapon boosters, speed boosters, etc., not important for demo)
    public class ShipBoostBehaviour : BaseGameplayBehaviour {

        public struct ActiveBoosterData {

            public EBoosterType BoosterType;
            public float Duration;
            public float EndTime;
        }

        [SerializeField]
        private Ship _ship;

        public ReactiveProperty<ActiveBoosterData> _activeBoosterData = new ReactiveProperty<ActiveBoosterData>();
        public ReactiveProperty<ActiveBoosterData> ActiveBooster => _activeBoosterData;


        private EWeaponType _originalWeapon;

        protected override void OnDestroy() {
            base.OnDestroy();

            _activeBoosterData = null;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                case EGameplayCommand.TotalLoss:
                case EGameplayCommand.SpawnPlayer:
                case EGameplayCommand.DespawnPlayer:
                case EGameplayCommand.EnemiesSpawned:
                    ResetState();
                    break;
            }
        }

        private void ActualizeUpdateState() {
            UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);

            if (_activeBoosterData.Value.BoosterType != EBoosterType.None &&
                _activeBoosterData.Value.Duration > 0) {

                UniRXHelper.SubscribeToUpdate(TickBoosterDuration, ref _updateDisposable);
            }
        }

        private void TickBoosterDuration(long _) {
            var currentTime = Time.time;
            if (currentTime > _activeBoosterData.Value.EndTime) {
                EndBooster();
            }
        }

        public void Accept(Booster booster) {
            if (booster == null) {
                return;
            }

            EndBooster();

            _activeBoosterData.Value = new ActiveBoosterData() {
                BoosterType = booster.Type,
                Duration = booster.Duration,
                EndTime = booster.Duration > 0f ? Time.time + booster.Duration : float.MaxValue,
            };
            ProcessNewActiveBoosterApplied();
            ActualizeUpdateState();
        }

        private void EndBooster() {
            if (_activeBoosterData.Value.BoosterType != EBoosterType.None) {
                ProcessBoosterExpired();
                ActualizeUpdateState();
            }
        }

        private void ProcessNewActiveBoosterApplied() {
            switch (_activeBoosterData.Value.BoosterType) {
                case EBoosterType.Weapon_Triple:
                    PrepareForWeaponBoosterEffect();
                    break;
            }
        }

        private void PrepareForWeaponBoosterEffect() {
            if (_originalWeapon == EWeaponType.None) {
                _originalWeapon = _ship.CurrentWeapon != null ? _ship.CurrentWeapon.Type : EWeaponType.None;
            }
        }

        private void ProcessBoosterExpired() {
            switch (_activeBoosterData.Value.BoosterType) {
                case EBoosterType.Weapon_Triple:
                    ProcessWeaponBoosterExpired();
                    break;
            }

            _activeBoosterData.Value = new ActiveBoosterData();
        }

        private void ProcessWeaponBoosterExpired() {
            if (_originalWeapon != EWeaponType.None) {
                _ship.InstallWeapon(_originalWeapon);
                _originalWeapon = EWeaponType.None;
            }
        }

        private void ResetState() {
            _originalWeapon = EWeaponType.None;
            _activeBoosterData.Value = new ActiveBoosterData();
        }
    }
}