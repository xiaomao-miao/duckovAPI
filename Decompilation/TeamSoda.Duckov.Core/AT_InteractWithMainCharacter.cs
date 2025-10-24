using System;
using NodeCanvas.Framework;

// Token: 0x02000040 RID: 64
public class AT_InteractWithMainCharacter : ActionTask
{
	// Token: 0x06000180 RID: 384 RVA: 0x00007760 File Offset: 0x00005960
	protected override void OnExecute()
	{
		base.OnExecute();
		this.interactable.InteractWithMainCharacter();
		base.EndAction();
	}

	// Token: 0x04000113 RID: 275
	public InteractableBase interactable;
}
