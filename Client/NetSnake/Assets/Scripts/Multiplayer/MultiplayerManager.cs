using UnityEngine;
using Colyseus;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _menu;

    [field: SerializeField] public Skins skins;

    #region Server
    private const string GameRoomName = "state_handler";

    private ColyseusRoom<State> _room;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        Instance.InitializeClient();
        //Connection();
    }

    public async void Connection(int skinIndex) {
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"skins", skins.length },
            { "skin", skinIndex }
        };

        _room = await Instance.client.JoinOrCreate<State>(GameRoomName, data);
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
        _room.State.players.OnRemove += RemoveEnemy;

        _room.State.apples.ForEach(CreateApple);
        _room.State.apples.OnAdd += (key, apple) => CreateApple(apple);
        _room.State.apples.OnRemove += RemoveApple;
    }

    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom() {
        _room?.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data) => _room?.Send(key, data);

    public void SendMessage(string key, string json) => _room?.Send(key, json);
    #endregion



    #region Player
    [SerializeField] private PlayerAim _aim;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Snake _snakePrefab;

    private void CreatePlayer(Player player) {
        Vector3 position = new Vector3(player.x, 0, player.z);
        Quaternion quaternion = Quaternion.identity;

        var snake = Instantiate(_snakePrefab, position, quaternion);
        snake.Init(player.seg, true);

        var aim = Instantiate(_aim, position, quaternion);
        aim.Init(snake.head, snake.speed);

        var controller = Instantiate(_controllerPrefab);
        controller.Init(_room.SessionId, aim, player, snake);

        snake.SetSkin(skins.GetMaterial(player.skin));
    }
    #endregion



    #region Enemy
    Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    private void CreateEnemy(string key, Player player) {
        Vector3 position = new Vector3(player.x, 0, player.z);

        var snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player.seg);

        var enemy = snake.AddComponent<EnemyController>();
        enemy.Init(key, player, snake);

        _enemies.Add(key, enemy);

        snake.SetSkin(skins.GetMaterial(player.skin));
    }

    private void RemoveEnemy(string key, Player player) {
        if (_enemies.ContainsKey(key) == false) return;

        var enemy = _enemies[key];
        _enemies?.Remove(key);
        enemy.Destroy();
    }
    #endregion



    #region Apple
    [SerializeField] private Apple _applePrefab;
    private Dictionary<Vector2float, Apple> _apples = new Dictionary<Vector2float, Apple>();

    private void CreateApple(Vector2float vector2Float) {
        Vector3 position = new Vector3(vector2Float.x, 0, vector2Float.z);
        var apple = Instantiate(_applePrefab, position, Quaternion.identity);
        apple.Init(vector2Float);
        _apples.Add(vector2Float, apple);

    }

    private void RemoveApple(int key, Vector2float vector2Float) {
        if (_apples.ContainsKey(vector2Float) == false) return;

        var apple = _apples[vector2Float];
        _apples.Remove(vector2Float);
        apple.Destroy();
    }
    #endregion

    public void ViewMenu() {
        _menu.SetActive(true);
    }
}
