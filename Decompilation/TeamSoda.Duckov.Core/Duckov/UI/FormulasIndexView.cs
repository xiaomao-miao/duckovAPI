using System;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000384 RID: 900
	public class FormulasIndexView : View, ISingleSelectionMenu<FormulasIndexEntry>
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x0006D2EC File Offset: 0x0006B4EC
		private PrefabPool<FormulasIndexEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<FormulasIndexEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0006D325 File Offset: 0x0006B525
		public FormulasIndexEntry GetSelection()
		{
			return this.selectedEntry;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x0006D32D File Offset: 0x0006B52D
		public static FormulasIndexView Instance
		{
			get
			{
				return View.GetViewInstance<FormulasIndexView>();
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0006D334 File Offset: 0x0006B534
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0006D33C File Offset: 0x0006B53C
		public static void Show()
		{
			if (FormulasIndexView.Instance == null)
			{
				return;
			}
			FormulasIndexView.Instance.Open(null);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0006D357 File Offset: 0x0006B557
		public bool SetSelection(FormulasIndexEntry selection)
		{
			this.selectedEntry = selection;
			return true;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0006D364 File Offset: 0x0006B564
		protected override void OnOpen()
		{
			base.OnOpen();
			this.selectedEntry = null;
			this.Pool.ReleaseAll();
			foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
			{
				if (!craftingFormula.hideInIndex && (!GameMetaData.Instance.IsDemo || !craftingFormula.lockInDemo))
				{
					this.Pool.Get(null).Setup(this, craftingFormula);
				}
			}
			this.RefreshDetails();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0006D408 File Offset: 0x0006B608
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0006D41C File Offset: 0x0006B61C
		internal void OnEntryClicked(FormulasIndexEntry entry)
		{
			FormulasIndexEntry formulasIndexEntry = this.selectedEntry;
			this.selectedEntry = entry;
			this.selectedEntry.Refresh();
			if (formulasIndexEntry)
			{
				formulasIndexEntry.Refresh();
			}
			this.RefreshDetails();
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0006D458 File Offset: 0x0006B658
		private void RefreshDetails()
		{
			if (this.selectedEntry && this.selectedEntry.Valid)
			{
				this.detailsDisplay.Setup(new CraftingFormula?(this.selectedEntry.Formula));
				return;
			}
			this.detailsDisplay.Setup(null);
		}

		// Token: 0x0400154C RID: 5452
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400154D RID: 5453
		[SerializeField]
		private FormulasIndexEntry entryTemplate;

		// Token: 0x0400154E RID: 5454
		[SerializeField]
		private FormulasDetailsDisplay detailsDisplay;

		// Token: 0x0400154F RID: 5455
		private PrefabPool<FormulasIndexEntry> _pool;

		// Token: 0x04001550 RID: 5456
		private FormulasIndexEntry selectedEntry;
	}
}
