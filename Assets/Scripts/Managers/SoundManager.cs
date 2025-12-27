using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BingoRoulette
{
	public class SoundManager : SingletonTemplate<SoundManager>
	{
		[Header("Mixer")] 
		[SerializeField] private AudioMixer _mixer;

		[Header("Audio Sources")] 
		[SerializeField] private AudioSource _bgmPlayer;
		[SerializeField] private AudioSource _uiPlayer;

		[Header("SFX Pool")] 
		[SerializeField] private int _sfxPoolSize = 10;

		private readonly Dictionary<ESound, SoundData> _soundData = new();
		private readonly Dictionary<ESound, AudioClip> _clipCache = new();
		private readonly Dictionary<ESound, float> _lastPlayTime = new();
		private readonly List<AudioSource> _sfxPool = new();

		protected override void Awake()
		{
			base.Awake();
			LoadSoundData();
			InitSFXPool();
			LoadVolume();
		}

		public void Play(ESound key)
		{
			if (!_soundData.TryGetValue(key, out var data))
			{
				Debug.LogError($"[SoundManager] SoundData not found : {key}");
				return;
			}

			switch (data.SoundType)
			{
				case ESoundType.BGM: PlayBGM(key); break;
				case ESoundType.UI: PlayUI(key); break;
				case ESoundType.SFX: PlaySFX(key); break;
			}
		}

		public void StopBGM()
		{
			_bgmPlayer.Stop();
			_bgmPlayer.clip = null;
		}

		public void SetVolume(ESoundType type, float volume)
		{
			volume = Mathf.Clamp(volume, 0.0001f, 1f);
			PlayerPrefs.SetFloat(type.ToString(), volume);

			var db = Mathf.Log10(volume) * 20f;
			_mixer.SetFloat(type.ToString(), db);
		}

		private void PlayBGM(ESound key)
		{
			var clip = GetClip(key);
			if (clip == null)
			{
				return;
			}

			if (_bgmPlayer.clip == clip && _bgmPlayer.isPlaying)
			{
				return;
			}

			var data = _soundData[key];

			_bgmPlayer.Stop();
			_bgmPlayer.clip = clip;
			_bgmPlayer.loop = data.Loop;
			_bgmPlayer.pitch = data.Pitch;
			_bgmPlayer.volume = data.Volume;
			_bgmPlayer.Play();
		}

		private void PlaySFX(ESound key)
		{
			if (!CanPlay(key))
			{
				return;
			}

			var clip = GetClip(key);
			if (clip == null)
			{
				return;
			}

			var data = _soundData[key];
			var src = GetAvailableSFXSource();

			src.pitch = data.Pitch;
			src.PlayOneShot(clip, data.Volume);

			_lastPlayTime[key] = Time.time;
		}

		private void PlayUI(ESound key)
		{
			if (!CanPlay(key))
			{
				return;
			}

			var clip = GetClip(key);
			if (clip == null)
			{
				return;
			}

			var data = _soundData[key];
			_uiPlayer.pitch = data.Pitch;
			_uiPlayer.PlayOneShot(clip, data.Volume);

			_lastPlayTime[key] = Time.time;
		}

		private void LoadSoundData()
		{
			var loaded = ResourceManager.Instance.LoadAll<ESound, SoundData>(GlobalValue.SOResourcePath);

			foreach (var pair in loaded)
			{
				_soundData[pair.Key] = pair.Value;
			}
		}

		private AudioClip GetClip(ESound key)
		{
			if (_clipCache.TryGetValue(key, out var cached))
			{
				return cached;
			}

			var data = _soundData[key];
			var clip = Resources.Load<AudioClip>(data.AssetPath);

			if (clip == null)
			{
				Debug.LogError($"[SoundManager] AudioClip not found : {data.AssetPath}");
				return null;
			}

			_clipCache[key] = clip;
			return clip;
		}

		private void InitSFXPool()
		{
			var sfxGroups = _mixer.FindMatchingGroups("SFX");
			var sfxGroup = sfxGroups.Length > 0 ? sfxGroups[0] : null;

			if (sfxGroup == null)
			{
				Debug.LogError("[SoundManager] SFX AudioMixerGroup not found");
				return;
			}

			for (var i = 0; i < _sfxPoolSize; i++)
			{
				var src = gameObject.AddComponent<AudioSource>();
				src.playOnAwake = false;
				src.loop = false;
				src.spatialBlend = 0f;
				src.outputAudioMixerGroup = sfxGroup;
				_sfxPool.Add(src);
			}
		}

		private void LoadVolume()
		{
			foreach (ESoundType type in Enum.GetValues(typeof(ESoundType)))
			{
				var volume = PlayerPrefs.GetFloat(type.ToString(), 1f);
				SetVolume(type, volume);
			}
		}

		private bool CanPlay(ESound key)
		{
			var data = _soundData[key];

			if (data.CoolTime <= 0f)
			{
				return true;
			}

			if (!_lastPlayTime.TryGetValue(key, out var last))
			{
				return true;
			}

			return Time.time - last >= data.CoolTime;
		}

		private AudioSource GetAvailableSFXSource()
		{
			foreach (var src in _sfxPool)
			{
				if (!src.isPlaying)
				{
					return src;
				}
			}

			return _sfxPool[0];
		}
	}
}
