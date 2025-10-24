using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200017F RID: 383
public class MainMenuCamera : MonoBehaviour
{
	// Token: 0x06000B89 RID: 2953 RVA: 0x00030C40 File Offset: 0x0002EE40
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		float t = mousePosition.x / num;
		float t2 = mousePosition.y / num2;
		base.transform.localRotation = quaternion.Euler(0f, Mathf.Lerp(this.yawRange.x, this.yawRange.y, t) * 0.017453292f, 0f, math.RotationOrder.ZXY);
		if (this.pitchRoot)
		{
			this.pitchRoot.localRotation = quaternion.Euler(Mathf.Lerp(this.pitchRange.x, this.pitchRange.y, t2) * 0.017453292f, 0f, 0f, math.RotationOrder.ZXY);
		}
		base.transform.localPosition = new Vector3(Mathf.Lerp(this.posRangeX.x, this.posRangeX.y, t), Mathf.Lerp(this.posRangeY.x, this.posRangeY.y, t2), 0f);
	}

	// Token: 0x040009D2 RID: 2514
	public Vector2 yawRange;

	// Token: 0x040009D3 RID: 2515
	public Vector2 pitchRange;

	// Token: 0x040009D4 RID: 2516
	public Transform pitchRoot;

	// Token: 0x040009D5 RID: 2517
	[FormerlySerializedAs("posRange")]
	public Vector2 posRangeX;

	// Token: 0x040009D6 RID: 2518
	public Vector2 posRangeY;
}
