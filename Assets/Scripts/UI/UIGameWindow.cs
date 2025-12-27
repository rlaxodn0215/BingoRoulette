using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingoRoulette
{
	public class UIGameWindow : MonoBehaviour
	{
		[SerializeField] private GameObject _slotRoot;
		[SerializeField] private TMP_Text _pointText;

		private List<UISlotView> _slots = new();
		private SlotBoardController _slotBoardController;

		private void Start()
		{
			for (var i = _slotRoot.transform.childCount - 1; i >= 0; i--)
			{
				Destroy(_slotRoot.transform.GetChild(i).gameObject);
			}
			
			var dict = ResourceManager.Instance.LoadAll<EPrefab, PrefabData>(GlobalValue.SOResourcePath);

			if (dict.TryGetValue(EPrefab.Slot, out var slotPrefabData))
			{
				for (var i = 0; i < 25; i++)
				{
					var go = ResourceManager.Instance.Instantiate(slotPrefabData.AssetPath, _slotRoot.transform);
					var view = go.GetComponent<UISlotView>();
					view.AddSlotListener(SelectSlot, i);
					view.SetNormalSlotActive(false);
					_slots.Add(view);
				}
			}
			
			_slotBoardController = new SlotBoardController();
		}

		private void SelectSlot(int idx)
		{
			//Debug.Log($"Select Slot {idx}");
			_slots[idx].SetNormalSlotActive(true);
		}
	}
}
