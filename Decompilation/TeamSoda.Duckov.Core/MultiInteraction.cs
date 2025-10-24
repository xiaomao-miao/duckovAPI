using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class MultiInteraction : MonoBehaviour
{
	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0003AFDE File Offset: 0x000391DE
	public ReadOnlyCollection<InteractableBase> Interactables
	{
		get
		{
			return this.interactables.AsReadOnly();
		}
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0003AFEB File Offset: 0x000391EB
	private void OnTriggerEnter(Collider other)
	{
		if (CharacterMainControl.Main.gameObject == other.gameObject)
		{
			MultiInteractionMenu instance = MultiInteractionMenu.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetupAndShow(this).Forget();
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0003B019 File Offset: 0x00039219
	private void OnTriggerExit(Collider other)
	{
		if (CharacterMainControl.Main.gameObject == other.gameObject)
		{
			MultiInteractionMenu instance = MultiInteractionMenu.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Hide().Forget();
		}
	}

	// Token: 0x04000C3A RID: 3130
	[SerializeField]
	private List<InteractableBase> interactables;
}
