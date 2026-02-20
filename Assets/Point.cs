using System;
using UnityEngine;

public class Point : MonoBehaviour
{
    private Vector3 playerPosition;
    private void Start()
    {
        playerPosition = GameplayHandler.Instance.player.position;
    }

    public void AddPoints(int value)
    {
        PlayerHandler.Instance.souls += value;
        GameplayHandler.Instance.uiHandler.ModifySkulls(value);
        Destroy(transform.gameObject);
    }

    public void GetCollected(Transform transform)
    {
        var transf = gameObject.transform;
        transf.parent = transform;
        transf.position = transform.position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, playerPosition) < 0.6f)
        {
            if(CompareTag("point"))
                AddPoints(1);
            else if(CompareTag("bonusPoint"))
                AddPoints(10);
        }
    }
}
