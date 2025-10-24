using System;
using Duckov;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class PostAudioEventOnEnter : StateMachineBehaviour
{
	// Token: 0x06000C3E RID: 3134 RVA: 0x00033A13 File Offset: 0x00031C13
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		AudioManager.Post(this.eventName, animator.gameObject);
	}

	// Token: 0x04000A9B RID: 2715
	[SerializeField]
	private string eventName;
}
