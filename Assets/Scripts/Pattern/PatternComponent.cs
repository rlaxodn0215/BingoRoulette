using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjectRoulette
{
	public struct PatternResultData
	{
		public List<List<int>> Patterns;
		public float Ratio;
	}

	public class PatternComponent // 정보 보여주는 UI Class 제작하기
	{
		public EPattern PatternName { get; private set; }

		private readonly PatternData _patternDataCache;
		private PatternData _currentPatternData;
		public PatternData CurrentPatternData
		{
			get => _currentPatternData;
			set
			{
				_currentPatternData = value;
				isDataDirty = true;
			}
		}

		public bool isDataDirty { get; private set; } = false;


		public PatternComponent(PatternData patternData)
		{
			CurrentPatternData = _patternDataCache = patternData;
			PatternName = (EPattern)CurrentPatternData.Key;
		}

		public void ResetData()
		{
			_currentPatternData = _patternDataCache;
			isDataDirty = false;
		}

		public PatternResultData GetPatternResultData(List<SlotInfo> slotInfoList)
		{
			var resultData = new PatternResultData
			{
				Patterns = new List<List<int>>(),
				Ratio = 0f
			};

			if (slotInfoList == null || slotInfoList.Count != 25)
			{
				Debug.LogError("Invalid slotInfoList");
				return resultData;
			}

			// --- Convert slotInfoList to 0/1 board ---
			var board = new int[25];
			for (var i = 0; i < 25; i++)
			{
				board[i] = slotInfoList[i].SymbolType == ESymbol.None ? 0 : 1;
			}

			// --- Pre-calc board 2D indices ---
			const int boardSize = 5;

			// Parse pattern string -> int[25]
			var pattern = ParsePattern(CurrentPatternData.PatternType);
			if (pattern == null || pattern.Length != 25)
			{
				Debug.LogError("Invalid pattern string");
				return resultData;
			}

			// Get relative positions of "1" in the pattern
			var patternOnes = new List<(int r, int c)>();

			for (var i = 0; i < 25; i++)
			{
				if (pattern[i] == 1)
				{
					patternOnes.Add((i / boardSize, i % boardSize));
				}
			}

			if (patternOnes.Count == 0)
			{
				Debug.LogError("No pattern found");
				return resultData;
			}

			var rowOffset = GlobalValue.SlotRowCount - 1;
			var colOffset = GlobalValue.SlotColCount - 1;

			// Try all offsets
			for (var rowShift = -rowOffset; rowShift <= rowOffset; rowShift++)
			{
				for (var colShift = -colOffset; colShift <= colOffset; colShift++)
				{
					var match = true;
					var matchedSlots = new List<int>();

					foreach (var (pr, pc) in patternOnes)
					{
						var tr = pr + rowShift;
						var tc = pc + colShift;

						// Out of board → fail
						if (tr < 0 || tr >= GlobalValue.SlotRowCount || tc < 0 || tc >= GlobalValue.SlotColCount)
						{
							match = false;
							break;
						}

						var index = tr * GlobalValue.SlotRowCount + tc;

						if (board[index] != 1)
						{
							match = false;
							break;
						}

						matchedSlots.Add(index);
					}

					if (!match) continue;
					resultData.Patterns.Add(matchedSlots);
				}
			}

			// 가산 비율 계산
			resultData.Ratio = CurrentPatternData.PatternRatio;

			return resultData;

			// Pattern string "{0,1,0,...}" parser
			int[] ParsePattern(string patternStr)
			{
				if (string.IsNullOrEmpty(patternStr))
				{
					return null;
				}

				patternStr = patternStr.Replace("{", "").Replace("}", "");
				var tokens = patternStr.Split(',');

				var result = new int[tokens.Length];
				for (var i = 0; i < tokens.Length; i++)
				{
					result[i] = int.TryParse(tokens[i], out var v) ? v : 0;
				}

				return result;
			}
		}
	}
}
