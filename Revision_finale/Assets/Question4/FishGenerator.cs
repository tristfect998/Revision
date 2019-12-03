using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishGenerator : MonoBehaviour {
    
    public int mapWidth = 256;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate;
    public int seed;
    public Vector2 offset;
    public int movementValue = 72;
    float[,] noiseMapMovement;

    float TimeSinceLastMovement = 5f;

    public int maxNumberOfFishes = 100;
    public GameObject fishPrefab;

    private void Start() {
        noiseMapMovement = Noise.GenerateNoiseMap(mapWidth, mapWidth, seed + 4, noiseScale, octaves, persistance, lacunarity, offset);
    }

    private void Update()
    {
        if(TimeSinceLastMovement <= 0)
        {
            int newSeedValue = Random.Range(8, 42);
            noiseMapMovement = Noise.GenerateNoiseMap(mapWidth, mapWidth, newSeedValue, noiseScale, octaves, persistance, lacunarity, offset);
            moveFishes();
            TimeSinceLastMovement = 5f;
        }
        else
        {
            moveFishes();
            TimeSinceLastMovement -= Time.deltaTime;
        }
    }

    public void GenerateFishes() {
        DeleteFishes();
        GenerateFishesWithNoise();
        print("Test");
    }

    public void DeleteFishes() {
        foreach (Fish fish in FindObjectsOfType<Fish>()) {
            DestroyImmediate(fish.gameObject);
        }
    }
    
  
    private void OnValidate() {
        if(mapWidth< 1) {
            mapWidth = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if(octaves < 0) {
            octaves = 0;
        }

    }

    private void GenerateFishesWithNoise()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapWidth, seed, noiseScale, octaves, persistance, lacunarity, offset);
        for (int i = 0; i < mapWidth; i++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                float value = noiseMap[i, y];
                if(value >= 0.80)
                {
                    GameObject fish = Instantiate(fishPrefab, new Vector3(i, y, 0), new Quaternion(0, 0, 0, 0));
                }
            }
        }
    }

    private void moveFishes()
    {
        foreach (Fish currentFish in FindObjectsOfType<Fish>())
        {
            float FishX = Mathf.Abs((int)currentFish.transform.position.x) % mapWidth;
            float FishY = Mathf.Abs((int)currentFish.transform.position.y) % mapWidth;
            float value = noiseMapMovement[(int)FishX, (int)FishY];
            if (value <= 0.2)
            {
                currentFish.transform.Translate(new Vector3(0, movementValue, 0));
            }
            else if(value <= 0.4)
            {
                currentFish.transform.Translate(new Vector3(0, -movementValue, 0));
            }
            else if(value <= 0.6)
            {
                currentFish.transform.Translate(new Vector3(movementValue, movementValue, 0));
            }
            else if (value <= 0.8)
            {
                currentFish.transform.Translate(new Vector3(movementValue, 0, 0));
            }
            else
            {
                currentFish.transform.Translate(new Vector3(-movementValue, 0, 0));
            }
        }
    }
}

