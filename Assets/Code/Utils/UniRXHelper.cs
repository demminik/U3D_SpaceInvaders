using System;
using UniRx;

namespace SpaceInvaders.Utils {

    public static class UniRXHelper {

        public static void SubscribeToUpdate(Action<long> updateCallback, ref IDisposable updateDisposable) {
            UnsubscribeFromUpdate(ref updateDisposable);

            updateDisposable = Observable.EveryUpdate().Subscribe(updateCallback);
        }

        public static void UnsubscribeFromUpdate(ref IDisposable updateDisposable) {
            if (updateDisposable != null) {
                updateDisposable.Dispose();
                updateDisposable = null;
            }
        }
    }
}