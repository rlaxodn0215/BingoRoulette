using System.Collections.Generic;
using UnityEngine;

namespace BingoRoulette
{
	public class ProbabilityUtility
	{
		private static readonly int ProbabilityMaxCount = GlobalValue.ProbabilityMaxCount;

		/// <summary>
		/// 연관된 확률들 중 특정 확률이 일어날 확률
		/// 리스트의 모든 확률의 값은 ProbabilityMaxCount와 같아야 한다.
		/// </summary>
		/// <param name="probabilityList"> 확률들 리스트 </param>
		/// <returns></returns>
		public static int GetRandomProbability(List<int> probabilityList)
		{
			var sum = 0;
			foreach (var item in probabilityList)
			{
				sum += item;
			}

			if (sum != ProbabilityMaxCount)
			{
				Debug.LogError("Sum of probability is not 1000");
				return -1;
			}

			sum = 0;
			var randomNum = Random.Range(1, ProbabilityMaxCount + 1);
			for (var i = 0; i < probabilityList.Count; i++)
			{
				sum += probabilityList[i];
				if (randomNum <= sum)
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
