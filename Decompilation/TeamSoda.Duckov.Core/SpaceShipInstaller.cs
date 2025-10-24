using System;
using Duckov;
using Duckov.Quests;
using Duckov.UI;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class SpaceShipInstaller : MonoBehaviour
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001A331 File Offset: 0x00018531
	// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0001A33E File Offset: 0x0001853E
	private bool Installed
	{
		get
		{
			return SavesSystem.Load<bool>(this.saveDataKey);
		}
		set
		{
			SavesSystem.Save<bool>(this.saveDataKey, value);
		}
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0001A34C File Offset: 0x0001854C
	private void Awake()
	{
		if (this.buildFx)
		{
			this.buildFx.SetActive(false);
		}
		this.interactable.overrideInteractName = true;
		this.interactable._overrideInteractNameKey = this.interactKey;
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0001A384 File Offset: 0x00018584
	public void Install()
	{
		if (this.buildFx)
		{
			this.buildFx.SetActive(true);
		}
		AudioManager.Post("Archived/Building/Default/Constructed", base.gameObject);
		this.Installed = true;
		this.SyncGraphic(true);
		this.interactable.gameObject.SetActive(false);
		NotificationText.Push(this.notificationKey.ToPlainText());
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001A3EA File Offset: 0x000185EA
	private void SyncGraphic(bool _installed)
	{
		if (this.builtGraphic)
		{
			this.builtGraphic.SetActive(_installed);
		}
		if (this.unbuiltGraphic)
		{
			this.unbuiltGraphic.SetActive(!_installed);
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001A424 File Offset: 0x00018624
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.inited)
		{
			bool flag = this.Installed;
			if (flag)
			{
				TaskEvent.EmitTaskEvent(this.saveDataKey);
			}
			else if (QuestManager.IsQuestFinished(this.questID))
			{
				flag = true;
				this.Installed = true;
			}
			this.interactable.gameObject.SetActive(!flag && QuestManager.IsQuestActive(this.questID));
			this.SyncGraphic(flag);
			this.inited = true;
		}
		if (!this.Installed && !this.interactable.gameObject.activeSelf && QuestManager.IsQuestActive(this.questID))
		{
			this.interactable.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000564 RID: 1380
	[SerializeField]
	private string saveDataKey;

	// Token: 0x04000565 RID: 1381
	[SerializeField]
	private int questID;

	// Token: 0x04000566 RID: 1382
	[SerializeField]
	private InteractableBase interactable;

	// Token: 0x04000567 RID: 1383
	[SerializeField]
	[LocalizationKey("Default")]
	private string notificationKey;

	// Token: 0x04000568 RID: 1384
	[SerializeField]
	[LocalizationKey("Default")]
	private string interactKey;

	// Token: 0x04000569 RID: 1385
	private bool inited;

	// Token: 0x0400056A RID: 1386
	public GameObject builtGraphic;

	// Token: 0x0400056B RID: 1387
	public GameObject unbuiltGraphic;

	// Token: 0x0400056C RID: 1388
	public GameObject buildFx;
}
