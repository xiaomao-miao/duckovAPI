using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000153 RID: 339
public class CanvasScalerController : MonoBehaviour
{
	// Token: 0x06000A6B RID: 2667 RVA: 0x0002DB8D File Offset: 0x0002BD8D
	private void OnValidate()
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0002DBA9 File Offset: 0x0002BDA9
	private void Awake()
	{
		this.OnValidate();
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0002DBB1 File Offset: 0x0002BDB1
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0002DBBC File Offset: 0x0002BDBC
	private void Refresh()
	{
		if (this.canvasScaler == null)
		{
			return;
		}
		Vector2Int currentResolution = this.GetCurrentResolution();
		float num = (float)currentResolution.x / (float)currentResolution.y;
		Vector2 referenceResolution = this.canvasScaler.referenceResolution;
		float num2 = referenceResolution.x / referenceResolution.y;
		if (num > num2)
		{
			this.canvasScaler.matchWidthOrHeight = 1f;
		}
		else
		{
			this.canvasScaler.matchWidthOrHeight = 0f;
		}
		this.cachedResolution = currentResolution;
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0002DC37 File Offset: 0x0002BE37
	private void FixedUpdate()
	{
		if (!this.ResolutionMatch())
		{
			this.Refresh();
		}
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0002DC48 File Offset: 0x0002BE48
	private bool ResolutionMatch()
	{
		Vector2Int currentResolution = this.GetCurrentResolution();
		return this.cachedResolution.x == currentResolution.x && this.cachedResolution.y == currentResolution.y;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0002DC86 File Offset: 0x0002BE86
	private Vector2Int GetCurrentResolution()
	{
		return new Vector2Int(Display.main.renderingWidth, Display.main.renderingHeight);
	}

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private CanvasScaler canvasScaler;

	// Token: 0x0400091C RID: 2332
	private Vector2Int cachedResolution;
}
