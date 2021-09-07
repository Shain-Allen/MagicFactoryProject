using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;
using static ResourceHelpers;

public class MysticDrillLogic : Placeable
{
	private Vector3 outputLocation;
	private bool isMining = false;
	private List<BaseOre> minableOres = new List<BaseOre>();
	[SerializeField]
	private int miningRadius = 2;

	public override void PlacedAction(GridControl grid_)
	{
		// Add this 3x3 to the relative positons, minus 0,0 because it will be auto added
		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				if (x != 0 && y != 0)
					relativePositions.Add(new Vector2(x, y));
		base.PlacedAction(grid_);

		// Fill the minableOres list with all the minable ores
		BaseOre tempOre;
		for (int x = -miningRadius; x <= miningRadius; x++)
			for (int y = -miningRadius; y <= miningRadius; y++)
				if (tempOre = GetResourceAt<BaseOre>(grid, transform.position + new Vector3(x, y)))
					minableOres.Add(tempOre);

		outputLocation = transform.up * 2;
		grid.Tick += TryMineOre;
	}

	public override void RemovedAction()
	{
		grid.Tick -= TryMineOre;
		base.RemovedAction();
	}

	public void TryMineOre(object sender, EventArgs e)
	{
		if (!isMining && minableOres.Count > 0)
			StartCoroutine(Mining());
	}

	private IEnumerator Mining()
	{
		int oreInListToMine = UnityEngine.Random.Range(0, minableOres.Count);
		BaseOre outputOre = minableOres[oreInListToMine];
		GameObject returnOre;
		isMining = true;

		yield return new WaitForSeconds(grid.beltCycleTime * 4);

		BeltLogic outputBelt = GetPlaceableAt<BeltLogic>(grid, transform.position + outputLocation);
		if (outputBelt)
		{
			yield return new WaitWhile(() => outputBelt.AllowItem(null));

			if (outputBelt.AllowItem(null))
			{
				outputOre.MineOre(out returnOre);
				outputBelt.setItemSlot(null, Instantiate(returnOre, outputBelt.transform.position, Quaternion.identity, outputBelt.transform.parent));
				if (outputOre == null)
					minableOres.RemoveAt(oreInListToMine);
			}
		}
		isMining = false;
		yield return null;
	}
}