using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationScript : MonoBehaviour {

    [SerializeField] private int levelWidth, levelHeight;
    [SerializeField] private GameObject tileBottom, tileTop;

    private int lastHeight = 1;
    private int sameHeightCounter = 0;

    public void Start() {
        GenerateTerrain();
    }

    private void GenerateTerrain() {

        // For each column of the level
        for(int x = 0; x < levelWidth; x++) {

            int height = GetNextHeight();
            for(int y = 0; y < height; y++) {
                SpawnTile(tileBottom, x, y);
            }

            if(height > 0) SpawnTile(tileTop, x, height);
        }
    }

    private int GetNextHeight() {
        int height = 0;
        int chance = Random.Range(0, 100);

        if(chance < (50 - 10*sameHeightCounter)) {
            height = lastHeight;
            sameHeightCounter++;
        } else {
            height = Random.Range(0, levelHeight);

            lastHeight = height;
            sameHeightCounter = 0;
        }

        return height;
    }

    private void SpawnTile(GameObject tile, int x, int y) {
        tile = Instantiate(tile, new Vector2(x, y), Quaternion.identity);
        tile.transform.parent = this.transform;
    }

}
