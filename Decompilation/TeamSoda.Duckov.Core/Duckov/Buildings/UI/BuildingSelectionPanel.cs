using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031D RID: 797
	public class BuildingSelectionPanel : MonoBehaviour
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x0005FCD4 File Offset: 0x0005DED4
		private PrefabPool<BuildingBtnEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<BuildingBtnEntry>(this.buildingBtnTemplate, null, new Action<BuildingBtnEntry>(this.OnGetButtonEntry), new Action<BuildingBtnEntry>(this.OnReleaseButtonEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x0005FD23 File Offset: 0x0005DF23
		private void OnGetButtonEntry(BuildingBtnEntry entry)
		{
			entry.onButtonClicked += this.OnButtonSelected;
			entry.onRecycleRequested += this.OnRecycleRequested;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0005FD49 File Offset: 0x0005DF49
		private void OnReleaseButtonEntry(BuildingBtnEntry entry)
		{
			entry.onButtonClicked -= this.OnButtonSelected;
			entry.onRecycleRequested -= this.OnRecycleRequested;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0005FD6F File Offset: 0x0005DF6F
		private void OnRecycleRequested(BuildingBtnEntry entry)
		{
			Action<BuildingBtnEntry> action = this.onRecycleRequested;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0005FD82 File Offset: 0x0005DF82
		private void OnButtonSelected(BuildingBtnEntry entry)
		{
			Action<BuildingBtnEntry> action = this.onButtonSelected;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06001A83 RID: 6787 RVA: 0x0005FD98 File Offset: 0x0005DF98
		// (remove) Token: 0x06001A84 RID: 6788 RVA: 0x0005FDD0 File Offset: 0x0005DFD0
		public event Action<BuildingBtnEntry> onButtonSelected;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06001A85 RID: 6789 RVA: 0x0005FE08 File Offset: 0x0005E008
		// (remove) Token: 0x06001A86 RID: 6790 RVA: 0x0005FE40 File Offset: 0x0005E040
		public event Action<BuildingBtnEntry> onRecycleRequested;

		// Token: 0x06001A87 RID: 6791 RVA: 0x0005FE75 File Offset: 0x0005E075
		public void Show()
		{
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0005FE77 File Offset: 0x0005E077
		internal void Setup(BuildingArea targetArea)
		{
			this.targetArea = targetArea;
			this.Refresh();
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0005FE88 File Offset: 0x0005E088
		public void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (BuildingInfo buildingInfo in BuildingSelectionPanel.GetBuildingsToDisplay())
			{
				BuildingBtnEntry buildingBtnEntry = this.Pool.Get(null);
				buildingBtnEntry.Setup(buildingInfo);
				buildingBtnEntry.transform.SetAsLastSibling();
			}
			foreach (BuildingBtnEntry buildingBtnEntry2 in this.Pool.ActiveEntries)
			{
				if (!buildingBtnEntry2.CostEnough)
				{
					buildingBtnEntry2.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0005FF30 File Offset: 0x0005E130
		public static BuildingInfo[] GetBuildingsToDisplay()
		{
			BuildingDataCollection instance = BuildingDataCollection.Instance;
			if (instance == null)
			{
				return new BuildingInfo[0];
			}
			List<BuildingInfo> list = new List<BuildingInfo>();
			foreach (BuildingInfo item in instance.Infos)
			{
				if (item.CurrentAmount > 0 || item.RequirementsSatisfied())
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x040012FF RID: 4863
		[SerializeField]
		private BuildingBtnEntry buildingBtnTemplate;

		// Token: 0x04001300 RID: 4864
		private PrefabPool<BuildingBtnEntry> _pool;

		// Token: 0x04001301 RID: 4865
		private BuildingArea targetArea;
	}
}
