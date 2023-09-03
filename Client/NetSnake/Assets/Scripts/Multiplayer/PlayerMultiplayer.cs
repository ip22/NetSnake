using UnityEngine;

public class PlayerMultiplayer : MonoBehaviour
{
    [SerializeField] private PlayerAim _aim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    [SerializeField] private Leaderboard _leaderboard;

    private int _skin;

    private MultiplayerManager _multiplayerManager;

    public int PlayerSkin() {
        return _skin;
    }

    public void CreatePlayer(Player player) {
        _multiplayerManager = MultiplayerManager.Instance;

        Vector3 position = new Vector3(player.x, 0, player.z);
        Quaternion quaternion = Quaternion.identity;

        var snake = Instantiate(_snakePrefab, position, quaternion);
        snake.Init(player.seg, true);

        _skin = player.skin;

        var aim = Instantiate(_aim, position, quaternion);
        aim.Init(snake.head, snake.speed);

        var controller = Instantiate(_controllerPrefab);
        controller.Init(_multiplayerManager.SessionID(), aim, player, snake);

        snake.SetSkin(_multiplayerManager.skins.GetSnakeMaterial(_skin));

        _leaderboard.AddLeader(_multiplayerManager.SessionID(), player);
    }
}