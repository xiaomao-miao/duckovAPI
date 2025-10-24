using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200020C RID: 524
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Profile Extrude")]
	public class SplineProfileExtrude : MonoBehaviour
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0003C4B3 File Offset: 0x0003A6B3
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0003C4BB File Offset: 0x0003A6BB
		// (set) Token: 0x06000F5B RID: 3931 RVA: 0x0003C4C3 File Offset: 0x0003A6C3
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

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0003C4CC File Offset: 0x0003A6CC
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0003C4D4 File Offset: 0x0003A6D4
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0003C4DC File Offset: 0x0003A6DC
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

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0003C4E5 File Offset: 0x0003A6E5
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0003C4ED File Offset: 0x0003A6ED
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

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0003C4FC File Offset: 0x0003A6FC
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0003C504 File Offset: 0x0003A704
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

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0003C517 File Offset: 0x0003A717
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0003C51F File Offset: 0x0003A71F
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

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0003C532 File Offset: 0x0003A732
		public int ProfileSeg
		{
			get
			{
				return this.profile.Length;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0003C53C File Offset: 0x0003A73C
		// (set) Token: 0x06000F67 RID: 3943 RVA: 0x0003C544 File Offset: 0x0003A744
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

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0003C54D File Offset: 0x0003A74D
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x0003C555 File Offset: 0x0003A755
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

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0003C584 File Offset: 0x0003A784
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

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0003C597 File Offset: 0x0003A797
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

		// Token: 0x06000F6C RID: 3948 RVA: 0x0003C5AC File Offset: 0x0003A7AC
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

		// Token: 0x06000F6D RID: 3949 RVA: 0x0003C624 File Offset: 0x0003A824
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

		// Token: 0x06000F6E RID: 3950 RVA: 0x0003C683 File Offset: 0x0003A883
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0003C696 File Offset: 0x0003A896
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0003C6A9 File Offset: 0x0003A8A9
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0003C6D6 File Offset: 0x0003A8D6
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0003C6F4 File Offset: 0x0003A8F4
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.profile, this.m_Mesh, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0003C768 File Offset: 0x0003A968
		private void Extrude<T>(T spline, SplineProfileExtrude.Vertex[] profile, Mesh mesh, float segmentsPerUnit, float2 range) where T : ISpline
		{
			int num = profile.Length;
			if (num < 2)
			{
				return;
			}
			float num2 = Mathf.Abs(range.y - range.x);
			int num3 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num2 * segmentsPerUnit), 1);
			float num4 = 0f;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			Vector3 b = Vector3.zero;
			for (int i = 0; i < num3; i++)
			{
				float num5 = math.lerp(range.x, range.y, (float)i / ((float)num3 - 1f));
				if (num5 > 1f)
				{
					num5 = 1f;
				}
				if (num5 < 1E-07f)
				{
					num5 = 1E-07f;
				}
				float3 v;
				float3 v2;
				float3 v3;
				spline.Evaluate(num5, out v, out v2, out v3);
				Vector3 normalized = v2.normalized;
				Vector3 normalized2 = v3.normalized;
				Vector3 a = Vector3.Cross(normalized, normalized2);
				float num6 = 1f / (float)(num - 1);
				if (i > 0)
				{
					num4 += (v - b).magnitude;
				}
				for (int j = 0; j < num; j++)
				{
					SplineProfileExtrude.Vertex vertex = profile[j];
					float u = vertex.u;
					float y = vertex.position.y;
					float x = vertex.position.x;
					float z = vertex.position.z;
					Vector3 item = Quaternion.FromToRotation(Vector3.up, normalized2) * vertex.normal;
					Vector3 item2 = v + x * a + y * normalized2 + z * normalized;
					list.Add(item2);
					list3.Add(new Vector2(u * this.uFactor, num4 * this.vFactor));
					list2.Add(item);
				}
				b = v;
			}
			SplineProfileExtrude.<>c__DisplayClass53_0<T> CS$<>8__locals1;
			CS$<>8__locals1.triangles = new List<int>();
			for (int k = 0; k < num3 - 1; k++)
			{
				int num7 = k * num;
				for (int l = 0; l < num - 1; l++)
				{
					int num8 = num7 + l;
					SplineProfileExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num8,
						num8 + 1,
						num8 + num
					}, ref CS$<>8__locals1);
					SplineProfileExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num8 + 1,
						num8 + 1 + num,
						num8 + num
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

		// Token: 0x06000F74 RID: 3956 RVA: 0x0003CA2D File Offset: 0x0003AC2D
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0003CA35 File Offset: 0x0003AC35
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0003CA48 File Offset: 0x0003AC48
		private void FlattenSpline()
		{
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0003CABA File Offset: 0x0003ACBA
		[CompilerGenerated]
		internal static void <Extrude>g__AddTriangles|53_0<T>(int[] indicies, ref SplineProfileExtrude.<>c__DisplayClass53_0<T> A_1) where T : ISpline
		{
			A_1.triangles.AddRange(indicies);
		}

		// Token: 0x04000C74 RID: 3188
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000C75 RID: 3189
		[SerializeField]
		private SplineProfileExtrude.Vertex[] profile;

		// Token: 0x04000C76 RID: 3190
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000C77 RID: 3191
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000C78 RID: 3192
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000C79 RID: 3193
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000C7A RID: 3194
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000C7B RID: 3195
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000C7C RID: 3196
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 1f);

		// Token: 0x04000C7D RID: 3197
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000C7E RID: 3198
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000C7F RID: 3199
		private Mesh m_Mesh;

		// Token: 0x04000C80 RID: 3200
		private bool m_RebuildRequested;

		// Token: 0x04000C81 RID: 3201
		private float m_NextScheduledRebuild;

		// Token: 0x020004E9 RID: 1257
		[Serializable]
		private struct Vertex
		{
			// Token: 0x04001D49 RID: 7497
			public Vector3 position;

			// Token: 0x04001D4A RID: 7498
			public Vector3 normal;

			// Token: 0x04001D4B RID: 7499
			public float u;
		}
	}
}
