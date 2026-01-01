using UnityEngine;

namespace BingoRoulette
{
	public class GameManager : SingletonTemplate<GameManager>
	{
		[SerializeField] private Texture2D _cursorTexture;
		[SerializeField] private Vector2 _hotSpot = Vector2.zero; // 클릭 기준점
		[SerializeField] private CursorMode _cursorMode = CursorMode.Auto;

		protected override void Awake()
		{
			base.Awake();

			Cursor.SetCursor(_cursorTexture, _hotSpot, _cursorMode);
		}
	}
}
