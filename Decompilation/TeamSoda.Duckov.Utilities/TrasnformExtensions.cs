using System;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000013 RID: 19
	public static class TrasnformExtensions
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00003ED0 File Offset: 0x000020D0
		public static void DestroyAllChildren(this Transform transform)
		{
			while (transform.childCount > 0)
			{
				Transform child = transform.GetChild(0);
				child.SetParent(null);
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
		}
	}
}
