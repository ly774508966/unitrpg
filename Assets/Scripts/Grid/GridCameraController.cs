﻿using UnityEngine;
using System.Collections;

public class GridCameraController : CameraController {
	
	public MapGrid grid;
    public GameObject GridHighlightPrefab;

    private GameObject GridHighlight;

    public void Start() {

        GridHighlight = Instantiate(GridHighlightPrefab) as GameObject;
    }

	public void Update () {
		base.Update();

		Vector2 gridCenter = grid.transform.position;
		float vSize = Camera.main.orthographicSize * 2.0f;
		float hSize = vSize * ((float)Screen.width / Screen.height);
		
		float halfHSize = hSize * 0.5f;
		float halfVSize = vSize * 0.5f;
		
		float gridHalfWidth = (grid.width/2.0f) * grid.tileSizeInPixels;
		float maxX = gridCenter.x + gridHalfWidth;
		float minX = gridCenter.x - gridHalfWidth;
		
		float leftEdge = transform.position.x - halfHSize;
		if (leftEdge < minX) {
			transform.position = new Vector3(minX + halfHSize, transform.position.y, transform.position.z);
		}
		
		float rightEdge = transform.position.x + halfHSize;
		if (rightEdge > maxX) {
			transform.position = new Vector3(maxX - halfHSize, transform.position.y, transform.position.z);
		}
		
		
		float gridHalfHeight = (grid.height/2.0f) * grid.tileSizeInPixels;
		float maxY = gridCenter.y + gridHalfHeight;
		float minY = gridCenter.y - gridHalfHeight;
		
		float topEdge = transform.position.y + halfVSize;
		if (topEdge > maxY) {
			transform.position = new Vector3(transform.position.x, maxY - halfVSize, transform.position.z);
		}
		
		float bottomEdge = transform.position.y - halfVSize;
		if (bottomEdge < minY) {
			transform.position = new Vector3(transform.position.x, minY + halfVSize, transform.position.z);
		}

        Vector2? mouseGridPos = grid.GetMouseGridPosition();
        Vector3 gridPosWorldCenter = grid.GetWorldPosForGridPos(mouseGridPos.Value);
        GridHighlight.transform.localPosition = gridPosWorldCenter;
	}
}
