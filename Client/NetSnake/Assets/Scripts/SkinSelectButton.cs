using UnityEngine;

public class SkinSelectButton : MonoBehaviour
{
    public void SelectSkin(int value) {
        MultiplayerManager.Instance.Connection(value);
    }

    public void SelectSkinRestart(int value) {
        MultiplayerManager.Instance.Restart(value);
    }
}
