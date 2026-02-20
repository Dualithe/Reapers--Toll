using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Assign Three Enemy Prefabs Below")]
    [SerializeField] private GameObject enemyType1Prefab;
    [SerializeField] private GameObject enemyType2Prefab;
    [SerializeField] private GameObject enemyType3Prefab;

    [Header("Configure Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();
    public List<string> waveData = new List<string>();
    private UIHandler uiHandler;

    public int currentMap = 1;

    private void Start()
    {
        LoadWaveData();
        uiHandler = GameplayHandler.Instance.uiHandler;
        if (uiHandler != null)
            uiHandler.SetWaveCount(waveData.Count);
        SpawnEnemies();
    }


    public void SpawnEnemies()
    {
        StartCoroutine(DoSpawn());
    }

public void LoadWaveData()
{
    string fileName = $"map{currentMap}.txt";
    string path = Path.Combine(Application.streamingAssetsPath, fileName);

    if (File.Exists(path))
    {
        waveData.Clear();
        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
                waveData.Add(line.Trim());
        }
    }
}


    private IEnumerator DoSpawn()
    {
        foreach (var wave in waveData)
        {
            var groups = wave.Trim().Split(' ');
            var availablePoints = new List<Transform>(spawnPoints);
            Shuffle(availablePoints);

            int groupsCompleted = 0;
            int totalGroups = groups.Length;

            for (int i = 0; i < groups.Length; i++)
            {
                if (availablePoints.Count == 0) break;
                var spawnPoint = availablePoints[0];
                availablePoints.RemoveAt(0);

                StartCoroutine(SpawnGroupAtPoint(groups[i], spawnPoint, () => { groupsCompleted++; }));
            }

            yield return new WaitUntil(() => groupsCompleted >= totalGroups);
            yield return new WaitForSeconds(5f);
            
            
            if (uiHandler != null)
                uiHandler.DecrementWave();
        } 
        GameplayHandler.Instance.GameWon();
    }

    private IEnumerator SpawnGroupAtPoint(string group, Transform spawnPoint, Action onComplete)
    {
        foreach (char symbol in group)
        {
            GameObject prefab = GetPrefab(symbol);
            if (prefab != null)
            {
                GameObject enemyGO = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                var enemyScript = enemyGO.GetComponent<Enemy>();
                if (enemyScript != null && GameplayHandler.Instance != null)
                    enemyScript.target = GameplayHandler.Instance.player;
            }
            yield return new WaitForSeconds(1f);
        }
        onComplete?.Invoke();
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private GameObject GetPrefab(char type)
    {
        switch (type)
        {
            case 'A': return enemyType1Prefab;
            case 'B': return enemyType2Prefab;
            case 'C': return enemyType3Prefab;
        }
        return null;
    }
}
