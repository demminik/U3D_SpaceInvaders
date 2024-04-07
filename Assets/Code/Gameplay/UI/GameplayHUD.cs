using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Combat;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Meta;
using SpaceInvaders.Utils;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Gameplay.UI {

    // simple and stupid all-in-one HUD
    public class GameplayHUD : BaseGameplayBehaviour {

        [SerializeField]
        private TMP_Text _txtScores;

        [SerializeField]
        private TMP_Text _txtWave;

        [SerializeField]
        private TMP_Text _txtLives;

        [SerializeField]
        private Canvas _canvasBoost;

        [SerializeField]
        private Image _imgBoosterProgress;

        [SerializeField]
        private Button _btnPause;

        private GameplayStats _stats;
        private PlayerShipAccessor _playerShipAccessor;

        private ShipBoostBehaviour.ActiveBoosterData _playerActiveBoosterData;
        private IDisposable _playerBoosterChangeDisposable;

        [Inject]
        private void HandleInjection(GameplayStats stats,
            PlayerShipAccessor playerShipAccessor) {
            _stats = stats;
            _stats.PlayerLives.Subscribe(ProcessPlayerLivesChanged).AddTo(_disposable);
            _stats.Score.Subscribe(ProcessScoreChanged).AddTo(_disposable);
            _stats.WaveNumber.Subscribe(ProcessWaveChanged).AddTo(_disposable);

            _playerShipAccessor = playerShipAccessor;
            _playerShipAccessor.OnShipChanged += SubscribeToPlayerBoosterChanges;
            SubscribeToPlayerBoosterChanges(_playerShipAccessor.PlayerShip);
            ProcessPlayerBoosterChanges(_playerShipAccessor.PlayerShip != null && _playerShipAccessor.PlayerShip.BoostBehaviour != null ?
                _playerShipAccessor.PlayerShip.BoostBehaviour.ActiveBooster.Value :
                new ShipBoostBehaviour.ActiveBoosterData());
        }

        private void Awake() {
            _btnPause.onClick.AddListener(ProcessBtnPauseClick);
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            UnsubscribeFromPlayerBoosterChanges();

            if (_playerShipAccessor != null) {
                _playerShipAccessor.OnShipChanged -= SubscribeToPlayerBoosterChanges;
            }
        }

        private void ProcessPlayerLivesChanged(int value) {
            _txtLives.text = value.ToString();
        }

        private void ProcessScoreChanged(int value) {
            _txtScores.text = value.ToString();
        }

        private void ProcessWaveChanged(int value) {
            _txtWave.text = value.ToString();
        }

        private void SubscribeToPlayerBoosterChanges(Ship playerShip) {
            UnsubscribeFromPlayerBoosterChanges();

            if (playerShip != null && playerShip.BoostBehaviour != null) {
                _playerBoosterChangeDisposable = playerShip.BoostBehaviour.ActiveBooster.Subscribe(ProcessPlayerBoosterChanges);
            }
        }

        private void UnsubscribeFromPlayerBoosterChanges() {
            if (_playerBoosterChangeDisposable != null) {
                _playerBoosterChangeDisposable.Dispose();
                _playerBoosterChangeDisposable = null;
            }
        }

        private void ProcessPlayerBoosterChanges(ShipBoostBehaviour.ActiveBoosterData activeBoosterData) {
            _playerActiveBoosterData = activeBoosterData;
            if (_playerActiveBoosterData.BoosterType == Boosters.EBoosterType.None || _playerActiveBoosterData.Duration <= 0f) {
                _canvasBoost.enabled = false;
                UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
            } else {
                _canvasBoost.enabled = true;
                UniRXHelper.SubscribeToUpdate(Tick, ref _updateDisposable);
                RefreshPlayerBoosterTimeLeft();
            }
        }

        private void Tick(long _) {
            RefreshPlayerBoosterTimeLeft();
        }

        private void RefreshPlayerBoosterTimeLeft() {
            var duration = _playerActiveBoosterData.EndTime - (_playerActiveBoosterData.EndTime - _playerActiveBoosterData.Duration);
            var timeLeft = Mathf.Max(0f, _playerActiveBoosterData.EndTime - Time.time);
            _imgBoosterProgress.fillAmount = timeLeft / duration;
        }

        private void ProcessBtnPauseClick() {
            _gameplayCommand.Execute(EGameplayCommand.Pause);
        }
    }
}