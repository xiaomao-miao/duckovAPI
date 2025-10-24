using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Options.UI
{
	// Token: 0x0200025E RID: 606
	public class OptionsPanel : UIPanel, ISingleSelectionMenu<OptionsPanel_TabButton>
	{
		// Token: 0x060012CC RID: 4812 RVA: 0x00046A98 File Offset: 0x00044C98
		private void Start()
		{
			this.Setup();
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00046AA0 File Offset: 0x00044CA0
		private void Setup()
		{
			foreach (OptionsPanel_TabButton optionsPanel_TabButton in this.tabButtons)
			{
				optionsPanel_TabButton.onClicked = (Action<OptionsPanel_TabButton, PointerEventData>)Delegate.Combine(optionsPanel_TabButton.onClicked, new Action<OptionsPanel_TabButton, PointerEventData>(this.OnTabButtonClicked));
			}
			if (this.selection == null)
			{
				this.selection = this.tabButtons[0];
			}
			this.SetSelection(this.selection);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00046B3C File Offset: 0x00044D3C
		private void OnTabButtonClicked(OptionsPanel_TabButton button, PointerEventData data)
		{
			data.Use();
			this.SetSelection(button);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00046B4C File Offset: 0x00044D4C
		protected override void OnOpen()
		{
			base.OnOpen();
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00046B54 File Offset: 0x00044D54
		public OptionsPanel_TabButton GetSelection()
		{
			return this.selection;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00046B5C File Offset: 0x00044D5C
		public bool SetSelection(OptionsPanel_TabButton selection)
		{
			this.selection = selection;
			foreach (OptionsPanel_TabButton optionsPanel_TabButton in this.tabButtons)
			{
				optionsPanel_TabButton.NotifySelectionChanged(this, selection);
			}
			return true;
		}

		// Token: 0x04000E29 RID: 3625
		[SerializeField]
		private List<OptionsPanel_TabButton> tabButtons;

		// Token: 0x04000E2A RID: 3626
		private OptionsPanel_TabButton selection;
	}
}
