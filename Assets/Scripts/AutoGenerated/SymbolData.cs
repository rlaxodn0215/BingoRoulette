using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace ProjectRoulette
{
	[CreateAssetMenu(fileName = "SymbolData", menuName = "Scriptable Objects/SymbolData")]
	public class SymbolData : SOBase
	{
		public int Key;
		public int LocalizationKey;
		public int ChanceWeight;
		public int Point;
		public float SameSymbolPatternRatio;

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
					var targetType = typeof(int);
					if (targetType.IsEnum)
						ChanceWeight = (int)Enum.Parse(targetType, dataList[2]);
					else
						ChanceWeight = (int)Convert.ChangeType(dataList[2], targetType);
				}
				catch { }
			}
			if (dataList.Count > 3 && !string.IsNullOrEmpty(dataList[3]))
			{
				try
				{
					var targetType = typeof(int);
					if (targetType.IsEnum)
						Point = (int)Enum.Parse(targetType, dataList[3]);
					else
						Point = (int)Convert.ChangeType(dataList[3], targetType);
				}
				catch { }
			}
			if (dataList.Count > 4 && !string.IsNullOrEmpty(dataList[4]))
			{
				try
				{
					var targetType = typeof(float);
					if (targetType.IsEnum)
						SameSymbolPatternRatio = (float)Enum.Parse(targetType, dataList[4]);
					else
						SameSymbolPatternRatio = (float)Convert.ChangeType(dataList[4], targetType);
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
