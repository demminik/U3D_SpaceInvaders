namespace SpaceInvaders.Gameplay {

    public enum EGameplayCommand {

        None = 0,

        Restart,
        NextWave,
        TotalLoss,  

        Pause,
        Unpause,

        SpawnPlayer,
        SpawnEnemies,

        DespawnPlayer,
        DespawnEnemies,

        PlayerSpawned,
        EnemiesSpawned,

        PlayerEmerged,
        EnemiesEmerged,
    }
}