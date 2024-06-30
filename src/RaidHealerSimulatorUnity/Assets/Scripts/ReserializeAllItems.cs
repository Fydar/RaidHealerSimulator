#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utility
{
	public class ReserializeAllItems : MonoBehaviour
	{
	#if UNITY_2017_1_OR_NEWER
		[MenuItem("Toolkit/Reserialize")]
		public static void Reserialize()
		{
			AssetDatabase.ForceReserializeAssets();
		}
	#endif
	}
}
#endif
