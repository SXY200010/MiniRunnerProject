using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject floorGroupPrefab;
    public Transform player;
    public float floorLength = 4f;
    public int tilesAhead = 6;
    public int maxTiles = 10;

    private float lastSpawnZ = 0f;
    private Queue<GameObject> floorQueue = new Queue<GameObject>();

    public GameObject fogPrefab;
    private bool fogSpawned = false;

    public GameObject coinPrefab;

    public GameObject obstaclePrefab1;
    public GameObject obstaclePrefab2;

    [Range(0f, 1f)]
    public float obstacleSpawnChance = 0.5f;

    void Start()
    {
        for (int i = 0; i < tilesAhead; i++)
        {
            SpawnFloorGroup(i * floorLength);
        }

        lastSpawnZ = tilesAhead * floorLength;
    }

    void Update()
    {
        if (player.position.z + tilesAhead * floorLength > lastSpawnZ)
        {
            SpawnFloorGroup(lastSpawnZ);
            ScoreManager.instance.AddFloor(); 
            lastSpawnZ += floorLength;

            if (floorQueue.Count >= maxTiles)
            {
                Destroy(floorQueue.Dequeue());
            }
        }
    }

    void SpawnFloorGroup(float zPosition)
    {
        Vector3 spawnPos = new Vector3(0, 0, zPosition);
        GameObject obj = Instantiate(floorGroupPrefab, spawnPos, Quaternion.identity);
        floorQueue.Enqueue(obj);

        if (zPosition > floorLength * 2f)  
        {
            SpawnObstaclesAndCoins(zPosition);
        }

        if (!fogSpawned && fogPrefab != null)
        {
            GameObject fog = Instantiate(fogPrefab);
            FogFollower follower = fog.GetComponent<FogFollower>();
            if (follower != null)
            {
                follower.player = player;
            }
            fogSpawned = true;
        }
    }


    void SpawnObstaclesAndCoins(float zPosition)
    {
        float[] laneX = { -4f, 0f, 4f };

        int coinLaneIndex = Random.Range(0, laneX.Length);
        float coinX = laneX[coinLaneIndex];
        for (int i = 0; i < 3; i++)
        {
            float z = zPosition + 0.5f + i * 2f;
            Vector3 coinPos = new Vector3(coinX, 1f, z);
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }

        if (Random.value > obstacleSpawnChance) return;

        List<int> availableLanes = new List<int> { 0, 1, 2 };
        availableLanes.Remove(coinLaneIndex);
        int obstacleLaneIndex = availableLanes[Random.Range(0, availableLanes.Count)];
        float obstacleX = laneX[obstacleLaneIndex];
        float obstacleZ = zPosition + Random.Range(1f, floorLength - 1f);
        Vector3 obstaclePos = new Vector3(obstacleX, 0f, obstacleZ);

        GameObject prefabToSpawn = (Random.value < 0.5f) ? obstaclePrefab1 : obstaclePrefab2;
        Instantiate(prefabToSpawn, obstaclePos, Quaternion.identity);
    }

}
