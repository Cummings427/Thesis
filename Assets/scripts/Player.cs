using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public Location gridPosition;
	public float moveSpeed = 8.0f;
	public Vector3 moveDestination;
	public bool moving = false;
	public bool attacking = false;
	public float damageRollSides = 6; //d6
	
	private int attackPoints = 1;
	private int movePoints = 1;
	private string playerName = "AI!";
	private int attackRange = 1;
	private float attackChance = 0.75f;
	private float defenseReduction = 0.15f;
	private int damageBase = 5;
	private int movementPerActionPoint = 2;
	private int HealthPower = 25;
	// Use this for initialization
	
	//movement animation
	public List<Vector3> positionQueue = new List<Vector3> ();	
	//
	
	void Awake ()
	{
		moveDestination = transform.position;
	}
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public virtual void TurnUpdate ()
	{
		if (attackPoints <= 0 || movePoints <= 0) {
			attackPoints = 1;
			movePoints = 1;
			moving = false;
			attacking = false;			
			GameManager.instance.nextTurn ();
		}
	}
	
	public virtual void TurnOnGUI ()
	{
		
	}
	
	public int getHealthPower(){
		return HealthPower;	
	}
	
	public void setHealthPower(int healthpower){
		HealthPower = healthpower;	
	}
	
	public int getmovementPerActionPoint(){
		return movementPerActionPoint;	
	}
	
	public void setmovementPerActionPoint(int mpap){
		movementPerActionPoint = mpap;	
	}
	
	public int getdamageBase(){
		return damageBase;	
	}
	
	public void setdamageBase(int damagebase){
		damageBase = damagebase;	
	}
	
	public float getdefenseReduction(){
		return defenseReduction;	
	}
	
	public void setdefenseReduction(float dr){
		defenseReduction = dr;	
	}
	
	public float getattackChance(){
		return attackChance;	
	}
	
	public void setattackChance(float ac){
		attackChance = ac;	
	}
	
	public int getattackRange(){
		return attackRange;	
	}
	
	public void setattackRange(int ar){
		attackRange = ar;	
	}
	
	public string getplayerName(){
		return playerName;	
	}
	
	public void setplayerName(string pn){
		playerName = pn;	
	}
	
	public int getmovePoints(){
		return movePoints;	
	}
	
	public void setmovePoints(int mp){
		movePoints = mp;	
	}
	
	public int getattackPoints(){
		return attackPoints;	
	}
	
	public void setattackPoints(int ap){
		attackPoints = ap;	
	}
	
	public Location getLocation()
	{
		return gridPosition;
	}
	
	public List<Location> getMoveableLocations()
	{
		var locations = new List<Location>();
		var game = GameManager.instance;
		
		for (int i = gridPosition.X - movementPerActionPoint; i < gridPosition.X + movementPerActionPoint; i++)
		{
			for (int j = gridPosition.Y - movementPerActionPoint; j < gridPosition.Y + movementPerActionPoint; j++)
			{
				var location = new Location(i, j);
				
				if (game.isLocationInBounds(location))
				{
					locations.Add(location);
				}
			}
		}
		
		return locations;
	}
	
	public List<Location> getAttackableLocations()
	{
		var locations = new List<Location>();
		var game = GameManager.instance;
		
		for (int i = (gridPosition.X - attackRange); i <= (gridPosition.X + attackRange); i++)
		{
			for (int j = (gridPosition.Y - attackRange); j <= (gridPosition.Y + attackRange); j++)
			{
				var location = new Location(i, j);
				
				if (game.isLocationInBounds(location))
				{
					locations.Add(location);
				}
			}
		}
		
		return locations;
	}
	
	public void move(Location location)
	{
		var game = GameManager.instance;
		
		if (!game.isLocationOccupied(location))
		{
			gridPosition = location;
			Debug.Log("Move to position " + location.X + ", " + location.Y);
		}
		else
		{
			Debug.Log("Tried to move to a occupied locaiton");
		}
	}
	
	public void takeDamage(int damage)
	{
		//TODO add damage reduction
		setHealthPower(getHealthPower()- damage);
	}
	
	public void attack(Player target)
	{
		List<Location> locations = getAttackableLocations();
		
		if (! locations.Contains(target.getLocation()))
		{
			Debug.Log("Can not attack player");
			return;
		}
		
		target.takeDamage(getdamageBase());
		Debug.Log("Target was reduced to " + target.getHealthPower() + " health");
	}
}
