using UnityEngine;
using UnityEngine.EventSystems;

namespace BingoRoulette
{
	public class UIButtonSound : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private ESound _clickSound;

		public void OnPointerClick(PointerEventData eventData)
		{
			SoundManager.Instance.Play(_clickSound);
		}
	}
}
