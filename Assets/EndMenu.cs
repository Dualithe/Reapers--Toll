using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        GameplayHandler.Instance.ShowMenu();
    }
}
