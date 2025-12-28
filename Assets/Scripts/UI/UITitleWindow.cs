using UnityEngine;
using UnityEngine.UI;

namespace BingoRoulette
{
	public class UITitleWindow : MonoBehaviour
	{
		[Header("Buttons")] 
		[SerializeField] private Button _startButton;
		[SerializeField] private Button _settingButton;
		[SerializeField] private Button _exitButton;

		[Header("Popups")] 
		[SerializeField] private GameObject _settingsPopup;

		[Header("GameUI")] 
		[SerializeField] private UIGameWindow _uiGameWindow;

		private void Start()
		{
			_startButton.onClick.AddListener(OnStartButton);
			_settingButton.onClick.AddListener(OnSettingButton);
			_exitButton.onClick.AddListener(OnExitButton);
		}

		private void OnStartButton()
		{
			_uiGameWindow.ResetGame();
			_uiGameWindow.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}

		private void OnSettingButton()
		{
			_settingsPopup.SetActive(true);
		}

		private void OnExitButton()
		{
			Application.Quit();
		}
	}
}
