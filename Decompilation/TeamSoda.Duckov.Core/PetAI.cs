using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class PetAI : MonoBehaviour
{
	// Token: 0x060001A2 RID: 418 RVA: 0x00008344 File Offset: 0x00006544
	private void Start()
	{
		if (this.setMainCharacterAsMaster)
		{
			this.SetMaster(CharacterMainControl.Main);
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00008359 File Offset: 0x00006559
	public void SetMaster(CharacterMainControl _master)
	{
		if (!_master)
		{
			return;
		}
		this.master = _master;
		_master.OnSetPositionEvent -= this.OnMainCharacterSetPosition;
		_master.OnSetPositionEvent += this.OnMainCharacterSetPosition;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00008390 File Offset: 0x00006590
	private void SyncSpeed()
	{
		if (!this.master)
		{
			return;
		}
		if (this.distanceToMaster > 10f)
		{
			this.walkSpeedStat.BaseValue = this.master.CharacterWalkSpeed + 2f;
			this.runSpeedStat.BaseValue = this.master.CharacterRunSpeed + 2f;
			return;
		}
		if (this.distanceToMaster < 8f)
		{
			this.walkSpeedStat.BaseValue = this.master.CharacterWalkSpeed;
			this.runSpeedStat.BaseValue = this.master.CharacterRunSpeed;
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000842C File Offset: 0x0000662C
	private void InitStats()
	{
		if (!this.selfCharacter)
		{
			return;
		}
		Item characterItem = this.selfCharacter.CharacterItem;
		if (!characterItem)
		{
			return;
		}
		this.statInited = true;
		this.walkSpeedStat = characterItem.Stats["WalkSpeed"];
		this.runSpeedStat = characterItem.Stats["RunSpeed"];
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000848F File Offset: 0x0000668F
	private void OnDestroy()
	{
		if (this.master != null)
		{
			this.master.OnSetPositionEvent -= this.OnMainCharacterSetPosition;
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x000084B6 File Offset: 0x000066B6
	private void Awake()
	{
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x000084B8 File Offset: 0x000066B8
	private void Update()
	{
		if (!this.selfCharacter && this.selfAiCharacterController)
		{
			this.selfCharacter = this.selfAiCharacterController.CharacterMainControl;
		}
		if (this.master != null)
		{
			if (!this.statInited)
			{
				this.InitStats();
			}
			this.distanceToMaster = Vector3.Distance(base.transform.position, this.master.transform.position);
			this.SyncSpeed();
			if (!this.standBy && this.distanceToMaster > 40f)
			{
				this.SetPosition(this.master.transform.position + Vector3.forward * 0.4f + Vector3.up * 0.5f);
			}
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00008590 File Offset: 0x00006790
	private void OnMainCharacterSetPosition(CharacterMainControl character, Vector3 targetPos)
	{
		if (!this.master || !this.master.IsMainCharacter || LevelManager.Instance.IsBaseLevel)
		{
			return;
		}
		this.SetPosition(targetPos + Vector3.forward * 0.4f + Vector3.up * 0.5f);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x000085F3 File Offset: 0x000067F3
	private void SetPosition(Vector3 targetPos)
	{
		this.selfAiCharacterController.CharacterMainControl.SetPosition(targetPos);
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00008606 File Offset: 0x00006806
	public void SetStandBy(bool _standBy, Vector3 pos)
	{
		this.standBy = _standBy;
		this.standByPos = pos;
	}

	// Token: 0x0400015A RID: 346
	[HideInInspector]
	public CharacterMainControl master;

	// Token: 0x0400015B RID: 347
	public AICharacterController selfAiCharacterController;

	// Token: 0x0400015C RID: 348
	public bool setMainCharacterAsMaster;

	// Token: 0x0400015D RID: 349
	public bool standBy;

	// Token: 0x0400015E RID: 350
	public Vector3 standByPos = Vector3.zero;

	// Token: 0x0400015F RID: 351
	private bool statInited;

	// Token: 0x04000160 RID: 352
	private Stat walkSpeedStat;

	// Token: 0x04000161 RID: 353
	private Stat runSpeedStat;

	// Token: 0x04000162 RID: 354
	private CharacterMainControl selfCharacter;

	// Token: 0x04000163 RID: 355
	private float distanceToMaster;
}
