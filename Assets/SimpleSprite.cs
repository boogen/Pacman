using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Simple sprite.
/// Displays 2D quad at xy position
/// </summary>
public class SimpleSprite : MonoBehaviour
{
	public Texture2D m_texture;
	
	private Mesh m_mesh;
	private BoxCollider m_bc;
	
	public Mesh createMesh()
	{
		Mesh mesh = new Mesh();
		
		Vector3[] vertices = new Vector3[] 
		{
			new Vector3(-0.5f, -0.5f, 0),
			new Vector3(0.5f, 0.5f, 0),
			new Vector3(0.5f, -0.5f, 0),
			new Vector3(-0.5f, 0.5f, 0)
		};
		
		Vector2[] uv = new Vector2[]
		{
			new Vector2(0, 0),
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, 1)
		};
		
		int[] triangles = new int[]
		{
			0, 1, 2, 
			0, 3, 1
		};
		
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		
		return mesh;
	}	
	
	// Use this for initialization
	void Start ()
	{
		if( m_texture == null )
			throw new Exception( "Texture is null!" );
		
		m_mesh = createMesh();
		
		MeshFilter mf = gameObject.AddComponent<MeshFilter>();

		mf.mesh = m_mesh;
		m_bc = gameObject.AddComponent<BoxCollider>();
		
		// setup material	
		Material mtl = new Material( Shader.Find( "Custom/PixelShader" ) );
		mtl.SetTexture( "_MainTex", m_texture );
		mtl.SetColor( "_Color", Color.white );
		
		Renderer m_renderer = gameObject.GetComponent<MeshRenderer>();
		if( m_renderer == null )
			m_renderer = gameObject.AddComponent<MeshRenderer>();

		m_renderer.material = mtl;		
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
