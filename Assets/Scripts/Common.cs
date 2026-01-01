using UnityEngine;

namespace BingoRoulette
{
	public enum ESound
	{
		BGM_1,
		ButtonClick,
		SlotClick,
		DeathSlotClick,
		Bingo,
		GameOver
	}
	
	public enum ESoundType
	{
		Master,
		BGM,
		UI,
		SFX
	}

	public enum ESlotColor
	{
		Berry,
		Blue,
		Cyan,
		Green,
		Indigo,
		Lime,
		Orange,
		Pink,
		Purple,
		Red,
		Teal,
		Violet,
		Yellow,
		Max,
		Gray
	}

	public enum ESlotState
	{
		None,
		Active
	}

	public enum EPrefab
	{
		Slot,
	}

	public enum EFeel
	{
		Stars,
		Life,
		OneLife,
		Point,
		Bingo,
		ClickDeathSlot
	}
}
