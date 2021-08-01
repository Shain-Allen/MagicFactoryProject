using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOre
{
	public GameObject ore;
	public GameObject outputItem;
	public int remainingOre;

	public BaseOre(GameObject ore, GameObject outputItem, int remainingOre)
	{
		this.ore = ore;
		this.outputItem = outputItem;
		this.remainingOre = remainingOre;
	}
}