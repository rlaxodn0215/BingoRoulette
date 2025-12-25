using System.Collections.Generic;
using UnityEngine;

namespace ProjectPang
{
	public interface ISlotInfo
	{
		public bool IsEmpty { get; set; }
		public bool IsBlock { get; set; }
		public int GetBlockIndex();
	}
	
	public interface IMapSetting
	{
		public string MapName { get; set; }
		public int Width { get; set; }
		public bool IsTimeAttack{ get; set; }
		public int TimeLimit{ get; set; }
		public bool IsTargetPoint{ get; set; }
		public int TargetPoint{ get; set; }
		public List<ISlotInfo> GetMapInfo();
	}
}
