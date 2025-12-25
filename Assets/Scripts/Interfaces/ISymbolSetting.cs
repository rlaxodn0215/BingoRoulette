using UnityEngine;

namespace ProjectPang
{
	public interface ISymbolInfo
	{
		public int GetSymbolIndex();
		public int GetSymbolValue();
	}
	
	public interface ISymbolSetting
	{
		public bool IsShowNextSymbol { get; set; }
		public ISymbolInfo GetSymbol();
	}
}
