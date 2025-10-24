using System;
using Saves;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class BaseSceneUtilities : MonoBehaviour
{
	// Token: 0x060009E1 RID: 2529 RVA: 0x0002A79F File Offset: 0x0002899F
	private void Save()
	{
		LevelManager.Instance.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		SavesSystem.SaveFile(true);
		this.lastTimeSaved = Time.realtimeSinceStartup;
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0002A7C1 File Offset: 0x000289C1
	private float TimeSinceLastSave
	{
		get
		{
			return Time.realtimeSinceStartup - this.lastTimeSaved;
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0002A7CF File Offset: 0x000289CF
	private void Awake()
	{
		this.lastTimeSaved = Time.realtimeSinceStartup;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0002A7DC File Offset: 0x000289DC
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (this.TimeSinceLastSave > this.saveInterval)
		{
			this.Save();
		}
	}

	// Token: 0x040008B3 RID: 2227
	[SerializeField]
	private float saveInterval = 5f;

	// Token: 0x040008B4 RID: 2228
	private float lastTimeSaved = float.MinValue;
}
