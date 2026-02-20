using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public ScytheController controller;
    public bool inputAllowed;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inputAllowed && !EventSystem.current.IsPointerOverGameObject())
        {
            controller.OnClick();
        }
    }
}