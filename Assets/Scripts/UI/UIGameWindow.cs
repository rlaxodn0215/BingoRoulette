using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoRoulette
{
	public class UIGameWindow : MonoBehaviour
	{
		[SerializeField] private GameObject _slotRoot;
		[SerializeField] private TMP_Text _pointText;
		[SerializeField] private TMP_Text _lifeText;
		[SerializeField] private List<Image> _bingoCountImages;
		[SerializeField] private UIGameOverPopup _gameOverPopup;

		private List<UISlotView> _slots = new();
		private SlotBoardController _slotBoardController;
		private List<int> _emptySlotIndices = new();
		private ulong _point = 0;
		private int _life = 3;
		private int _bingoCount = 0;

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

		public void ResetGame()
		{
			ChangePoint(0);
			ChangeLife(3);
			ChangeBingoCount(0);
			_emptySlotIndices = Enumerable.Range(0, 25).ToList();
			_slots.ForEach(slot => slot.SetNormalSlotActive(false));
		}

		private void SelectSlot(int idx)
		{
			if (idx < 0 || idx >= 25)
			{
				return;
			}

			var usedSlotIndices = Enumerable.Range(0, 25).Except(_emptySlotIndices).ToList();
			if (usedSlotIndices.Contains(idx))
			{
				return;
			}

			//Debug.Log($"Select Slot {idx}");
			var deadSlotIndex = _slotBoardController.GetDeadSlotIndex(_emptySlotIndices, usedSlotIndices);
			//var deadSlotIndex = -1;

			// 고양이 보여주기 (비동기)
			ShowDeadSlot(deadSlotIndex).Forget();

			if (idx == deadSlotIndex)
			{
				ChangeLife(_life - 1);
				SoundManager.Instance.Play(ESound.DeathSlotClick);
			}
			else
			{
				_slots[idx].SetNormalSlotActive(true);
				_emptySlotIndices.Remove(idx);
				usedSlotIndices.Add(idx);

				var bingoIndices = _slotBoardController.GetBingoSlotIndices(usedSlotIndices);

				if (bingoIndices.Count > 0)
				{
					ChangePoint(_point + (ulong)bingoIndices.Count);

					foreach (var bingoIndex in bingoIndices)
					{
						_slots[bingoIndex].SetNormalSlotActive(false);
						_emptySlotIndices.Add(bingoIndex);
					}

					ChangeBingoCount(_bingoCount + (bingoIndices.Count + 4) / 5);
					SoundManager.Instance.Play(ESound.Bingo);
				}
				else
				{
					ChangePoint(_point + 1);
				}
			}
		}

		private async UniTask ShowDeadSlot(int idx)
		{
			if (idx < 0 || idx >= 25) return;
			_slots[idx].SetDeathSlotActive(true);
			await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
			_slots[idx].SetDeathSlotActive(false);
		}

		private void ChangePoint(ulong point)
		{
			_point = point;
			_pointText.text = $"{_point}";
			// 효과 이벤트
		}

		private void ChangeBingoCount(int count)
		{
			_bingoCount = count;

			var quot = _bingoCount / _bingoCountImages.Count;
			var rest = _bingoCount % _bingoCountImages.Count;

			// rest로 투명도 설정
			for (var i = 0; i < _bingoCountImages.Count; i++)
			{
				_bingoCountImages[i].color =
					(i < rest) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.5f);
			}

			_bingoCount = rest;
			// 몫 만큼 Life 증가
			ChangeLife(_life + quot);
		}

		private void ChangeLife(int life)
		{
			_life = life;
			_lifeText.text = $"{_life}";
			// 효과 이벤트

			// 사망
			if (_life <= 0)
			{
				GameOver();
			}
		}

		private void GameOver()
		{
			//Debug.Log("Game Over");
			_gameOverPopup.gameObject.SetActive(true);
			_gameOverPopup.SetPoint(_point);
			SoundManager.Instance.Play(ESound.GameOver);
		}
	}
}
