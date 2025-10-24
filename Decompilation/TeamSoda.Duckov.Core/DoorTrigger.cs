using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class DoorTrigger : MonoBehaviour
{
	// Token: 0x060006B8 RID: 1720 RVA: 0x0001E28C File Offset: 0x0001C48C
	private void OnTriggerEnter(Collider collision)
	{
		if (this.parent.IsOpen)
		{
			return;
		}
		if (!this.parent.NoRequireItem)
		{
			return;
		}
		if (this.parent.Interact && !this.parent.Interact.gameObject.activeInHierarchy)
		{
			return;
		}
		if (collision.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		CharacterMainControl component = collision.gameObject.GetComponent<CharacterMainControl>();
		if (!component || component.Team == Teams.player)
		{
			return;
		}
		this.parent.Open();
	}

	// Token: 0x04000677 RID: 1655
	public Door parent;
}
