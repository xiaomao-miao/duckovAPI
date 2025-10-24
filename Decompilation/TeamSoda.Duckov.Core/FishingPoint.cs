using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A4 RID: 164
public class FishingPoint : MonoBehaviour
{
	// Token: 0x0600059D RID: 1437 RVA: 0x0001931B File Offset: 0x0001751B
	private void Awake()
	{
		this.OnPlayerTakeFishingRod(null);
		this.Interactable.OnInteractFinishedEvent.AddListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractFinished));
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00019340 File Offset: 0x00017540
	private void OnDestroy()
	{
		if (this.Interactable)
		{
			this.Interactable.OnInteractFinishedEvent.RemoveListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractFinished));
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0001936B File Offset: 0x0001756B
	private void OnPlayerTakeFishingRod(FishingRod rod)
	{
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00019370 File Offset: 0x00017570
	private void OnInteractFinished(CharacterMainControl character, InteractableBase interact)
	{
		if (!character)
		{
			return;
		}
		character.SetPosition(this.playerPoint.position);
		character.SetAimPoint(this.playerPoint.position + this.playerPoint.forward * 10f);
		character.movementControl.SetAimDirection(this.playerPoint.forward);
		character.StartAction(this.action);
	}

	// Token: 0x04000515 RID: 1301
	public InteractableBase Interactable;

	// Token: 0x04000516 RID: 1302
	public Action_Fishing action;

	// Token: 0x04000517 RID: 1303
	public Transform playerPoint;
}
