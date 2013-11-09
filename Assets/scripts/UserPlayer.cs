using UnityEngine;
using System.Collections;

public class UserPlayer : Player {

	public GameObject[] Kill;
	
	void AddCompnent(){
		base.setHealthPower(20);
		base.setmovementPerActionPoint(5);
		base.setdamageBase(7);
		base.setdefenseReduction(0.15f);
		base.setattackChance(0.75f);
		base.setattackRange(1);
		base.setplayerName("User!");
		base.setmovePoints(1);
		base.setattackPoints(1);
	}
	// Use this for initialization
	void Start () {
				AddCompnent();

	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex] == this) {
			transform.renderer.material.color = Color.yellow;
		} else {
			transform.renderer.material.color = Color.white;
		}
		
		if (getHealthPower() <= 0) {
			transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
			transform.renderer.material.color = Color.red;
		}
	}
	
	public override void TurnUpdate ()
	{	
		if (positionQueue.Count > 0) {
			transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f) {
				transform.position = positionQueue[0];
				positionQueue.RemoveAt(0);
				if (positionQueue.Count == 0) {
					setmovePoints(0);
				}
			}
			
		}
		
		base.TurnUpdate ();
	}
	
	public override void TurnOnGUI () {
		float buttonHeight = 25;
		float buttonWidth = 100;
		
		Rect buttonRect = new Rect(0, buttonHeight, buttonWidth, buttonHeight);
		
		
		//move button
		if (GUI.Button(buttonRect, "Move")) {
			if (!moving) {
				GameManager.instance.removeTileHighlights();
				moving = true;
				attacking = false;
				//GameManager.instance.highlightTilesAt(gridPosition, Color.blue, getmovementPerActionPoint());
			} else {
				moving = false;
				attacking = false;
				GameManager.instance.removeTileHighlights();
			}
		}
		
		//attack button
		buttonRect = new Rect(buttonWidth * 1, buttonHeight, buttonWidth, buttonHeight);
		
		if (GUI.Button(buttonRect, "Attack")) {
			if (!attacking) {
				GameManager.instance.removeTileHighlights();
				moving = false;
				attacking = true;
				//GameManager.instance.highlightTilesAt(gridPosition, Color.red, getattackRange());
			} else {
				moving = false;
				attacking = false;
				GameManager.instance.removeTileHighlights();
			}
		}
		
		//end turn button
		buttonRect = new Rect(buttonWidth * 2, buttonHeight, buttonWidth, buttonHeight);		
		
		if (GUI.Button(buttonRect, "End Turn")) {
			GameManager.instance.removeTileHighlights();
			setattackPoints(1);
			setmovePoints(1);
			moving = false;
			attacking = false;			
			GameManager.instance.nextTurn();
		}
		
		buttonRect = new Rect(buttonWidth * 3, buttonHeight, buttonWidth, buttonHeight);		
		
		if (GUI.Button(buttonRect, "Special")) {
			if (!attacking) {
				GameManager.instance.removeTileHighlights();
				moving = false;
				attacking = true;
				//GameManager.instance.highlightTilesAt(gridPosition, Color.red, getattackRange());
			} else {
				moving = false;
				attacking = false;
				GameManager.instance.removeTileHighlights();
			}
		}
		
		base.TurnOnGUI ();
	}
}
