using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000147 RID: 327
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PipeRenderer : MonoBehaviour
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0002C5E4 File Offset: 0x0002A7E4
	public PipeRenderer.OrientedPoint[] extrudeShape
	{
		get
		{
			if (this.extrudeShapeProvider == null)
			{
				Debug.LogWarning("Extrude shape is null, please add an extrude shape such as \"CircularExtrudeShape\"");
				return new PipeRenderer.OrientedPoint[]
				{
					new PipeRenderer.OrientedPoint
					{
						position = Vector3.zero
					}
				};
			}
			return this.extrudeShapeProvider.GenerateShape();
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002C638 File Offset: 0x0002A838
	public PipeRenderer.OrientedPoint[] splineShape
	{
		get
		{
			if (this.splineShapeProvider == null)
			{
				Debug.LogWarning("Spline shape is null, please add a spline shape such as \"Beveled Line Shape\"");
				return new PipeRenderer.OrientedPoint[]
				{
					new PipeRenderer.OrientedPoint
					{
						position = Vector3.zero
					}
				};
			}
			return this.splineShapeProvider.GenerateShape();
		}
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0002C68B File Offset: 0x0002A88B
	public float GetTotalLength()
	{
		return PipeHelperFunctions.GetTotalLength(this.splineInUse);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0002C698 File Offset: 0x0002A898
	public static Mesh GeneratePipeMesh(PipeRenderer.OrientedPoint[] extrudeShape, PipeRenderer.OrientedPoint[] splineShape, Color vertexColor, float uvTwist = 0f, float extrudeShapeScale = 1f, AnimationCurve extrudeShapeScaleCurve = null, float sectionLength = 0f, bool caps = false, bool recalculateNormal = true, bool revertFaces = false)
	{
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<int> list3 = new List<int>();
		List<Vector2> list4 = new List<Vector2>();
		float num = 0f;
		float totalLength = PipeHelperFunctions.GetTotalLength(splineShape);
		if (sectionLength <= 0f)
		{
			sectionLength = totalLength;
		}
		for (int i = 0; i < splineShape.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = splineShape[i];
			Vector3 position = orientedPoint.position;
			Quaternion rotation = orientedPoint.rotation;
			if (i > 0)
			{
				PipeRenderer.OrientedPoint orientedPoint2 = splineShape[i - 1];
				num += (position - orientedPoint2.position).magnitude;
			}
			float time = num % sectionLength / sectionLength;
			float num2 = (extrudeShapeScaleCurve != null) ? extrudeShapeScaleCurve.Evaluate(time) : 1f;
			foreach (PipeRenderer.OrientedPoint orientedPoint3 in extrudeShape)
			{
				Vector3 vector = position + extrudeShapeScale * num2 * (rotation * orientedPoint3.position);
				Vector3 vector2 = recalculateNormal ? (vector - position).normalized : (rotation * orientedPoint3.normal);
				Vector2 item = new Vector2(orientedPoint3.uv.y + num * uvTwist, orientedPoint.uv.x);
				list.Add(vector);
				list2.Add(revertFaces ? (-vector2) : vector2);
				list4.Add(item);
			}
			if (i > 0)
			{
				int num3 = i * extrudeShape.Length;
				for (int k = 0; k < extrudeShape.Length - 1; k++)
				{
					int num4 = num3 + k;
					int num5 = num3 + k + 1;
					int item2 = num5 - extrudeShape.Length;
					int item3 = num4 - extrudeShape.Length;
					if (revertFaces)
					{
						list3.Add(num5);
						list3.Add(item3);
						list3.Add(num4);
						list3.Add(item2);
						list3.Add(item3);
						list3.Add(num5);
					}
					else
					{
						list3.Add(num4);
						list3.Add(item3);
						list3.Add(num5);
						list3.Add(num5);
						list3.Add(item3);
						list3.Add(item2);
					}
				}
			}
		}
		if (caps)
		{
			Vector3 item4 = -splineShape[0].tangent;
			int num6 = 0;
			int[] array = new int[extrudeShape.Length];
			for (int l = 0; l < extrudeShape.Length; l++)
			{
				int index = num6 + l;
				list.Add(list[index]);
				list2.Add(item4);
				list4.Add(list4[index]);
				array[l] = list.Count - 1;
			}
			Vector3 vector3 = Vector3.zero;
			for (int m = 0; m < array.Length; m++)
			{
				vector3 += list[m];
			}
			vector3 /= (float)array.Length;
			list.Add(vector3);
			list2.Add(item4);
			list4.Add(Vector2.one * 0f / 5f);
			int item5 = list.Count - 1;
			for (int n = 0; n < array.Length - 1; n++)
			{
				list3.Add(item5);
				list3.Add(array[n + 1]);
				list3.Add(array[n]);
			}
			item4 = splineShape[splineShape.Length - 1].tangent;
			num6 = extrudeShape.Length * (splineShape.Length - 1);
			for (int num7 = 0; num7 < extrudeShape.Length; num7++)
			{
				int index2 = num6 + num7;
				list.Add(list[index2]);
				list2.Add(item4);
				list4.Add(list4[index2]);
				array[num7] = list.Count - 1;
			}
			vector3 = Vector3.zero;
			for (int num8 = 0; num8 < array.Length; num8++)
			{
				vector3 += list[array[num8]];
			}
			vector3 /= (float)array.Length;
			list.Add(vector3);
			list2.Add(item4);
			list4.Add(Vector2.one * 0f / 5f);
			item5 = list.Count - 1;
			for (int num9 = 0; num9 < array.Length - 1; num9++)
			{
				list3.Add(array[num9]);
				list3.Add(array[num9 + 1]);
				list3.Add(item5);
			}
		}
		Color[] array2 = new Color[list.Count];
		for (int num10 = 0; num10 < array2.Length; num10++)
		{
			array2[num10] = vertexColor;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = list.ToArray();
		mesh.uv = list4.ToArray();
		mesh.triangles = list3.ToArray();
		mesh.normals = list2.ToArray();
		mesh.RecalculateTangents();
		mesh.colors = array2;
		mesh.name = "Generated Mesh";
		return mesh;
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0002CB60 File Offset: 0x0002AD60
	private void OnDrawGizmosSelected()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		this.splineInUse = this.splineShape;
		this.meshFilter.mesh = PipeRenderer.GeneratePipeMesh(this.extrudeShape, this.splineInUse, this.vertexColor, this.uvTwist, this.extrudeShapeScale, this.useExtrudeShapeScaleCurve ? this.extrudeShapeScaleCurve : null, this.sectionLength, this.caps, this.recalculateNormal, this.revertFaces);
		if (this.drawSplinePoints)
		{
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < this.splineInUse.Length; i++)
			{
				PipeRenderer.OrientedPoint orientedPoint = this.splineInUse[i];
				Vector3 vector = localToWorldMatrix.MultiplyPoint(orientedPoint.position);
				Gizmos.DrawWireCube(vector, Vector3.one * 0.01f);
				Vector3 a = localToWorldMatrix.MultiplyVector(orientedPoint.tangent);
				Gizmos.DrawLine(vector, vector + a * 0.02f);
			}
		}
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x0002CC64 File Offset: 0x0002AE64
	private void Start()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0002CC80 File Offset: 0x0002AE80
	public Vector3 GetPositionByOffset(float offset, out Quaternion rotation)
	{
		if (this.splineInUse == null || this.splineInUse.Length < 1)
		{
			rotation = Quaternion.identity;
			return Vector3.zero;
		}
		if (offset <= 0f)
		{
			rotation = this.splineInUse[0].rotation;
			return this.splineInUse[0].position;
		}
		float num = 0f;
		for (int i = 1; i < this.splineInUse.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = this.splineInUse[i];
			PipeRenderer.OrientedPoint orientedPoint2 = this.splineInUse[i - 1];
			float num2 = num;
			float magnitude = (orientedPoint.position - orientedPoint2.position).magnitude;
			num += magnitude;
			float num3 = num;
			if (num3 > offset)
			{
				float num4 = num3 - num2;
				float num5 = (offset - num2) / num4;
				rotation = Quaternion.Lerp(orientedPoint2.rotation, orientedPoint.rotation, num5);
				return orientedPoint2.position + (orientedPoint.position - orientedPoint2.position) * num5;
			}
		}
		rotation = this.splineInUse[this.splineInUse.Length - 1].rotation;
		return this.splineInUse[this.splineInUse.Length - 1].position;
	}

	// Token: 0x040008EE RID: 2286
	public MeshFilter meshFilter;

	// Token: 0x040008EF RID: 2287
	public ShapeProvider splineShapeProvider;

	// Token: 0x040008F0 RID: 2288
	public ShapeProvider extrudeShapeProvider;

	// Token: 0x040008F1 RID: 2289
	[Header("UV")]
	public float uvTwist;

	// Token: 0x040008F2 RID: 2290
	[Header("Options")]
	public float extrudeShapeScale = 1f;

	// Token: 0x040008F3 RID: 2291
	public float sectionLength = 10f;

	// Token: 0x040008F4 RID: 2292
	public bool caps;

	// Token: 0x040008F5 RID: 2293
	public bool useExtrudeShapeScaleCurve;

	// Token: 0x040008F6 RID: 2294
	public AnimationCurve extrudeShapeScaleCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x040008F7 RID: 2295
	public Color vertexColor = Color.white;

	// Token: 0x040008F8 RID: 2296
	public bool recalculateNormal = true;

	// Token: 0x040008F9 RID: 2297
	public bool revertFaces;

	// Token: 0x040008FA RID: 2298
	[Header("Gizmos")]
	public bool drawSplinePoints;

	// Token: 0x040008FB RID: 2299
	public PipeRenderer.OrientedPoint[] splineInUse;

	// Token: 0x020004A5 RID: 1189
	[Serializable]
	public struct OrientedPoint
	{
		// Token: 0x04001C2D RID: 7213
		public Vector3 position;

		// Token: 0x04001C2E RID: 7214
		public Quaternion rotation;

		// Token: 0x04001C2F RID: 7215
		public Vector3 tangent;

		// Token: 0x04001C30 RID: 7216
		public Vector3 rotationalAxisVector;

		// Token: 0x04001C31 RID: 7217
		public Vector3 normal;

		// Token: 0x04001C32 RID: 7218
		public Vector2 uv;
	}
}
