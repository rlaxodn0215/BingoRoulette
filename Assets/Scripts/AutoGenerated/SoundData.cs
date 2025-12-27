using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace BingoRoulette
{
	[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
	public class SoundData : SOBase
	{
		public int Key;
		public ESoundType SoundType;
		public bool Loop;
		public float CoolTime;
		public float Volume;
		public float Pitch;
		public string AssetPath;

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
					var targetType = typeof(ESoundType);
					if (targetType.IsEnum)
						SoundType = (ESoundType)Enum.Parse(targetType, dataList[1]);
					else
						SoundType = (ESoundType)Convert.ChangeType(dataList[1], targetType);
				}
				catch { }
			}
			if (dataList.Count > 2 && !string.IsNullOrEmpty(dataList[2]))
			{
				try
				{
					var targetType = typeof(bool);
					if (targetType.IsEnum)
						Loop = (bool)Enum.Parse(targetType, dataList[2]);
					else
						Loop = (bool)Convert.ChangeType(dataList[2], targetType);
				}
				catch { }
			}
			if (dataList.Count > 3 && !string.IsNullOrEmpty(dataList[3]))
			{
				try
				{
					var targetType = typeof(float);
					if (targetType.IsEnum)
						CoolTime = (float)Enum.Parse(targetType, dataList[3]);
					else
						CoolTime = (float)Convert.ChangeType(dataList[3], targetType);
				}
				catch { }
			}
			if (dataList.Count > 4 && !string.IsNullOrEmpty(dataList[4]))
			{
				try
				{
					var targetType = typeof(float);
					if (targetType.IsEnum)
						Volume = (float)Enum.Parse(targetType, dataList[4]);
					else
						Volume = (float)Convert.ChangeType(dataList[4], targetType);
				}
				catch { }
			}
			if (dataList.Count > 5 && !string.IsNullOrEmpty(dataList[5]))
			{
				try
				{
					var targetType = typeof(float);
					if (targetType.IsEnum)
						Pitch = (float)Enum.Parse(targetType, dataList[5]);
					else
						Pitch = (float)Convert.ChangeType(dataList[5], targetType);
				}
				catch { }
			}
			if (dataList.Count > 6 && !string.IsNullOrEmpty(dataList[6]))
			{
				try
				{
					var targetType = typeof(string);
					if (targetType.IsEnum)
						AssetPath = (string)Enum.Parse(targetType, dataList[6]);
					else
						AssetPath = (string)Convert.ChangeType(dataList[6], targetType);
				}
				catch { }
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	#endif
	}
}
