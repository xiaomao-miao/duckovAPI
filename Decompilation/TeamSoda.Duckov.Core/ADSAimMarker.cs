using System;
using System.Collections.Generic;
using Duckov.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000075 RID: 117
public class ADSAimMarker : MonoBehaviour
{
	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x000136CE File Offset: 0x000118CE
	public float CanvasAlpha
	{
		get
		{
			return this.canvasAlpha;
		}
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x000136D8 File Offset: 0x000118D8
	public void CollectCrosshairs()
	{
		this.crosshairs.Clear();
		foreach (SingleCrosshair item in base.GetComponentsInChildren<SingleCrosshair>())
		{
			this.crosshairs.Add(item);
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00013718 File Offset: 0x00011918
	private void Awake()
	{
		this.proceduralImageCanvasGroups = new List<CanvasGroup>();
		for (int i = 0; i < this.proceduralImages.Count; i++)
		{
			this.proceduralImageCanvasGroups.Add(this.proceduralImages[i].GetComponent<CanvasGroup>());
		}
		this.selfRect = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00013770 File Offset: 0x00011970
	private void LateUpdate()
	{
		if (this.selfRect)
		{
			this.selfRect.localScale = Vector3.one;
		}
		this.followUI.position = Vector3.Lerp(this.followUI.position, this.aimMarkerUI.position, Time.deltaTime * this.followSpeed);
		if (Vector3.Distance(this.followUI.position, this.aimMarkerUI.position) > this.followMaxDistance)
		{
			this.followUI.position = Vector3.MoveTowards(this.aimMarkerUI.position, this.followUI.position, this.followMaxDistance);
		}
		foreach (SingleCrosshair singleCrosshair in this.crosshairs)
		{
			if (singleCrosshair)
			{
				singleCrosshair.UpdateScatter(this.scatter);
			}
		}
		this.SetSniperRenderer();
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00013874 File Offset: 0x00011A74
	public void SetAimMarkerPos(Vector3 pos)
	{
		this.aimMarkerUI.position = pos;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00013884 File Offset: 0x00011A84
	public void OnShoot()
	{
		foreach (PunchReceiver punchReceiver in this.shootPunchReceivers)
		{
			if (punchReceiver)
			{
				punchReceiver.Punch();
			}
		}
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000138E0 File Offset: 0x00011AE0
	public void SetScatter(float _currentScatter, float _minScatter)
	{
		this.scatter = _currentScatter;
		this.minScatter = _minScatter;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x000138F0 File Offset: 0x00011AF0
	public void SetAdsValue(float _adsValue)
	{
		this.adsValue = _adsValue;
		this.canvasAlpha = _adsValue;
		if (this.adsAlphaRemap.y > this.adsAlphaRemap.x)
		{
			this.canvasAlpha = Mathf.Clamp01((_adsValue - this.adsAlphaRemap.x) / (this.adsAlphaRemap.y - this.adsAlphaRemap.x));
		}
		this.canvasGroup.alpha = this.canvasAlpha;
		for (int i = 0; i < this.proceduralImages.Count; i++)
		{
			ProceduralImage proceduralImage = this.proceduralImages[i];
			if (proceduralImage)
			{
				float num = Mathf.Clamp(this.scatter - this.minScatter, 0f, 10f) * 2f;
				proceduralImage.FalloffDistance = Mathf.Lerp(25f, 1f, this.canvasAlpha) + num;
				CanvasGroup canvasGroup = this.proceduralImageCanvasGroups[i];
				if (canvasGroup)
				{
					canvasGroup.alpha = Mathf.Clamp(1f - (num - 2f) / 15f, 0.3f, 1f);
				}
			}
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00013A14 File Offset: 0x00011C14
	private void SetSniperRenderer()
	{
		if (this.sniperRoundRenderer)
		{
			Vector2 v = RectTransformUtility.WorldToScreenPoint(null, this.aimMarkerUI.position) / new Vector2((float)Screen.width, (float)Screen.height);
			this.sniperRoundRenderer.material.SetVector(this.sniperCenterShaderHash, v);
		}
		if (this.followSniperRoundRenderer)
		{
			Vector2 v2 = RectTransformUtility.WorldToScreenPoint(null, this.followUI.position) / new Vector2((float)Screen.width, (float)Screen.height);
			this.followSniperRoundRenderer.material.SetVector(this.sniperCenterShaderHash, v2);
		}
	}

	// Token: 0x04000395 RID: 917
	[HideInInspector]
	public ADSAimMarker selfPrefab;

	// Token: 0x04000396 RID: 918
	public bool hideNormalCrosshair = true;

	// Token: 0x04000397 RID: 919
	public AimMarker parentAimMarker;

	// Token: 0x04000398 RID: 920
	public RectTransform aimMarkerUI;

	// Token: 0x04000399 RID: 921
	public RectTransform followUI;

	// Token: 0x0400039A RID: 922
	public CanvasGroup canvasGroup;

	// Token: 0x0400039B RID: 923
	public float followSpeed;

	// Token: 0x0400039C RID: 924
	public float followMaxDistance = 30f;

	// Token: 0x0400039D RID: 925
	private float adsValue = -1f;

	// Token: 0x0400039E RID: 926
	private float canvasAlpha;

	// Token: 0x0400039F RID: 927
	public Vector2 adsAlphaRemap = new Vector2(0f, 1f);

	// Token: 0x040003A0 RID: 928
	public List<ProceduralImage> proceduralImages;

	// Token: 0x040003A1 RID: 929
	private List<CanvasGroup> proceduralImageCanvasGroups;

	// Token: 0x040003A2 RID: 930
	public List<PunchReceiver> shootPunchReceivers;

	// Token: 0x040003A3 RID: 931
	public List<SingleCrosshair> crosshairs;

	// Token: 0x040003A4 RID: 932
	public Graphic sniperRoundRenderer;

	// Token: 0x040003A5 RID: 933
	public Graphic followSniperRoundRenderer;

	// Token: 0x040003A6 RID: 934
	private float scatter;

	// Token: 0x040003A7 RID: 935
	private float minScatter;

	// Token: 0x040003A8 RID: 936
	private RectTransform selfRect;

	// Token: 0x040003A9 RID: 937
	private int sniperCenterShaderHash = Shader.PropertyToID("_RoundCenter");
}
