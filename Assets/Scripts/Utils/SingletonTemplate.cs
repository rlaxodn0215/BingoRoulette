using UnityEngine;

namespace BingoRoulette
{
	public class SingletonTemplate<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}

				_instance = FindFirstObjectByType<T>();

				if (_instance != null)
				{
					return _instance;
				}

				var go = new GameObject(typeof(T).Name);
				_instance = go.AddComponent<T>();

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}
