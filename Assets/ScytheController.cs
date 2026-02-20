using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject trailPrefab;
    public float projectileSpeed = 10f;
    public float trailSpacing = 0.2f;

    private GameObject activeProjectile;
    private List<GameObject> spawnedTrails = new List<GameObject>();
    private Vector3 fireDirection;
    private bool isRetracting = false;
    private Vector3 origin;
    private Coroutine moveCoroutine;
    private new Camera camera;

    private void Start()
    {
        StartCoroutine(AssignCameraAfterInitializations());
    }

    IEnumerator AssignCameraAfterInitializations()
    {
        while (GameplayHandler.Instance == null || GameplayHandler.Instance.Camera == null)
            yield return null;

        camera = GameplayHandler.Instance.Camera;
    }

    public void OnClick()
    {
        if (camera == null) return;

        if (activeProjectile == null)
        {
            Shoot();
        }
        else
        {
            Retract();
        }
    }

    void Shoot()
    {
        var transform1 = transform;
        origin = transform1.position;
        fireDirection = transform1.right;
        activeProjectile = Instantiate(projectilePrefab, origin, transform1.rotation);
        isRetracting = false;
        moveCoroutine = StartCoroutine(MoveProjectile());
    }

    void Retract()
    {
        isRetracting = true;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        if (activeProjectile != null)
            moveCoroutine = StartCoroutine(RetractProjectile());
    }

    IEnumerator MoveProjectile()
    {
        float spawnTimer = 0f;
        while (!isRetracting && activeProjectile != null && !IsOutOfCameraView(activeProjectile.transform.position))
        {
            float step = projectileSpeed * Time.deltaTime;
            activeProjectile.transform.position += fireDirection * step;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= trailSpacing)
            {
                spawnTimer = 0f;
                var trailObj = Instantiate(trailPrefab, activeProjectile.transform.position, Quaternion.identity);
                spawnedTrails.Add(trailObj);
            }
            yield return null;
        }
        if (IsOutOfCameraView(activeProjectile.transform.position))
        {
            Retract();
        }
    }

    IEnumerator RetractProjectile()
    {
        int i = spawnedTrails.Count - 1;
        while (i >= 0 && activeProjectile != null)
        {
            Vector3 target = spawnedTrails[i].transform.position;
            while (Vector3.Distance(activeProjectile.transform.position, target) > 0.1f)
            {
                activeProjectile.transform.position = Vector3.MoveTowards(activeProjectile.transform.position, target, projectileSpeed * Time.deltaTime);
                yield return null;
            }
            Destroy(spawnedTrails[i]);
            spawnedTrails.RemoveAt(i);
            i--;
        }
        while (activeProjectile != null && Vector3.Distance(activeProjectile.transform.position, origin) > 0.1f)
        {
            activeProjectile.transform.position = Vector3.MoveTowards(activeProjectile.transform.position, origin, projectileSpeed * Time.deltaTime);
            yield return null;
        }
        if (activeProjectile != null)
        {
            Destroy(activeProjectile);
            activeProjectile = null;
        }
    }

    bool IsOutOfCameraView(Vector3 pos)
    {
        if (camera == null) return false;
        Vector3 viewportPos = camera.WorldToViewportPoint(pos);
        return viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f || viewportPos.z < 0f;
    }
}
