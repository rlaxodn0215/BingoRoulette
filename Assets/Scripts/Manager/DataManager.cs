using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectRoulette
{
	public class DataManager : IManagerInit
	{
		private Dictionary<ESymbol, SymbolData> _symbolData = new Dictionary<ESymbol, SymbolData>();
		private Dictionary<EPattern, PatternData> _patternData = new Dictionary<EPattern, PatternData>();
		private Dictionary<EItem, ItemData> _itemData = new Dictionary<EItem, ItemData>();
		public List<ItemData> ListItemData { get; private set; } = new List<ItemData>();

		public void InitAwake()
		{
			// Load Data
			var itemData = Resources.LoadAll<ItemData>(GlobalValue.SOpath);
			ListItemData = itemData.ToList();	// Make Item List
			var symbolData = Resources.LoadAll<SymbolData>(GlobalValue.SOpath);
			var patternData = Resources.LoadAll<PatternData>(GlobalValue.SOpath);

			// Cache Data
			for (var i = 1; i < (int)EItem.Max; i++)
			{
				_itemData[(EItem)i] = itemData.FirstOrDefault(x => x.Key == i);
			}

			for (var i = 1; i < (int)ESymbol.Max; i++)
			{
				_symbolData[(ESymbol)i] = symbolData.FirstOrDefault(x => x.Key == i);
			}

			for (var i = 1; i < (int)EPattern.Max; i++)
			{
				_patternData[(EPattern)i] = patternData.FirstOrDefault(x => x.Key == i);
			}
		}

		public void InitStart()
		{
		}

		public ItemData GetItemData(EItem item)
		{
			var itemData = _itemData.GetValueOrDefault(item);

			if (itemData == null)
			{
				Debug.LogError("Item Not Found : " + item);
			}

			return itemData;
		}

		public SymbolData GetSymbolData(ESymbol data)
		{
			var symbolData = _symbolData.GetValueOrDefault(data);

			if (symbolData == null)
			{
				Debug.LogError("Symbol Not Found : " + data);
			}

			return symbolData;
		}

		public PatternData GetPatternData(EPattern item)
		{
			var patternData = _patternData.GetValueOrDefault(item);

			if (patternData == null)
			{
				Debug.LogError("Pattern Not Found : " + item);
			}

			return patternData;
		}

		public void SaveGame()
		{
		}

		public void LoadGame()
		{
		}
	}
}
