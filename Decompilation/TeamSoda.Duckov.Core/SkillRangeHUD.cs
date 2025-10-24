using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class SkillRangeHUD : MonoBehaviour
{
	// Token: 0x06000659 RID: 1625 RVA: 0x0001CC84 File Offset: 0x0001AE84
	public void SetRange(float range)
	{
		this.rangeTarget.localScale = Vector3.one * range;
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0001CC9C File Offset: 0x0001AE9C
	public void SetProgress(float progress)
	{
		if (this.rangeMat == null)
		{
			this.rangeMat = this.rangeRenderer.material;
		}
		if (this.rangeMat == null)
		{
			return;
		}
		this.rangeMat.SetFloat("_Progress", progress);
	}

	// Token: 0x04000622 RID: 1570
	public Transform rangeTarget;

	// Token: 0x04000623 RID: 1571
	public Renderer rangeRenderer;

	// Token: 0x04000624 RID: 1572
	private Material rangeMat;
}
