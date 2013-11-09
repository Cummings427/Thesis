using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public Vector2 gridPosition = Vector2.zero;
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
}
