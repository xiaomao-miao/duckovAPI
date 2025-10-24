using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
[RequireComponent(typeof(PipeRenderer))]
public class CircularExtrudeShape : ShapeProvider
{
	// Token: 0x06000A32 RID: 2610 RVA: 0x0002BA00 File Offset: 0x00029C00
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		Vector3 vector = Vector3.up * this.radius;
		float num = 360f / (float)this.subdivision;
		float num2 = 1f / (float)this.subdivision;
		PipeRenderer.OrientedPoint[] array = new PipeRenderer.OrientedPoint[this.subdivision + 1];
		for (int i = 0; i < this.subdivision; i++)
		{
			Quaternion rotation = Quaternion.AngleAxis(num * (float)i, Vector3.forward);
			Vector3 position = rotation * vector + this.offset;
			array[i] = new PipeRenderer.OrientedPoint
			{
				position = position,
				rotation = rotation,
				uv = num2 * (float)i * Vector2.one
			};
		}
		array[this.subdivision] = new PipeRenderer.OrientedPoint
		{
			position = vector + this.offset,
			rotation = Quaternion.AngleAxis(0f, Vector3.forward),
			uv = Vector2.one
		};
		return array;
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0002BB0C File Offset: 0x00029D0C
	private void OnDrawGizmosSelected()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
		if (this.pipeRenderer != null && this.pipeRenderer.extrudeShapeProvider == null)
		{
			this.pipeRenderer.extrudeShapeProvider = this;
		}
	}

	// Token: 0x040008E2 RID: 2274
	public PipeRenderer pipeRenderer;

	// Token: 0x040008E3 RID: 2275
	public float radius = 1f;

	// Token: 0x040008E4 RID: 2276
	public int subdivision = 12;

	// Token: 0x040008E5 RID: 2277
	public Vector3 offset = Vector3.zero;
}
