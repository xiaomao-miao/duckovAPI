using System;
using Drawing;
using Duckov.Utilities;
using SodaCraft.Localizations;
using Unity.Mathematics;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000312 RID: 786
	public class Building : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060019E8 RID: 6632 RVA: 0x0005D9B2 File Offset: 0x0005BBB2
		private int guid
		{
			get
			{
				return this.data.GUID;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x0005D9BF File Offset: 0x0005BBBF
		public int GUID
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x0005D9C7 File Offset: 0x0005BBC7
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0005D9CF File Offset: 0x0005BBCF
		public Vector2Int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0005D9D8 File Offset: 0x0005BBD8
		public Vector3 GetOffset(BuildingRotation rotation = BuildingRotation.Zero)
		{
			bool flag = rotation % BuildingRotation.Half > BuildingRotation.Zero;
			float num = (float)((flag ? this.dimensions.y : this.dimensions.x) - 1);
			float num2 = (float)((flag ? this.dimensions.x : this.dimensions.y) - 1);
			return new Vector3(num / 2f, 0f, num2 / 2f);
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x0005DA42 File Offset: 0x0005BC42
		// (set) Token: 0x060019ED RID: 6637 RVA: 0x0005DA40 File Offset: 0x0005BC40
		[LocalizationKey("Default")]
		public string DisplayNameKey
		{
			get
			{
				return "Building_" + this.ID;
			}
			set
			{
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0005DA54 File Offset: 0x0005BC54
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0005DA61 File Offset: 0x0005BC61
		public static string GetDisplayName(string id)
		{
			return ("Building_" + id).ToPlainText();
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060019F2 RID: 6642 RVA: 0x0005DA75 File Offset: 0x0005BC75
		// (set) Token: 0x060019F1 RID: 6641 RVA: 0x0005DA73 File Offset: 0x0005BC73
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Building_" + this.ID + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x0005DA8C File Offset: 0x0005BC8C
		public string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0005DA9C File Offset: 0x0005BC9C
		private void Awake()
		{
			if (this.graphicsContainer == null)
			{
				Debug.LogError("建筑" + this.DisplayName + "未配置 Graphics Container");
				Transform transform = base.transform.Find("Graphics");
				this.graphicsContainer = ((transform != null) ? transform.gameObject : null);
			}
			if (this.functionContainer == null)
			{
				Debug.LogError("建筑" + this.DisplayName + "未配置 Function Container");
				Transform transform2 = base.transform.Find("Function");
				this.functionContainer = ((transform2 != null) ? transform2.gameObject : null);
			}
			this.CreateAreaMesh();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0005DB44 File Offset: 0x0005BD44
		private void CreateAreaMesh()
		{
			if (this.areaMesh == null)
			{
				this.areaMesh = UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.BuildingBlockAreaMesh, base.transform);
				this.areaMesh.transform.localPosition = Vector3.zero;
				this.areaMesh.transform.localRotation = quaternion.identity;
				this.areaMesh.transform.localScale = new Vector3((float)this.dimensions.x - 0.02f, 1f, (float)this.dimensions.y - 0.02f);
				this.areaMesh.transform.SetParent(this.functionContainer.transform, true);
			}
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0005DC06 File Offset: 0x0005BE06
		private void RegisterEvents()
		{
			BuildingManager.OnBuildingDestroyed += this.OnBuildingDestroyed;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0005DC19 File Offset: 0x0005BE19
		private void OnBuildingDestroyed(int guid)
		{
			if (guid == this.GUID)
			{
				this.Release();
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0005DC2A File Offset: 0x0005BE2A
		private void Release()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0005DC37 File Offset: 0x0005BE37
		private void UnregisterEvents()
		{
			BuildingManager.OnBuildingDestroyed -= this.OnBuildingDestroyed;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0005DC4C File Offset: 0x0005BE4C
		public void DrawGizmos()
		{
			if (!GizmoContext.InSelection(this))
			{
				return;
			}
			using (Draw.WithColor(new Color(1f, 1f, 1f, 0.5f)))
			{
				using (Draw.InLocalSpace(base.transform))
				{
					float3 rhs = this.GetOffset(BuildingRotation.Zero);
					float2 size = new float2(0.9f, 0.9f);
					for (int i = 0; i < this.Dimensions.y; i++)
					{
						for (int j = 0; j < this.Dimensions.x; j++)
						{
							Draw.SolidPlane(new float3((float)j, 0f, (float)i) - rhs, Vector3.up, size);
						}
					}
				}
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0005DD48 File Offset: 0x0005BF48
		internal void Setup(BuildingManager.BuildingData data)
		{
			this.data = data;
			base.transform.localRotation = Quaternion.Euler(0f, (float)(data.Rotation * (BuildingRotation)90), 0f);
			this.RegisterEvents();
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0005DD7B File Offset: 0x0005BF7B
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0005DD84 File Offset: 0x0005BF84
		internal void SetupPreview()
		{
			this.functionContainer.SetActive(false);
			Collider[] componentsInChildren = this.graphicsContainer.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x040012A9 RID: 4777
		[SerializeField]
		private string id;

		// Token: 0x040012AA RID: 4778
		[SerializeField]
		private Vector2Int dimensions;

		// Token: 0x040012AB RID: 4779
		[SerializeField]
		private GameObject graphicsContainer;

		// Token: 0x040012AC RID: 4780
		[SerializeField]
		private GameObject functionContainer;

		// Token: 0x040012AD RID: 4781
		private BuildingManager.BuildingData data;

		// Token: 0x040012AE RID: 4782
		public bool unlockAchievement;

		// Token: 0x040012AF RID: 4783
		private GameObject areaMesh;
	}
}
