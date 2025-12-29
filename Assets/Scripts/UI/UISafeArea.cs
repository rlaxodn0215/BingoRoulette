using UnityEngine;

[ExecuteAlways]
public class UIScaleByAspect : MonoBehaviour
{
	public Vector2 referenceResolution = new Vector2(1920, 1080);

	RectTransform rect;
	RectTransform canvasRect;

	Vector2 lastCanvasSize;

	void Awake()
	{
		Init();
		Apply();
	}

	void OnEnable()
	{
		Init();
		Apply();
	}

	void Update()
	{
		if (rect == null || canvasRect == null)
			return;

		if (canvasRect.rect.size != lastCanvasSize)
			Apply();
	}

	void Init()
	{
		rect = GetComponent<RectTransform>();

		Canvas canvas = GetComponentInParent<Canvas>();
		if (canvas == null) return;

		canvasRect = canvas.GetComponent<RectTransform>();
	}

	void Apply()
	{
		Vector2 canvasSize = canvasRect.rect.size;
		if (canvasSize.x <= 0 || canvasSize.y <= 0)
			return;

		// 기준 대비 얼마나 줄이거나 늘릴지 계산
		float scaleX = canvasSize.x / referenceResolution.x;
		float scaleY = canvasSize.y / referenceResolution.y;

		// ⭐ 핵심: 작은 쪽 기준으로 통일 (비율 유지)
		float scale = Mathf.Min(scaleX, scaleY);

		rect.localScale = Vector3.one * scale;
		rect.anchoredPosition = Vector2.zero;

		lastCanvasSize = canvasSize;
	}
}
