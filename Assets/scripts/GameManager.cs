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
	
	//List of players that are in the grid
	public List <Player> players = new List<Player>();
	
	//Holds the current location of the players on the grid
	public Player[,] grid = new Player[11, 11];
	
	
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
	
	public bool isLocationInBounds(Location location)
	{
		return (location.X >= 0) && (location.X < mapSize) && (location.Y >= 0) && (location.Y < mapSize);
	}
	
	public bool isLocationOccupied(Location location)
	{
		if (! isLocationInBounds(location))
		{
			Debug.Log("Location is out of bounds");
			return false;
		}
		
		return grid[location.X, location.Y] != null;
	}
	
	public Location tileToLocation(Tile tile)
	{
		return new Location((int) tile.gridPosition.x, (int) tile.gridPosition.y);
	}
	
	public void highlightMoveableTiles(Location originLocation) {
		Player player = players[currentPlayerIndex];
		
		List<Location> locations = player.getMoveableLocations();
		
		foreach (Location location in locations)
		{
			map[location.X][location.Y].transform.renderer.material.color = Color.blue;
		}
	}
	
	public void highlightAttackableTiles(Location originLocation) {
		Player player = players[currentPlayerIndex];
		
		List<Location> locations = player.getAttackableLocations();
		
		foreach (Location location in locations)
		{
			map[location.X][location.Y].transform.renderer.material.color = Color.red;
		}
	}
	
	public void removeTileHighlights() {
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				map[i][j].transform.renderer.material.color = Color.white;
			}
		}
	}
	
	public void movePlayer(Player player, Location location)
	{
		
	}
 	
	public void moveCurrentPlayer(Location location) {
		
		if (isLocationOccupied(location))
		{
			Debug.Log("Can't move to occupied locaiton");
			return;
		}
		
		//Get the player that wants to move
		Player mover = players[currentPlayerIndex];
		
		//Get the current location
		Location current = mover.getLocation();
		
		//Remove the highlighted tiles
		removeTileHighlights();
		
		//Get the movement
		List<Tile> tiles = TilePathFinder.FindPath(map[current.X][current.Y], map[location.X][location.Y]);
		
		
		foreach (Tile t in tiles) {
			mover.positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 1.5f * Vector3.up);
		}
		
		//Let the player know he has moved
		mover.move(location);
		
		//Update the grid
		grid[current.X, current.Y] = null;
		grid[location.X, location.Y] = mover;
	}
	
	public void attackWithCurrentPlayer (Location location) {
		
		if (isLocationOccupied(location))
		{
			Player attacker = players[currentPlayerIndex];
			Player target = grid[location.X, location.Y];
			
			attacker.attack(target);
		}
		else
		{
			Debug.Log("No player at the locaiton");
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
		Location location = new Location(0, 0);
		
		assassinPlayer = ((GameObject)Instantiate(AssassinPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -0 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Assassin>();
		assassinPlayer.gridPosition = location;
		players.Add(assassinPlayer);
		grid[0, 0] = assassinPlayer;
		
		/*archerplayer = ((GameObject)Instantiate(ArcherPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -0 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Archer>();
		archerplayer.gridPosition = new Vector2(0,0);
		players.Add(archerplayer);
		
		assassinPlayer = ((GameObject)Instantiate(AssassinPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),1.5f, -(mapSize-1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Assassin>();
		assassinPlayer.gridPosition = new Vector2();
		players.Add(assassinPlayer);*/
		
		knightplayer = ((GameObject)Instantiate(KnightPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -1 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Knight>();
		knightplayer.gridPosition = new Location(0, 1);
		players.Add(knightplayer);
		grid[0, 1] = knightplayer;
		
		ai = ((GameObject)Instantiate(AIPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),1.5f, -(mapSize-2) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AI>();
		ai.gridPosition = new Location(mapSize - 1, mapSize - 2);
		players.Add(ai);
		ai.target = assassinPlayer;
		grid[mapSize - 1, mapSize - 2] = ai;
				

		//AI ai = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(6 - Mathf.Floor(mapSize/2),1.5f, -4 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AI>();
		
		//players.Add(ai);
	}
}
