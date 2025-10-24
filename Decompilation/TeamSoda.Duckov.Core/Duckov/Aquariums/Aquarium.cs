using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.Aquariums
{
	// Token: 0x0200031F RID: 799
	public class Aquarium : MonoBehaviour
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x000601A5 File Offset: 0x0005E3A5
		private string ItemSaveKey
		{
			get
			{
				return "Aquarium/Item/" + this.id;
			}
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000601B7 File Offset: 0x0005E3B7
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000601CA File Offset: 0x0005E3CA
		private void Start()
		{
			this.Load().Forget();
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000601D7 File Offset: 0x0005E3D7
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000601EC File Offset: 0x0005E3EC
		private UniTask Load()
		{
			Aquarium.<Load>d__14 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<Aquarium.<Load>d__14>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x0006022F File Offset: 0x0005E42F
		private void OnChildChanged(Item item)
		{
			this.dirty = true;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00060238 File Offset: 0x0005E438
		private void FixedUpdate()
		{
			if (this.loading)
			{
				return;
			}
			if (this.dirty)
			{
				this.Refresh();
				this.dirty = false;
			}
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00060258 File Offset: 0x0005E458
		private void Refresh()
		{
			if (this.aquariumItem == null)
			{
				return;
			}
			foreach (Item item in this.aquariumItem.GetAllChildren(false, true))
			{
				if (!(item == null) && item.Tags.Contains("Fish"))
				{
					this.GetOrCreateGraphic(item) == null;
				}
			}
			this.graphicRecords.RemoveAll((Aquarium.ItemGraphicPair e) => e == null || e.graphic == null);
			for (int i = 0; i < this.graphicRecords.Count; i++)
			{
				Aquarium.ItemGraphicPair itemGraphicPair = this.graphicRecords[i];
				if (itemGraphicPair.item == null || itemGraphicPair.item.ParentItem != this.aquariumItem)
				{
					if (itemGraphicPair.graphic != null)
					{
						UnityEngine.Object.Destroy(itemGraphicPair.graphic);
					}
					this.graphicRecords.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00060380 File Offset: 0x0005E580
		private ItemGraphicInfo GetOrCreateGraphic(Item item)
		{
			if (item == null)
			{
				return null;
			}
			Aquarium.ItemGraphicPair itemGraphicPair = this.graphicRecords.Find((Aquarium.ItemGraphicPair e) => e != null && e.item == item);
			if (itemGraphicPair != null && itemGraphicPair.graphic != null)
			{
				return itemGraphicPair.graphic;
			}
			ItemGraphicInfo itemGraphicInfo = ItemGraphicInfo.CreateAGraphic(item, this.graphicsParent);
			if (itemGraphicPair != null)
			{
				this.graphicRecords.Remove(itemGraphicPair);
			}
			if (itemGraphicInfo == null)
			{
				return null;
			}
			IAquariumContent component = itemGraphicInfo.GetComponent<IAquariumContent>();
			if (component != null)
			{
				component.Setup(this);
			}
			this.graphicRecords.Add(new Aquarium.ItemGraphicPair
			{
				item = item,
				graphic = itemGraphicInfo
			});
			return itemGraphicInfo;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x0006043C File Offset: 0x0005E63C
		public void Loot()
		{
			LootView.LootItem(this.aquariumItem);
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00060449 File Offset: 0x0005E649
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			if (!this.loaded)
			{
				return;
			}
			this.aquariumItem.Save(this.ItemSaveKey);
		}

		// Token: 0x0400130A RID: 4874
		[SerializeField]
		private string id = "Default";

		// Token: 0x0400130B RID: 4875
		[SerializeField]
		private Transform graphicsParent;

		// Token: 0x0400130C RID: 4876
		[ItemTypeID]
		private int aquariumItemTypeID = 1158;

		// Token: 0x0400130D RID: 4877
		private Item aquariumItem;

		// Token: 0x0400130E RID: 4878
		private List<Aquarium.ItemGraphicPair> graphicRecords = new List<Aquarium.ItemGraphicPair>();

		// Token: 0x0400130F RID: 4879
		private bool loading;

		// Token: 0x04001310 RID: 4880
		private bool loaded;

		// Token: 0x04001311 RID: 4881
		private int loadToken;

		// Token: 0x04001312 RID: 4882
		private bool dirty = true;

		// Token: 0x020005BC RID: 1468
		private class ItemGraphicPair
		{
			// Token: 0x0400206E RID: 8302
			public Item item;

			// Token: 0x0400206F RID: 8303
			public ItemGraphicInfo graphic;
		}
	}
}
