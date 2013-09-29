using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	
	int[,] collision;
	int[, ] map;
	bool[,] visited;
	float[,] scent;
	List<GameObject> ghosts;
	TilePlotter plotter;
	Ghost pacman;
	Ghost pacman2;
	bool paused = false;
	GameObject cubemaster;
	GameObject game;
	List<Ghost> pacmans;
	Atlas atlas;
	
	void Start () {
		Camera camera = this.gameObject.GetComponent<Camera>();
	//	this.gameObject.transform.localPosition = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
		Vector3 size = camera.ViewportToWorldPoint(new Vector3(1, 1, 0)) * 2;

		game = new GameObject("Game");
		
		GameObject board = new GameObject("board");
		SimpleSprite sprite = board.AddComponent<SimpleSprite>();
		sprite.m_texture = Resources.Load("board") as Texture2D;
		board.transform.parent = game.transform;
		board.transform.localScale = new Vector3(size.x, size.y, 1);
		board.transform.localPosition = new Vector3(0, 0, 0);
		
		GameObject atlasobject = new GameObject("Atlas");
		atlas = atlasobject.AddComponent<Atlas>();
		atlas.Init();
		
		map = new int[,] {
		{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
		{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
		{ 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0 ,0, 1, 0 },
		{ 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0 ,0, 1, 0 },
		{ 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0 ,0, 1, 0 },
		{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
		{ 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0 ,0, 1, 0 },
		{ 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0 ,0, 1, 0 },
		{ 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
		{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
		{ 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0 ,0, 1, 0 },
		{ 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0 },
		{ 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0 },
		{ 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0 },
		{ 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0 },
		{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
		{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
		{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};
		
		// transpose for columns first
		collision = new int[map.GetLength(1), map.GetLength(0)];
		visited = new bool[map.GetLength(1), map.GetLength(0)];
		scent = new float[map.GetLength(1), map.GetLength(0)];
		

		
		
		plotter = new TilePlotter();
		plotter.init(size, collision.GetLength(0), collision.GetLength(1));
		
		
		string[] names = new string[] { "redghost", "yellowghost", "pinkghost", "blueghost" };
		Vector2[] startposition = new Vector2[] { new Vector2(9, 12), new Vector2(11, 12), new Vector2(9, 13), new Vector2(11, 13) };
		
		ghosts = new List<GameObject>();
		for (int i = 0; i < names.Length; ++i) {
			GameObject ghost = new GameObject(names[i]);
			Ghost g = ghost.AddComponent<Ghost>();
			g.init(names[i], atlas, plotter, 2);
			ghost.transform.parent = game.transform;
			ghost.transform.localPosition = plotter.localToWorld( (int) startposition[i].x, (int) startposition[i].y);
			ghosts.Add(ghost);
		}
		
		pacmans = new List<Ghost>();
		GameObject pac = new GameObject("pacman");
		pacman = pac.AddComponent<Ghost>();
		pacman.init("pacman", atlas, plotter, 10);
		pacman.speed = 4;
		pacman.transform.parent = game.transform;
		pacman.transform.localPosition = plotter.localToWorld(1, 1);
		pacmans.Add(pacman);
		
		computeCollisions();
		computeScent();

	}
	
	void Init() {
		
	}
	
	void computeCollisions() {
		for (int i = 0; i < map.GetLength(0); ++i) {
			for (int j = 0; j < map.GetLength(1); ++j) {
				collision[j, i] = 1 - map[i, j];
			}
		}
		
		foreach (GameObject obj in ghosts) {
			Vector2 local = plotter.worldToLocal(obj.transform.localPosition.x, obj.transform.localPosition.y);
			collision[ (int) local.x, (int) local.y ] = 2;
		}
	}
	

	void resetScent() {
		for (int i = 0; i < visited.GetLength(0); ++i) {
			for (int j = 0; j < visited.GetLength(1); ++j) {
				scent[i, j] = 0.0f;
			}
		}
	}
	
	void computeScent() {
		
		resetScent();
		
		Queue openlist = new Queue();
		
		foreach (Ghost pac in pacmans) {
			openlist.Enqueue(pac.row * plotter.width + pac.col);
			scent[pac.col, pac.row] = int.MaxValue;
		}

		
		int counter = 0;
		float[] values = new float[] {0.5f, 0, 1 / 4096.0f};
			
		while (openlist.Count > 0) {
			int head = (int) openlist.Dequeue();
			int col = head % plotter.width;
			int row = (int)Mathf.Floor(head / plotter.width);
			
			Vector2[] neighbors = new Vector2[] { new Vector2(col - 1, row), new Vector2(col, row - 1), new Vector2(col+ 1, row), new Vector2(col, row + 1) };
			
			float scent_value = scent[col, row];
			float count = 0;
			for (int i = 0; i < neighbors.GetLength(0); ++i) {
				Vector2 n = neighbors[i];
				
				int c = (int) n.x;
				int r = (int) n.y;
				if (c >= 0 && c < collision.GetLength(0)) {
					if (scent_value < scent[c, r] * values[collision[col, row] ]) {
						scent_value = scent[c, r] * values[collision[col, row] ];
					}
				}
				
			}
			

			for (int i = 0; i < neighbors.GetLength(0); ++i) {
				Vector2 n = neighbors[i];
				
				int c = (int) n.x;
				int r = (int) n.y;
		
				int index = c + plotter.width * r;
				if (!openlist.Contains(index) && c >= 0 && c < collision.GetLength(0) && collision[c, r] != 1 && scent_value * values[collision[c, r] ] > scent[c, r]  ) {
					openlist.Enqueue(index);
				}
					
				
			}
			

			
			if (scent_value > scent[col, row]) {
				scent[ col, row ] = scent_value;
			}
			
	
		}
		
		
	}
	
	bool isFree(int c, int r) {
		foreach (GameObject obj in ghosts) {
			Ghost ghost = obj.GetComponent<Ghost>();
			
			if (c == 1 && r == 1) {
				return false;
			}
			if (ghost.target.x == c && ghost.target.y == r ) {
				return false;
			}
		}
		
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha2) && pacmans.Count == 1) {
			GameObject pac = new GameObject("pacman2");
			pacman2 = pac.AddComponent<Ghost>();
			pacman2.init("pacman", atlas, plotter, 10);
			pacman2.speed = 4;
			pacman2.transform.parent = game.transform;
			pacman2.transform.localPosition = plotter.localToWorld(1, 1);
			pacmans.Add(pacman2);
		}
		
        if (Input.GetKey(KeyCode.LeftArrow)) {
			if (pacman.col - 1 >= 0 && collision[pacman.col - 1, pacman.row] != 1) {
				pacman.target.x = pacman.col - 1;
				pacman.target.y = pacman.row;
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow)) {
			if (pacman.col + 1 < collision.GetLength(0) && collision[pacman.col + 1, pacman.row] != 1) {
				pacman.target.x = pacman.col + 1;
				pacman.target.y = pacman.row;
			}
		}
		else if (Input.GetKey(KeyCode.UpArrow)) {
			if (collision[pacman.col, pacman.row - 1] != 1) {
				pacman.target.x = pacman.col;
				pacman.target.y = pacman.row - 1;
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			if (collision[pacman.col, pacman.row + 1] != 1) {
				pacman.target.x = pacman.col;
				pacman.target.y = pacman.row + 1;
			}
		}
		if (pacmans.Count == 2) {
	        if (Input.GetKey(KeyCode.A)) {
				if (pacman2.col - 1 >= 0 && collision[pacman2.col - 1, pacman2.row] != 1) {
					pacman2.target.x = pacman2.col - 1;
					pacman2.target.y = pacman2.row;
				}
			}
			else if (Input.GetKey(KeyCode.D)) {
				if (pacman2.col + 1 < collision.GetLength(0) && collision[pacman2.col + 1, pacman2.row] != 1) {
					pacman2.target.x = pacman2.col + 1;
					pacman2.target.y = pacman2.row;
				}
			}
			else if (Input.GetKey(KeyCode.W)) {
				if (collision[pacman2.col, pacman2.row - 1] != 1) {
					pacman2.target.x = pacman2.col;
					pacman2.target.y = pacman2.row - 1;
				}
			}
			else if (Input.GetKey(KeyCode.S)) {
				if (collision[pacman2.col, pacman2.row + 1] != 1) {
					pacman2.target.x = pacman2.col;
					pacman2.target.y = pacman2.row + 1;
				}
			}
		}
		
		if (Input.GetKeyDown(KeyCode.P)) {
			paused = !paused;
			
			if (paused) {
				
				cubemaster = new GameObject("cubes");
				cubemaster.transform.parent = game.transform;
				cubemaster.transform.localPosition = Vector3.zero;
				int index = 0;
				for (int i = 0; i < scent.GetLength(0); ++i) {
					for (int j = 0; j < scent.GetLength(1); ++j) {
						if (scent[i, j] > 0) {
							GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
							cube.name = "cube" + index.ToString();
							index++;
							cube.transform.parent = cubemaster.transform;
							cube.transform.localScale = new Vector3(plotter.tile_width, plotter.tile_height, 100 * Mathf.Log(1 + scent[i, j]) );
							Vector3 local = plotter.localToWorld(i, j);
							cube.transform.localPosition = new Vector3(local.x, local.y, -cube.transform.localScale.z / 2);
						}
					}
				}
				
				
			}
			else {
				Destroy(cubemaster);
			}
		}
         
		if (paused) {
			return;
		}
		
		computeCollisions();
		computeScent();
		
		foreach (GameObject obj in ghosts) {
			Ghost ghost = obj.GetComponent<Ghost>();
			ghost.target = new Vector2(-1, -1);
		}
		
		foreach (GameObject obj in ghosts) {
			Vector2 local = plotter.worldToLocal(obj.transform.localPosition.x, obj.transform.localPosition.y);
			
			Vector2[] neighbors = new Vector2[] { new Vector2(local.x - 1, local.y), new Vector2(local.x, local.y - 1), new Vector2(local.x + 1, local.y), new Vector2(local.x - 1, local.y), new Vector2(local.x, local.y + 1) };
			
			Ghost ghost = obj.GetComponent<Ghost>();
			float max_scent = 0;
			Vector2 target = new Vector2(-1, -1);
			foreach (Vector2 n in neighbors) {
				if (n.x >= 0 && n.x < collision.GetLength(0)) {
					
					int c = (int) n.x;
					int r = (int) n.y;
					if ( scent[ c, r ] > max_scent && isFree( c, r) ) {
						max_scent = scent[c, r];
						target = n;
					}
				}
			}
			
			if ( max_scent > scent[ (int) local.x, (int) local.y ] ) {
				ghost.target = target;
			}
			
		}
		
	}
}
