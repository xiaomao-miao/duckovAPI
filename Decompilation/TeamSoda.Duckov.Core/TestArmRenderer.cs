using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class TestArmRenderer : MonoBehaviour
{
	// Token: 0x06000119 RID: 281 RVA: 0x000058EC File Offset: 0x00003AEC
	private void Awake()
	{
		LineRenderer[] array = this.lineRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].useWorldSpace = true;
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00005918 File Offset: 0x00003B18
	private void LateUpdate()
	{
		this.DrawLine(this.lineRenderers[0], this.joints[0], this.joints[1]);
		this.DrawLine(this.lineRenderers[1], this.joints[1], this.joints[2]);
		this.DrawLine(this.lineRenderers[2], this.joints[3], this.joints[4]);
		this.DrawLine(this.lineRenderers[3], this.joints[4], this.joints[5]);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000599D File Offset: 0x00003B9D
	private void DrawLine(LineRenderer lineRenderer, Transform start, Transform end)
	{
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, start.position);
		lineRenderer.SetPosition(1, end.position);
	}

	// Token: 0x040000AA RID: 170
	public LineRenderer[] lineRenderers;

	// Token: 0x040000AB RID: 171
	public Transform[] joints;
}
