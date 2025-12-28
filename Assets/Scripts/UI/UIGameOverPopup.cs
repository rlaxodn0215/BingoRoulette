using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoRoulette
{
	public class UIGameOverPopup : MonoBehaviour
	{
		[SerializeField] private TMP_Text _pointText;
		[SerializeField] private Button _gotoMenuButton;
		[SerializeField] private Button _restartButton;
		
		[Header( "GameUI" )]
		[SerializeField] private UITitleWindow _titleWindow;
		[SerializeField] private UIGameWindow _gameWindow;

		private void Start()
		{
			_gotoMenuButton.onClick.AddListener(OnClicGotokMenu);
			_restartButton.onClick.AddListener(OnClickRestart);
		}

		public void SetPoint(ulong point)
		{
			_pointText.text = $"{point}";
		}

		private void OnClicGotokMenu()
		{
			_titleWindow.gameObject.SetActive(true);
			_gameWindow.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
		
		private void OnClickRestart()
		{
			_gameWindow.ResetGame();
			gameObject.SetActive(false);
		}
	}
}
