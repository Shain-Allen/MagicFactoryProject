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

			Debug.Log(newButton.name);

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
		for (int i = 0; i < itemSelectorChoices.Length; i++)
		{
			itemSelectorChoices[i].onClick.RemoveAllListeners();
			itemSelectorChoices[i].onClick.AddListener(() => UpdateItemSlot(invslot_, i));
			itemSelectorChoices[i].onClick.AddListener(() => itemSelector.gameObject.SetActive(false));
		}
	}

	private void UpdateItemSlot(Button invslot_, int ItemIndex)
	{
		ResetInvSlot(invslot_);
		invslot_.onClick.AddListener(() => brush.ChangeBrushItem(ItemIndex));
		invslot_.GetComponent<Image>().sprite = placeables.itemList[ItemIndex].GetComponent<SpriteRenderer>().sprite;
		invslot_.GetComponent<Image>().color = placeables.itemList[ItemIndex].GetComponent<SpriteRenderer>().color;
	}
}
