using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    private class LoginScorePair
    {
        public string login;
        public float score;
    }

    [SerializeField] private Text _text;

    Dictionary<string, LoginScorePair> _leaders = new Dictionary<string, LoginScorePair>();

    public void AddLeader(string sessionID, Player player) {
        if (_leaders.ContainsKey(sessionID)) return;

        _leaders.Add(sessionID, new LoginScorePair {
            login = player.login,
            score = player.score
        });

        UpdateBoard();
    }
    public void RemoveLeader(string sessionID) {
        if (_leaders.ContainsKey(sessionID) == false) return;
        _leaders.Remove(sessionID);

        UpdateBoard();
    }

    public void UpdateScore(string sissionID, int score) {
        if (_leaders.ContainsKey(sissionID) == false) return;

        _leaders[sissionID].score = score;

        UpdateBoard();
    }

    private void UpdateBoard() {
        int topCount = Mathf.Clamp(_leaders.Count, 0, 8);
        var top8 = _leaders.OrderByDescending(pair => pair.Value.score).Take(topCount);

        string text = "";
        int i = 1;

        foreach (var leader in top8) {
            text += $"{i}. <b>{leader.Value.login}:</b> {leader.Value.score} - <b>{MultiplayerManager.Instance.Attempts()}</b>\n";
            i++;
        }

        _text.text = text;
    }
}
