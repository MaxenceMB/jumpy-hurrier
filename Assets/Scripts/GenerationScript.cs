using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationScript : MonoBehaviour {

    public int levelWidth, levelHeight, startLength, endLength;
    public GameObject tileTop, tileBottom;

    private int lastHeight = 1;
    private int lastPlatformHeight;
    private int sameHeightCounter = 0;

    public void Start() {
        GenerateTerrain();
    }

    private void GenerateTerrain() {

        // For the begining 
        GenerateStartTerrain();

        // For each column of the level
        for(int x = this.startLength; x < (this.levelWidth - this.endLength); x++) {

            int height = GetNextHeight();
            for(int y = 0; y < height; y++) {
                SpawnTile(this.tileBottom, x, y);
            }

            if(height > 0) SpawnTile(this.tileTop, x, height);
        }

        // For the end
        GenerateEndTerrain();        
    }

    private void GenerateStartTerrain() {
        for(int x = 0; x < this.startLength; x++) {
            SpawnTile(this.tileBottom, x, 0);
            SpawnTile(this.tileBottom, x, 1);
            SpawnTile(this.tileTop, x, 2);
        }
    }

    private void GenerateEndTerrain() {
        int endStart = this.levelWidth - this.endLength;

        for(int x = 0; x < this.endLength; x++) {
            for(int y = 0; y < this.lastPlatformHeight; y++) {
                SpawnTile(this.tileBottom, endStart+x, y);
            }
            SpawnTile(this.tileTop, endStart+x, this.lastPlatformHeight);
        }
    }

    private int GetNextHeight() {
        int height = 0;
        int chance = Random.Range(0, 100);

        if(chance < (75 - 5*this.sameHeightCounter)) {
            height = this.lastHeight;
            this.sameHeightCounter++;
        } else {
            height = Random.Range(0, this.levelHeight+3)-3;
            height = (height <= 0) ? 0 : height;

            this.sameHeightCounter = (height == this.lastHeight) ? this.sameHeightCounter+1 : 0;
        }
        
        lastHeight = height;
        if(height != 0) this.lastPlatformHeight = height;

        return height;
    }

    private void SpawnTile(GameObject tile, int x, int y) {
        tile = Instantiate(tile, new Vector2(x, y), Quaternion.identity);
        tile.transform.parent = this.transform;
    }

    public void RegenerateTerrain() {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        GenerateTerrain();
    }

}
