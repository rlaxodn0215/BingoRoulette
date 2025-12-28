using System.Collections.Generic;
using UnityEngine;

namespace BingoRoulette
{
	public class ProbabilityUtility
	{
		private static readonly int ProbabilityMaxCount = GlobalValue.ProbabilityMaxCount;

		/// <summary>
		/// 연관된 확률들 중 특정 확률이 일어날 확률
		/// - (수정) probabilityList에 0이 포함되어도 동작
		/// - (수정) 합이 ProbabilityMaxCount(=1000)과 다르더라도, 리스트 합(sum)을 기준으로 랜덤 선택
		/// </summary>
		/// <param name="probabilityList"> 확률들 리스트 </param>
		/// <returns>선택된 probabilityList의 인덱스</returns>
		public static int GetRandomProbability(List<int> probabilityList)
		{
			var sum = 0;
			foreach (var item in probabilityList)
			{
				sum += item;
			}

			if (sum <= 0)
			{
				Debug.LogError("Sum of probability is 0 or less");
				return -1;
			}

			// 경고만 출력하고 sum 기준으로 정상 동작하게 함(호환성 + 안정성)
			if (sum != ProbabilityMaxCount)
			{
				Debug.LogWarning($"Sum of probability is not {ProbabilityMaxCount}. sum: {sum}");
			}

			var acc = 0;
			var randomNum = Random.Range(1, sum + 1); // [수정] sum 기준
			for (var i = 0; i < probabilityList.Count; i++)
			{
				acc += probabilityList[i];
				if (randomNum <= acc)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		///  특정 확율로 발생했는지 확인
		/// </summary>
		/// <param name="probability"> 발생할 확률 </param>
		/// <returns> 확률 발생 유무 </returns>
		public static bool IsChanceSuccess(int probability)
		{
			return Random.Range(0, ProbabilityMaxCount) < probability;
		}
	}
}
