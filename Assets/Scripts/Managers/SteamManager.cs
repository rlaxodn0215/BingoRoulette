using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

namespace BingoRoulette
{
	public class SteamManager : SingletonTemplate<SteamManager>
	{
		public static bool IsInitialized { get; private set; }

		// ===============================
		// Leaderboard 정의
		// ===============================
		private class LeaderboardInfo
		{
			public string ApiName;
			public SteamLeaderboard_t Handle;
			public bool Ready;

			public LeaderboardInfo(string apiName)
			{
				ApiName = apiName;
				Handle = new SteamLeaderboard_t();
				Ready = false;
			}
		}

		private Dictionary<ELeaderboard, LeaderboardInfo> _leaderboards;
		private Dictionary<ELeaderboard, CallResult<LeaderboardFindResult_t>> _findResults;
		private Dictionary<ELeaderboard, CallResult<LeaderboardScoreUploaded_t>> _uploadResults;

		// ===============================
		// Unity LifeCycle
		// ===============================
		protected override void Awake()
		{
			base.Awake();

			if (!SteamAPI.Init())
			{
				Debug.LogError("[Steam] SteamAPI.Init 실패");
				return;
			}

			IsInitialized = true;
			Debug.Log("[Steam] SteamAPI.Init 성공");

			InitLeaderboards();
		}

		private void Update()
		{
			if (IsInitialized)
			{
				SteamAPI.RunCallbacks();
			}
		}

		private void OnDestroy()
		{
			if (IsInitialized)
			{
				SteamAPI.Shutdown();
				IsInitialized = false;
			}
		}

		public void PrepareToQuit()
		{
			if (!IsInitialized) return;

			Debug.Log("[Steam] PrepareToQuit");

			// 1. 남아있는 Stats 즉시 저장
			SteamUserStats.StoreStats();

			// 2. 콜백 더 이상 안 돌게
			IsInitialized = false;

			// 3. Steam 종료
			SteamAPI.Shutdown();
		}

		// ===============================
		// Leaderboard Init
		// ===============================
		private void InitLeaderboards()
		{
			_leaderboards = new Dictionary<ELeaderboard, LeaderboardInfo>
			{
				{ ELeaderboard.LB_SCORE_BEST, new LeaderboardInfo(nameof(ELeaderboard.LB_SCORE_BEST)) },
				{ ELeaderboard.LB_TRY_COUNT_BEST, new LeaderboardInfo(nameof(ELeaderboard.LB_TRY_COUNT_BEST)) },
				{ ELeaderboard.LB_BINGO_COUNT_BEST, new LeaderboardInfo(nameof(ELeaderboard.LB_BINGO_COUNT_BEST)) }
			};

			_findResults = new Dictionary<ELeaderboard, CallResult<LeaderboardFindResult_t>>();
			_uploadResults = new Dictionary<ELeaderboard, CallResult<LeaderboardScoreUploaded_t>>();

			//foreach (var pair in _leaderboards)
			{
				// var type = pair.Key;
				// var info = pair.Value;

				var type = ELeaderboard.LB_SCORE_BEST;
				var info = _leaderboards[type];

				var callResult =
					CallResult<LeaderboardFindResult_t>.Create((result, ioFail) =>
						OnFindLeaderboard(type, result, ioFail));

				_findResults[type] = callResult;

				var handle = SteamUserStats.FindLeaderboard(info.ApiName);
				callResult.Set(handle);
			}

			Debug.Log("[Steam] 모든 리더보드 찾기 요청");
		}

		private void OnFindLeaderboard(
			ELeaderboard type,
			LeaderboardFindResult_t result,
			bool ioFailure)
		{
			if (ioFailure || result.m_bLeaderboardFound == 0)
			{
				Debug.LogError($"[Steam] 리더보드 찾기 실패: {type}");
				return;
			}

			_leaderboards[type].Handle = result.m_hSteamLeaderboard;
			_leaderboards[type].Ready = true;

			Debug.Log($"[Steam] 리더보드 준비 완료: {type}");
		}

		// ===============================
		// Stat / Achievement
		// ===============================
		public void CheckSteamPointAchievement(ulong point)
		{
			foreach (var pair in GlobalValue.PointAchievements)
			{
				if (point >= (ulong)pair.Key)
				{
					UnlockAchievement(pair.Value);
				}
			}
		}

		public void UpdateStat(EStat statKey, int newValue, bool isAdd = false)
		{
			SteamUserStats.GetStat(statKey.ToString(), out int value);

			if (isAdd)
			{
				newValue += value;
			}
			else
			{
				if (newValue <= value) return;
			}

			SteamUserStats.SetStat(statKey.ToString(), newValue);
			SteamUserStats.StoreStats();

			switch (statKey)
			{
				case EStat.STAT_TRY_COUNT:
					UploadToLeaderboard(ELeaderboard.LB_TRY_COUNT_BEST, newValue);
					break;
				case EStat.STAT_BINGO_COUNT:
					UploadToLeaderboard(ELeaderboard.LB_BINGO_COUNT_BEST, newValue);
					break;
				case EStat.STAT_BEST_POINT:
					UploadToLeaderboard(ELeaderboard.LB_SCORE_BEST, newValue);
					break;
			}
		}

		private void UnlockAchievement(EAchievement achievement)
		{
			if (!IsInitialized)
				return;

			var apiName = achievement.ToString();
			SteamUserStats.GetAchievement(apiName, out var achieved);
			if (achieved) return;

			SteamUserStats.SetAchievement(apiName);
			SteamUserStats.StoreStats();

			Debug.Log($"[Steam] Achievement achieved: {apiName}");
		}

		// ===============================
		// Leaderboard Upload
		// ===============================
		private void UploadToLeaderboard(ELeaderboard type, int value)
		{
			var info = _leaderboards[type];
			if (!info.Ready)
				return;

			var uploadResult =
				CallResult<LeaderboardScoreUploaded_t>.Create((result, ioFail) =>
				{
					if (ioFail)
					{
						Debug.LogError($"[Steam] 리더보드 업로드 실패: {type}");
						return;
					}

					Debug.Log($"[Steam] 리더보드 업로드 완료: {type} / {value}");
				});

			_uploadResults[type] = uploadResult;

			var handle =
				SteamUserStats.UploadLeaderboardScore(
					info.Handle,
					ELeaderboardUploadScoreMethod
						.k_ELeaderboardUploadScoreMethodKeepBest,
					value,
					null,
					0
				);

			uploadResult.Set(handle);
		}
	}
}
