using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : Player
{
	
	public Player target;
	
	void AddCompnent ()
	{
		base.setHealthPower (20);
		base.setmovementPerActionPoint (5);
		base.setdamageBase (7);
		base.setdefenseReduction (0.15f);
		base.setattackChance (0.75f);
		base.setattackRange (1);
		base.setplayerName ("Jerk Face");
		base.setmovePoints (1);
		base.setattackPoints (1);
	}
	
	// Use this for initialization
	void Awake ()
	{
	
		//Kill = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	void Start ()
	{
		AddCompnent ();
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (GameManager.instance.players [GameManager.instance.currentPlayerIndex] == this) {
			transform.renderer.material.color = Color.blue;
		} else {
			transform.renderer.material.color = Color.white;
		}
		
		if (getHealthPower () <= 0) {
			transform.rotation = Quaternion.Euler (new Vector3 (90, 0, 0));
			transform.position = new Vector3 (-10, 0, 0);
			transform.renderer.material.color = Color.red;
		}
	}
	
	public override void TurnUpdate ()
	{
		//find path to kill object
		Location targetLoc = target.getLocation();
		List<Location> attackable = getAttackableLocations();
		
		if (attackable.Contains(targetLoc))
		{
			Debug.Log("AI attacked target");
			this.attack(target);
			GameManager.instance.nextTurn ();
			return;
		}
		
		List<Location> moveable = getMoveableLocations();
		int index = Random.Range(0, moveable.Count);
		GameManager.instance.moveCurrentPlayer(moveable[index]);
		
		if (positionQueue.Count > 0) {
			transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f) {
				transform.position = positionQueue[0];
				positionQueue.RemoveAt(0);
				if (positionQueue.Count == 0) {
					Debug.Log (positionQueue);
					setmovePoints(0);
				}
			}
		}
		
		base.TurnUpdate ();
	}
		
		
		/*var obj = Kill[1];
		moveDestination = new Vector3 (obj.transform.position.x, 2, obj.transform.position.z);
		Debug.Log("Path to kill object update to + " + moveDestination.ToString());
		
		//Debug.Log ("begin");
		if (Vector3.Distance (moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			Debug.Log ("meow");
			if (Vector3.Distance (moveDestination, transform.position) <= 0.1f) {
				Tile desireTile = GameManager.instance.getTileAtLocation((int) moveDestination.x, (int) moveDestination.y);
				GameManager.instance.moveCurrentPlayer(desireTile);
				Debug.Log ("dog");
				transform.position = moveDestination;
				setmovePoints (0);
				//positionQueue[0] = new Vector3 (0, 1.5f, 0);
				Debug.Log (moveDestination);
				//}
			} else {
				//moveDestination = new Vector3(0 - Mathf.Floor(GameManager.instance.mapSize/2),1.5f, -0 + Mathf.Floor(GameManager.instance.mapSize/2));
				//moveDestination = new Vector3 (0, 1.5f, 0);
				//foreach (GameObject obj in Kill) {
				//	moveDestination = new Vector3 (obj.transform.position.x, obj.transform.position.y + 2, obj.transform.position.z);
				//}	
				Tile desireTile = GameManager.instance.getTileAtLocation((int) moveDestination.x, (int) moveDestination.y);
				GameManager.instance.moveCurrentPlayer(desireTile);
				Debug.Log ("cat");
				transform.position = moveDestination;
				setmovePoints (0);
				//positionQueue[0] = new Vector3 (0, 1.5f, 0);
				Debug.Log (moveDestination);
		
			}
		}
		else
		{
			GameManager.instance.nextTurn ();
		}
		
		base.TurnUpdate ();
	}
	/*{

		if (Vector3.Distance (moveDestination, transform.position) > 0.1f) {
		//GameManager.instance.highlightTilesAt(gridPosition, Color.blue, getmovementPerActionPoint());
		transform.position = moveDestination;
		moveDestination = new Vector3 (0, 1.5f, 0);
			Debug.Log ("woof");
			Debug.Log (moveDestination);
			Debug.Log (positionQueue);
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			//transform.position = new Vector3 (0, 1.5f, 0);
			setmovePoints(0);
			GameManager.instance.moveCurrentPlayer(this);
			if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f) {
				Debug.Log ("eee");
				transform.position = positionQueue[0];
				positionQueue.RemoveAt(0);
				if (positionQueue.Count == 0) {
					setmovePoints(0);
				}
			}
		}
		base.TurnUpdate ();
	}*/
	
	public override void TurnOnGUI ()
	{
		//GameManager.instance.highlightTilesAt(gridPosition, Color.blue, getmovementPerActionPoint());
		/*float buttonHeight = 25;
		float buttonWidth = 100;
		Rect buttonRect = new Rect(0, buttonHeight, buttonWidth, buttonHeight);
				
		//move button
		
		
		if (GUI.Button(buttonRect, "Move")) {
			if (!moving) {
				GameManager.instance.removeTileHighlights();
				//moving = true;
				attacking = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, getmovementPerActionPoint());
			} else {
				//moving = false;
				attacking = false;
				GameManager.instance.removeTileHighlights();
			} moving = !moving;
		}*/
		base.TurnOnGUI ();
	}
}

