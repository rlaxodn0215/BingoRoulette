using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BingoRoulette
{
	public class UITitleWindow : MonoBehaviour
	{
		[Header("Buttons")] [SerializeField] private Button _startButton;
		[SerializeField] private Button _settingButton;
		[SerializeField] private Button _exitButton;

		[Header("Popups")] [SerializeField] private GameObject _settingsPopup;

		[Header("GameUI")] [SerializeField] private UIGameWindow _uiGameWindow;

		private bool _isQuitting = false;

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
			if (_isQuitting) return;
			_isQuitting = true;

			StartCoroutine(QuitFastSafe());
		}

		private IEnumerator QuitFastSafe()
		{
			yield return null;

			// 3. Steam 먼저 정리
			SteamManager.Instance?.PrepareToQuit();

			// 4. 종료
			Application.Quit();
		}
	}
}
