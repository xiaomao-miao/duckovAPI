using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class MultiCirceExtrudeShape : ShapeProvider
{
	// Token: 0x06000A35 RID: 2613 RVA: 0x0002BB88 File Offset: 0x00029D88
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 360f / (float)this.subdivision;
		float num2 = 1f / (float)(this.subdivision * this.circles.Length);
		for (int i = 0; i < this.circles.Length; i++)
		{
			MultiCirceExtrudeShape.Circle circle = this.circles[i];
			float radius = circle.radius;
			Vector3 origin = circle.origin;
			Vector3 vector = Vector3.up * radius;
			for (int j = 0; j < this.subdivision; j++)
			{
				Quaternion rotation = Quaternion.AngleAxis(num * (float)j, Vector3.forward);
				Vector3 position = origin + rotation * vector;
				list.Add(new PipeRenderer.OrientedPoint
				{
					position = position,
					rotation = rotation,
					uv = num2 * (float)(i * this.subdivision + j) * Vector2.one
				});
			}
			list.Add(new PipeRenderer.OrientedPoint
			{
				position = vector,
				rotation = Quaternion.AngleAxis(0f, Vector3.forward),
				uv = num2 * (float)((i + 1) * this.subdivision) * Vector2.one
			});
		}
		return list.ToArray();
	}

	// Token: 0x040008E6 RID: 2278
	public MultiCirceExtrudeShape.Circle[] circles;

	// Token: 0x040008E7 RID: 2279
	public int subdivision = 4;

	// Token: 0x020004A4 RID: 1188
	[Serializable]
	public struct Circle
	{
		// Token: 0x04001C2B RID: 7211
		public Vector3 origin;

		// Token: 0x04001C2C RID: 7212
		public float radius;
	}
}
