using UnityEngine;
using System.Collections;

public class TilePlotter {
	public int width;
	public int height;
	public float tile_width;
	public float tile_height;
	private Vector3 size;

	public void init(Vector3 size, int width, int height) {
		this.size = size;
		this.width = width;
		this.height = height;
		tile_width = size.x / width;
		tile_height = size.y / height;
	}
	
	
	public Vector3 localToWorld(int col, int row) {
		return new Vector3( ( col + 0.5f ) * tile_width - size.x / 2 , size.y - ( row + 0.5f )  * tile_height - size.y / 2, -0.01f);
	}
	
	public Vector2 worldToLocal(float x, float y) {
		return new Vector2( Mathf.Floor( (x + size.x / 2) / tile_width ), height - Mathf.Ceil( (y + size.y / 2)  / tile_height));
	}
	
}
