using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using Saves;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002EC RID: 748
	public class Garden : MonoBehaviour
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x00057972 File Offset: 0x00055B72
		public string GardenID
		{
			get
			{
				return this.gardenID;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0005797A File Offset: 0x00055B7A
		public string SaveKey
		{
			get
			{
				return "Garden_" + this.gardenID;
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x060017F6 RID: 6134 RVA: 0x0005798C File Offset: 0x00055B8C
		// (remove) Token: 0x060017F7 RID: 6135 RVA: 0x000579C0 File Offset: 0x00055BC0
		public static event Action OnSizeAddersChanged;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060017F8 RID: 6136 RVA: 0x000579F4 File Offset: 0x00055BF4
		// (remove) Token: 0x060017F9 RID: 6137 RVA: 0x00057A28 File Offset: 0x00055C28
		public static event Action OnAutoWatersChanged;

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00057A5B File Offset: 0x00055C5B
		// (set) Token: 0x060017FB RID: 6139 RVA: 0x00057A63 File Offset: 0x00055C63
		public bool AutoWater
		{
			get
			{
				return this.autoWater;
			}
			set
			{
				this.autoWater = value;
				if (value)
				{
					this.WaterAll();
				}
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00057A78 File Offset: 0x00055C78
		private void WaterAll()
		{
			foreach (Crop crop in this.dictioanry.Values)
			{
				if (!(crop == null) && !crop.Watered)
				{
					crop.Water();
				}
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x00057AE0 File Offset: 0x00055CE0
		// (set) Token: 0x060017FE RID: 6142 RVA: 0x00057AE8 File Offset: 0x00055CE8
		public Vector2Int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				this.sizeDirty = true;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x00057AF8 File Offset: 0x00055CF8
		public PrefabPool<CellDisplay> CellPool
		{
			get
			{
				if (this._cellPool == null)
				{
					this._cellPool = new PrefabPool<CellDisplay>(this.cellDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._cellPool;
			}
		}

		// Token: 0x17000456 RID: 1110
		public Crop this[Vector2Int coord]
		{
			get
			{
				Crop result;
				if (this.dictioanry.TryGetValue(coord, out result))
				{
					return result;
				}
				return null;
			}
			private set
			{
				this.dictioanry[coord] = value;
			}
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00057B64 File Offset: 0x00055D64
		private void Awake()
		{
			Garden.gardens[this.gardenID] = this;
			SavesSystem.OnCollectSaveData += this.Save;
			Garden.OnSizeAddersChanged += this.RefreshSize;
			Garden.OnAutoWatersChanged += this.RefreshAutowater;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00057BB5 File Offset: 0x00055DB5
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			Garden.OnSizeAddersChanged -= this.RefreshSize;
			Garden.OnAutoWatersChanged -= this.RefreshAutowater;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00057BEA File Offset: 0x00055DEA
		private void Start()
		{
			this.RegenerateCellDisplays();
			this.Load();
			this.RefreshSize();
			this.RefreshAutowater();
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00057C04 File Offset: 0x00055E04
		private void FixedUpdate()
		{
			if (this.sizeDirty)
			{
				this.RegenerateCellDisplays();
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00057C14 File Offset: 0x00055E14
		private void RefreshAutowater()
		{
			bool flag = false;
			using (List<IGardenAutoWaterProvider>.Enumerator enumerator = Garden.autoWaters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TakeEffect(this.gardenID))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != this.AutoWater)
			{
				this.AutoWater = flag;
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00057C84 File Offset: 0x00055E84
		private void RefreshSize()
		{
			Vector2Int a = Vector2Int.zero;
			foreach (IGardenSizeAdder gardenSizeAdder in Garden.sizeAdders)
			{
				if (gardenSizeAdder != null)
				{
					a += gardenSizeAdder.GetValue(this.gardenID);
				}
			}
			this.Size = new Vector2Int(3 + a.x, 3 + a.y);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00057D08 File Offset: 0x00055F08
		public void SetSize(int x, int y)
		{
			this.RegenerateCellDisplays();
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00057D10 File Offset: 0x00055F10
		private void RegenerateCellDisplays()
		{
			this.sizeDirty = false;
			this.CellPool.ReleaseAll();
			Vector2Int vector2Int = this.Size;
			for (int i = 0; i < vector2Int.y; i++)
			{
				for (int j = 0; j < vector2Int.x; j++)
				{
					Vector3 localPosition = this.CoordToLocalPosition(new Vector2Int(j, i));
					CellDisplay cellDisplay = this.CellPool.Get(null);
					cellDisplay.transform.localPosition = localPosition;
					cellDisplay.Setup(this, j, i);
				}
			}
			Vector3 vector = this.CoordToLocalPosition(new Vector2Int(0, 0)) - new Vector3(this.grid.cellSize.x, 0f, this.grid.cellSize.y) / 2f;
			Vector3 vector2 = this.CoordToLocalPosition(new Vector2Int(vector2Int.x, vector2Int.y)) - new Vector3(this.grid.cellSize.x, 0f, this.grid.cellSize.y) / 2f;
			float num = vector2.x - vector.x;
			float num2 = vector2.z - vector.z;
			Vector3 localPosition2 = vector;
			Vector3 localPosition3 = new Vector3(vector.x, 0f, vector2.z);
			Vector3 localPosition4 = vector2;
			Vector3 localPosition5 = new Vector3(vector2.x, 0f, vector.z);
			Vector3 localScale = new Vector3(1f, 1f, num2);
			Vector3 localScale2 = new Vector3(1f, 1f, num);
			Vector3 localScale3 = new Vector3(1f, 1f, num2);
			Vector3 localScale4 = new Vector3(1f, 1f, num);
			this.border00.localPosition = localPosition2;
			this.border01.localPosition = localPosition3;
			this.border11.localPosition = localPosition4;
			this.border10.localPosition = localPosition5;
			this.corner00.localPosition = localPosition2;
			this.corner01.localPosition = localPosition3;
			this.corner11.localPosition = localPosition4;
			this.corner10.localPosition = localPosition5;
			this.border00.localScale = localScale;
			this.border01.localScale = localScale2;
			this.border11.localScale = localScale3;
			this.border10.localScale = localScale4;
			this.border00.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.border01.localRotation = Quaternion.Euler(0f, 90f, 0f);
			this.border11.localRotation = Quaternion.Euler(0f, 180f, 0f);
			this.border10.localRotation = Quaternion.Euler(0f, 270f, 0f);
			Vector3 localPosition6 = (vector + vector2) / 2f;
			this.interactBox.transform.localPosition = localPosition6;
			this.interactBox.center = Vector3.zero;
			this.interactBox.size = new Vector3(num + 0.5f, 1f, num2 + 0.5f);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0005803E File Offset: 0x0005623E
		private Crop CreateCropInstance(string id)
		{
			return UnityEngine.Object.Instantiate<Crop>(this.cropTemplate, base.transform);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00058054 File Offset: 0x00056254
		public void Save()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			Garden.SaveData value = new Garden.SaveData(this);
			SavesSystem.Save<Garden.SaveData>(this.SaveKey, value);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0005807C File Offset: 0x0005627C
		public void Load()
		{
			this.Clear();
			this.dictioanry.Clear();
			Garden.SaveData saveData = SavesSystem.Load<Garden.SaveData>(this.SaveKey);
			if (saveData == null)
			{
				return;
			}
			foreach (CropData cropData in saveData.crops)
			{
				Crop crop = this.CreateCropInstance(cropData.cropID);
				crop.Initialize(this, cropData);
				this[cropData.coord] = crop;
			}
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0005810C File Offset: 0x0005630C
		private void Clear()
		{
			foreach (Crop crop in this.dictioanry.Values.ToList<Crop>())
			{
				if (!(crop == null))
				{
					UnityEngine.Object.Destroy(crop.gameObject);
				}
			}
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00058178 File Offset: 0x00056378
		public bool IsCoordValid(Vector2Int coord)
		{
			Vector2Int vector2Int = this.Size;
			return vector2Int.x <= 0 || vector2Int.y <= 0 || (coord.x < vector2Int.x && coord.y < vector2Int.y && coord.x >= 0 && coord.y >= 0);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x000581DB File Offset: 0x000563DB
		public bool IsCoordOccupied(Vector2Int coord)
		{
			return this[coord] != null;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x000581EC File Offset: 0x000563EC
		public bool Plant(Vector2Int coord, string cropID)
		{
			if (!this.IsCoordValid(coord))
			{
				return false;
			}
			if (this.IsCoordOccupied(coord))
			{
				return false;
			}
			if (!CropDatabase.IsIdValid(cropID))
			{
				Debug.Log("[Garden] Invalid crop id " + cropID, this);
				return false;
			}
			Crop crop = this.CreateCropInstance(cropID);
			crop.InitializeNew(this, cropID, coord);
			this[coord] = crop;
			if (this.autoWater)
			{
				crop.Water();
			}
			return true;
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00058254 File Offset: 0x00056454
		public void Water(Vector2Int coord)
		{
			Crop crop = this[coord];
			if (crop == null)
			{
				return;
			}
			crop.Water();
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0005827C File Offset: 0x0005647C
		public Vector3 CoordToWorldPosition(Vector2Int coord)
		{
			Vector3 position = this.CoordToLocalPosition(coord);
			return base.transform.TransformPoint(position);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x000582A0 File Offset: 0x000564A0
		public Vector3 CoordToLocalPosition(Vector2Int coord)
		{
			Vector3 cellCenterLocal = this.grid.GetCellCenterLocal((Vector3Int)coord);
			float z = this.grid.cellSize.z;
			float y = cellCenterLocal.y - z / 2f;
			Vector3 result = cellCenterLocal;
			result.y = y;
			return result;
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x000582E8 File Offset: 0x000564E8
		public Vector2Int WorldPositionToCoord(Vector3 wPos)
		{
			Vector3 worldPosition = wPos + Vector3.up * 0.1f * this.grid.cellSize.z;
			return (Vector2Int)this.grid.WorldToCell(worldPosition);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00058331 File Offset: 0x00056531
		internal void Release(Crop crop)
		{
			UnityEngine.Object.Destroy(crop.gameObject);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00058340 File Offset: 0x00056540
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			float x = this.grid.cellSize.x;
			float y = this.grid.cellSize.y;
			Vector2Int vector2Int = this.Size;
			for (int i = 0; i <= vector2Int.x; i++)
			{
				Vector3 vector = Vector3.right * (float)i * x;
				Vector3 to = vector + Vector3.forward * (float)vector2Int.y * y;
				Gizmos.DrawLine(vector, to);
			}
			for (int j = 0; j <= vector2Int.y; j++)
			{
				Vector3 vector2 = Vector3.forward * (float)j * y;
				Vector3 to2 = vector2 + Vector3.right * (float)vector2Int.x * x;
				Gizmos.DrawLine(vector2, to2);
			}
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x00058421 File Offset: 0x00056621
		internal static void Register(IGardenSizeAdder obj)
		{
			Garden.sizeAdders.Add(obj);
			Action onSizeAddersChanged = Garden.OnSizeAddersChanged;
			if (onSizeAddersChanged == null)
			{
				return;
			}
			onSizeAddersChanged();
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0005843D File Offset: 0x0005663D
		internal static void Register(IGardenAutoWaterProvider obj)
		{
			Garden.autoWaters.Add(obj);
			Action onAutoWatersChanged = Garden.OnAutoWatersChanged;
			if (onAutoWatersChanged == null)
			{
				return;
			}
			onAutoWatersChanged();
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00058459 File Offset: 0x00056659
		internal static void Unregister(IGardenSizeAdder obj)
		{
			Garden.sizeAdders.Remove(obj);
			Action onSizeAddersChanged = Garden.OnSizeAddersChanged;
			if (onSizeAddersChanged == null)
			{
				return;
			}
			onSizeAddersChanged();
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00058476 File Offset: 0x00056676
		internal static void Unregister(IGardenAutoWaterProvider obj)
		{
			Garden.autoWaters.Remove(obj);
			Action onAutoWatersChanged = Garden.OnAutoWatersChanged;
			if (onAutoWatersChanged == null)
			{
				return;
			}
			onAutoWatersChanged();
		}

		// Token: 0x0400117D RID: 4477
		[SerializeField]
		private string gardenID = "Default";

		// Token: 0x0400117E RID: 4478
		public static List<IGardenSizeAdder> sizeAdders = new List<IGardenSizeAdder>();

		// Token: 0x0400117F RID: 4479
		public static List<IGardenAutoWaterProvider> autoWaters = new List<IGardenAutoWaterProvider>();

		// Token: 0x04001182 RID: 4482
		public static Dictionary<string, Garden> gardens = new Dictionary<string, Garden>();

		// Token: 0x04001183 RID: 4483
		[SerializeField]
		private Grid grid;

		// Token: 0x04001184 RID: 4484
		[SerializeField]
		private Crop cropTemplate;

		// Token: 0x04001185 RID: 4485
		[SerializeField]
		private Transform border00;

		// Token: 0x04001186 RID: 4486
		[SerializeField]
		private Transform border01;

		// Token: 0x04001187 RID: 4487
		[SerializeField]
		private Transform border11;

		// Token: 0x04001188 RID: 4488
		[SerializeField]
		private Transform border10;

		// Token: 0x04001189 RID: 4489
		[SerializeField]
		private Transform corner00;

		// Token: 0x0400118A RID: 4490
		[SerializeField]
		private Transform corner01;

		// Token: 0x0400118B RID: 4491
		[SerializeField]
		private Transform corner11;

		// Token: 0x0400118C RID: 4492
		[SerializeField]
		private Transform corner10;

		// Token: 0x0400118D RID: 4493
		[SerializeField]
		private BoxCollider interactBox;

		// Token: 0x0400118E RID: 4494
		[SerializeField]
		private Vector2Int size;

		// Token: 0x0400118F RID: 4495
		[SerializeField]
		private bool autoWater;

		// Token: 0x04001190 RID: 4496
		public Vector3 cameraRigCenter = new Vector3(3f, 0f, 3f);

		// Token: 0x04001191 RID: 4497
		private bool sizeDirty;

		// Token: 0x04001192 RID: 4498
		[SerializeField]
		private CellDisplay cellDisplayTemplate;

		// Token: 0x04001193 RID: 4499
		private PrefabPool<CellDisplay> _cellPool;

		// Token: 0x04001194 RID: 4500
		private Dictionary<Vector2Int, Crop> dictioanry = new Dictionary<Vector2Int, Crop>();

		// Token: 0x02000584 RID: 1412
		[Serializable]
		private class SaveData
		{
			// Token: 0x06002867 RID: 10343 RVA: 0x000950B4 File Offset: 0x000932B4
			public SaveData(Garden garden)
			{
				this.crops = new List<CropData>();
				foreach (Crop crop in garden.dictioanry.Values)
				{
					if (!(crop == null))
					{
						this.crops.Add(crop.Data);
					}
				}
			}

			// Token: 0x04001FB5 RID: 8117
			[SerializeField]
			public List<CropData> crops;
		}
	}
}
