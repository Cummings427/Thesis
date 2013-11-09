using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject AIPrefab;
	public GameObject KnightPrefab;
	public GameObject ArcherPrefab;
	public GameObject AssassinPrefab;
	
	public int mapSize = 11;
	
	public List <List<Tile>> map = new List<List<Tile>>();
	public List <Player> players = new List<Player>();
	public int currentPlayerIndex = 0;
	
	void Awake() {
		instance = this;
	}
	
	// Use this for initialization
	void Start () {	
		generateMap();
		generatePlayers();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (players[currentPlayerIndex].getHealthPower() > 0) players[currentPlayerIndex].TurnUpdate();
		else nextTurn();
	}
	
	void OnGUI () {
		if (players[currentPlayerIndex].getHealthPower() > 0) players[currentPlayerIndex].TurnOnGUI();
	}
	
	public void nextTurn() {
		if (currentPlayerIndex + 1 < players.Count) {
			currentPlayerIndex++;
		} else {
			currentPlayerIndex = 0;
		}
	}
	
	public Tile getTileAtLocation(int x, int y)
	{
		return map[x][y];
	}
	
	public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance) {
		List <Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance);
		
		foreach (Tile t in highlightedTiles) {
			t.transform.renderer.material.color = highlightColor;
		}
	}
	
	public void removeTileHighlights() {
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				if (!map[i][j].impassible) map[i][j].transform.renderer.material.color = Color.white;
			}
		}
	}
 	
	public void moveCurrentPlayer(Tile destTile) {
		if (destTile.transform.renderer.material.color != Color.white && !destTile.impassible) {
			removeTileHighlights();
			players[currentPlayerIndex].moving = false;
			foreach(Tile t in TilePathFinder.FindPath(map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y],destTile)) {
				players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 1.5f * Vector3.up);
			}			
			players[currentPlayerIndex].gridPosition = destTile.gridPosition;
		} else {
			Debug.Log ("Invalid");
		}
	}
	
	public void attackWithCurrentPlayer(Tile destTile) {
		if (destTile.transform.renderer.material.color != Color.white && !destTile.impassible) {
			
			Player target = null;
			foreach (Player p in players) {
				if (p.gridPosition == destTile.gridPosition) {
					target = p;
				}
			}
			
			if (target != null) {
				if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - players[currentPlayerIndex].getattackRange() && players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + players[currentPlayerIndex].getattackRange() &&
					players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - players[currentPlayerIndex].getattackRange() && players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + players[currentPlayerIndex].getattackRange()) {
					players[currentPlayerIndex].setattackPoints(0);
					
					removeTileHighlights();
					players[currentPlayerIndex].moving = false;			
					
					//attack logic
					//roll to hit
					bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].getattackChance();
					
					if (hit) {
						//damage logic
						int amountOfDamage = (int)Mathf.Floor(players[currentPlayerIndex].getdamageBase()); //+ Random.Range(0, players[currentPlayerIndex].damageRollSides));
						
						target.setHealthPower(target.getHealthPower()- amountOfDamage);
						
						Debug.Log(players[currentPlayerIndex].getplayerName() + " hit " + target.getplayerName() + " for " + amountOfDamage + " damage!");
					} else {
						Debug.Log(players[currentPlayerIndex].getplayerName() + " missed " + target.getplayerName());
					}
				} else {
					Debug.Log ("Target is not adjacent!");
				}
			}
		} else {
			Debug.Log ("destination invalid");
		}
	}
	
	void generateMap() {
		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++) {
			List <Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) {
				Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize/2),0, -j + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i, j);
				row.Add (tile);
			}
			map.Add(row);
		}
	}
	
	void generatePlayers() {
	//	UserPlayer player;
		AI ai;
		Knight knightplayer;
		Archer archerplayer;
		Assassin assassinPlayer;
		
		archerplayer = ((GameObject)Instantiate(ArcherPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -0 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Archer>();
		archerplayer.gridPosition = new Vector2(0,0);
		players.Add(archerplayer);
		
		assassinPlayer = ((GameObject)Instantiate(AssassinPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),1.5f, -(mapSize-1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Assassin>();
		assassinPlayer.gridPosition = new Vector2();
		players.Add(assassinPlayer);
		
		knightplayer = ((GameObject)Instantiate(KnightPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -1 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Knight>();
		knightplayer.gridPosition = new Vector2(0,1);
		players.Add(knightplayer);
		
		ai = ((GameObject)Instantiate(AIPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),1.5f, -(mapSize-2) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AI>();
		ai.gridPosition = new Vector2(mapSize-1,mapSize-2);
		players.Add(ai);
				

		//AI ai = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(6 - Mathf.Floor(mapSize/2),1.5f, -4 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AI>();
		
		//players.Add(ai);
	}
}
