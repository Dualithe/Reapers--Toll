using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        GameplayHandler.Instance.StartGame();
    }
}
