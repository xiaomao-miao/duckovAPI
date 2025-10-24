using System;
using System.Collections.Generic;
using Drawing;
using Duckov.Achievements;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000311 RID: 785
	public class BuildingArea : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0005D30E File Offset: 0x0005B50E
		public string AreaID
		{
			get
			{
				return this.areaID;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x0005D316 File Offset: 0x0005B516
		public Vector2Int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x0005D31E File Offset: 0x0005B51E
		public Vector2Int LowerLeftCorner
		{
			get
			{
				return this.CenterCoord - (this.size - Vector2Int.one);
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x0005D33B File Offset: 0x0005B53B
		private Vector2Int CenterCoord
		{
			get
			{
				return new Vector2Int(Mathf.RoundToInt(base.transform.position.x), Mathf.RoundToInt(base.transform.position.z));
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x0005D36C File Offset: 0x0005B56C
		private int Width
		{
			get
			{
				return this.size.x;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x0005D379 File Offset: 0x0005B579
		private int Height
		{
			get
			{
				return this.size.y;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x0005D386 File Offset: 0x0005B586
		public BuildingManager.BuildingAreaData AreaData
		{
			get
			{
				return BuildingManager.GetOrCreateAreaData(this.AreaID);
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x0005D393 File Offset: 0x0005B593
		public Plane Plane
		{
			get
			{
				return new Plane(base.transform.up, base.transform.position);
			}
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0005D3B0 File Offset: 0x0005B5B0
		private void Awake()
		{
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0005D3C3 File Offset: 0x0005B5C3
		private void OnDestroy()
		{
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0005D3D8 File Offset: 0x0005B5D8
		private void OnBuildingBuilt(int guid)
		{
			BuildingManager.BuildingData buildingData = BuildingManager.GetBuildingData(guid, null);
			if (buildingData == null)
			{
				return;
			}
			this.Display(buildingData);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0005D3F8 File Offset: 0x0005B5F8
		private void Start()
		{
			this.RepaintAll();
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0005D400 File Offset: 0x0005B600
		public void DrawGizmos()
		{
			if (!GizmoContext.InSelection(this))
			{
				return;
			}
			int num = this.CenterCoord.x - (this.size.x - 1);
			int num2 = this.CenterCoord.x + (this.size.x - 1) + 1;
			int num3 = this.CenterCoord.y - (this.size.y - 1);
			int num4 = this.CenterCoord.y + (this.size.y - 1) + 1;
			Vector3 b = new Vector3(-0.5f, 0f, -0.5f);
			for (int i = num; i <= num2; i++)
			{
				Draw.Line(new Vector3((float)i, 0f, (float)num3) + b, new Vector3((float)i, 0f, (float)num4) + b);
			}
			for (int j = num3; j <= num4; j++)
			{
				Draw.Line(new Vector3((float)num, 0f, (float)j) + b, new Vector3((float)num2, 0f, (float)j) + b);
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0005D52C File Offset: 0x0005B72C
		public bool IsPlacementWithinRange(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			coord -= this.CenterCoord;
			return coord.x > -this.size.x && coord.y > -this.size.y && coord.x + dimensions.x <= this.size.x && coord.y + dimensions.y <= this.size.y;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0005D5CC File Offset: 0x0005B7CC
		public Vector2Int CursorToCoord(Vector3 point, Vector2Int dimensions, BuildingRotation rotation)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			int x = Mathf.RoundToInt(point.x) - dimensions.x / 2;
			int y = Mathf.RoundToInt(point.z) - dimensions.y / 2;
			return new Vector2Int(x, y);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0005D628 File Offset: 0x0005B828
		private void ReleaseAllBuildings()
		{
			for (int i = this.activeBuildings.Count - 1; i >= 0; i--)
			{
				Building building = this.activeBuildings[i];
				if (!(building == null))
				{
					UnityEngine.Object.Destroy(building.gameObject);
				}
			}
			this.activeBuildings.Clear();
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0005D67C File Offset: 0x0005B87C
		public void RepaintAll()
		{
			this.ReleaseAllBuildings();
			BuildingManager.BuildingAreaData areaData = this.AreaData;
			if (areaData == null)
			{
				return;
			}
			foreach (BuildingManager.BuildingData building in areaData.Buildings)
			{
				this.Display(building);
			}
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0005D6E0 File Offset: 0x0005B8E0
		private void Display(BuildingManager.BuildingData building)
		{
			if (building == null)
			{
				return;
			}
			Building prefab = building.Info.Prefab;
			if (prefab == null)
			{
				Debug.LogError("No prefab for building " + building.ID);
				return;
			}
			for (int i = this.activeBuildings.Count - 1; i >= 0; i--)
			{
				Building building2 = this.activeBuildings[i];
				if (building2 == null)
				{
					this.activeBuildings.RemoveAt(i);
				}
				else if (building2.GUID == building.GUID)
				{
					Debug.LogError(string.Format("重复显示建筑{0}({1})", building.Info.DisplayName, building.GUID));
					return;
				}
			}
			Building building3 = UnityEngine.Object.Instantiate<Building>(prefab, base.transform);
			building3.Setup(building);
			building3.transform.position = building.GetTransformPosition();
			this.activeBuildings.Add(building3);
			if (building3.unlockAchievement && AchievementManager.Instance)
			{
				AchievementManager.Instance.Unlock("Building_" + building3.ID.Trim());
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0005D7FC File Offset: 0x0005B9FC
		internal Vector3 CoordToWorldPosition(Vector2Int coord, Vector2Int dimensions, BuildingRotation rotation)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			return new Vector3((float)coord.x - 0.5f + (float)dimensions.x / 2f, 0f, (float)coord.y - 0.5f + (float)dimensions.y / 2f);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0005D868 File Offset: 0x0005BA68
		internal bool PhysicsCollide(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord, float castBeginHeight = 0f, float castHeight = 2f)
		{
			if (rotation % BuildingRotation.Half != BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			this.raycastHitCount = 0;
			for (int i = coord.y; i < coord.y + dimensions.y; i++)
			{
				for (int j = coord.x; j < coord.x + dimensions.x; j++)
				{
					Vector3 vector = new Vector3((float)j, castBeginHeight, (float)i);
					this.raycastHitCount += Physics.RaycastNonAlloc(vector, Vector3.up, this.raycastHitBuffer, castHeight, this.physicsCollisionLayers);
					this.raycastHitCount += Physics.RaycastNonAlloc(vector + Vector3.up * castHeight, Vector3.down, this.raycastHitBuffer, castHeight, this.physicsCollisionLayers);
					if (this.raycastHitCount > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0005D964 File Offset: 0x0005BB64
		internal Building GetBuildingInstanceAt(Vector2Int coord)
		{
			BuildingManager.BuildingData buildingData = this.AreaData.GetBuildingAt(coord);
			if (buildingData == null)
			{
				return null;
			}
			return this.activeBuildings.Find((Building e) => e != null && e.GUID == buildingData.GUID);
		}

		// Token: 0x040012A3 RID: 4771
		[SerializeField]
		private string areaID;

		// Token: 0x040012A4 RID: 4772
		[SerializeField]
		private Vector2Int size;

		// Token: 0x040012A5 RID: 4773
		[SerializeField]
		private LayerMask physicsCollisionLayers = -1;

		// Token: 0x040012A6 RID: 4774
		private List<Building> activeBuildings = new List<Building>();

		// Token: 0x040012A7 RID: 4775
		private int raycastHitCount;

		// Token: 0x040012A8 RID: 4776
		private RaycastHit[] raycastHitBuffer = new RaycastHit[5];
	}
}
