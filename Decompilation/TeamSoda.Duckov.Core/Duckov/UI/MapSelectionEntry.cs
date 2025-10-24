using System;
using Duckov.Economy;
using Duckov.Quests;
using Duckov.Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003B7 RID: 951
	public class MapSelectionEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x00078ABE File Offset: 0x00076CBE
		public Cost Cost
		{
			get
			{
				return this.cost;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x00078AC6 File Offset: 0x00076CC6
		public bool ConditionsSatisfied
		{
			get
			{
				return this.conditions == null || this.conditions.Satisfied();
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x00078ADD File Offset: 0x00076CDD
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x00078AE5 File Offset: 0x00076CE5
		public int BeaconIndex
		{
			get
			{
				return this.beaconIndex;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x00078AED File Offset: 0x00076CED
		public Sprite FullScreenImage
		{
			get
			{
				return this.fullScreenImage;
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00078AF5 File Offset: 0x00076CF5
		public void Setup(MapSelectionView master)
		{
			this.master = master;
			this.Refresh();
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00078B04 File Offset: 0x00076D04
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00078B0C File Offset: 0x00076D0C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.ConditionsSatisfied)
			{
				return;
			}
			this.master.NotifyEntryClicked(this, eventData);
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00078B24 File Offset: 0x00076D24
		private void Refresh()
		{
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
			this.displayNameText.text = sceneInfo.DisplayName;
			this.lockedIndicator.gameObject.SetActive(!this.ConditionsSatisfied);
			this.costDisplay.Setup(this.cost, 1);
			this.costDisplay.gameObject.SetActive(!this.cost.IsFree);
		}

		// Token: 0x04001763 RID: 5987
		[SerializeField]
		private MapSelectionView master;

		// Token: 0x04001764 RID: 5988
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001765 RID: 5989
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x04001766 RID: 5990
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x04001767 RID: 5991
		[SerializeField]
		private Condition[] conditions;

		// Token: 0x04001768 RID: 5992
		[SerializeField]
		private Cost cost;

		// Token: 0x04001769 RID: 5993
		[SerializeField]
		[SceneID]
		private string sceneID;

		// Token: 0x0400176A RID: 5994
		[SerializeField]
		private int beaconIndex;

		// Token: 0x0400176B RID: 5995
		[SerializeField]
		private Sprite fullScreenImage;
	}
}
