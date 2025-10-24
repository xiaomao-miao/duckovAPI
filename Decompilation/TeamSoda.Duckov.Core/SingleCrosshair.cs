using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class SingleCrosshair : MonoBehaviour
{
	// Token: 0x060004A9 RID: 1193 RVA: 0x00015474 File Offset: 0x00013674
	public void UpdateScatter(float _scatter)
	{
		this.currentScatter = _scatter;
		RectTransform rectTransform = base.transform as RectTransform;
		rectTransform.localRotation = Quaternion.Euler(0f, 0f, this.rotation);
		Vector3 a = Vector3.zero;
		if (this.axis != Vector3.zero)
		{
			a = rectTransform.parent.InverseTransformDirection(rectTransform.TransformDirection(this.axis));
		}
		rectTransform.anchoredPosition = a * (this.minDistance + this.currentScatter * this.scatterMoveScale);
		if (this.controlRectWidthHeight)
		{
			float d = this.minScale + this.currentScatter * this.scatterScaleFactor;
			rectTransform.sizeDelta = Vector2.one * d;
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00015532 File Offset: 0x00013732
	private void OnValidate()
	{
		this.UpdateScatter(0f);
	}

	// Token: 0x040003E7 RID: 999
	public float rotation;

	// Token: 0x040003E8 RID: 1000
	public Vector3 axis;

	// Token: 0x040003E9 RID: 1001
	public float minDistance;

	// Token: 0x040003EA RID: 1002
	public float scatterMoveScale = 5f;

	// Token: 0x040003EB RID: 1003
	private float currentScatter;

	// Token: 0x040003EC RID: 1004
	public bool controlRectWidthHeight;

	// Token: 0x040003ED RID: 1005
	public float minScale = 100f;

	// Token: 0x040003EE RID: 1006
	public float scatterScaleFactor = 5f;
}
