using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class MultipleBezierShape : ShapeProvider
{
	// Token: 0x06000A4C RID: 2636 RVA: 0x0002D3FC File Offset: 0x0002B5FC
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		for (int i = 0; i < this.points.Length / 4; i++)
		{
			Vector3 p = this.points[i * 4];
			Vector3 p2 = this.points[i * 4 + 1];
			Vector3 p3 = this.points[i * 4 + 2];
			Vector3 p4 = this.points[i * 4 + 3];
			PipeRenderer.OrientedPoint[] collection = BezierSpline.GenerateShape(p, p2, p3, p4, this.subdivisions);
			if (list.Count > 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			list.AddRange(collection);
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		PipeHelperFunctions.RecalculateUvs(ref result, 1f, 0f);
		PipeHelperFunctions.RotatePoints(ref result, this.rotationOffset, this.twist);
		return result;
	}

	// Token: 0x04000900 RID: 2304
	public Vector3[] points;

	// Token: 0x04000901 RID: 2305
	public int subdivisions = 16;

	// Token: 0x04000902 RID: 2306
	public bool lockedHandles;

	// Token: 0x04000903 RID: 2307
	public float rotationOffset;

	// Token: 0x04000904 RID: 2308
	public float twist;

	// Token: 0x04000905 RID: 2309
	public bool edit;
}
