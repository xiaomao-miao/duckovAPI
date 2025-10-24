using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class PaperBox : MonoBehaviour
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x000197AC File Offset: 0x000179AC
	private void Update()
	{
		if (!this.character)
		{
			return;
		}
		if (!this.setActiveWhileStandStill)
		{
			return;
		}
		bool flag = this.character.Velocity.magnitude < 0.2f;
		if (this.setActiveWhileStandStill.gameObject.activeSelf != flag)
		{
			this.setActiveWhileStandStill.gameObject.SetActive(flag);
		}
	}

	// Token: 0x04000531 RID: 1329
	[HideInInspector]
	public CharacterMainControl character;

	// Token: 0x04000532 RID: 1330
	public Transform setActiveWhileStandStill;
}
