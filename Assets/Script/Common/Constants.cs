using UnityEngine;

public class Constants : MonoBehaviour
{

    public const string ServerURL = "http://localhost:3000";
    public enum GameType { SinglePlay, DualPlay, MultiPlay}

    public enum PlayerType { None, PlayerA, PlayerB }

    public const int BlockColumCount = 3;
}
