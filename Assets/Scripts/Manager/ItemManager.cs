using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRoulette
{
	public class ItemManager : IManagerInit
	{
		// Active Items
		public List<ItemComponent> ListActiveItem = new List<ItemComponent>();
		private readonly DataManager _dataManger = Managers.Data;

		public void InitAwake()
		{
		}

		public void InitStart()
		{
		}

		// ListActiveItem으로 Roulette보드에 있는 심볼/패턴 수정 (이벤트)

		public List<ItemComponent> GachaItems(int count, int round)
		{
			// 1. 랜덤으로 count 만큼 아이템을 뽑는다.
			// 2. 뽑은 아이템 중
			//	플레이어가 이미 가지고 있는 심볼, 패턴일 경우 
			//	Upgrade 아이템이지만 해당 아이템을 가자고 있지 않을 경우
			// 해당 아이템 다시 뽑기 (성립 할 때 까지 / 100번까지만 시도하기)
			// 또한 CreatePattern일 경우 라운드별 가산 비율 적용
			return null;
		}

		private ItemComponent CreateItem(ItemData itemData)
		{
			var itemComponent = new ItemComponent(itemData);
			return itemComponent;
		}

		// public ItemComponent GetItem(int id)
		// {
		// 	foreach (var item in ListItems)
		// 	{
		// 		if (item.Id == id)
		// 		{
		// 			return item;
		// 		}
		// 	}
		//
		// 	Debug.LogError("Item Not Found ID : " + id);
		// 	return null;
		// }
		//
		// public void ReturnItem(int id)
		// {
		// 	foreach (var item in ListItems)
		// 	{
		// 		if (item.Id != id) continue;
		// 		item.Execute();
		// 		return;
		// 	}
		//
		// 	Debug.LogError("Item Not Found ID: " + id);
		// }
	}
}
