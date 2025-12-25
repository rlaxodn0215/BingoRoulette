using UnityEngine;

namespace ProjectRoulette
{
	public interface IManagerInit
	{
		public void InitAwake();
		public void InitStart();
	}

	public class Managers : MonoBehaviour
	{
		private static Managers _instance;

		private DataManager _data = new DataManager();
		private ItemManager _item = new ItemManager();
		private UIManager _ui = new UIManager();
		private GameManger _game = new GameManger();

		public static DataManager Data => _instance._data;
		public static ItemManager Item => _instance._item;
		public static UIManager UI => _instance._ui;
		public static GameManger Game => _instance._game;

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;
			DontDestroyOnLoad(gameObject);
			InitAwake();
		}

		private void Start()
		{
			InitStart();
		}

		private void InitAwake()
		{
			Data.InitAwake();
			Item.InitAwake();
			UI.InitAwake();
			Game.InitAwake();
		}

		private void InitStart()
		{
			Data.InitStart();
			Item.InitStart();
			UI.InitStart();
			Game.InitStart();
		}

		public void Clear()
		{
		}
	}
}
