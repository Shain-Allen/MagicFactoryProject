using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarMenuControl : MonoBehaviour
{
	[SerializeField]
	private Transform hotbar;

	[SerializeField]
	private Transform itemSelector;

	private Button[] itemSelectorChoices;

	[SerializeField]
	private GameObject buttonSlot;

	[SerializeField]
	private ObjectDictionary placeables;

	[SerializeField]
	private TileBrush brush;

	private void Start()
	{
		for (int i = 0; i < hotbar.childCount; i++)
		{
			ResetInvSlot(hotbar.GetChild(i).GetComponent<Button>());
		}

		itemSelectorChoices = new Button[placeables.itemList.Length];

		for (int i = 0; i < placeables.itemList.Length; i++)
		{
			GameObject newButton = Instantiate(buttonSlot, itemSelector.transform.position, Quaternion.identity, itemSelector.transform);

			newButton.GetComponent<Image>().sprite = placeables.itemList[i].GetComponent<SpriteRenderer>().sprite;
			newButton.GetComponent<Image>().color = placeables.itemList[i].GetComponent<SpriteRenderer>().color;
			itemSelectorChoices[i] = newButton.GetComponent<Button>();
		}
	}

	private void ResetInvSlot(Button invslot_)
	{
		invslot_.onClick.RemoveAllListeners();

		invslot_.gameObject.AddComponent<CustomButton>().SetButtonReference(itemSelector, this, invslot_);
		//Debug.Log($"Setting button references {invslot_.gameObject.GetComponent<CustomButton>()}");
		//invslot_.onClick.AddListener(() => itemSelector.gameObject.SetActive(true));
		//invslot_.onClick.AddListener(() => UpdateItemSelector(invslot_));
	}

	public void UpdateItemSelector(Button invslot_)
	{
		for (int i = 0; i < placeables.itemList.Length; i++)
		{
			Button tempButton = itemSelectorChoices[i];
			tempButton.onClick.RemoveAllListeners();
			int index = i;
			tempButton.onClick.AddListener(() => UpdateItemSlot(invslot_, index));
			tempButton.onClick.AddListener(() => itemSelector.gameObject.SetActive(false));
			Debug.Log(i);
		}
	}

	private void UpdateItemSlot(Button invslot_, int itemIndex_)
	{
		invslot_.onClick.RemoveAllListeners();
		Debug.Log(itemIndex_);
		invslot_.onClick.AddListener(() => brush.ChangeBrushItem(itemIndex_));
		invslot_.GetComponent<Image>().sprite = placeables.itemList[itemIndex_].GetComponent<SpriteRenderer>().sprite;
		invslot_.GetComponent<Image>().color = placeables.itemList[itemIndex_].GetComponent<SpriteRenderer>().color;
	}
}
