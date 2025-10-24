using System;
using Duckov.UI;

namespace Duckov.PerkTrees.Interactable
{
	// Token: 0x0200025C RID: 604
	public class PerkTreeUIInvoker : InteractableBase
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x00046878 File Offset: 0x00044A78
		protected override bool ShowUnityEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0004687B File Offset: 0x00044A7B
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			PerkTreeView.Show(PerkTreeManager.GetPerkTree(this.perkTreeID));
			base.StopInteract();
		}

		// Token: 0x04000E25 RID: 3621
		public string perkTreeID;
	}
}
