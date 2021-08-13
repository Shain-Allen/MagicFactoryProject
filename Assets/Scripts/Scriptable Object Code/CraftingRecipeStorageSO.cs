using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipeStorageSO", menuName = "Crafting Recipe Storage")]
public class CraftingRecipeStorageSO : ScriptableObject
{
	public List<CraftingRecipeSO> craftingRecipes;

	public string recipeLocation;

	private int startOfPath = 0;
	private int endOfUnneededPath = 19;
	private int startOfFileExtension = 51;

	private void OnValidate()
	{
		if (recipeLocation == "")
			return;

		craftingRecipes.Clear();

		string[] availableRecipes = Directory.GetFiles(recipeLocation, "*.asset");

		foreach (string recipe in availableRecipes)
		{
			craftingRecipes.Add((CraftingRecipeSO)Resources.Load(recipe.Remove(startOfPath, endOfUnneededPath).Remove(startOfFileExtension)));
			//Debug.Log(recipe.Remove(startOfPath, endOfUnneededPath).Remove(startOfFileExtension));
		}
	}
}