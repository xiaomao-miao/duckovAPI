using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200010B RID: 267
public class PlayerPositionBackupManager : MonoBehaviour
{
	// Token: 0x14000045 RID: 69
	// (add) Token: 0x0600091F RID: 2335 RVA: 0x000287A8 File Offset: 0x000269A8
	// (remove) Token: 0x06000920 RID: 2336 RVA: 0x000287DC File Offset: 0x000269DC
	private static event Action OnStartRecoverEvent;

	// Token: 0x06000921 RID: 2337 RVA: 0x0002880F File Offset: 0x00026A0F
	private void Awake()
	{
		this.backups = new List<PlayerPositionBackupManager.PlayerPositionBackupEntry>();
		MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		PlayerPositionBackupManager.OnStartRecoverEvent += this.OnStartRecover;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0002883E File Offset: 0x00026A3E
	private void OnDestroy()
	{
		MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		PlayerPositionBackupManager.OnStartRecoverEvent -= this.OnStartRecover;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00028864 File Offset: 0x00026A64
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.mainCharacter)
		{
			this.mainCharacter = CharacterMainControl.Main;
		}
		if (!this.mainCharacter)
		{
			return;
		}
		this.backupTimer -= Time.deltaTime;
		if (this.backupTimer < 0f && this.CheckCanBackup())
		{
			this.BackupCurrentPos();
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x000288CC File Offset: 0x00026ACC
	private bool CheckCanBackup()
	{
		if (!this.mainCharacter)
		{
			return false;
		}
		if (!this.mainCharacter.IsOnGround)
		{
			return false;
		}
		if (Mathf.Abs(this.mainCharacter.Velocity.y) > 2f)
		{
			return false;
		}
		int count = this.backups.Count;
		if (count > 0)
		{
			Vector3 position = this.backups[count - 1].position;
			if (Vector3.Distance(this.mainCharacter.transform.position, position) < this.minBackupDistance)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0002895A File Offset: 0x00026B5A
	private void OnSubSceneLoaded(MultiSceneCore multiSceneCore, Scene scene)
	{
		this.backups.Clear();
		this.backupTimer = this.backupTimeSpace;
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00028974 File Offset: 0x00026B74
	public void BackupCurrentPos()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.mainCharacter)
		{
			return;
		}
		this.backupTimer = this.backupTimeSpace;
		PlayerPositionBackupManager.PlayerPositionBackupEntry item = default(PlayerPositionBackupManager.PlayerPositionBackupEntry);
		item.position = this.mainCharacter.transform.position;
		item.sceneID = SceneManager.GetActiveScene().buildIndex;
		this.backups.Add(item);
		if (this.backups.Count > this.listSize)
		{
			this.backups.RemoveAt(0);
		}
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00028A01 File Offset: 0x00026C01
	public static void StartRecover()
	{
		Action onStartRecoverEvent = PlayerPositionBackupManager.OnStartRecoverEvent;
		if (onStartRecoverEvent == null)
		{
			return;
		}
		onStartRecoverEvent();
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00028A14 File Offset: 0x00026C14
	private void OnStartRecover()
	{
		if (this.mainCharacter.CurrentAction != null && this.mainCharacter.CurrentAction.Running)
		{
			this.mainCharacter.CurrentAction.StopAction();
		}
		this.mainCharacter.Interact(this.backupInteract);
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00028A68 File Offset: 0x00026C68
	public void SetPlayerToBackupPos()
	{
		if (this.backups.Count <= 0)
		{
			return;
		}
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		Vector3 position = this.mainCharacter.transform.position;
		ref PlayerPositionBackupManager.PlayerPositionBackupEntry ptr = this.backups[this.backups.Count - 1];
		this.backups.RemoveAt(this.backups.Count - 1);
		Vector3 position2 = ptr.position;
		if (Vector3.Distance(position, position2) > this.minBackupDistance)
		{
			this.mainCharacter.SetPosition(position2);
			return;
		}
		this.SetPlayerToBackupPos();
	}

	// Token: 0x0400082D RID: 2093
	private List<PlayerPositionBackupManager.PlayerPositionBackupEntry> backups;

	// Token: 0x0400082E RID: 2094
	private CharacterMainControl mainCharacter;

	// Token: 0x0400082F RID: 2095
	public float backupTimeSpace = 3f;

	// Token: 0x04000830 RID: 2096
	public float minBackupDistance = 3f;

	// Token: 0x04000831 RID: 2097
	private float backupTimer = 3f;

	// Token: 0x04000832 RID: 2098
	public InteractableBase backupInteract;

	// Token: 0x04000833 RID: 2099
	public int listSize = 20;

	// Token: 0x02000491 RID: 1169
	private struct PlayerPositionBackupEntry
	{
		// Token: 0x04001BCD RID: 7117
		public int sceneID;

		// Token: 0x04001BCE RID: 7118
		public Vector3 position;
	}
}
