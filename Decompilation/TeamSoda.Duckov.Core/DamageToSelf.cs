using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class DamageToSelf : MonoBehaviour
{
	// Token: 0x060009F9 RID: 2553 RVA: 0x0002AC16 File Offset: 0x00028E16
	private void Start()
	{
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0002AC18 File Offset: 0x00028E18
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			this.dmg.fromCharacter = CharacterMainControl.Main;
			CharacterMainControl.Main.Health.Hurt(this.dmg);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			float value = CharacterMainControl.Main.CharacterItem.GetStat("InventoryCapacity").Value;
			Debug.Log(string.Format("InventorySize:{0}", value));
		}
	}

	// Token: 0x040008BD RID: 2237
	public DamageInfo dmg;
}
