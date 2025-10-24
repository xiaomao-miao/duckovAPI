using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class BezierSpline : ShapeProvider
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x0002CE28 File Offset: 0x0002B028
	public static Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		float d = Mathf.Pow(1f - t, 3f);
		float d2 = 3f * t * (1f - t) * (1f - t);
		float d3 = 3f * t * t * (1f - t);
		float d4 = t * t * t;
		return d * p1 + d2 * p2 + d3 * p3 + d4 * p4;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x0002CEAC File Offset: 0x0002B0AC
	public static Vector3 GetTangent(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		float d = -3f * (1f - t) * (1f - t);
		float d2 = 3f * (1f - t) * (1f - t) - 6f * t * (1f - t);
		float d3 = 6f * t * (1f - t) - 3f * t * t;
		float d4 = 3f * t * t;
		return d * p1 + d2 * p2 + d3 * p3 + d4 * p4;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0002CF50 File Offset: 0x0002B150
	public static Vector3 GetNormal(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		Vector3 tangent = BezierSpline.GetTangent(p1, p2, p3, p4, t);
		Vector3 b = 6f * (1f - t) * p1 - (6f * (1f - t) + (6f - 12f * t)) * p2 + (6f - 12f * t - 6f * t) * p3 + 6f * t * p4;
		Vector3 normalized = tangent.normalized;
		Vector3 normalized2 = (normalized + b).normalized;
		return Vector3.Cross(Vector3.Cross(normalized, normalized2), normalized).normalized;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x0002D00C File Offset: 0x0002B20C
	public static PipeRenderer.OrientedPoint[] GenerateShape(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int subdivisions)
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 1f / (float)subdivisions;
		float num2 = 0f;
		Vector3 b = Vector3.zero;
		for (int i = 0; i <= subdivisions; i++)
		{
			float t = (float)i * num;
			Vector3 point = BezierSpline.GetPoint(p0, p1, p2, p3, t);
			Vector3 tangent = BezierSpline.GetTangent(p0, p1, p2, p3, t);
			Vector3 normal = BezierSpline.GetNormal(p0, p1, p2, p3, t);
			if (i > 0)
			{
				num2 += (point - b).magnitude;
			}
			Quaternion rotation = Quaternion.identity;
			rotation = Quaternion.LookRotation(tangent, normal);
			PipeRenderer.OrientedPoint item = new PipeRenderer.OrientedPoint
			{
				position = point,
				tangent = tangent,
				normal = normal,
				rotation = rotation,
				rotationalAxisVector = Vector3.forward,
				uv = Vector2.one * num2
			};
			list.Add(item);
			b = point;
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		return result;
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x0002D110 File Offset: 0x0002B310
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 1f / (float)this.subdivisions;
		Vector3 p = this.points[0];
		Vector3 p2 = this.points[1];
		Vector3 p3 = this.points[2];
		Vector3 p4 = this.points[3];
		float num2 = 0f;
		Vector3 b = Vector3.zero;
		for (int i = 0; i <= this.subdivisions; i++)
		{
			float t = (float)i * num;
			Vector3 point = BezierSpline.GetPoint(p, p2, p3, p4, t);
			Vector3 tangent = BezierSpline.GetTangent(p, p2, p3, p4, t);
			Vector3 normal = BezierSpline.GetNormal(p, p2, p3, p4, t);
			if (i > 0)
			{
				num2 += (point - b).magnitude;
			}
			Quaternion rotation = Quaternion.identity;
			rotation = Quaternion.LookRotation(tangent, normal);
			PipeRenderer.OrientedPoint item = new PipeRenderer.OrientedPoint
			{
				position = point,
				tangent = tangent,
				normal = normal,
				rotation = rotation,
				rotationalAxisVector = Vector3.forward,
				uv = Vector2.one * num2
			};
			list.Add(item);
			b = point;
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		return result;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0002D260 File Offset: 0x0002B460
	private void OnDrawGizmosSelected()
	{
		if (this.drawGizmos)
		{
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < this.points.Length; i++)
			{
				Gizmos.DrawWireCube(localToWorldMatrix.MultiplyPoint(this.points[i]), Vector3.one * 0.1f);
			}
			float num = 1f / (float)this.subdivisions;
			for (int j = 0; j < this.subdivisions; j++)
			{
				Vector3 vector = BezierSpline.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], num * (float)j);
				Vector3 vector2 = BezierSpline.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], num * (float)(j + 1));
				vector = localToWorldMatrix.MultiplyPoint(vector);
				vector2 = localToWorldMatrix.MultiplyPoint(vector2);
				Gizmos.DrawLine(vector, vector2);
				Vector3 vector3 = BezierSpline.GetTangent(this.points[0], this.points[1], this.points[2], this.points[3], num * (float)j);
				vector3 = localToWorldMatrix.MultiplyVector(vector3);
				Vector3 to = vector + vector3 * 0.1f;
				Gizmos.DrawLine(vector, to);
			}
		}
	}

	// Token: 0x040008FC RID: 2300
	public PipeRenderer pipeRenderer;

	// Token: 0x040008FD RID: 2301
	public Vector3[] points = new Vector3[4];

	// Token: 0x040008FE RID: 2302
	public int subdivisions = 12;

	// Token: 0x040008FF RID: 2303
	public bool drawGizmos;
}
