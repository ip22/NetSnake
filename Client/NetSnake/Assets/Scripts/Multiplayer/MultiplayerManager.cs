using UnityEngine;
using Colyseus;
using System.Collections.Generic;

// TODO Restart

public class StartSettings
{
    public int redApples;
    public int greenApples;
    public int badApples;

    public StartSettings(int redApples, int greenApples, int badApples) {
        this.redApples = redApples;
        this.greenApples = greenApples;
        this.badApples = badApples;
    }
}

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _menu;

    [SerializeField] private PlayerMultiplayer _playerMultiplayer;
    [SerializeField] private EnemyMultiplayer _enemyMultiplayer;
    [SerializeField] private ApplesManager _applesManager;
    [SerializeField] private Leaderboard _leaderboard;
    [field: SerializeField] public Skins skins;

    private Player _player;
    private string _login;
    private const string GameRoomName = "state_handler";
    private ColyseusRoom<State> _room;

    public string SessionID() => _room.SessionId;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        Instance.InitializeClient();
        //Connection();
    }

    private int _attempts = 1;

    public int Attempts() => _attempts;

    public int PlayerSkin() => _playerMultiplayer.PlayerSkin();

    public async void Connection(int skinIndex) {
        _login = PlayerSettings.Instance.Login;

        var settings = new StartSettings(90, 30, 50);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            { "skins", skins.length },
            { "skin", skinIndex },
            { "login", _login },
            { "red", settings.redApples },
            { "green", settings.greenApples },
            { "bad", settings.badApples }
        };

        _room = await Instance.client.JoinOrCreate<State>(GameRoomName, data);
        _room.OnStateChange += OnChange;

        print($"Connection ID: {SessionID()}");
    }


    private void OnChange(State state, bool isFirstState) {
        var roomState = _room.State;

        if (isFirstState == false) return;

        _room.OnStateChange -= OnChange;

        state.players.ForEach((key, player) => {
            if (key == _room.SessionId) {
                _player = player;
                _playerMultiplayer.CreatePlayer(player, false, Vector3.zero);
            } else _enemyMultiplayer.CreateEnemy(key, player);
        });

        roomState.players.OnAdd += _enemyMultiplayer.CreateEnemy;
        roomState.players.OnRemove += _enemyMultiplayer.RemoveEnemy;

        roomState.apples.ForEach(_applesManager.CreateApple);
        roomState.apples.OnAdd += (key, apple) => _applesManager.CreateApple(apple);
        roomState.apples.OnRemove += _applesManager.RemoveApple;
    }

    public void Restart(int skinIndex) {
        _room.State.players.OnAdd -= _enemyMultiplayer.CreateEnemy;
        _attempts++;

        var position = new Vector3(Random.Range(-127, 127), 0, Random.Range(-127, 127));

        Dictionary<string, object> data = new Dictionary<string, object>() {
            { "login", _login},
            { "skin", skinIndex },
            { "x", position.x},
            { "z", position.z}
        };

        _playerMultiplayer.CreatePlayer(_player, true, position);

        SendMessage("restart", data);

        _room.State.players.OnAdd += _enemyMultiplayer.CreateEnemy;

        print($"Restart ID: {SessionID()}");
    }

    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom() => _room?.Leave();

    public void SendMessage(string key, Dictionary<string, object> data) => _room?.Send(key, data);

    public void SendMessage(string key, string json) => _room?.Send(key, json);

    public void UpdateScore(string sissionID, int score) => _leaderboard.UpdateScore(sissionID, score);

    public void ViewMenu() => _menu.SetActive(true);
}