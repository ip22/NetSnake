using System.Collections.Generic;
using UnityEngine;

public class EnemyMultiplayer : MonoBehaviour
{
    [SerializeField] private Snake _snakePrefab;

    [SerializeField] private Leaderboard _leaderboard;

    Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    private MultiplayerManager _multiplayerManager;

    public void CreateEnemy(string key, Player player) {
        _multiplayerManager = MultiplayerManager.Instance;

        if (_multiplayerManager.Attempts() > 1) return;

        Vector3 position = new Vector3(player.x, 0, player.z);

        var snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player.seg);

        var enemy = snake.gameObject.AddComponent<EnemyController>();
        enemy.Init(key, player, snake);
        _enemies.Add(key, enemy);

        snake.SetSkin(_multiplayerManager.skins.GetSnakeMaterial(player.skin));

        _leaderboard.AddLeader(key, player);

        print("Create Enemy at: " + player.x + " " + player.z);
    }

    public void RemoveEnemy(string key, Player player) {
        _leaderboard.RemoveLeader(key);

        if (_enemies.ContainsKey(key) == false) {
            Debug.LogError("Can't remove. Enemy is not exist.");
            return;
        }

        var enemy = _enemies[key];
        _enemies?.Remove(key);
        enemy.Destroy();
    }
}
