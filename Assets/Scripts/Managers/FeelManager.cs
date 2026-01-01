using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace BingoRoulette
{
	public class FeelManager : SingletonTemplate<FeelManager>
	{
		private Dictionary<EFeel, MMF_Player> _dicFeelPlayer = new();

		protected override void Awake()
		{
			base.Awake();
			InitFeelPlayers();
		}

		public void Play(EFeel feel)
		{
			if (_dicFeelPlayer.TryGetValue(feel, out var player))
			{
				player.PlayFeedbacks();
			}
		}

		public void Play(EFeel feel, List<int> indexList)
		{
			if (!_dicFeelPlayer.TryGetValue(feel, out var player))
			{
				return;
			}

			var feedbackList = player.FeedbacksList;
			foreach (var index in indexList)
			{
				feedbackList[index].Play(transform.position, 1f);
			}
		}

		public void Stop(EFeel feel)
		{
			if (_dicFeelPlayer.TryGetValue(feel, out var player))
			{
				player.StopFeedbacks();
			}
		}

		public MMF_Player GetPlayer(EFeel feel)
		{
			return _dicFeelPlayer[feel];
		}

		public void AddFeedback<T>(EFeel feel, T feedback, int index) where T : MMF_Feedback
		{
			feedback.Initialization(_dicFeelPlayer[feel], index);
			_dicFeelPlayer[feel].AddFeedback(feedback);
		}

		private void InitFeelPlayers()
		{
			_dicFeelPlayer.Clear();
			Traverse(transform);
		}

		private void Traverse(Transform current)
		{
			// 자기 자신은 제외하고 자식부터 검사하고 싶다면 여기서 return 조건 추가 가능
			if (current != transform)
			{
				if (System.Enum.TryParse(current.name, out EFeel feel))
				{
					var player = current.GetComponentInChildren<MMF_Player>(true);
					if (player != null)
					{
						if (!_dicFeelPlayer.TryAdd(feel, player))
						{
							Debug.LogWarning($"[FeelManager] Duplicate EFeel detected: {feel} ({current.name})");
						}
					}
					else
					{
						Debug.LogWarning($"[FeelManager] MMF_Player not found in {current.name}");
					}
				}
			}

			// 재귀 순회
			foreach (Transform child in current)
			{
				Traverse(child);
			}
		}
	}
}
