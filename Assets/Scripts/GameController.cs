using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Dialogue currentDialogue;

    private void Awake() => instance = this;
}