using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Coins")]
    [SerializeField] private int maxCoinCount = 10;
    [SerializeField] private float coinSpawnInterval = 5f;
    [SerializeField] private GameObject coinPrefab;

    [Header("Bombs")]
    [SerializeField] private int bombHitPenalty = 5;

    [Header("Sheep")]
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private Transform sheepSpawnPoint;
    [SerializeField] private int sheepCost;

    [Header("Misc")]
    [SerializeField] private Tilemap worldTilemap;
    [SerializeField] private Tilemap level2Entry;

    [Header("UI")]
    [SerializeField] private CoinsCollectedUI coinsCollectedCounter;
    [SerializeField] private AlertUI alertUI;

    // Bounding the max retries of coin spawn so the game won't starve
    private const int spawnMaxRetryCount = 3;

    private float coinSpawnTimer = 0f;
    private int coinCount = 0;

    private bool hasSheepSpawned = false;

    private void Awake()
    {
        if (null != Instance)
        {
            Debug.LogError("Only one instance of GameManager is allowed");
        }

        Instance = this;
    }

    private void Start()
    {
        Player.Instance.OnCoinCollected += Player_OnCoinCollected;
        Player.Instance.OnBombCollided += Player_OnBombCollided;
        Player.Instance.OnBombInVicinity += Player_OnBombInVicinity;
    }

    private void Sheep_OnInteraction(object sender, EventArgs e)
    {
        coinsCollectedCounter.DecrementCounter(sheepCost);
        level2Entry.gameObject.SetActive(false);
    }

    private void Player_OnBombInVicinity(object sender, System.EventArgs e)
    {
        alertUI.Activate();
    }

    private void Player_OnBombCollided(object sender, System.EventArgs e)
    {
        coinsCollectedCounter.DecrementCounter(bombHitPenalty);
    }

    private void Player_OnCoinCollected(object sender, Player.OnCoinCollectedEventArgs e)
    {
        coinCount--;
        coinsCollectedCounter.CollectCoin(e.collectedCoin.transform.position);
    }

    private void Update()
    {
        coinSpawnTimer += Time.deltaTime;
        if ((coinSpawnTimer >= coinSpawnInterval) && (coinCount < maxCoinCount))
        {
            GameObject coinObject;
            coinSpawnTimer = 0f;
            if (TrySpawnObject(coinPrefab, out coinObject))
            {
                coinCount++;
            }
        }

        if (!Player.Instance.IsBombInVicinity)
        {
            alertUI.Deactivate();
        }

        TrySpawnSheep();
    }

    private bool TrySpawnSheep()
    {
        if (!hasSheepSpawned && (coinsCollectedCounter.CoinsCollected >= sheepCost))
        {
            GameObject sheep = Instantiate(sheepPrefab, sheepSpawnPoint);
            sheep.GetComponent<Sheep>().OnInteracted += Sheep_OnInteraction;
            hasSheepSpawned = true;
            return true;
        }

        return false;
    }

    private bool TrySpawnObject(GameObject prefab, out GameObject obj)
    {
        if (!TryGetRandomEmptyCell(out Vector3 random_cell_position))
        {
            obj = null;
            return false;
        }

        obj = Instantiate(prefab, null);
        obj.transform.position = random_cell_position;
        return true;
    }

    private bool TryGetRandomEmptyCell(out Vector3 position)
    {
        bool reachedMax = false;
        int retryCount = 0;
        Vector3 random_cell_position = Vector3.zero;
        do
        {
            int rand_x = UnityEngine.Random.Range(
                worldTilemap.cellBounds.xMin, worldTilemap.cellBounds.xMax);
            int rand_y = UnityEngine.Random.Range(
                worldTilemap.cellBounds.yMin, worldTilemap.cellBounds.yMax);
            random_cell_position = worldTilemap.CellToWorld(new Vector3Int(rand_x, rand_y, 0));

            retryCount++;
            reachedMax = retryCount >= spawnMaxRetryCount;

            // Attempting to spawn the coin until it's not colliding with anything
            // and for a max of coinMaxRetryCount times
        } while ((!reachedMax) &&
                  Physics2D.BoxCast(random_cell_position, Vector2.one, 0f, Vector2.zero));

        position = random_cell_position;
        return !reachedMax;
    }
}
