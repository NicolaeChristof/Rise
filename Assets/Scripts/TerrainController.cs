using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    public Terrain terrain;

    public Texture2D guideImage;

    [Range(1, 250)]
    public int width, length;

    [Range(1, 100)]
    public int height;

    [Range(0.01f, 1.0f)]
    public float bumpiness;

    [Range(1.0f, 50.0f)]
    public float perlinX, perlinY;

    [Range(1.0f, 10.0f)]
    public float perlinXSpan, perlinYSpan;

    [Range(1, 6)]
    public int octaves, persistence;

    [Range(0.0f, 1.0f)]
    public float mixValue;

    public bool movement, updateTerrain;

    private float[,] heightMap;

    private float offsetX, offsetY;

    // Use this for initialization
    void Start () {

        offsetX = offsetY = 0.0f;

        CreateTerrain();
        
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (updateTerrain) {

            if (movement) {

                offsetX += 0.025f;
                offsetY += 0.05f;

            }

            CreateTerrain();

        }
    }

    void CreateTerrain () {

        terrain.terrainData.size = new Vector3(width, height, length);

        terrain.terrainData.heightmapResolution = width + 1;

        CalculateHeightMap();

        terrain.terrainData.SetHeights(0, 0, heightMap);

    }

    void CalculateHeightMap () {

        heightMap = new float[width, length];

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < length; j++) {

                heightMap[i, j] = MapHeight(j, i);

            }
        }
    }

    float MapHeight (float x, float y) {

        float xfrac = x / width;
        float yfrac = y / length;

        float greyScaleVal = guideImage.GetPixelBilinear(xfrac, yfrac).grayscale;

        float noiseVal = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float maxValue = 0.0f;

        float inputX, inputY;

        float finalVal, output;

        for (int i = 0; i < octaves; i++) {

            inputX = perlinX + offsetX + xfrac * perlinXSpan * frequency;
            inputY = perlinY + offsetY + yfrac * perlinYSpan * frequency;

            noiseVal += Mathf.PerlinNoise(inputX, inputY) * amplitude;

            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;

        }

        finalVal = noiseVal / maxValue;

        output = (greyScaleVal * mixValue) + finalVal * (1 - mixValue);

        output *= bumpiness;

        return output;

    }
}
