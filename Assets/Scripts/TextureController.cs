using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour {

    public Terrain terrain;

    [Range(0.0f, 1.0f)]
    public float grassWeight, dirtWeight, sandWeight;

    [Range(0.0f, 1.0f)]
    public float textureCutoff, offset;

    public bool updateTextures;

    private float[,,] textureMap;

    private int terrainWidth, terrainlength;

    private float maxHeight;

    private int alphaWidth, alphaHeight, alphaLayers;

    // Use this for initialization
    void Start () {

        Time.fixedDeltaTime = 0.25f;

        terrainWidth = (int)terrain.terrainData.size.x;
        terrainlength = (int)terrain.terrainData.size.z;

        maxHeight = terrain.terrainData.size.y;

        alphaWidth = terrain.terrainData.alphamapWidth;
        alphaHeight = terrain.terrainData.alphamapHeight;
        alphaLayers = terrain.terrainData.alphamapLayers;

        AssignTextures();

    }

    // Update is called once per frame
    void FixedUpdate () {

        if (updateTextures) {

            AssignTextures();

            updateTextures = false;

        }

    }

    void AssignTextures () {

        float height, slope;

        Vector3 normal;

        textureMap = new float[alphaWidth, alphaHeight, alphaLayers];

        for (int i = 0; i < alphaWidth; i++) {

            for (int j = 0; j < alphaHeight; j++) {

                height = CalculateHeight(j, i);

                slope = CalculateSlope(j, i);

                // normal = CalculateNormal(j, i); // can be used to apply textures to normals facing a specific direction

                // Debug.Log(height + " " + slope + " " + normal);

                if (height < textureCutoff) {

                    textureMap[i, j, 0] = 1 - (slope + offset);
                    textureMap[i, j, 1] = slope;
                    textureMap[i, j, 2] = offset;

                } else {

                    textureMap[i, j, 0] = offset;
                    textureMap[i, j, 1] = slope;
                    textureMap[i, j, 2] = 1 - (slope + offset);

                }

                textureMap[i, j, 0] *= grassWeight;
                textureMap[i, j, 1] *= dirtWeight;
                textureMap[i, j, 2] *= sandWeight;

            }
        }

        terrain.terrainData.SetAlphamaps(0, 0, textureMap);

    }

    float CalculateHeight (int x, int y) {

        float height;

        float normX, normY;

        int positionX, positionY;

        normX = x * 1.0f / (alphaWidth - 1);
        normY = y * 1.0f / (alphaHeight - 1);

        positionX = (int)(normX * terrainWidth);
        positionY = (int)(normY * terrainlength);

        height = terrain.terrainData.GetHeight(positionX, positionY) / maxHeight;

        return height;

    }

    float CalculateSlope (int x, int y) {

        float steepness;

        float normX, normY;

        float angle;

        normX = x * 1.0f / (alphaWidth - 1);
        normY = y * 1.0f / (alphaHeight - 1);

        angle = terrain.terrainData.GetSteepness(normX, normY);

        steepness = angle / 90.0f;

        return steepness;

    }

    Vector3 CalculateNormal (int x, int y) {

        Vector3 normal;

        normal = terrain.terrainData.GetInterpolatedNormal(x, y);

        return normal;

    }
}
