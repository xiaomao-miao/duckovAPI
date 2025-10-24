using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class AISpecialAttachmentBase : MonoBehaviour
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x0001616A File Offset: 0x0001436A
	public void Init(AICharacterController _ai, CharacterMainControl _character)
	{
		this.aiCharacterController = _ai;
		this.character = _character;
		this.OnInited();
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00016180 File Offset: 0x00014380
	protected virtual void OnInited()
	{
	}

	// Token: 0x0400041A RID: 1050
	public AICharacterController aiCharacterController;

	// Token: 0x0400041B RID: 1051
	public CharacterMainControl character;
}
