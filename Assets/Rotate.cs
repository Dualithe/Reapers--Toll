using UnityEngine;

public class Rotate : MonoBehaviour
{
    private new Camera camera;
    
    private void Start()
    {
        StartCoroutine(AssignCameraAfterInitializations());
    }

    System.Collections.IEnumerator AssignCameraAfterInitializations()
    {
        while (GameplayHandler.Instance == null || GameplayHandler.Instance.Camera == null)
            yield return null;

        camera = GameplayHandler.Instance.Camera;
    }

    void Update()
    {
        if (camera == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, camera.WorldToScreenPoint(transform.position).z));

        Vector3 direction = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}