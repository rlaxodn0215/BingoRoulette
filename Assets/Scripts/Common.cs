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

	public enum EAchievement
	{
		GET_100_POINTS,
		GET_200_POINTS,
		GET_300_POINTS,
		GET_400_POINTS,
		GET_500_POINTS,
		GET_600_POINTS,
		GET_700_POINTS,
		GET_800_POINTS,
		GET_900_POINTS,
		GET_1000_POINTS
	}

	public enum EStat
	{
		STAT_TRY_COUNT,
		STAT_BINGO_COUNT,
		STAT_BEST_POINT
	}

	public enum ELeaderboard
	{
		LB_SCORE_BEST,
		LB_BINGO_COUNT_BEST,
		LB_TRY_COUNT_BEST
	}
}
