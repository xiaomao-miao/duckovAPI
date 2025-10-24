using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000138 RID: 312
public class MapImageToShader : MonoBehaviour
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x0002AF84 File Offset: 0x00029184
	private void Start()
	{
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0002AF88 File Offset: 0x00029188
	private void Update()
	{
		if (!this.material)
		{
			this.material = base.GetComponent<Image>().material;
		}
		if (!this.material)
		{
			return;
		}
		Rect rect = this.rect.rect;
		Vector3 vector = rect.min;
		Vector3 vector2 = rect.max;
		Vector3 vector3 = new Vector3(vector.x, vector.y);
		Vector3 a = new Vector3(vector.x, vector2.y);
		Vector3 a2 = new Vector3(vector2.x, vector.y);
		Vector3 v = base.transform.TransformPoint(vector3);
		Vector3 v2 = base.transform.TransformVector(a - vector3);
		Vector3 v3 = base.transform.TransformVector(a2 - vector3);
		this.material.SetVector(this.zeroPointID, v);
		this.material.SetVector(this.upVectorID, v2);
		this.material.SetVector(this.rightVectorID, v3);
		this.material.SetFloat(this.scaleID, this.rect.lossyScale.x);
	}

	// Token: 0x040008C4 RID: 2244
	public RectTransform rect;

	// Token: 0x040008C5 RID: 2245
	private Material material;

	// Token: 0x040008C6 RID: 2246
	private int zeroPointID = Shader.PropertyToID("_ZeroPoint");

	// Token: 0x040008C7 RID: 2247
	private int upVectorID = Shader.PropertyToID("_UpVector");

	// Token: 0x040008C8 RID: 2248
	private int rightVectorID = Shader.PropertyToID("_RightVector");

	// Token: 0x040008C9 RID: 2249
	private int scaleID = Shader.PropertyToID("_Scale");
}
