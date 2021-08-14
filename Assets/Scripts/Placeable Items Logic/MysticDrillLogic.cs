using System;
using System.Collections;
using UnityEngine;
using static PlaceableHelpers;
using static HelpFuncs;

public class MysticDrillLogic : Placeable
{
	private GridControl grid;

	private Vector3 outputLocation;

	public bool isMining = false;

	public override void PlacedAction(GridControl _grid)
	{
		grid = _grid;
		outputLocation = transform.up * 2;

		Debug.Log(gameObject.transform.position + outputLocation);

		AddToWorld(grid, this, Vector3.up + Vector3.left); AddToWorld(grid, this, Vector3.up); AddToWorld(grid, this, Vector3.up + Vector3.right);
		AddToWorld(grid, this, Vector3.left); AddToWorld(grid, this); AddToWorld(grid, this, Vector3.right);
		AddToWorld(grid, this, Vector3.down + Vector3.left); AddToWorld(grid, this, Vector3.down); AddToWorld(grid, this, Vector3.down + Vector3.right);

		grid.Tick += TryMineOre;
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, Vector3.up + Vector3.left); RemoveFromWorld(grid, this, Vector3.up); RemoveFromWorld(grid, this, Vector3.up + Vector3.right);
		RemoveFromWorld(grid, this, Vector3.left); RemoveFromWorld(grid, this); RemoveFromWorld(grid, this, Vector3.right);
		RemoveFromWorld(grid, this, Vector3.down + Vector3.left); RemoveFromWorld(grid, this, Vector3.down); RemoveFromWorld(grid, this, Vector3.down + Vector3.right);

		grid.Tick -= TryMineOre;

		Destroy(gameObject);
	}

	public void TryMineOre(object sender, EventArgs e)
	{
		//Debug.Log("Event triggered");
		if (!isMining)
		{
			GameObject outputOre = null;
			while (!outputOre)
			{
				Debug.Log("Attempt to find ore");
				Vector2 orePos = new Vector2(UnityEngine.Random.Range(-1, 2) + transform.position.x, UnityEngine.Random.Range(-1, 2) + transform.position.y);
				GameObject chunkParent;
				if (grid.worldChunks.TryGetValue(GetChunk(transform.position), out chunkParent))
					chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(orePos).x, PosToPosInChunk(orePos).y].GetComponent<BaseOre>().MineOre(out outputOre);
			}
			StartCoroutine(Mining(this, outputOre));
		}
	}

	private IEnumerator Mining(MysticDrillLogic drill, GameObject outputOre)
	{
		Debug.Log("IEnumerator Triggered");
		drill.isMining = true;

		float timeElapsed = 0;

		while (timeElapsed <= grid.beltCycleTime * 4)
		{
			timeElapsed += Time.deltaTime;
		}

		GameObject chunkParent;
		BeltLogic outputBelt;
		if (grid.worldChunks.TryGetValue(GetChunk(drill.transform.position + drill.outputLocation), out chunkParent))
			if (chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(drill.transform.position).x, PosToPosInChunk(drill.transform.position).y].TryGetComponent<BeltLogic>(out outputBelt))
				if (outputBelt.getItemSlot())
				{
					GameObject itemSlot = outputBelt.getItemSlot();
					itemSlot = Instantiate(outputOre, outputBelt.transform.position, Quaternion.identity, chunkParent.transform);
					Debug.Log("Item mined");
				}


		drill.isMining = false;
		yield return null;
	}
}
