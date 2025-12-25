using UnityEngine;

namespace ProjectRoulette
{
	public class ItemComponent // 정보 보여주는 UI Class 제작하기
	{
		private static int _id = 0;
		private int _itemId = 0;

		public int Id
		{
			get
			{
				if (_itemId == 0)
				{
					_itemId = ++_id;
				}

				return _itemId;
			}
		}

		public EItem ItemName { get; private set; }
		private readonly ItemData _itemDataCache;
		private ItemData _currentItemData;

		public ItemData CurrentItemData
		{
			get => _currentItemData;
			set
			{
				_currentItemData = value;
				isDataDirty = true;
			}
		}

		public bool isDataDirty { get; private set; } = false;

		public ItemComponent(ItemData itemData)
		{
			CurrentItemData = _itemDataCache = itemData;
			ItemName = (EItem)CurrentItemData.Key;
		}

		public void ResetData()
		{
			_currentItemData = _itemDataCache;
			isDataDirty = false;
		}


		public void Execute()
		{
			// Create Symbol or Pattern

			// Item Use
		}
	}
}
