using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    public Vector2 boxCenter = Vector2.zero;
    public Vector2 boxSize = new Vector2(10f, 10f);

    [Header("Prefab Settings")]
    public GameObject prefabToSpawn;
    public float itemRadius = 0.5f;
    public int maxAttemptsPerSpawn = 50;

    [Header("Spawning Timing")]
    public float spawnInterval = 2f;
    public int maxItemCount = 10;

    public bool allowSpawning;
    
    private List<Vector2> spawnedPositions = new List<Vector2>();
    private float spawnTimer;
    
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnedPositions.Count < maxItemCount && spawnTimer >= spawnInterval && allowSpawning)
        {
            spawnTimer = 0f;
            TrySpawnItem();
        }
    }

    void TrySpawnItem()
    {
        for (int attempt = 0; attempt < maxAttemptsPerSpawn; attempt++)
        {
            Vector2 candidate = RandomPointInBox(boxCenter, boxSize);

            Vector2 rel = candidate - boxCenter;
            float angle = Mathf.Atan2(rel.y, rel.x) * Mathf.Rad2Deg;
            angle = Mathf.Abs(NormalizeAngle(angle));

            float absAngleFromUp = Mathf.Abs(Mathf.DeltaAngle(angle, 90f));
            float absAngleFromDown = Mathf.Abs(Mathf.DeltaAngle(angle, -90f));
            if (absAngleFromUp < 20f || absAngleFromDown < 20f)
                continue;

            bool overlaps = false;
            foreach (Vector2 pos in spawnedPositions)
            {
                if (Vector2.Distance(pos, candidate) < itemRadius * 2f)
                {
                    overlaps = true;
                    break;
                }
            }
            if (!overlaps)
            {
                var point = Instantiate(prefabToSpawn, new Vector3(candidate.x, candidate.y, 0f), Quaternion.identity);
                point.tag = "bonusPoint";
                spawnedPositions.Add(candidate);
                break;
            }
        }
    }

    Vector2 RandomPointInBox(Vector2 center, Vector2 size)
    {
        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);
        return new Vector2(x, y);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawCube(new Vector3(boxCenter.x, boxCenter.y, 0f), new Vector3(boxSize.x, boxSize.y, 0.01f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(boxCenter.x, boxCenter.y, 0f), new Vector3(boxSize.x, boxSize.y, 0.01f));
    }
}
