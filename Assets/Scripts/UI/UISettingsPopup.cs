using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BingoRoulette
{
	public class UISettingsPopup : MonoBehaviour
	{
		[Header("Display Settings")] [SerializeField]
		private TMP_Dropdown _displayResolutionDropdown;

		[SerializeField] private TMP_Dropdown _windowTypeDropdown;

		[Header("Audio Settings")] [Header("Master Volume")] [SerializeField]
		private Slider _masterVolumeSlider;

		[SerializeField] private TMP_Text _maseterVolumeText;

		[Header("BGM Volume")] [SerializeField]
		private Slider _bgmVolumeSlider;

		[SerializeField] private TMP_Text _bgmVolumeText;

		[Header("SFX Volume")] [SerializeField]
		private Slider _sfxVolumeSlider;

		[SerializeField] private TMP_Text _sfxVolumeText;

		[Header("UI Volume")] [SerializeField] private Slider _uiVolumeSlider;
		[SerializeField] private TMP_Text _uiVolumeText;

		private Resolution[] _displayResolutions;

		private void Start()
		{
			InitResolutions();
			InitWindowType();
			InitVolumeSliders();
		}

		private void InitResolutions()
		{
			_displayResolutions = Screen.resolutions;

			_displayResolutionDropdown.ClearOptions();

			var options = new List<string>();
			var currentIndex = 0;

			foreach (var t in _displayResolutions)
			{
				var option = $"{t.width} x {t.height}";

				// 같은 해상도 중복 제거
				if (!options.Contains(option))
					options.Add(option);

				// 현재 해상도 체크
				if (t.width == Screen.currentResolution.width &&
				    t.height == Screen.currentResolution.height)
				{
					currentIndex = options.Count - 1;
				}
			}

			_displayResolutionDropdown.AddOptions(options);
			_displayResolutionDropdown.value = currentIndex;
			_displayResolutionDropdown.RefreshShownValue();

			// 드롭다운 변경 이벤트 연결
			_displayResolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
		}

		private void InitWindowType()
		{
			_windowTypeDropdown.ClearOptions();

			var modes = new List<string>()
			{
				"Fullscreen",
				/*"Fullscreen Window",*/
				"Windowed"
			};

			_windowTypeDropdown.AddOptions(modes);

			// 현재 모드에 맞게 기본값 설정
			var index = Screen.fullScreenMode switch
			{
				FullScreenMode.ExclusiveFullScreen => 0,
				FullScreenMode.FullScreenWindow => 1,
				FullScreenMode.Windowed => 2,
				_ => 0
			};

			_windowTypeDropdown.value = index;
			_windowTypeDropdown.RefreshShownValue();

			// 드롭다운 변경 이벤트 등록
			_windowTypeDropdown.onValueChanged.AddListener(OnWindowModeChanged);
		}

		private void OnResolutionChanged(int index)
		{
			var res = _displayResolutionDropdown.options[index].text.Split('x');

			var width = int.Parse(res[0].Trim());
			var height = int.Parse(res[1].Trim());

			// 최신 방식: refreshRateRatio 사용
			var refreshRateRatio = Screen.currentResolution.refreshRateRatio;

			Screen.SetResolution(
				width,
				height,
				Screen.fullScreenMode,
				refreshRateRatio
			);
		}

		private void OnWindowModeChanged(int index)
		{
			Screen.fullScreenMode = index switch
			{
				0 => // Fullscreen
					FullScreenMode.ExclusiveFullScreen,
				1 => // Fullscreen Window
					FullScreenMode.FullScreenWindow,
				2 => // Windowed
					FullScreenMode.Windowed,
				_ => Screen.fullScreenMode
			};
		}

		private void InitVolumeSliders()
		{
			_masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
			_bgmVolumeSlider.onValueChanged.AddListener(ChangeBGMVolume);
			_sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
			_uiVolumeSlider.onValueChanged.AddListener(ChangeUIVolume);

			foreach (var pair in SoundManager.Instance.SoundVolume)
			{
				SetVolume(pair.Key, pair.Value);

				switch (pair.Key)
				{
					case ESoundType.Master:
						_masterVolumeSlider.value = pair.Value;
						break;
					case ESoundType.BGM:
						_bgmVolumeSlider.value = pair.Value;
						break;
					case ESoundType.SFX:
						_sfxVolumeSlider.value = pair.Value;
						break;
					case ESoundType.UI:
						_uiVolumeSlider.value = pair.Value;
						break;
				}
			}
		}

		private void ChangeMasterVolume(float volume)
		{
			SetVolume(ESoundType.Master, volume);
		}

		private void ChangeBGMVolume(float volume)
		{
			SetVolume(ESoundType.BGM, volume);
		}

		private void ChangeSFXVolume(float volume)
		{
			SetVolume(ESoundType.SFX, volume);
		}

		private void ChangeUIVolume(float volume)
		{
			SetVolume(ESoundType.UI, volume);
		}

		private void SetVolume(ESoundType type, float volume)
		{
			SoundManager.Instance.SetVolume(type, volume);

			switch (type)
			{
				case ESoundType.Master:
					_maseterVolumeText.text = $"{(int)(volume * 100f)} / 100";
					break;
				case ESoundType.BGM:
					_bgmVolumeText.text = $"{(int)(volume * 100f)} / 100";
					break;
				case ESoundType.SFX:
					_sfxVolumeText.text = $"{(int)(volume * 100f)} / 100";
					break;
				case ESoundType.UI:
					_uiVolumeText.text = $"{(int)(volume * 100f)} / 100";
					break;
			}
		}
	}
}
