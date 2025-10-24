using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E3 RID: 739
	public class MasterKeysView : View, ISingleSelectionMenu<MasterKeysIndexEntry>
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x00056D09 File Offset: 0x00054F09
		public static MasterKeysView Instance
		{
			get
			{
				return View.GetViewInstance<MasterKeysView>();
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00056D10 File Offset: 0x00054F10
		protected override void Awake()
		{
			base.Awake();
			this.listDisplay.onEntryPointerClicked += this.OnEntryClicked;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00056D2F File Offset: 0x00054F2F
		private void OnEntryClicked(MasterKeysIndexEntry entry)
		{
			this.RefreshInspectorDisplay();
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00056D37 File Offset: 0x00054F37
		public MasterKeysIndexEntry GetSelection()
		{
			return this.listDisplay.GetSelection();
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00056D44 File Offset: 0x00054F44
		public bool SetSelection(MasterKeysIndexEntry selection)
		{
			this.listDisplay.GetSelection();
			return true;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00056D53 File Offset: 0x00054F53
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.SetSelection(null);
			this.RefreshListDisplay();
			this.RefreshInspectorDisplay();
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00056D7A File Offset: 0x00054F7A
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00056D8D File Offset: 0x00054F8D
		private void RefreshListDisplay()
		{
			this.listDisplay.Refresh();
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00056D9C File Offset: 0x00054F9C
		private void RefreshInspectorDisplay()
		{
			MasterKeysIndexEntry selection = this.GetSelection();
			this.inspector.Setup(selection);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00056DBC File Offset: 0x00054FBC
		internal static void Show()
		{
			if (MasterKeysView.Instance == null)
			{
				Debug.Log(" Master keys view Instance is null");
				return;
			}
			MasterKeysView.Instance.Open(null);
		}

		// Token: 0x04001147 RID: 4423
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001148 RID: 4424
		[SerializeField]
		private MasterKeysIndexList listDisplay;

		// Token: 0x04001149 RID: 4425
		[SerializeField]
		private MasterKeysIndexInspector inspector;
	}
}
