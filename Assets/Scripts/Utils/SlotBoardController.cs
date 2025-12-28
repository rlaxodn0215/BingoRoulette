using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BingoRoulette
{
	public class SlotBoardController
	{
		// 데스 슬롯 결정
		public int GetDeadSlotIndex(List<int> emptySlotIndices, List<int> usedSlotIndices)
		{
			if (emptySlotIndices == null || emptySlotIndices.Count == 0)
			{
				return -1;
			}

			var tuning = GetDefaultDeadSlotTuning();

			var usedSet = new HashSet<int>(usedSlotIndices);
			var lines = BuildBingoLines();

			// 슬롯별 점수 계산
			var scores = new List<float>();
			foreach (var slot in emptySlotIndices)
			{
				var score = CalculateSlotScore(slot, usedSet, lines, tuning);
				scores.Add(score);
			}

			// 3️⃣ 점수 → 확률(GlobalValue.ProbabilityMaxCount 기준)
			var probabilityList = ConvertScoreToProbability(scores);

			// 4️⃣ 확률 선택 (반환값은 probabilityList 인덱스)
			var picked = ProbabilityUtility.GetRandomProbability(probabilityList);
			if (picked < 0 || picked >= emptySlotIndices.Count)
			{
				return -1;
			}

			// 실제 데스 슬롯 인덱스로 매핑
			return emptySlotIndices[picked];
		}

		// 빙고 판별 함수
		public List<int> GetBingoSlotIndices(List<int> usedSlotIndices)
		{
			var result = new List<int>();
			var usedSet = new HashSet<int>(usedSlotIndices);

			for (var row = 0; row < 5; row++)
			{
				var line = new List<int>();
				for (var col = 0; col < 5; col++)
				{
					var idx = row * 5 + col;
					if (!usedSet.Contains(idx))
					{
						line.Clear();
						break;
					}

					line.Add(idx);
				}

				if (line.Count == 5)
					result.AddRange(line);
			}

			for (var col = 0; col < 5; col++)
			{
				var line = new List<int>();
				for (var row = 0; row < 5; row++)
				{
					var idx = row * 5 + col;
					if (!usedSet.Contains(idx))
					{
						line.Clear();
						break;
					}

					line.Add(idx);
				}

				if (line.Count == 5)
					result.AddRange(line);
			}

			{
				var line = new List<int>();
				for (var i = 0; i < 5; i++)
				{
					var idx = i * 6;
					if (!usedSet.Contains(idx))
					{
						line.Clear();
						break;
					}

					line.Add(idx);
				}

				if (line.Count == 5)
					result.AddRange(line);
			}

			{
				var line = new List<int>();
				for (var i = 0; i < 5; i++)
				{
					var idx = (i + 1) * 4;
					if (!usedSet.Contains(idx))
					{
						line.Clear();
						break;
					}

					line.Add(idx);
				}

				if (line.Count == 5)
					result.AddRange(line);
			}

			return result.Distinct().ToList();
		}

		// 점수 계산
		/* 점수 공식
		 * score = BaseScore + Σ (각 라인 기여도) + CenterSlotBonus (조건부)

		각 라인 기여도 = LineBaseWeight +
			(filled ^ FilledPower) * FilledSlotWeight * FilledMultiplier * (대각선이면 DiagonalMultiplier) +
			(filled == 4 ? NearBingoBonus : 0)
		*/
		private float CalculateSlotScore(int slot, HashSet<int> usedSet, List<int[]> lines, DeadSlotTuning t)
		{
			// 이미 선택된 슬롯은 데스 슬롯으로 나오면 안 되므로 점수(=확률) 0 처리
			if (usedSet.Contains(slot))
			{
				return 0f;
			}

			var score = t.BaseScore;

			foreach (var line in lines)
			{
				if (!line.Contains(slot))
				{
					continue;
				}

				var filled = line.Count(usedSet.Contains);
				var lineScore = t.LineBaseWeight;
				var filledTerm = Mathf.Pow(filled, t.FilledPower) * t.FilledSlotWeight * t.FilledMultiplier;

				if (IsDiagonal(line))
				{
					filledTerm *= t.DiagonalMultiplier;
				}

				if (filled == 4)
				{
					filledTerm += t.NearBingoBonus;
				}

				lineScore += filledTerm;
				score += lineScore;
			}

			// 중앙 슬롯 보정
			if (slot == 12)
			{
				score += t.CenterSlotBonus;
			}

			// [수정] used 슬롯은 이미 0으로 반환했으므로, 그 외 슬롯만 최소값 보정 적용
			return Mathf.Max(t.MinScoreClamp, score);
		}

		// 확률 변환
		private List<int> ConvertScoreToProbability(List<float> scores)
		{
			var max = GlobalValue.ProbabilityMaxCount;

			// [수정] 점수 0(=확률 0) 슬롯은 합산에서 제외해 정확히 0 유지
			var sum = 0f;
			foreach (var s in scores)
			{
				if (s > 0f)
				{
					sum += s;
				}
			}

			var probs = new List<int>();
			var acc = 0;

			// [수정] score가 0이면 probability도 0으로 유지 (기존 Mathf.Max(1, ...) 제거)
			foreach (var s in scores)
			{
				var p = (s <= 0f || sum <= 0f) ? 0 : Mathf.RoundToInt(s / sum * max);
				probs.Add(p);
				acc += p;
			}

			// [수정] 합 보정: 0이 아닌(후보) 인덱스에만 diff를 더해 0 확률을 깨지 않음
			var diff = max - acc;
			if (diff != 0)
			{
				var targetIndex = -1;

				// 확률이 이미 있는 슬롯에 보정치를 더함
				for (var i = 0; i < probs.Count; i++)
				{
					if (probs[i] > 0)
					{
						targetIndex = i;
						break;
					}
				}

				// 전부 0이면(이상 케이스) 첫 요소에 몰아줌
				if (targetIndex < 0)
				{
					targetIndex = 0;
				}

				probs[targetIndex] += diff;
			}

			return probs;
		}

		// 빙고 라인 정의
		private List<int[]> BuildBingoLines()
		{
			var lines = new List<int[]>();

			for (var r = 0; r < 5; r++)
			{
				lines.Add(new[] { r * 5, r * 5 + 1, r * 5 + 2, r * 5 + 3, r * 5 + 4 });
			}

			for (var c = 0; c < 5; c++)
			{
				lines.Add(new[] { c, c + 5, c + 10, c + 15, c + 20 });
			}

			lines.Add(new[] { 0, 6, 12, 18, 24 });
			lines.Add(new[] { 4, 8, 12, 16, 20 });

			return lines;
		}

		private bool IsDiagonal(int[] line)
		{
			return (line[0] == 0 && line[1] == 6) ||
			       (line[0] == 4 && line[1] == 8);
		}

		// 기본 튜닝 값
		private DeadSlotTuning GetDefaultDeadSlotTuning()
		{
			return new DeadSlotTuning
			{
				BaseScore = 1f,
				MinScoreClamp = 0.3f,

				LineBaseWeight = 4f,
				FilledSlotWeight = 7f,
				FilledMultiplier = 1.4f,
				FilledPower = 2.5f,

				DiagonalMultiplier = 1.4f,
				CenterSlotBonus = 30f,

				NearBingoBonus = 220f,
			};
		}

		/// <summary>
		/// 데스 슬롯 확률 계산을 위한 튜닝 파라미터 묶음
		/// 모든 값은 "슬롯 점수(score)" 계산에 사용되며
		/// 최종적으로 확률(ProbabilityMaxCount 기준)로 변환된다.
		/// </summary>
		[System.Serializable]
		public struct DeadSlotTuning
		{
			/* =========================
			 *  기본 점수 관련
			 * ========================= */

			/// <summary>
			/// 모든 후보 슬롯이 기본적으로 가지는 초기 점수
			/// - 아무 빙고 라인에도 속하지 않아도 최소한의 확률을 갖게 함
			/// - 너무 낮으면 특정 슬롯이 거의 안 뽑힘
			/// </summary>
			public float BaseScore;

			/// <summary>
			/// 계산 결과 score가 이 값보다 낮아지지 않도록 보정하는 최소값
			/// - 확률 0이 되는 슬롯을 방지
			/// - 값이 클수록 "완전 랜덤성"이 증가
			/// </summary>
			public float MinScoreClamp;


			/* =========================
			 *  빙고 라인 기본 가중치
			 * ========================= */

			/// <summary>
			/// 슬롯이 하나의 빙고 라인에 포함될 때 추가되는 기본 점수
			/// - "이 슬롯은 라인에 속해 있다" 자체의 가치
			/// - 0이면 filled(선택된 개수)만으로 판단
			/// </summary>
			public float LineBaseWeight;

			/// <summary>
			/// 해당 빙고 라인에 이미 선택된 슬롯 수(filled)에 곱해지는 가중치
			/// - filled가 많을수록 점수 증가량이 커짐
			/// </summary>
			public float FilledSlotWeight;

			/// <summary>
			/// filled에 대한 전체 배율
			/// - 난이도 조절에 매우 유용
			/// - 턴 후반으로 갈수록 키우면 AI가 더 공격적으로 변함
			/// </summary>
			public float FilledMultiplier;

			/// <summary>
			/// filled 값을 몇 제곱으로 반영할지 결정
			/// - 1 : 선형 증가 (완만)
			/// - 2 : 제곱 (빙고 직전 급격히 상승)
			/// - 3 : 세제곱 (매우 악랄)
			/// </summary>
			public float FilledPower;


			/* =========================
			 *  특수 라인 / 위치 보정
			 * ========================= */

			/// <summary>
			/// 대각선 빙고 라인에 속한 슬롯의 가중치 배율
			/// - 대각선은 전략적으로 중요하므로 보통 > 1
			/// </summary>
			public float DiagonalMultiplier;

			/// <summary>
			/// 중앙 슬롯(12번)에 추가로 부여되는 점수
			/// - 중앙은 4개의 라인에 속하므로
			///   AI가 "자연스럽게" 중앙을 노리게 됨
			/// </summary>
			public float CenterSlotBonus;


			/* =========================
			 *  위험 상황 보정
			 * ========================= */

			/// <summary>
			/// 빙고 직전 상태 (라인 내 4/5가 채워졌을 때)
			/// 해당 슬롯에 추가로 더해지는 보너스 점수
			/// - 값이 클수록 "빙고 차단" 성향이 강해짐
			/// </summary>
			public float NearBingoBonus;
		}
	}
}
