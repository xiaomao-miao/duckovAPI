using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class RoundCornerSquareExtrudeShape : ShapeProvider
{
	// Token: 0x06000A3A RID: 2618 RVA: 0x0002C2B0 File Offset: 0x0002A4B0
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		float num = this.size;
		float num2 = this.bevelSize;
		Vector2 a = new Vector2(-num / 2f + num2, num / 2f - num2);
		Vector2 v = a + new Vector2(-1f, 1f).normalized * num2;
		Vector2 v2 = a + new Vector2(0f, 1f) * num2;
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
			uv = num2 * Vector2.one
		};
		PipeRenderer.OrientedPoint orientedPoint3 = orientedPoint2;
		orientedPoint3.position.x = -orientedPoint3.position.x;
		orientedPoint3.normal.x = -orientedPoint3.normal.x;
		orientedPoint3.uv = Vector2.one - orientedPoint3.uv;
		PipeRenderer.OrientedPoint orientedPoint4 = orientedPoint;
		orientedPoint4.position.x = -orientedPoint4.position.x;
		orientedPoint4.normal.x = -orientedPoint4.normal.x;
		orientedPoint4.uv = Vector2.one;
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		list.Add(orientedPoint);
		list.Add(orientedPoint2);
		list.Add(orientedPoint3);
		list.Add(orientedPoint4);
		for (int i = 1; i <= 3; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint5 = orientedPoint;
			PipeRenderer.OrientedPoint orientedPoint6 = orientedPoint2;
			PipeRenderer.OrientedPoint orientedPoint7 = orientedPoint3;
			PipeRenderer.OrientedPoint orientedPoint8 = orientedPoint4;
			for (int j = 0; j < i; j++)
			{
				orientedPoint5 = RoundCornerSquareExtrudeShape.<GenerateShape>g__Rotate90|2_0(orientedPoint5);
				orientedPoint6 = RoundCornerSquareExtrudeShape.<GenerateShape>g__Rotate90|2_0(orientedPoint6);
				orientedPoint7 = RoundCornerSquareExtrudeShape.<GenerateShape>g__Rotate90|2_0(orientedPoint7);
				orientedPoint8 = RoundCornerSquareExtrudeShape.<GenerateShape>g__Rotate90|2_0(orientedPoint8);
			}
			orientedPoint5.uv += (float)i * Vector2.one;
			orientedPoint6.uv += (float)i * Vector2.one;
			orientedPoint7.uv += (float)i * Vector2.one;
			orientedPoint8.uv += (float)i * Vector2.one;
			list.Add(orientedPoint5);
			list.Add(orientedPoint6);
			list.Add(orientedPoint7);
			list.Add(orientedPoint8);
		}
		list.Reverse();
		return list.ToArray();
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0002C59C File Offset: 0x0002A79C
	[CompilerGenerated]
	internal static PipeRenderer.OrientedPoint <GenerateShape>g__Rotate90|2_0(PipeRenderer.OrientedPoint original)
	{
		Quaternion rotation = Quaternion.AngleAxis(-90f, Vector3.forward);
		PipeRenderer.OrientedPoint result = original;
		result.position = rotation * original.position;
		result.normal = rotation * original.normal;
		return result;
	}

	// Token: 0x040008EC RID: 2284
	public float size = 1f;

	// Token: 0x040008ED RID: 2285
	public float bevelSize = 0.1f;
}
