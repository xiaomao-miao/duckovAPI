using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200020B RID: 523
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Flat Extrude")]
	public class SplineFlatExtrude : MonoBehaviour
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0003BECE File Offset: 0x0003A0CE
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0003BED6 File Offset: 0x0003A0D6
		// (set) Token: 0x06000F3A RID: 3898 RVA: 0x0003BEDE File Offset: 0x0003A0DE
		public SplineContainer Container
		{
			get
			{
				return this.m_Container;
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0003BEE7 File Offset: 0x0003A0E7
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0003BEEF File Offset: 0x0003A0EF
		// (set) Token: 0x06000F3D RID: 3901 RVA: 0x0003BEF7 File Offset: 0x0003A0F7
		public bool RebuildOnSplineChange
		{
			get
			{
				return this.m_RebuildOnSplineChange;
			}
			set
			{
				this.m_RebuildOnSplineChange = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0003BF00 File Offset: 0x0003A100
		// (set) Token: 0x06000F3F RID: 3903 RVA: 0x0003BF08 File Offset: 0x0003A108
		public int RebuildFrequency
		{
			get
			{
				return this.m_RebuildFrequency;
			}
			set
			{
				this.m_RebuildFrequency = Mathf.Max(value, 1);
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0003BF17 File Offset: 0x0003A117
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x0003BF1F File Offset: 0x0003A11F
		public float SegmentsPerUnit
		{
			get
			{
				return this.m_SegmentsPerUnit;
			}
			set
			{
				this.m_SegmentsPerUnit = Mathf.Max(value, 0.0001f);
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0003BF32 File Offset: 0x0003A132
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0003BF3A File Offset: 0x0003A13A
		public float Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = Mathf.Max(value, 1E-05f);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0003BF4D File Offset: 0x0003A14D
		// (set) Token: 0x06000F45 RID: 3909 RVA: 0x0003BF55 File Offset: 0x0003A155
		public int ProfileSeg
		{
			get
			{
				return this.m_ProfileSeg;
			}
			set
			{
				this.m_ProfileSeg = value;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0003BF5E File Offset: 0x0003A15E
		// (set) Token: 0x06000F47 RID: 3911 RVA: 0x0003BF66 File Offset: 0x0003A166
		public float Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0003BF6F File Offset: 0x0003A16F
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x0003BF77 File Offset: 0x0003A177
		public Vector2 Range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = new Vector2(Mathf.Min(value.x, value.y), Mathf.Max(value.x, value.y));
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0003BFA6 File Offset: 0x0003A1A6
		public Spline Spline
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Spline;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0003BFB9 File Offset: 0x0003A1B9
		public IReadOnlyList<Spline> Splines
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Splines;
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0003BFCC File Offset: 0x0003A1CC
		internal void Reset()
		{
			base.TryGetComponent<SplineContainer>(out this.m_Container);
			MeshFilter meshFilter;
			if (base.TryGetComponent<MeshFilter>(out meshFilter))
			{
				meshFilter.sharedMesh = (this.m_Mesh = this.CreateMeshAsset());
			}
			MeshRenderer meshRenderer;
			if (base.TryGetComponent<MeshRenderer>(out meshRenderer) && meshRenderer.sharedMaterial == null)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Material sharedMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
				Object.DestroyImmediate(gameObject);
				meshRenderer.sharedMaterial = sharedMaterial;
			}
			this.Rebuild();
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0003C044 File Offset: 0x0003A244
		private void Start()
		{
			if (this.m_Container == null || this.m_Container.Spline == null)
			{
				Debug.LogError("Spline Extrude does not have a valid SplineContainer set.");
				return;
			}
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				Debug.LogError("SplineExtrude.createMeshInstance is disabled, but there is no valid mesh assigned. Please create or assign a writable mesh asset.");
			}
			this.Rebuild();
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0003C0A3 File Offset: 0x0003A2A3
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0003C0B6 File Offset: 0x0003A2B6
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0003C0C9 File Offset: 0x0003A2C9
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0003C0F6 File Offset: 0x0003A2F6
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0003C114 File Offset: 0x0003A314
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.m_Mesh, this.m_Width, this.m_ProfileSeg, this.m_Height, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0003C194 File Offset: 0x0003A394
		private void Extrude<T>(T spline, Mesh mesh, float width, int profileSegments, float height, float segmentsPerUnit, float2 range) where T : ISpline
		{
			if (profileSegments < 2)
			{
				return;
			}
			float num = Mathf.Abs(range.y - range.x);
			int num2 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num * segmentsPerUnit), 1);
			float num3 = 0f;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			Vector3 b = Vector3.zero;
			for (int i = 0; i < num2; i++)
			{
				float num4 = math.lerp(range.x, range.y, (float)i / ((float)num2 - 1f));
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				float3 v;
				float3 v2;
				float3 v3;
				spline.Evaluate(num4, out v, out v2, out v3);
				Vector3 normalized = v2.normalized;
				Vector3 normalized2 = v3.normalized;
				Vector3 a = Vector3.Cross(normalized, normalized2);
				float num5 = 1f / (float)(profileSegments - 1);
				if (i > 0)
				{
					num3 += (v - b).magnitude;
				}
				for (int j = 0; j < profileSegments; j++)
				{
					float num6 = num5 * (float)j;
					float num7 = (num6 - 0.5f) * 2f;
					float d = Mathf.Cos(num7 * 3.1415927f * 0.5f) * height;
					float d2 = num7 * width;
					Vector3 item = v + d2 * a + d * normalized2;
					list.Add(item);
					list3.Add(new Vector2(num6 * this.uFactor, num3 * this.vFactor));
					list2.Add(normalized2);
				}
				b = v;
			}
			SplineFlatExtrude.<>c__DisplayClass53_0<T> CS$<>8__locals1;
			CS$<>8__locals1.triangles = new List<int>();
			for (int k = 0; k < num2 - 1; k++)
			{
				int num8 = k * profileSegments;
				for (int l = 0; l < profileSegments - 1; l++)
				{
					int num9 = num8 + l;
					SplineFlatExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num9,
						num9 + 1,
						num9 + profileSegments
					}, ref CS$<>8__locals1);
					SplineFlatExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num9 + 1,
						num9 + 1 + profileSegments,
						num9 + profileSegments
					}, ref CS$<>8__locals1);
				}
			}
			mesh.Clear();
			mesh.vertices = list.ToArray();
			mesh.uv = list3.ToArray();
			mesh.triangles = CS$<>8__locals1.triangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003C412 File Offset: 0x0003A612
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0003C41A File Offset: 0x0003A61A
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0003C42D File Offset: 0x0003A62D
		private void FlattenSpline()
		{
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0003C4A5 File Offset: 0x0003A6A5
		[CompilerGenerated]
		internal static void <Extrude>g__AddTriangles|53_0<T>(int[] indicies, ref SplineFlatExtrude.<>c__DisplayClass53_0<T> A_1) where T : ISpline
		{
			A_1.triangles.AddRange(indicies);
		}

		// Token: 0x04000C66 RID: 3174
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000C67 RID: 3175
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000C68 RID: 3176
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000C69 RID: 3177
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000C6A RID: 3178
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000C6B RID: 3179
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000C6C RID: 3180
		[SerializeField]
		private int m_ProfileSeg = 2;

		// Token: 0x04000C6D RID: 3181
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000C6E RID: 3182
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 0.999f);

		// Token: 0x04000C6F RID: 3183
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000C70 RID: 3184
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000C71 RID: 3185
		private Mesh m_Mesh;

		// Token: 0x04000C72 RID: 3186
		private bool m_RebuildRequested;

		// Token: 0x04000C73 RID: 3187
		private float m_NextScheduledRebuild;
	}
}
