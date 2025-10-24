using System;

// Token: 0x020000DA RID: 218
public class InteractableCarriable : InteractableBase
{
	// Token: 0x060006E3 RID: 1763 RVA: 0x0001F064 File Offset: 0x0001D264
	protected override void Start()
	{
		base.Start();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0001F073 File Offset: 0x0001D273
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0001F076 File Offset: 0x0001D276
	protected override void OnInteractStart(CharacterMainControl character)
	{
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0001F078 File Offset: 0x0001D278
	protected override void OnInteractFinished()
	{
		if (!this.interactCharacter)
		{
			return;
		}
		CharacterMainControl interactCharacter = this.interactCharacter;
		base.StopInteract();
		interactCharacter.Carry(this.carryTarget);
	}

	// Token: 0x0400069A RID: 1690
	public Carriable carryTarget;
}
