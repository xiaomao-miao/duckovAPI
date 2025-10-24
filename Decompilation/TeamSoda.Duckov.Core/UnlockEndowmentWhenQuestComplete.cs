using System;
using Duckov.Endowment;
using Duckov.Quests;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class UnlockEndowmentWhenQuestComplete : MonoBehaviour
{
	// Token: 0x0600098B RID: 2443 RVA: 0x000297B0 File Offset: 0x000279B0
	private void Awake()
	{
		if (this.quest == null)
		{
			this.quest = base.GetComponent<Quest>();
		}
		if (this.quest != null)
		{
			this.quest.onCompleted += this.OnQuestCompleted;
		}
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x000297FC File Offset: 0x000279FC
	private void Start()
	{
		if (this.quest.Complete && !EndowmentManager.GetEndowmentUnlocked(this.endowmentToUnlock))
		{
			EndowmentManager.UnlockEndowment(this.endowmentToUnlock);
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00029824 File Offset: 0x00027A24
	private void OnDestroy()
	{
		if (this.quest != null)
		{
			this.quest.onCompleted -= this.OnQuestCompleted;
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0002984B File Offset: 0x00027A4B
	private void OnQuestCompleted(Quest quest)
	{
		if (!EndowmentManager.GetEndowmentUnlocked(this.endowmentToUnlock))
		{
			EndowmentManager.UnlockEndowment(this.endowmentToUnlock);
		}
	}

	// Token: 0x04000868 RID: 2152
	[SerializeField]
	private Quest quest;

	// Token: 0x04000869 RID: 2153
	[SerializeField]
	private EndowmentIndex endowmentToUnlock;
}
