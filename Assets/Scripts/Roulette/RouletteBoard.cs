using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace ProjectRoulette
{
	public struct PointInfo
	{
		public ulong Point;
		public List<int> ListIndex;
	}

	public class SlotInfo
	{
		public ESymbol SymbolType;

		public SlotInfo(ESymbol symbolType)
		{
			SymbolType = symbolType;
		}
	}

	public class RouletteBoard
	{
		public List<SlotInfo> ListBoardSlotInfo { get; private set; } = new List<SlotInfo>();
		public List<int> BlockSlotIndex { get; private set; } = new List<int>();

		public ulong Point { get; private set; }
		public List<PointInfo> ListPointInfo { get; private set; } = new List<PointInfo>();

		public ESymbol CurrentSymbolType { get; private set; }
		public ESymbol NextSymbolType { get; private set; }

		// Events
		public UnityEvent<ulong> OnPointChanged = new UnityEvent<ulong>(); // ulong - 바뀐 점수

		public UnityEvent<int, ESymbol>
			OnSlotClicked = new UnityEvent<int, ESymbol>(); // int - index, ESymbol - current symbol type

		public UnityEvent<int> OnBlockSlotClicked = new UnityEvent<int>();
		public UnityEvent<ESymbol> OnUpdateSymbol = new UnityEvent<ESymbol>(); // ESymbol - new symbol type
		public UnityEvent OnRouletteReset = new UnityEvent();

		// 현재 룰렛에 적용되는 심볼 및 패턴
		public List<SymbolData> ListUseSymbol = new List<SymbolData>(); // Index+1 값이 ESymbol enum 값이랑 동일
		public List<PatternComponent> ListUsePattern = new List<PatternComponent>();

		private List<List<int>> _patternHistory = new List<List<int>>();
		private int _slotCount = GlobalValue.SlotRowCount * GlobalValue.SlotColCount;

		public void Init()
		{
			for (var i = 0; i < _slotCount; i++)
			{
				ListBoardSlotInfo.Add(new SlotInfo(ESymbol.None));
			}

			RefreshBoard();
		}

		public void SelectBoardButton(int index)
		{
			Debug.Log("Board Button Selected : " + index);

			if (index < 0 || index >= _slotCount)
			{
				Debug.LogError("Index is out of range");
				return;
			}

			if (CheckBlockSlot(index))
			{
				Debug.Log("Block Slot Clicked");
				return;
			}

			ListBoardSlotInfo[index].SymbolType = CurrentSymbolType;
			ListPointInfo = GetPointInfo();
			UpdateSymbol();

			OnPointChanged?.Invoke(Point);
			OnSlotClicked?.Invoke(index, CurrentSymbolType);
		}

		private void RefreshBoard()
		{
			Debug.Log("Reset Roulette Board");

			foreach (var slotInfo in ListBoardSlotInfo)
			{
				slotInfo.SymbolType = ESymbol.None;
			}

			CurrentSymbolType = GetNextSymbolType();
			NextSymbolType = GetNextSymbolType();
			OnUpdateSymbol?.Invoke(NextSymbolType);
		}

		private List<PointInfo> GetPointInfo()
		{
			var list = new List<PointInfo>();

			// 새로 나오는 패턴 넣기
			var boardPatternData = new List<PatternResultData>();
			foreach (var pattern in ListUsePattern)
			{
				var patternData = pattern.GetPatternResultData(ListBoardSlotInfo);
				RemoveSameElements(patternData.Patterns, _patternHistory);
				boardPatternData.Add(patternData);
				_patternHistory.AddRange(patternData.Patterns);
			}

			// 점수 계산
			foreach (var patternResultData in boardPatternData)
			{
				foreach (var pattern in patternResultData.Patterns)
				{
					// 한 패턴에 있는 모든 심볼의 값을 더한 다음 패턴 비율을 곱한다.
					var total = 0;
					var symbol = ESymbol.None;
					var isSameSymbol = true;
					foreach (var slotIndex in pattern)
					{
						var symbolType = ListBoardSlotInfo[slotIndex].SymbolType;
						total += ListUseSymbol[(int)symbolType - 1].Point;
						if (symbol == ESymbol.None)
						{
							symbol = symbolType;
						}

						if (symbol != symbolType)
						{
							isSameSymbol = false;
						}
					}

					// 만약 동일 심볼이면 추가 배율 가산이 있다.
					if (isSameSymbol)
					{
						total *= (int)(ListUseSymbol[(int)symbol - 1].SameSymbolPatternRatio);
					}

					var point = (ulong)(total * patternResultData.Ratio);
					list.Add(new PointInfo { Point = point, ListIndex = pattern });
				}
			}

			return list;

			void RemoveSameElements(List<List<int>> patterns, List<List<int>> patternHistory)
			{
				// patternHistory의 리스트를 정렬 문자열로 HashSet에 저장
				var bSet = new HashSet<string>();

				for (var i = 0; i < patternHistory.Count; i++)
				{
					var list = patternHistory[i];
					var sorted = new List<int>(list);
					sorted.Sort();

					var key = string.Join(",", sorted);
					bSet.Add(key);
				}

				// patterns에서 제거 (뒤에서 앞으로 순회)
				for (var i = patterns.Count - 1; i >= 0; i--)
				{
					var list = patterns[i];
					var sorted = new List<int>(list);
					sorted.Sort();

					var key = string.Join(",", sorted);

					if (bSet.Contains(key))
					{
						patterns.RemoveAt(i);
					}
				}
			}
		}

		private bool CheckBlockSlot(int slotIndex)
		{
			foreach (var blockIndex in BlockSlotIndex)
			{
				if (blockIndex == slotIndex)
				{
					return true;
				}
			}

			return false;
		}

		private void UpdateSymbol()
		{
			CurrentSymbolType = NextSymbolType;
			NextSymbolType = GetNextSymbolType();
			OnUpdateSymbol?.Invoke(NextSymbolType);
		}

		private ESymbol GetNextSymbolType()
		{
			var list = new List<int>();

			foreach (var symbolData in ListUseSymbol)
			{
				list.Add(symbolData.ChanceWeight);
			}

			return (ESymbol)(ProbabilityUtility.GetRandomProbability(list) + 1);
		}
	}
}
