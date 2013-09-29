using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour {
	
	
	TilePlotter plotter;
	
	public Vector2 target;
	Atlas atlas;
	string animation_name;
	
	public float speed = 2;
	
	public int col = 1;
	public int row = 1;
	
	// Use this for initialization
	void Start () {
		target = new Vector2(-1, -1);
	}
	
	public void init(string animation_name, Atlas atlas, TilePlotter plotter, float fps) {
		this.plotter = plotter;
		this.atlas = atlas;
		this.animation_name = animation_name;
		
		StopMotion animation = this.gameObject.AddComponent<StopMotion>();
		animation.texture = atlas.texture;
		animation.rects = atlas.getFrames(animation_name + "_down");
		animation.fps = fps;
		animation.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (target.x != -1) {
			Vector2 local = plotter.worldToLocal(transform.localPosition.x, transform.localPosition.y);
			col = (int) local.x;
			row = (int) local.y;
			
			int c = (int) target.x;
			int r = (int) target.y;
			Vector2 pos = plotter.localToWorld( (int) target.x, (int) target.y );
			
			pos.x = pos.x - transform.localPosition.x;
			pos.y = pos.y - transform.localPosition.y;
			
			if (col == c && pos.x != 0) {
				pos.y = 0;
			}
			else if (row == r && pos.y != 0) {
				pos.x = 0;
			}
			
			StopMotion animation = gameObject.GetComponent<StopMotion>();
			if (pos.x < 0) {
				animation.rects = atlas.getFrames(animation_name + "_left");
			}
			else if (pos.x > 0) {
				animation.rects = atlas.getFrames(animation_name + "_right");
			}
			if (pos.y < 0) {
				animation.rects = atlas.getFrames(animation_name + "_down");
			}
			else if (pos.y > 0) {
				animation.rects = atlas.getFrames(animation_name + "_up");
			}
			
			animation.Refresh();
			
			if (pos.magnitude > speed) {
				pos.Normalize();
				pos.x *= speed;
				pos.y *= speed;
			}
			
			transform.localPosition = new Vector3(transform.localPosition.x + pos.x, transform.localPosition.y + pos.y, -0.01f);
		}
	}
}
