using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawnerController : MonoBehaviour
{
    public GameObject crate;

    public float minSpawnTimer = 10f;
    public float maxSpawnTimer = 20f;

    public float minRadiusSpawn = 5f;
    public float maxRadiusSpawn = 10f;

    public int maxCratesSpawned = 0;

    private int numOfCrates = 0;
    private float currentSpawnTimer = 0f;
    private GameObject player;

    public static CrateSpawnerController Instance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        currentSpawnTimer = UnityEngine.Random.Range(minSpawnTimer, maxSpawnTimer);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (numOfCrates >= maxCratesSpawned)
        {
            return;
        }

        currentSpawnTimer -= Time.deltaTime;
        if (currentSpawnTimer <= 0f)
        {
            float randomRadius = Random.Range(minRadiusSpawn, maxRadiusSpawn);
            Vector3 randomPosition = Random.insideUnitCircle * randomRadius;

            Instantiate(crate, player.transform.position + randomPosition, Quaternion.identity);

            currentSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
            numOfCrates++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minRadiusSpawn);
        Gizmos.DrawWireSphere(transform.position, maxRadiusSpawn);
    }

    public void DecreaseNumberOfCrates()
    {
        numOfCrates--;
    }
}
