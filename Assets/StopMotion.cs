using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;

public class StopMotion : MonoBehaviour {
	
	ArrayList frames;
	List< Vector3[] > vertices = new List<Vector3[]>();
	
	public string name;
	public float fps;
	public bool loop = true;
	public delegate void Callback();
	public Callback callback;
	public bool playing = false;
	public bool finished = false;
	public float scale = 1.0f;
	
	public List<Rect> rects;
	public Texture2D texture;
	
	int frame = 0;
	float time = 0;
	
	public void Refresh() {
		frames.Clear();
		for (int i = 0; i < rects.Count; ++i) {
			Rect r = rects[i];
			
			Vector2[] uvs = new Vector2[4];
			uvs[0] = new Vector2(r.x / texture.width, 1 - (r.y + r.height) / texture.height);
			uvs[1] = new Vector2( ( r.x + r.width ) / texture.width, 1 - r.y / texture.height );
			uvs[2] = new Vector2( ( r.x + r.width ) / texture.width, 1-  ( r.y + r.height )  / texture.height );
			uvs[3] = new Vector2( r.x / texture.width, 1 -  r.y / texture.height );
			frames.Add(uvs);
		}
		
			MeshFilter mf = GetComponent<MeshFilter>();
			if (mf) {
				mf.mesh.vertices = (Vector3[]) vertices[frame];
				mf.mesh.uv = (Vector2[]) frames[frame];
			}
	}
	
	// Use this for initialization
	void Start () {
		Material mtl = new Material( Shader.Find( "Custom/PixelShader" ) );
		mtl.SetTexture( "_MainTex", texture );
		mtl.SetColor( "_Color", Color.white );
		
		Renderer m_renderer = gameObject.GetComponent<MeshRenderer>();
		if( m_renderer == null )
			m_renderer = gameObject.AddComponent<MeshRenderer>();

		m_renderer.material = mtl;	
		
		frames = new ArrayList();
		vertices.Clear();
		
		for (int i = 0; i < rects.Count; ++i) {
			Rect r = rects[i];
			transform.localScale = new Vector3( 2 * r.width, 2 * r.height, 1);
			
			
			
			Vector3[] verts = new Vector3[] 
			{
				new Vector3(-0.5f, -0.5f, 0),
				new Vector3(0.5f, 0.5f, 0),
				new Vector3(0.5f, -0.5f, 0),
				new Vector3(-0.5f, 0.5f, 0)
			};
			
			vertices.Add(verts);
		}
		
		Refresh();
		

		if( vertices.Count == 0 )
		{
			throw new Exception( "No vertices for StopMotion: " + name );
		}

		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.mesh = new Mesh();
		mf.mesh.vertices = vertices[0];
		mf.mesh.uv = (Vector2[]) frames[0];
		mf.mesh.triangles = new int[] {0, 1, 2, 0, 3, 1};
		mf.mesh.RecalculateNormals();

	}
	
	public void Play() {
		playing = true;
	}
	
	
	// Update is called once per frame
	void Update () {
		if (playing) {
			time += Time.deltaTime;
			if (time >= 1 / fps) {
				MeshFilter mf = GetComponent<MeshFilter>();
				mf.mesh.vertices = (Vector3[]) vertices[frame];
				mf.mesh.uv = (Vector2[]) frames[frame];
			
				if (loop || frame + 1 < frames.Count) {
					frame = (frame + 1) % frames.Count;
				}
				else {
					if (callback != null) {
						callback();
					}
					finished = true;
				}
				time -= 1 / fps;
			}
		}
	}
}
