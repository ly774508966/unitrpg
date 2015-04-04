﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MapGrid : MonoBehaviour {
	
    public float tileSizeInPixels = 32f;
    public int width;
    public int height;

	public delegate void GridClickHandler(Vector2 location);
	public event GridClickHandler OnGridClicked;
    private Dictionary<Vector2, MapTile> tilesByPosition = new Dictionary<Vector2, MapTile>();
	private AstarPath Pathfinder;

    public void RescanGraph() {
        Pathfinder.Scan();
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(width * tileSizeInPixels, height * tileSizeInPixels, 0));
    }

    public void Awake() {
		Pathfinder = GetComponent<AstarPath>();
    }

    private HashSet<Vector2> generateSurroundingPoints(Vector2 origin, int range) {
        float leftEdge = origin.x + range;
        float rightEdge = origin.x - range;

        float topEdge = origin.y + range;
        float bottomEdge = origin.y - range;

        HashSet<Vector2> results = new HashSet<Vector2>();
        for (int x = (int)(origin.x - range); x <= origin.x + range; x++) {
            for (int y = (int)(origin.y - range); y <= origin.y + range; y++) {
                Vector2 neighbor = new Vector2(x, y);
                if (neighbor != origin) {
                    results.Add(new Vector2(x, y));
                }
            }
        }

        return results;
    }


    private float mapRange(float fromStart, float fromEnd, float toStart, float toEnd, float value) {
        float inputRange = fromEnd - fromStart;
        float outputRange = toEnd - toStart;
        return (value - fromStart) * outputRange / inputRange + toStart;
        
    }

	public void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector2? maybePos = GetMouseGridPosition();
			if (maybePos.HasValue && OnGridClicked != null) {
				OnGridClicked(maybePos.Value);
			}
		}
	}

    public Vector2? GetMouseGridPosition() {
      
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int tileSize = (int)tileSizeInPixels;
        float widthExtent = (width / 2f) * tileSize;
        float heightExtent = (height / 2f) * tileSize;
        Vector2 result =  new Vector3(
            (float)Math.Floor(mapRange(-widthExtent, widthExtent, 0, width, mousePos.x)),
            (float)Math.Floor(mapRange(-heightExtent, heightExtent, 0, height, mousePos.y))
        );

        return result;
    }

    public void SelectTiles(ICollection<MapTile> tiles, Color color) {
        ClearSelection();
        foreach (MapTile tile in tiles) {
            tile.Select(color);
        }
    }

    public void ClearSelection() {
        Debug.Log("Re-implement ClearSelection");
    }

    public HashSet<Vector2> GetWalkableTilesInRange(Vector2 origin, int range) {
        return new RangeFinder(this).GetOpenTilesInRange(origin, range);
    }

    public Vector3 GetWorldPosForGridPos(Vector2 gridPos) {
        if (!IsInGrid(gridPos)) {
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        int tileSize = (int)tileSizeInPixels;
        float widthExtent = (width/2f)*tileSize;
        float heightExtent = (height/2f)*tileSize;

        float halfTileSize = tileSizeInPixels / 2; 

        // Map the input values for the x and y axis in grid space to world space.
        // Be sure to output the center of the tile in world space by adding
        // 1/2 the tile height and width!
        Vector3 result = new Vector3(
            mapRange(0, width, -widthExtent, widthExtent, gridPos.x) + halfTileSize,
            mapRange(0, height, -heightExtent, heightExtent, gridPos.y) + halfTileSize, 
            0
        );

        return result;
    }

    private bool IsInGrid(Vector2 gridPos) {
        return gridPos.x >= 0 && gridPos.x < width &&
               gridPos.y < height && gridPos.y >= 0;
    }
}