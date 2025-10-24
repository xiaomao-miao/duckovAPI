using System;
using Cysharp.Threading.Tasks;
using Duckov.Economy;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002E5 RID: 741
	public class Crop : MonoBehaviour
	{
		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00056F64 File Offset: 0x00055164
		public CropData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x00056F6C File Offset: 0x0005516C
		public CropInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x00056F74 File Offset: 0x00055174
		public float Progress
		{
			get
			{
				return (float)this.data.growTicks / (float)this.info.totalGrowTicks;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060017C6 RID: 6086 RVA: 0x00056F8F File Offset: 0x0005518F
		public bool Ripen
		{
			get
			{
				return this.initialized && this.data.growTicks >= this.info.totalGrowTicks;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060017C7 RID: 6087 RVA: 0x00056FB6 File Offset: 0x000551B6
		public bool Watered
		{
			get
			{
				return this.data.watered;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00056FC4 File Offset: 0x000551C4
		public string DisplayName
		{
			get
			{
				return this.Info.DisplayName;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060017C9 RID: 6089 RVA: 0x00056FE0 File Offset: 0x000551E0
		public TimeSpan RemainingTime
		{
			get
			{
				if (!this.initialized)
				{
					return TimeSpan.Zero;
				}
				long num = this.info.totalGrowTicks - this.data.growTicks;
				if (num < 0L)
				{
					return TimeSpan.Zero;
				}
				return TimeSpan.FromTicks(num);
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x060017CA RID: 6090 RVA: 0x00057024 File Offset: 0x00055224
		// (remove) Token: 0x060017CB RID: 6091 RVA: 0x00057058 File Offset: 0x00055258
		public static event Action<Crop, Crop.CropEvent> onCropStatusChange;

		// Token: 0x060017CC RID: 6092 RVA: 0x0005708C File Offset: 0x0005528C
		public bool Harvest()
		{
			if (!this.Ripen)
			{
				return false;
			}
			if (this.Watered)
			{
				this.data.score = this.data.score + 50;
			}
			int product = this.info.GetProduct(this.data.Ranking);
			if (product <= 0)
			{
				Debug.LogError("Crop product is invalid:\ncrop:" + this.info.id);
				return false;
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(product, (long)this.info.resultAmount)
			});
			cost.Return(false, false, 1, null).Forget();
			this.DestroyCrop();
			Action<Crop> action = this.onHarvest;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 != null)
			{
				action2(this, Crop.CropEvent.Harvest);
			}
			return true;
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00057154 File Offset: 0x00055354
		public void DestroyCrop()
		{
			Action<Crop> action = this.onBeforeDestroy;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 != null)
			{
				action2(this, Crop.CropEvent.BeforeDestroy);
			}
			this.garden.Release(this);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00057188 File Offset: 0x00055388
		public void InitializeNew(Garden garden, string id, Vector2Int coord)
		{
			CropData cropData = new CropData
			{
				gardenID = garden.GardenID,
				cropID = id,
				coord = coord,
				LastUpdateDateTime = DateTime.Now
			};
			this.Initialize(garden, cropData);
			Action<Crop> action = this.onPlant;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Plant);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x000571F4 File Offset: 0x000553F4
		public void Initialize(Garden garden, CropData data)
		{
			this.garden = garden;
			string cropID = data.cropID;
			CropInfo? cropInfo = CropDatabase.GetCropInfo(cropID);
			if (cropInfo == null)
			{
				Debug.LogError("找不到 corpInfo id: " + cropID);
				return;
			}
			this.info = cropInfo.Value;
			this.data = data;
			this.RefreshDisplayInstance();
			this.initialized = true;
			Vector3 localPosition = garden.CoordToLocalPosition(data.coord);
			base.transform.localPosition = localPosition;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0005726C File Offset: 0x0005546C
		private void RefreshDisplayInstance()
		{
			if (this.displayInstance != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(this.displayInstance.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(this.displayInstance.gameObject);
				}
			}
			if (this.info.displayPrefab == null)
			{
				Debug.LogError("找不到Display Prefab: " + this.info.DisplayName);
				return;
			}
			this.displayInstance = UnityEngine.Object.Instantiate<GameObject>(this.info.displayPrefab, this.displayParent);
			this.displayInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00057314 File Offset: 0x00055514
		public void Water()
		{
			if (this.data.watered)
			{
				return;
			}
			this.data.watered = true;
			Action<Crop> action = this.onWater;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Water);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00057353 File Offset: 0x00055553
		private void FixedUpdate()
		{
			this.Tick();
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0005735C File Offset: 0x0005555C
		private void Tick()
		{
			if (!this.initialized)
			{
				return;
			}
			TimeSpan timeSpan = DateTime.Now - this.data.LastUpdateDateTime;
			this.data.LastUpdateDateTime = DateTime.Now;
			if (!this.data.watered)
			{
				return;
			}
			if (this.Ripen)
			{
				return;
			}
			long ticks = timeSpan.Ticks;
			this.data.growTicks = this.data.growTicks + ticks;
			if (this.Ripen)
			{
				this.OnRipen();
			}
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000573D5 File Offset: 0x000555D5
		private void OnRipen()
		{
			Action<Crop> action = this.onRipen;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Ripen);
		}

		// Token: 0x04001150 RID: 4432
		[SerializeField]
		private Transform displayParent;

		// Token: 0x04001151 RID: 4433
		private Garden garden;

		// Token: 0x04001152 RID: 4434
		private bool initialized;

		// Token: 0x04001153 RID: 4435
		private CropData data;

		// Token: 0x04001154 RID: 4436
		private CropInfo info;

		// Token: 0x04001155 RID: 4437
		private GameObject displayInstance;

		// Token: 0x04001156 RID: 4438
		public Action<Crop> onPlant;

		// Token: 0x04001157 RID: 4439
		public Action<Crop> onWater;

		// Token: 0x04001158 RID: 4440
		public Action<Crop> onRipen;

		// Token: 0x04001159 RID: 4441
		public Action<Crop> onHarvest;

		// Token: 0x0400115A RID: 4442
		public Action<Crop> onBeforeDestroy;

		// Token: 0x0200057F RID: 1407
		public enum CropEvent
		{
			// Token: 0x04001FAB RID: 8107
			Plant,
			// Token: 0x04001FAC RID: 8108
			Water,
			// Token: 0x04001FAD RID: 8109
			Ripen,
			// Token: 0x04001FAE RID: 8110
			Harvest,
			// Token: 0x04001FAF RID: 8111
			BeforeDestroy
		}
	}
}
