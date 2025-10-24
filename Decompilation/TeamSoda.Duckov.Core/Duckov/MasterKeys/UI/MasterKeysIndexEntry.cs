using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002DF RID: 735
	public class MasterKeysIndexEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x00056370 File Offset: 0x00054570
		public int ItemID
		{
			get
			{
				return this.itemID;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x00056378 File Offset: 0x00054578
		public string DisplayName
		{
			get
			{
				if (this.status == null)
				{
					return "???";
				}
				if (!this.status.active)
				{
					return "???";
				}
				return this.metaData.DisplayName;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x000563A6 File Offset: 0x000545A6
		public Sprite Icon
		{
			get
			{
				if (this.status == null)
				{
					return this.undiscoveredIcon;
				}
				if (!this.status.active)
				{
					return this.undiscoveredIcon;
				}
				return this.metaData.icon;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x000563D6 File Offset: 0x000545D6
		public string Description
		{
			get
			{
				if (this.status == null)
				{
					return "???";
				}
				if (!this.status.active)
				{
					return "???";
				}
				return this.metaData.Description;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x00056404 File Offset: 0x00054604
		public bool Active
		{
			get
			{
				return this.status != null && this.status.active;
			}
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06001784 RID: 6020 RVA: 0x0005641C File Offset: 0x0005461C
		// (remove) Token: 0x06001785 RID: 6021 RVA: 0x00056454 File Offset: 0x00054654
		internal event Action<MasterKeysIndexEntry> onPointerClicked;

		// Token: 0x06001786 RID: 6022 RVA: 0x00056489 File Offset: 0x00054689
		public void Setup(int itemID, ISingleSelectionMenu<MasterKeysIndexEntry> menu)
		{
			this.itemID = itemID;
			this.metaData = ItemAssetsCollection.GetMetaData(itemID);
			this.menu = menu;
			this.Refresh();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x000564AC File Offset: 0x000546AC
		private void SetupNotDiscovered()
		{
			this.icon.sprite = (this.undiscoveredIcon ? this.undiscoveredIcon : this.metaData.icon);
			this.notDiscoveredLook.ApplyTo(this.icon);
			this.nameText.text = "???";
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00056505 File Offset: 0x00054705
		private void SetupActive()
		{
			this.icon.sprite = this.metaData.icon;
			this.activeLook.ApplyTo(this.icon);
			this.nameText.text = this.metaData.DisplayName;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00056544 File Offset: 0x00054744
		private void Refresh()
		{
			this.status = MasterKeysManager.GetStatus(this.itemID);
			if (this.status != null)
			{
				if (this.status.active)
				{
					this.SetupActive();
					return;
				}
			}
			else
			{
				this.SetupNotDiscovered();
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00056579 File Offset: 0x00054779
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Refresh();
			ISingleSelectionMenu<MasterKeysIndexEntry> singleSelectionMenu = this.menu;
			if (singleSelectionMenu != null)
			{
				singleSelectionMenu.SetSelection(this);
			}
			Action<MasterKeysIndexEntry> action = this.onPointerClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x04001125 RID: 4389
		[SerializeField]
		private Image icon;

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001127 RID: 4391
		[SerializeField]
		private MasterKeysIndexEntry.Look notDiscoveredLook;

		// Token: 0x04001128 RID: 4392
		[SerializeField]
		private MasterKeysIndexEntry.Look activeLook;

		// Token: 0x04001129 RID: 4393
		[SerializeField]
		private Sprite undiscoveredIcon;

		// Token: 0x0400112A RID: 4394
		[ItemTypeID]
		private int itemID;

		// Token: 0x0400112B RID: 4395
		private ItemMetaData metaData;

		// Token: 0x0400112C RID: 4396
		private MasterKeysManager.Status status;

		// Token: 0x0400112E RID: 4398
		private ISingleSelectionMenu<MasterKeysIndexEntry> menu;

		// Token: 0x0200057C RID: 1404
		[Serializable]
		public struct Look
		{
			// Token: 0x0600285D RID: 10333 RVA: 0x00094F41 File Offset: 0x00093141
			public void ApplyTo(Graphic graphic)
			{
				graphic.material = this.material;
				graphic.color = this.color;
			}

			// Token: 0x04001FA2 RID: 8098
			public Color color;

			// Token: 0x04001FA3 RID: 8099
			public Material material;
		}
	}
}
