using System.Collections.Generic;
using UnityEngine;

namespace ProjectRoulette
{
	public class GameManger : IManagerInit
	{
		// 라운드 관리, 룰렛 관리, 플레이어 관리
		public RouletteBoard Board { get; private set; } = new RouletteBoard();
		

		public void InitAwake()
		{
			Board.Init();
		}

		public void InitStart()
		{
			
		}
	}
}
