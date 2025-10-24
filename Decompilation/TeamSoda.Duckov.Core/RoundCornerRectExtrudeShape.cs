using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class RoundCornerRectExtrudeShape : ShapeProvider
{
	// Token: 0x06000A37 RID: 2615 RVA: 0x0002BCE0 File Offset: 0x00029EE0
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		float num = this.bevelSize;
		Vector2 vector = new Vector2(-this.size.x / 2f + num, this.size.y / 2f - num);
		Vector2 v = vector + new Vector2(-1f, 1f).normalized * num;
		Vector2 v2 = vector + new Vector2(0f, 1f) * num;
		PipeRenderer.OrientedPoint orientedPoint = new PipeRenderer.OrientedPoint
		{
			position = v,
			normal = new Vector2(-1f, 1f),
			uv = Vector2.zero
		};
		PipeRenderer.OrientedPoint orientedPoint2 = new PipeRenderer.OrientedPoint
		{
			position = v2,
			normal = new Vector3(0f, 1f),
			uv = num * Vector2.one
		};
		PipeRenderer.OrientedPoint orientedPoint3 = orientedPoint2;
		orientedPoint3.position.x = -orientedPoint3.position.x;
		orientedPoint3.normal.x = -orientedPoint3.normal.x;
		orientedPoint3.uv = Vector2.one - orientedPoint3.uv;
		PipeRenderer.OrientedPoint orientedPoint4 = orientedPoint;
		orientedPoint4.position.x = -orientedPoint4.position.x;
		orientedPoint4.normal.x = -orientedPoint4.normal.x;
		orientedPoint4.uv = Vector2.one;
		PipeRenderer.OrientedPoint item = orientedPoint4;
		item.uv = Vector2.zero;
		PipeRenderer.OrientedPoint orientedPoint5 = default(PipeRenderer.OrientedPoint);
		orientedPoint5.position = vector;
		orientedPoint5.position.x = -orientedPoint5.position.x;
		orientedPoint5.position.x = orientedPoint5.position.x + num;
		orientedPoint5.normal = new Vector3(1f, 0f);
		orientedPoint5.uv = orientedPoint2.uv;
		PipeRenderer.OrientedPoint orientedPoint6 = orientedPoint5;
		orientedPoint6.position.y = -orientedPoint6.position.y;
		orientedPoint6.uv = orientedPoint3.uv;
		PipeRenderer.OrientedPoint orientedPoint7 = orientedPoint4;
		orientedPoint7.position.y = -orientedPoint7.position.y;
		orientedPoint7.normal = new Vector2(1f, -1f);
		orientedPoint7.uv = Vector2.one;
		PipeRenderer.OrientedPoint item2 = orientedPoint7;
		item2.uv = Vector2.zero;
		PipeRenderer.OrientedPoint orientedPoint8 = orientedPoint3;
		orientedPoint8.position.y = -orientedPoint8.position.y;
		orientedPoint8.normal = Vector2.down;
		orientedPoint8.uv = orientedPoint5.uv;
		PipeRenderer.OrientedPoint orientedPoint9 = orientedPoint2;
		orientedPoint9.position.y = -orientedPoint9.position.y;
		orientedPoint9.normal = Vector2.down;
		orientedPoint9.uv = orientedPoint3.uv;
		PipeRenderer.OrientedPoint orientedPoint10 = orientedPoint;
		orientedPoint10.position.y = -orientedPoint10.position.y;
		orientedPoint10.normal = new Vector2(-1f, -1f);
		orientedPoint10.uv = Vector2.one;
		PipeRenderer.OrientedPoint item3 = orientedPoint10;
		item3.uv = Vector2.zero;
		PipeRenderer.OrientedPoint orientedPoint11 = orientedPoint6;
		orientedPoint11.position.x = -orientedPoint11.position.x;
		orientedPoint11.normal = Vector2.left;
		orientedPoint11.uv = orientedPoint2.uv;
		PipeRenderer.OrientedPoint orientedPoint12 = orientedPoint5;
		orientedPoint12.position.x = -orientedPoint12.position.x;
		orientedPoint12.normal = Vector2.left;
		orientedPoint12.uv = orientedPoint3.uv;
		PipeRenderer.OrientedPoint item4 = orientedPoint12;
		item4.uv = Vector2.zero;
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		list.Add(orientedPoint);
		list.Add(orientedPoint2);
		list.Add(orientedPoint3);
		list.Add(orientedPoint4);
		list.Add(item);
		list.Add(orientedPoint5);
		list.Add(orientedPoint6);
		list.Add(orientedPoint7);
		list.Add(item2);
		list.Add(orientedPoint8);
		list.Add(orientedPoint9);
		list.Add(orientedPoint10);
		list.Add(item3);
		list.Add(orientedPoint11);
		list.Add(orientedPoint12);
		list.Add(item4);
		list.Reverse();
		if (this.resample)
		{
			list = this.Resample(list, this.stepLength);
		}
		return list.ToArray();
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0002C178 File Offset: 0x0002A378
	private List<PipeRenderer.OrientedPoint> Resample(List<PipeRenderer.OrientedPoint> original, float stepLength)
	{
		if (stepLength < 0.01f)
		{
			return new List<PipeRenderer.OrientedPoint>();
		}
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		int i = 0;
		float num = 0f;
		while (i < original.Count)
		{
			PipeRenderer.OrientedPoint orientedPoint = original[i];
			PipeRenderer.OrientedPoint orientedPoint2 = (i + 1 >= original.Count) ? original[0] : original[i + 1];
			Vector3 vector = orientedPoint2.position - orientedPoint.position;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			for (float num2 = 0f; num2 < magnitude; num2 += stepLength)
			{
				Vector3 position = orientedPoint.position + normalized * num2;
				float t = num2 / magnitude;
				num += num2;
				PipeRenderer.OrientedPoint item = new PipeRenderer.OrientedPoint
				{
					position = position,
					normal = Vector3.Lerp(orientedPoint.normal, orientedPoint2.normal, t),
					uv = num * Vector2.one
				};
				list.Add(item);
			}
			i++;
		}
		return list;
	}

	// Token: 0x040008E8 RID: 2280
	public Vector2 size = Vector2.one;

	// Token: 0x040008E9 RID: 2281
	public float bevelSize = 0.1f;

	// Token: 0x040008EA RID: 2282
	public bool resample;

	// Token: 0x040008EB RID: 2283
	[Range(0.1f, 1f)]
	public float stepLength = 0.25f;
}
