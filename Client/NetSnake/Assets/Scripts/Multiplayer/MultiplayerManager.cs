using UnityEngine;
using Colyseus;
using System.Collections.Generic;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    #region Server
    private const string GameRoomName = "state_handler";

    private ColyseusRoom<State> _room;

    protected override void Awake() {
        base.Awake();
        InitializeClient();
        Connection();
    }

    private async void Connection() {
        _room = await client.JoinOrCreate<State>(GameRoomName);
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState) {
        if (isFirstState == false) return;
        _room.OnStateChange -= OnChange;
        state.players.ForEach((key, player) => {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnAdd += RemoveEnemy;
    }

    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom() {
        _room?.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data) => _room?.Send(key, data);
    #endregion

    #region Player
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    private void CreatePlayer(Player player) {
        Vector3 position = new Vector3(player.x, 0, player.z);

        var snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player.sg);

        var controller = Instantiate(_controllerPrefab);
        controller.Init(snake);
    }
    #endregion

    #region Enemy
    private void CreateEnemy(string key, Player value) {

    }

    private void RemoveEnemy(string key, Player value) {

    }
    #endregion
}
