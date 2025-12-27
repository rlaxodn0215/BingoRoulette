using UnityEngine;
using System;
using System.Collections.Generic;

namespace BingoRoulette
{
	public class ResourceManager : SingletonTemplate<ResourceManager>
	{
		/// <summary>
		/// 모든 리소스 캐시
		/// key = typeof(TData) + path
		/// </summary>
		private readonly Dictionary<string, UnityEngine.Object> _cache = new();

		/// <summary>
		/// Resources/{rootPath} 이하의 리소스를 전부 로드하여
		/// Enum 이름 == 파일명 기준 Dictionary로 반환
		/// </summary>
		public Dictionary<TEnum, TData> LoadAll<TEnum, TData>(string rootPath)
			where TEnum : struct, Enum
			where TData : UnityEngine.Object
		{
			var result = new Dictionary<TEnum, TData>();
			var list = Resources.LoadAll<TData>(rootPath);

			if (list == null || list.Length == 0)
			{
				Debug.LogWarning($"[ResourceManager] No resources found : {rootPath}");
				return result;
			}

			foreach (var data in list)
			{
				// Enum 존재 여부
				if (!Enum.TryParse<TEnum>(data.name, out var enumKey))
				{
					Debug.LogWarning($"[ResourceManager] Enum not found for asset : {data.name} ({typeof(TEnum).Name})");
					continue;
				}

				// 중복 매핑 방지
				if (result.ContainsKey(enumKey))
				{
					Debug.LogError($"[ResourceManager] Duplicate enum mapping : {enumKey} ({data.name})");
					continue;
				}

				result.Add(enumKey, data);

				// 캐싱
				var cacheKey = GetCacheKey<TData>(rootPath, data.name);
				if (!_cache.ContainsKey(cacheKey))
				{
					_cache.Add(cacheKey, data);
				}
			}

			Debug.Log($"[ResourceManager] Loaded {typeof(TData).Name}: {result.Count} from {rootPath}");

			return result;
		}

		public T Load<T>(string path) where T : UnityEngine.Object
		{
			var cacheKey = GetCacheKey<T>(path);

			if (_cache.TryGetValue(cacheKey, out var cached))
			{
				return cached as T;
			}

			var asset = Resources.Load<T>(path);
			if (asset == null)
			{
				Debug.LogError($"[ResourceManager] Resource not found : {path}");
				return null;
			}

			_cache[cacheKey] = asset;
			return asset;
		}

		public GameObject Instantiate(string path, Transform parent = null)
		{
			var prefab = Load<GameObject>(path);
			if (prefab == null)
			{
				return null;
			}

			var obj = UnityEngine.Object.Instantiate(prefab, parent);
			obj.name = prefab.name;
			return obj;
		}

		public T Instantiate<T>(string path, Transform parent = null) where T : Component
		{
			var prefab = Load<GameObject>(path);
			if (prefab == null)
			{
				return null;
			}

			var obj = UnityEngine.Object.Instantiate(prefab, parent);
			obj.name = prefab.name;
			return obj.GetComponent<T>();
		}

		public void Clear()
		{
			_cache.Clear();
			Resources.UnloadUnusedAssets();
		}

		private static string GetCacheKey<T>(string path)
		{
			return $"{typeof(T).FullName}:{path}";
		}

		private static string GetCacheKey<T>(string rootPath, string name)
		{
			return $"{typeof(T).FullName}:{rootPath}/{name}";
		}
	}
}
