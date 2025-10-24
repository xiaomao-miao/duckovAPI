using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000205 RID: 517
public class ForceUnmaskable : MonoBehaviour
{
	// Token: 0x06000F24 RID: 3876 RVA: 0x0003BC78 File Offset: 0x00039E78
	private void OnEnable()
	{
		MaskableGraphic[] components = base.GetComponents<MaskableGraphic>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].maskable = false;
		}
	}
}
