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

    #region Server
    private const string GameRoomName = "state_handler";

    private ColyseusRoom<State> _room;

    public string SessionID() {
        return _room.SessionId;
    }

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        Instance.InitializeClient();
        //Connection();
    }

    public async void Connection(int skinIndex) {
        var settings = new StartSettings(90, 30, 50);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            { "skins", skins.length },
            { "skin", skinIndex },
            { "login", PlayerSettings.Instance.Login },
            { "red", settings.redApples },
            { "green", settings.greenApples },
            { "bad", settings.badApples }
        };

        _room = await Instance.client.JoinOrCreate<State>(GameRoomName, data);
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState) {
        var roomState = _room.State;

        if (isFirstState == false) return;

        _room.OnStateChange -= OnChange;

        state.players.ForEach((key, player) => {
            if (key == _room.SessionId) _playerMultiplayer.CreatePlayer(player);
            else _enemyMultiplayer.CreateEnemy(key, player);
        });

        roomState.players.OnAdd += _enemyMultiplayer.CreateEnemy;
        roomState.players.OnRemove += _enemyMultiplayer.RemoveEnemy;

        roomState.apples.ForEach(_applesManager.CreateApple);
        roomState.apples.OnAdd += (key, apple) => _applesManager.CreateApple(apple);
        roomState.apples.OnRemove += _applesManager.RemoveApple;
    }

    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void LeaveRoom() => _room?.Leave();

    public void SendMessage(string key, Dictionary<string, object> data) => _room?.Send(key, data);

    public void SendMessage(string key, string json) => _room?.Send(key, json);
    #endregion

    public int PlayerSkin() { return _playerMultiplayer.PlayerSkin(); }

    public void UpdateScore(string sissionID, int score) => _leaderboard.UpdateScore(sissionID, score);

    public void ViewMenu() => _menu.SetActive(true);
}