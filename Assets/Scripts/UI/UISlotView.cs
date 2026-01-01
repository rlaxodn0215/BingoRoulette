using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace BingoRoulette
{
	public class UISlotView : MonoBehaviour
	{
		[SerializeField] private Image _innerBackgroundImage;
		[SerializeField] private GameObject _slotIcon;
		[SerializeField] private Button _normalSlotButton;
		[SerializeField] private GameObject _deathSlot;

		public TMP_Text ProbalbilityScoreText;

		public ESlotState SlotState = ESlotState.None;

		public void AddSlotListener(Action<int> listener, int slotIndex)
		{
			_normalSlotButton.onClick.AddListener(() => listener(slotIndex));
		}

		public void SetNormalSlotActive(bool isActive)
		{
			_slotIcon.SetActive(isActive);

			if (isActive)
			{
				if (SlotState == ESlotState.Active)
				{
					return;
				}

				var rnd = new Random();
				var idx = rnd.Next((int)ESlotColor.Max);
				SetSlotColor((ESlotColor)idx);
				SlotState = ESlotState.Active;
			}
			else
			{
				SetSlotColor(ESlotColor.Gray);
				SlotState = ESlotState.None;
			}
		}

		public void SetDeathSlotActive(bool isActive, bool isClicked)
		{
			_deathSlot.SetActive(isActive);
			_normalSlotButton.gameObject.SetActive(!isActive);

			var animator = _deathSlot.GetComponent<Animator>();
			if (isActive && isClicked)
			{
				animator.SetTrigger("Highlighted");
			}
			else
			{
				animator.SetTrigger("Normal");
			}
		}

		private void SetSlotColor(ESlotColor color)
		{
			switch (color)
			{
				case ESlotColor.Gray: SetColorHex("#DFE1E6"); break;
				case ESlotColor.Berry: SetColorHex("#FFDDF9"); break;
				case ESlotColor.Blue: SetColorHex("#D0EBFF"); break;
				case ESlotColor.Cyan: SetColorHex("#C5F6FA"); break;
				case ESlotColor.Green: SetColorHex("#D2F9D8"); break;
				case ESlotColor.Indigo: SetColorHex("#DCE5FF"); break;
				case ESlotColor.Lime: SetColorHex("#E9FAC9"); break;
				case ESlotColor.Orange: SetColorHex("#FFE8CC"); break;
				case ESlotColor.Pink: SetColorHex("#FFDEEB"); break;
				case ESlotColor.Purple: SetColorHex("#F4DAFA"); break;
				case ESlotColor.Red: SetColorHex("#FFE3E3"); break;
				case ESlotColor.Teal: SetColorHex("#C2FAE8"); break;
				case ESlotColor.Violet: SetColorHex("#E4DAFF"); break;
				case ESlotColor.Yellow: SetColorHex("#FFF2C0"); break;
			}

			return;

			void SetColorHex(string hex)
			{
				if (ColorUtility.TryParseHtmlString(hex, out Color color))
				{
					_innerBackgroundImage.color = color;
				}
				else
				{
					Debug.LogError($"Invalid HEX color: {hex}");
				}
			}
		}
	}
}
