using UnityEngine;
using System.Collections;

/// <summary>
/// Player script.cs is attached to the player.
	
/// </summary>

public class PlayerScript : MonoBehaviour 
{
	private Vector3 level0Rotation;
	private Vector3 level1Rotation;
	private Vector3 level2Rotation;
	private Vector3 level3Rotation;

	private int level0Mementos;
	private int level1Mementos;
	private int level2Mementos;
	private int level3Mementos;
	
	private int level;

	void Awake () 
	{
		level0Rotation = new Vector3 (0, 0, 0);
		level1Rotation = new Vector3 (0, 90, 0);
		level2Rotation = new Vector3 (0, 180, 0);
		level3Rotation = new Vector3 (0, 270, 0);
	
		level0Mementos = GameObject.Find("Level1").transform.childCount;
		level1Mementos = GameObject.Find("Level2").transform.childCount;
		level2Mementos = GameObject.Find("Level3").transform.childCount;
		level3Mementos = GameObject.Find("Level4").transform.childCount;
		
		level = 0;
	}
	
	void Start ()
	{
		
	}
	
	void Update () 
	{
		SceneManagement();
	}
	
	private void SceneManagement ()
	{
		switch (level)
		{
			case (0):
				Case0 ();
				break;
			case (1):
				Case1 ();
				break;
			case (2):
				Case2 ();
				break;
			case (3):
				Case3 ();
				break;
		}
	}
	
	private void Case0 ()
	{
		this.gameObject.transform.eulerAngles = level0Rotation;
		if (MementoScript.UsedLevelMementos == level0Mementos)
		{
			Debug.Log("ALL MEMENTOS FOUND IN LEVEL 0 !");
			NextLevel();
		}
	}
	
	private void Case1 ()
	{
		this.gameObject.transform.eulerAngles = level1Rotation;
		if (MementoScript.UsedLevelMementos == level1Mementos)
		{
			Debug.Log("ALL MEMENTOS FOUND IN LEVEL 1 !");
			NextLevel();
		}
	}
	
	private void Case2 ()
	{
		this.gameObject.transform.eulerAngles = level2Rotation;
		if (MementoScript.UsedLevelMementos == level2Mementos)
		{
			Debug.Log("ALL MEMENTOS FOUND IN LEVEL 2 !");
			NextLevel();
		}
	}
	
	private void Case3 ()
	{
		this.gameObject.transform.eulerAngles = level3Rotation;
		if (MementoScript.UsedLevelMementos == level3Mementos)
		{
			Debug.Log("ALL MEMENTOS FOUND IN LEVEL 3 !");
			NextLevel();
		}
	}
	
	
	private void NextLevel ()
	{
		MementoScript.UsedLevelMementos = 0;
		level++;
	}
}
