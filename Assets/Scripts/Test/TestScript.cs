using System;
using UnityEngine;

namespace ProjectRoulette
{
	public class TestScript : MonoBehaviour
	{
		public enum ETest
		{
			Test
		}
		private string val = "Test";
		void Start()
		{
			// 수정
			if (Enum.TryParse(val.Trim(), out ETest temp))
			{
				Debug.Log(temp);
			}
			
			//var temp = (ETest)Convert.ChangeType(val.Trim(), typeof(ETest));
			//Debug.Log(temp);
		}

		// Update is called once per frame
		void Update()
		{
		}
	}
}
