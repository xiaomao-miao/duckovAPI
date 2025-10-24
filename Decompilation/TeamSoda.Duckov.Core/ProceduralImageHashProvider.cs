using System;
using LeTai.TrueShadow;
using LeTai.TrueShadow.PluginInterfaces;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000207 RID: 519
[ExecuteInEditMode]
public class ProceduralImageHashProvider : MonoBehaviour, ITrueShadowCustomHashProvider
{
	// Token: 0x06000F29 RID: 3881 RVA: 0x0003BCD9 File Offset: 0x00039ED9
	private void Awake()
	{
		if (this.trueShadow == null)
		{
			this.trueShadow = base.GetComponent<TrueShadow>();
		}
		if (this.proceduralImage == null)
		{
			this.proceduralImage = base.GetComponent<ProceduralImage>();
		}
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x0003BD0F File Offset: 0x00039F0F
	private void Refresh()
	{
		this.trueShadow.CustomHash = this.Hash();
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0003BD22 File Offset: 0x00039F22
	private void OnValidate()
	{
		if (this.trueShadow == null)
		{
			this.trueShadow = base.GetComponent<TrueShadow>();
		}
		if (this.proceduralImage == null)
		{
			this.proceduralImage = base.GetComponent<ProceduralImage>();
		}
		this.Refresh();
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0003BD5E File Offset: 0x00039F5E
	private void OnRectTransformDimensionsChange()
	{
		this.Refresh();
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0003BD68 File Offset: 0x00039F68
	private int Hash()
	{
		return this.proceduralImage.InfoCache.GetHashCode() + this.proceduralImage.color.GetHashCode();
	}

	// Token: 0x04000C62 RID: 3170
	[SerializeField]
	private ProceduralImage proceduralImage;

	// Token: 0x04000C63 RID: 3171
	[SerializeField]
	private TrueShadow trueShadow;
}
