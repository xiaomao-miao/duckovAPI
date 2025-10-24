using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000316 RID: 790
	public class BuildingManager : MonoBehaviour
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0005E191 File Offset: 0x0005C391
		// (set) Token: 0x06001A1B RID: 6683 RVA: 0x0005E198 File Offset: 0x0005C398
		public static BuildingManager Instance { get; private set; }

		// Token: 0x06001A1C RID: 6684 RVA: 0x0005E1A0 File Offset: 0x0005C3A0
		private static int GenerateBuildingGUID(string buildingID)
		{
			BuildingManager.<>c__DisplayClass4_0 CS$<>8__locals1 = new BuildingManager.<>c__DisplayClass4_0();
			CS$<>8__locals1.<GenerateBuildingGUID>g__Regenerate|0();
			while (BuildingManager.Any((BuildingManager.BuildingData e) => e != null && e.GUID == CS$<>8__locals1.result))
			{
				CS$<>8__locals1.<GenerateBuildingGUID>g__Regenerate|0();
			}
			return CS$<>8__locals1.result;
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0005E1DC File Offset: 0x0005C3DC
		public int GetTokenAmount(string id)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry != null)
			{
				return buildingTokenAmountEntry.amount;
			}
			return 0;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0005E21C File Offset: 0x0005C41C
		private void SetTokenAmount(string id, int amount)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry != null)
			{
				buildingTokenAmountEntry.amount = amount;
				return;
			}
			buildingTokenAmountEntry = new BuildingManager.BuildingTokenAmountEntry
			{
				id = id,
				amount = amount
			};
			this.tokens.Add(buildingTokenAmountEntry);
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0005E280 File Offset: 0x0005C480
		private void AddToken(string id, int amount = 1)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry == null)
			{
				buildingTokenAmountEntry = new BuildingManager.BuildingTokenAmountEntry
				{
					id = id,
					amount = 0
				};
				this.tokens.Add(buildingTokenAmountEntry);
			}
			buildingTokenAmountEntry.amount += amount;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x0005E2E8 File Offset: 0x0005C4E8
		private bool PayToken(string id)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry == null)
			{
				return false;
			}
			if (buildingTokenAmountEntry.amount <= 0)
			{
				return false;
			}
			buildingTokenAmountEntry.amount--;
			return true;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0005E33C File Offset: 0x0005C53C
		public static Vector2Int[] GetOccupyingCoords(Vector2Int dimensions, BuildingRotation rotations, Vector2Int coord)
		{
			if (rotations % BuildingRotation.Half != BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			Vector2Int[] array = new Vector2Int[dimensions.x * dimensions.y];
			for (int i = 0; i < dimensions.y; i++)
			{
				for (int j = 0; j < dimensions.x; j++)
				{
					int num = j + dimensions.x * i;
					array[num] = coord + new Vector2Int(j, i);
				}
			}
			return array;
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0005E3BD File Offset: 0x0005C5BD
		public List<BuildingManager.BuildingAreaData> Areas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0005E3C8 File Offset: 0x0005C5C8
		public BuildingManager.BuildingAreaData GetOrCreateArea(string id)
		{
			BuildingManager.BuildingAreaData buildingAreaData = this.areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == id);
			if (buildingAreaData != null)
			{
				return buildingAreaData;
			}
			BuildingManager.BuildingAreaData buildingAreaData2 = new BuildingManager.BuildingAreaData(id);
			this.areas.Add(buildingAreaData2);
			return buildingAreaData2;
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0005E418 File Offset: 0x0005C618
		public BuildingManager.BuildingAreaData GetArea(string id)
		{
			return this.areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == id);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0005E449 File Offset: 0x0005C649
		private void CleanupAndSort()
		{
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0005E44B File Offset: 0x0005C64B
		public static BuildingInfo GetBuildingInfo(string id)
		{
			return BuildingDataCollection.GetInfo(id);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x0005E454 File Offset: 0x0005C654
		public static bool Any(string id, bool includeTokens = false)
		{
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			if (includeTokens && BuildingManager.Instance.GetTokenAmount(id) > 0)
			{
				return true;
			}
			using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Any(id))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0005E4D4 File Offset: 0x0005C6D4
		public static bool Any(Func<BuildingManager.BuildingData, bool> predicate)
		{
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Any(predicate))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0005E544 File Offset: 0x0005C744
		public static int GetBuildingAmount(string id)
		{
			if (BuildingManager.Instance == null)
			{
				return 0;
			}
			int num = 0;
			foreach (BuildingManager.BuildingAreaData buildingAreaData in BuildingManager.Instance.Areas)
			{
				using (List<BuildingManager.BuildingData>.Enumerator enumerator2 = buildingAreaData.Buildings.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.ID == id)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06001A2A RID: 6698 RVA: 0x0005E5F0 File Offset: 0x0005C7F0
		// (remove) Token: 0x06001A2B RID: 6699 RVA: 0x0005E624 File Offset: 0x0005C824
		public static event Action OnBuildingListChanged;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06001A2C RID: 6700 RVA: 0x0005E658 File Offset: 0x0005C858
		// (remove) Token: 0x06001A2D RID: 6701 RVA: 0x0005E68C File Offset: 0x0005C88C
		public static event Action<int> OnBuildingBuilt;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x06001A2E RID: 6702 RVA: 0x0005E6C0 File Offset: 0x0005C8C0
		// (remove) Token: 0x06001A2F RID: 6703 RVA: 0x0005E6F4 File Offset: 0x0005C8F4
		public static event Action<int> OnBuildingDestroyed;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06001A30 RID: 6704 RVA: 0x0005E728 File Offset: 0x0005C928
		// (remove) Token: 0x06001A31 RID: 6705 RVA: 0x0005E75C File Offset: 0x0005C95C
		public static event Action<int, BuildingInfo> OnBuildingBuiltComplex;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06001A32 RID: 6706 RVA: 0x0005E790 File Offset: 0x0005C990
		// (remove) Token: 0x06001A33 RID: 6707 RVA: 0x0005E7C4 File Offset: 0x0005C9C4
		public static event Action<int, BuildingInfo> OnBuildingDestroyedComplex;

		// Token: 0x06001A34 RID: 6708 RVA: 0x0005E7F7 File Offset: 0x0005C9F7
		private void Awake()
		{
			BuildingManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			this.Load();
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0005E816 File Offset: 0x0005CA16
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x0005E829 File Offset: 0x0005CA29
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x0005E834 File Offset: 0x0005CA34
		private void Load()
		{
			BuildingManager.SaveData saveData = SavesSystem.Load<BuildingManager.SaveData>("BuildingData");
			this.areas.Clear();
			if (saveData.data != null)
			{
				this.areas.AddRange(saveData.data);
			}
			this.tokens.Clear();
			if (saveData.tokenAmounts != null)
			{
				this.tokens.AddRange(saveData.tokenAmounts);
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x0005E894 File Offset: 0x0005CA94
		private void Save()
		{
			BuildingManager.SaveData value = new BuildingManager.SaveData
			{
				data = new List<BuildingManager.BuildingAreaData>(this.areas),
				tokenAmounts = new List<BuildingManager.BuildingTokenAmountEntry>(this.tokens)
			};
			SavesSystem.Save<BuildingManager.SaveData>("BuildingData", value);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0005E8DC File Offset: 0x0005CADC
		internal static BuildingManager.BuildingAreaData GetAreaData(string areaID)
		{
			if (BuildingManager.Instance == null)
			{
				return null;
			}
			return BuildingManager.Instance.Areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == areaID);
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005E920 File Offset: 0x0005CB20
		internal static BuildingManager.BuildingAreaData GetOrCreateAreaData(string areaID)
		{
			if (BuildingManager.Instance == null)
			{
				return null;
			}
			return BuildingManager.Instance.GetOrCreateArea(areaID);
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0005E93C File Offset: 0x0005CB3C
		internal static BuildingManager.BuildingData GetBuildingData(int guid, string areaID = null)
		{
			if (areaID == null)
			{
				using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
				{
					Predicate<BuildingManager.BuildingData> <>9__0;
					while (enumerator.MoveNext())
					{
						BuildingManager.BuildingAreaData buildingAreaData = enumerator.Current;
						List<BuildingManager.BuildingData> buildings = buildingAreaData.Buildings;
						Predicate<BuildingManager.BuildingData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((BuildingManager.BuildingData e) => e != null && e.GUID == guid));
						}
						BuildingManager.BuildingData buildingData = buildings.Find(match);
						if (buildingData != null)
						{
							return buildingData;
						}
					}
					goto IL_9B;
				}
				goto IL_74;
				IL_9B:
				return null;
			}
			IL_74:
			BuildingManager.BuildingAreaData areaData = BuildingManager.GetAreaData(areaID);
			if (areaData == null)
			{
				return null;
			}
			return areaData.Buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == guid);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0005E9F8 File Offset: 0x0005CBF8
		internal static BuildingBuyAndPlaceResults BuyAndPlace(string areaID, string id, Vector2Int coord, BuildingRotation rotation)
		{
			if (BuildingManager.Instance == null)
			{
				return BuildingBuyAndPlaceResults.NoReferences;
			}
			BuildingInfo buildingInfo = BuildingManager.GetBuildingInfo(id);
			if (!buildingInfo.Valid)
			{
				return BuildingBuyAndPlaceResults.InvalidBuildingInfo;
			}
			BuildingManager.GetBuildingAmount(id);
			if (buildingInfo.ReachedAmountLimit)
			{
				return BuildingBuyAndPlaceResults.ReachedAmountLimit;
			}
			BuildingManager.Instance.GetTokenAmount(id);
			if (!BuildingManager.Instance.PayToken(id) && !buildingInfo.cost.Pay(true, true))
			{
				return BuildingBuyAndPlaceResults.PaymentFailure;
			}
			BuildingManager.BuildingAreaData orCreateArea = BuildingManager.Instance.GetOrCreateArea(areaID);
			int num = BuildingManager.GenerateBuildingGUID(id);
			orCreateArea.Add(id, rotation, coord, num);
			Action onBuildingListChanged = BuildingManager.OnBuildingListChanged;
			if (onBuildingListChanged != null)
			{
				onBuildingListChanged();
			}
			Action<int> onBuildingBuilt = BuildingManager.OnBuildingBuilt;
			if (onBuildingBuilt != null)
			{
				onBuildingBuilt(num);
			}
			Action<int, BuildingInfo> onBuildingBuiltComplex = BuildingManager.OnBuildingBuiltComplex;
			if (onBuildingBuiltComplex != null)
			{
				onBuildingBuiltComplex(num, buildingInfo);
			}
			AudioManager.Post("UI/building_up");
			return BuildingBuyAndPlaceResults.Succeed;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0005EAC0 File Offset: 0x0005CCC0
		internal static bool DestroyBuilding(int guid, string areaID = null)
		{
			BuildingManager.BuildingData buildingData;
			BuildingManager.BuildingAreaData buildingAreaData;
			if (!BuildingManager.TryGetBuildingDataAndAreaData(guid, out buildingData, out buildingAreaData, areaID))
			{
				return false;
			}
			buildingAreaData.Remove(buildingData);
			Action onBuildingListChanged = BuildingManager.OnBuildingListChanged;
			if (onBuildingListChanged != null)
			{
				onBuildingListChanged();
			}
			Action<int> onBuildingDestroyed = BuildingManager.OnBuildingDestroyed;
			if (onBuildingDestroyed != null)
			{
				onBuildingDestroyed(guid);
			}
			Action<int, BuildingInfo> onBuildingDestroyedComplex = BuildingManager.OnBuildingDestroyedComplex;
			if (onBuildingDestroyedComplex != null)
			{
				onBuildingDestroyedComplex(guid, buildingData.Info);
			}
			return true;
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0005EB20 File Offset: 0x0005CD20
		internal static bool TryGetBuildingDataAndAreaData(int guid, out BuildingManager.BuildingData buildingData, out BuildingManager.BuildingAreaData areaData, string areaID = null)
		{
			buildingData = null;
			areaData = null;
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			if (areaID == null)
			{
				using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.areas.GetEnumerator())
				{
					Predicate<BuildingManager.BuildingData> <>9__0;
					while (enumerator.MoveNext())
					{
						BuildingManager.BuildingAreaData buildingAreaData = enumerator.Current;
						List<BuildingManager.BuildingData> buildings = buildingAreaData.Buildings;
						Predicate<BuildingManager.BuildingData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((BuildingManager.BuildingData e) => e != null && e.GUID == guid));
						}
						BuildingManager.BuildingData buildingData2 = buildings.Find(match);
						if (buildingData2 != null)
						{
							areaData = buildingAreaData;
							buildingData = buildingData2;
							return true;
						}
					}
					return false;
				}
			}
			BuildingManager.BuildingAreaData area = BuildingManager.Instance.GetArea(areaID);
			if (area == null)
			{
				return false;
			}
			BuildingManager.BuildingData buildingData3 = area.Buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == guid);
			if (buildingData3 != null)
			{
				areaData = area;
				buildingData = buildingData3;
			}
			return false;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0005EC10 File Offset: 0x0005CE10
		internal static UniTask<bool> ReturnBuilding(int guid, string areaID = null)
		{
			BuildingManager.<ReturnBuilding>d__53 <ReturnBuilding>d__;
			<ReturnBuilding>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<ReturnBuilding>d__.guid = guid;
			<ReturnBuilding>d__.areaID = areaID;
			<ReturnBuilding>d__.<>1__state = -1;
			<ReturnBuilding>d__.<>t__builder.Start<BuildingManager.<ReturnBuilding>d__53>(ref <ReturnBuilding>d__);
			return <ReturnBuilding>d__.<>t__builder.Task;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0005EC5C File Offset: 0x0005CE5C
		internal static UniTask<int> ReturnBuildings(string areaID = null, params int[] buildings)
		{
			BuildingManager.<ReturnBuildings>d__54 <ReturnBuildings>d__;
			<ReturnBuildings>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<ReturnBuildings>d__.areaID = areaID;
			<ReturnBuildings>d__.buildings = buildings;
			<ReturnBuildings>d__.<>1__state = -1;
			<ReturnBuildings>d__.<>t__builder.Start<BuildingManager.<ReturnBuildings>d__54>(ref <ReturnBuildings>d__);
			return <ReturnBuildings>d__.<>t__builder.Task;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0005ECA8 File Offset: 0x0005CEA8
		internal static UniTask<int> ReturnBuildingsOfType(string buildingID, string areaID = null)
		{
			BuildingManager.<ReturnBuildingsOfType>d__55 <ReturnBuildingsOfType>d__;
			<ReturnBuildingsOfType>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<ReturnBuildingsOfType>d__.buildingID = buildingID;
			<ReturnBuildingsOfType>d__.areaID = areaID;
			<ReturnBuildingsOfType>d__.<>1__state = -1;
			<ReturnBuildingsOfType>d__.<>t__builder.Start<BuildingManager.<ReturnBuildingsOfType>d__55>(ref <ReturnBuildingsOfType>d__);
			return <ReturnBuildingsOfType>d__.<>t__builder.Task;
		}

		// Token: 0x040012BF RID: 4799
		private List<BuildingManager.BuildingTokenAmountEntry> tokens = new List<BuildingManager.BuildingTokenAmountEntry>();

		// Token: 0x040012C0 RID: 4800
		[SerializeField]
		private List<BuildingManager.BuildingAreaData> areas = new List<BuildingManager.BuildingAreaData>();

		// Token: 0x040012C6 RID: 4806
		private const string SaveKey = "BuildingData";

		// Token: 0x040012C7 RID: 4807
		private static bool returningBuilding;

		// Token: 0x020005A8 RID: 1448
		[Serializable]
		public class BuildingTokenAmountEntry
		{
			// Token: 0x0400203A RID: 8250
			public string id;

			// Token: 0x0400203B RID: 8251
			public int amount;
		}

		// Token: 0x020005A9 RID: 1449
		[Serializable]
		public class BuildingAreaData
		{
			// Token: 0x17000774 RID: 1908
			// (get) Token: 0x060028B4 RID: 10420 RVA: 0x00096B97 File Offset: 0x00094D97
			public string AreaID
			{
				get
				{
					return this.areaID;
				}
			}

			// Token: 0x17000775 RID: 1909
			// (get) Token: 0x060028B5 RID: 10421 RVA: 0x00096B9F File Offset: 0x00094D9F
			public List<BuildingManager.BuildingData> Buildings
			{
				get
				{
					return this.buildings;
				}
			}

			// Token: 0x060028B6 RID: 10422 RVA: 0x00096BA8 File Offset: 0x00094DA8
			public bool Any(string buildingID)
			{
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					if (buildingData != null)
					{
						if (buildingData.ID == buildingID)
						{
							return true;
						}
						if (buildingData.Info.alternativeFor.Contains(buildingID))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060028B7 RID: 10423 RVA: 0x00096C24 File Offset: 0x00094E24
			public bool Add(string buildingID, BuildingRotation rotation, Vector2Int coord, int guid = -1)
			{
				BuildingManager.GetBuildingInfo(buildingID);
				if (guid < 0)
				{
					guid = BuildingManager.GenerateBuildingGUID(buildingID);
				}
				this.buildings.Add(new BuildingManager.BuildingData(guid, buildingID, rotation, coord));
				return true;
			}

			// Token: 0x060028B8 RID: 10424 RVA: 0x00096C50 File Offset: 0x00094E50
			public bool Remove(int buildingGUID)
			{
				BuildingManager.BuildingData buildingData = this.buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == buildingGUID);
				return buildingData != null && this.buildings.Remove(buildingData);
			}

			// Token: 0x060028B9 RID: 10425 RVA: 0x00096C93 File Offset: 0x00094E93
			public bool Remove(BuildingManager.BuildingData building)
			{
				return this.buildings.Remove(building);
			}

			// Token: 0x060028BA RID: 10426 RVA: 0x00096CA4 File Offset: 0x00094EA4
			public BuildingManager.BuildingData GetBuildingAt(Vector2Int coord)
			{
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					if (BuildingManager.GetOccupyingCoords(buildingData.Dimensions, buildingData.Rotation, buildingData.Coord).Contains(coord))
					{
						return buildingData;
					}
				}
				return null;
			}

			// Token: 0x060028BB RID: 10427 RVA: 0x00096D18 File Offset: 0x00094F18
			public HashSet<Vector2Int> GetAllOccupiedCoords()
			{
				HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					Vector2Int[] occupyingCoords = BuildingManager.GetOccupyingCoords(buildingData.Dimensions, buildingData.Rotation, buildingData.Coord);
					hashSet.AddRange(occupyingCoords);
				}
				return hashSet;
			}

			// Token: 0x060028BC RID: 10428 RVA: 0x00096D8C File Offset: 0x00094F8C
			public bool Collide(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
			{
				HashSet<Vector2Int> allOccupiedCoords = this.GetAllOccupiedCoords();
				foreach (Vector2Int item in BuildingManager.GetOccupyingCoords(dimensions, rotation, coord))
				{
					if (allOccupiedCoords.Contains(item))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060028BD RID: 10429 RVA: 0x00096DCB File Offset: 0x00094FCB
			internal bool Any(Func<BuildingManager.BuildingData, bool> predicate)
			{
				return this.buildings.Any(predicate);
			}

			// Token: 0x060028BE RID: 10430 RVA: 0x00096DD9 File Offset: 0x00094FD9
			public BuildingAreaData()
			{
			}

			// Token: 0x060028BF RID: 10431 RVA: 0x00096DEC File Offset: 0x00094FEC
			public BuildingAreaData(string areaID)
			{
				this.areaID = areaID;
			}

			// Token: 0x0400203C RID: 8252
			[SerializeField]
			private string areaID;

			// Token: 0x0400203D RID: 8253
			[SerializeField]
			private List<BuildingManager.BuildingData> buildings = new List<BuildingManager.BuildingData>();
		}

		// Token: 0x020005AA RID: 1450
		[Serializable]
		public class BuildingData
		{
			// Token: 0x17000776 RID: 1910
			// (get) Token: 0x060028C0 RID: 10432 RVA: 0x00096E06 File Offset: 0x00095006
			public int GUID
			{
				get
				{
					return this.guid;
				}
			}

			// Token: 0x17000777 RID: 1911
			// (get) Token: 0x060028C1 RID: 10433 RVA: 0x00096E0E File Offset: 0x0009500E
			public string ID
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x17000778 RID: 1912
			// (get) Token: 0x060028C2 RID: 10434 RVA: 0x00096E18 File Offset: 0x00095018
			public Vector2Int Dimensions
			{
				get
				{
					return this.Info.Dimensions;
				}
			}

			// Token: 0x17000779 RID: 1913
			// (get) Token: 0x060028C3 RID: 10435 RVA: 0x00096E33 File Offset: 0x00095033
			public Vector2Int Coord
			{
				get
				{
					return this.coord;
				}
			}

			// Token: 0x1700077A RID: 1914
			// (get) Token: 0x060028C4 RID: 10436 RVA: 0x00096E3B File Offset: 0x0009503B
			public BuildingRotation Rotation
			{
				get
				{
					return this.rotation;
				}
			}

			// Token: 0x1700077B RID: 1915
			// (get) Token: 0x060028C5 RID: 10437 RVA: 0x00096E43 File Offset: 0x00095043
			public BuildingInfo Info
			{
				get
				{
					return BuildingDataCollection.GetInfo(this.id);
				}
			}

			// Token: 0x060028C6 RID: 10438 RVA: 0x00096E50 File Offset: 0x00095050
			public BuildingData(int guid, string id, BuildingRotation rotation, Vector2Int coord)
			{
				this.guid = guid;
				this.id = id;
				this.coord = coord;
				this.rotation = rotation;
			}

			// Token: 0x060028C7 RID: 10439 RVA: 0x00096E78 File Offset: 0x00095078
			internal Vector3 GetTransformPosition()
			{
				Vector2Int dimensions = this.Dimensions;
				if (this.rotation % BuildingRotation.Half > BuildingRotation.Zero)
				{
					dimensions = new Vector2Int(dimensions.y, dimensions.x);
				}
				return new Vector3((float)this.coord.x - 0.5f + (float)dimensions.x / 2f, 0f, (float)this.coord.y - 0.5f + (float)dimensions.y / 2f);
			}

			// Token: 0x0400203E RID: 8254
			[SerializeField]
			private int guid;

			// Token: 0x0400203F RID: 8255
			[SerializeField]
			private string id;

			// Token: 0x04002040 RID: 8256
			[SerializeField]
			private Vector2Int coord;

			// Token: 0x04002041 RID: 8257
			[SerializeField]
			private BuildingRotation rotation;
		}

		// Token: 0x020005AB RID: 1451
		[Serializable]
		private struct SaveData
		{
			// Token: 0x04002042 RID: 8258
			[SerializeField]
			public List<BuildingManager.BuildingAreaData> data;

			// Token: 0x04002043 RID: 8259
			[SerializeField]
			public List<BuildingManager.BuildingTokenAmountEntry> tokenAmounts;
		}
	}
}
