using UnityEngine;
using System.Collections;

/// <summary>
/// Memento script.cs is attached to an empty gameObject to keep track of all the used mementos
/// </summary>

public class MementoScript 
{

	private static int usedLevelMementos = 0;
	private static int usedTotalMementos = 0;
	
	public MementoScript ()	
	{
		
	}
	
	public static int UsedLevelMementos 
	{
		get {return usedLevelMementos;}
		set {usedLevelMementos = value;}
	}
	
	public static int UsedTotalMementos
	{
		get {return usedTotalMementos;}
		set {usedTotalMementos = value;}
	}
}
