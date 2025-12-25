using System;
using System.Collections.Generic;
using UnityEngine;

// Enum 값 = 패턴 Key 값
public enum EPattern
{
	None,
	Single,
	Row3,
	Row4,
	Row5,
	Col3,
	Col4,
	Col5,
	Diag3_Main,
	Diag4_Main,
	Diag5_Main,
	Diag3_Anti,
	Diag4_Anti,
	Diag5_Anti,
	LShape,
	LShape_Anti,
	TShape,
	Cross,
	Square_2x2,
	ZShape,
	SShape,
	Arc_Left,
	Arc_Right,
	Plus_Small,
	WShape,
	All,
	Max,
}

// Enum 값 = 아이템 Key 값
public enum EItem
{
	None,
	A,
	B,
	C,
	D,
	Block,
	Mimic,
	Single,
	Row3,
	Row4,
	Row5,
	Col3,
	Col4,
	Col5,
	Diag3_Main,
	Diag4_Main,
	Diag5_Main,
	Diag3_Anti,
	Diag4_Anti,
	Diag5_Anti,
	LShape,
	LShape_Anti,
	TShape,
	Cross,
	Square_2x2,
	ZShape,
	SShape,
	Arc_Left,
	Arc_Right,
	Plus_Small,
	WShape,
	Apple,
	Max,
}

public enum EItemType
{
	CreateSymbol,
	CreatePattern,
	Upgrade
}

public enum EItemSkill
{
	None,
	BlocSlot,
	Mimic,
	UpSymbolChance,
	UpSymbolValue,
	UpPatternChance,
	UpPatternValue
}

public enum ESymbol
{
	None = -2,
	Block,
	A,
	B,
	C,
	Max
}

public enum EUI
{
	UITitleWindow,
	UIGameWindow,
	UIMapSelectPopup,
	UICustomPopup,
	UISettingsPopup,
}

public enum ESound
{
	ButtonClick,
	GameOver,
	Max
}

public enum EAudioMixerType
{
	Master,
	BGM,
	SFX
}

public enum ELocalizeKey // List의 인덱스와 동일
{
	KawaiiPang,
	Start,
	Custom,
	Settings,
	Exit,
}

public struct SymbolCustomData
{
	public ESymbol Symbol;
	public int ChanceWeight;
	public int Point;
}

public struct PatternCustomData
{
	public bool IsDefault;
	public int MinGetPointCount;
	public string CustomPattern;
}

[Serializable]
public class GameData
{
	public string DataName;
	public string DataPath;
	public int RowCount;
	public int ColCount;
	public List<SymbolCustomData> ListSymbolCustomData;
	public List<PatternCustomData> ListPatternCustomData;
	public bool IsTimeAttack;
	public int TimeLimit;
	public bool IsTargetPoint;
	public int TargetPoint;
	public bool IsRandomBlock;
	public int BlockCount;
	public List<int> ListBlockIndex;
}

[Serializable]
public class LocalizationData
{
	public string Language;
	public List<string> ListText;
}

