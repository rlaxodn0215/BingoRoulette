using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace ProjectRoulette
{
	[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
	public class ItemData : SOBase
	{
		public int Key;
		public int LocalizationKey;
		public EItemType ItemType;
		public EItemSkill ItemSkill;
		public List<ESymbol> Symbol;
		public float SymbolValue;
		public List<EPattern> Pattern;
		public float PatternValue;

	#if UNITY_EDITOR
		public override void UpdateData(List<string> dataList)
		{
			base.UpdateData(dataList);

			if (dataList.Count > 0 && !string.IsNullOrEmpty(dataList[0]))
			{
				try
				{
					var targetType = typeof(int);
					if (targetType.IsEnum)
						Key = (int)Enum.Parse(targetType, dataList[0]);
					else
						Key = (int)Convert.ChangeType(dataList[0], targetType);
				}
				catch { }
			}
			if (dataList.Count > 1 && !string.IsNullOrEmpty(dataList[1]))
			{
				try
				{
					var targetType = typeof(int);
					if (targetType.IsEnum)
						LocalizationKey = (int)Enum.Parse(targetType, dataList[1]);
					else
						LocalizationKey = (int)Convert.ChangeType(dataList[1], targetType);
				}
				catch { }
			}
			if (dataList.Count > 2 && !string.IsNullOrEmpty(dataList[2]))
			{
				try
				{
					var targetType = typeof(EItemType);
					if (targetType.IsEnum)
						ItemType = (EItemType)Enum.Parse(targetType, dataList[2]);
					else
						ItemType = (EItemType)Convert.ChangeType(dataList[2], targetType);
				}
				catch { }
			}
			if (dataList.Count > 3 && !string.IsNullOrEmpty(dataList[3]))
			{
				try
				{
					var targetType = typeof(EItemSkill);
					if (targetType.IsEnum)
						ItemSkill = (EItemSkill)Enum.Parse(targetType, dataList[3]);
					else
						ItemSkill = (EItemSkill)Convert.ChangeType(dataList[3], targetType);
				}
				catch { }
			}
			if (dataList.Count > 4 && !string.IsNullOrEmpty(dataList[4]))
			{
				Symbol = new List<ESymbol>();
				foreach (var val in dataList[4].Split(','))
				{
					try
					{
						var targetType = typeof(ESymbol);
						object converted;
						if (targetType.IsEnum)
							converted = Enum.Parse(targetType, val.Trim());
						else
							converted = Convert.ChangeType(val.Trim(), targetType);
						Symbol.Add((ESymbol)converted);
					}
					catch { }
				}
			}
			if (dataList.Count > 5 && !string.IsNullOrEmpty(dataList[5]))
			{
				try
				{
					var targetType = typeof(float);
					if (targetType.IsEnum)
						SymbolValue = (float)Enum.Parse(targetType, dataList[5]);
					else
						SymbolValue = (float)Convert.ChangeType(dataList[5], targetType);
				}
				catch { }
			}
			if (dataList.Count > 6 && !string.IsNullOrEmpty(dataList[6]))
			{
				Pattern = new List<EPattern>();
				foreach (var val in dataList[6].Split(','))
				{
					try
					{
						var targetType = typeof(EPattern);
						object converted;
						if (targetType.IsEnum)
							converted = Enum.Parse(targetType, val.Trim());
						else
							converted = Convert.ChangeType(val.Trim(), targetType);
						Pattern.Add((EPattern)converted);
					}
					catch { }
				}
			}
			if (dataList.Count > 7 && !string.IsNullOrEmpty(dataList[7]))
			{
				try
				{
					var targetType = typeof(float);
					if (targetType.IsEnum)
						PatternValue = (float)Enum.Parse(targetType, dataList[7]);
					else
						PatternValue = (float)Convert.ChangeType(dataList[7], targetType);
				}
				catch { }
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
	#endif
}
