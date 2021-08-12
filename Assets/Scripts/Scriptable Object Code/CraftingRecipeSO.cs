using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting Recpie")]
public class CraftingRecipeSO : ScriptableObject
{
	public enum MachineType
	{
		FURNACE
	}

	public enum ItemCategory
	{
		ORE,
		INGOT
	}

	//what group will the output item go into?
	public ItemCategory itemCategory;

	//what Machines can this be crafted in
	public MachineType[] machineType;

	//Can this be crafted in the Hand?
	public bool handCraftable;

	//what items are required for this recipe
	public GameObject[] baseIngredients;

	//how much of the above items are required?
	//make sure the Quanty is in the space index or spot as the item in BaseIngredients
	public int[] baseIngredientsQuanty;

	//what outputs dose this have?
	public GameObject[] outputItems;

	//how much of the above items dose this give
	public int[] OutputItemsQuanty;
}
