using System;
using System.Collections.Generic;
using Duckov.Quests;
using NodeCanvas.DialogueTrees;
using NodeCanvas.StateMachines;
using Saves;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B0 RID: 432
public class CutScene : MonoBehaviour
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00035643 File Offset: 0x00033843
	private string SaveKey
	{
		get
		{
			return "CutScene_" + this.id;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00035655 File Offset: 0x00033855
	private bool UseTrigger
	{
		get
		{
			return this.playTiming == CutScene.PlayTiming.OnTriggerEnter;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00035660 File Offset: 0x00033860
	private bool HideFSMOwnerField
	{
		get
		{
			return !this.fsmOwner && this.dialogueTreeOwner;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0003567C File Offset: 0x0003387C
	private bool HideDialogueTreeOwnerField
	{
		get
		{
			return this.fsmOwner && !this.dialogueTreeOwner;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0003569B File Offset: 0x0003389B
	private bool Played
	{
		get
		{
			return SavesSystem.Load<bool>(this.SaveKey);
		}
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x000356A8 File Offset: 0x000338A8
	public void MarkPlayed()
	{
		if (string.IsNullOrWhiteSpace(this.id))
		{
			return;
		}
		SavesSystem.Save<bool>(this.SaveKey, true);
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x000356C4 File Offset: 0x000338C4
	private void OnEnable()
	{
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000356C6 File Offset: 0x000338C6
	private void Awake()
	{
		if (this.UseTrigger)
		{
			this.InitializeTrigger();
		}
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x000356D8 File Offset: 0x000338D8
	private void InitializeTrigger()
	{
		if (this.trigger == null)
		{
			Debug.LogError("CutScene想要使用Trigger触发，但没有配置Trigger引用。", this);
		}
		OnTriggerEnterEvent onTriggerEnterEvent = this.trigger.AddComponent<OnTriggerEnterEvent>();
		onTriggerEnterEvent.onlyMainCharacter = true;
		onTriggerEnterEvent.triggerOnce = true;
		onTriggerEnterEvent.DoOnTriggerEnter.AddListener(new UnityAction(this.PlayIfNessisary));
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0003572D File Offset: 0x0003392D
	private void Start()
	{
		if (this.playTiming == CutScene.PlayTiming.Start)
		{
			this.PlayIfNessisary();
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x00035740 File Offset: 0x00033940
	private void Update()
	{
		if (this.playing)
		{
			if (this.fsmOwner)
			{
				if (!this.fsmOwner.isRunning)
				{
					this.playing = false;
					this.OnPlayFinished();
					return;
				}
			}
			else if (this.dialogueTreeOwner && !this.dialogueTreeOwner.isRunning)
			{
				this.playing = false;
				this.OnPlayFinished();
			}
		}
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x000357A4 File Offset: 0x000339A4
	private void OnPlayFinished()
	{
		this.MarkPlayed();
		if (this.setActiveFalseWhenFinished)
		{
			base.gameObject.SetActive(false);
		}
		if (this.playOnce && string.IsNullOrWhiteSpace(this.id))
		{
			Debug.LogError("CutScene没有填写ID，无法记录", base.gameObject);
		}
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x000357F0 File Offset: 0x000339F0
	public void PlayIfNessisary()
	{
		if (this.playOnce && this.Played)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!this.prerequisites.Satisfied())
		{
			return;
		}
		this.Play();
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x00035824 File Offset: 0x00033A24
	public void Play()
	{
		if (this.fsmOwner)
		{
			this.fsmOwner.StartBehaviour();
			this.playing = true;
			return;
		}
		if (this.dialogueTreeOwner)
		{
			if (this.setupActorReferencesUsingIDs)
			{
				this.SetupActors();
			}
			this.dialogueTreeOwner.StartBehaviour();
			this.playing = true;
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00035880 File Offset: 0x00033A80
	private void SetupActors()
	{
		if (this.dialogueTreeOwner == null)
		{
			return;
		}
		if (this.dialogueTreeOwner.behaviour == null)
		{
			Debug.LogError("Dialoguetree没有配置", this.dialogueTreeOwner);
			return;
		}
		foreach (DialogueTree.ActorParameter actorParameter in this.dialogueTreeOwner.behaviour.actorParameters)
		{
			string name = actorParameter.name;
			if (!string.IsNullOrEmpty(name))
			{
				DuckovDialogueActor duckovDialogueActor = DuckovDialogueActor.Get(name);
				if (duckovDialogueActor == null)
				{
					Debug.LogError("未找到actor ID:" + name);
				}
				else
				{
					this.dialogueTreeOwner.SetActorReference(name, duckovDialogueActor);
				}
			}
		}
	}

	// Token: 0x04000B0A RID: 2826
	[SerializeField]
	private string id;

	// Token: 0x04000B0B RID: 2827
	[SerializeField]
	private bool playOnce = true;

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private bool setActiveFalseWhenFinished = true;

	// Token: 0x04000B0D RID: 2829
	[SerializeField]
	private bool setupActorReferencesUsingIDs;

	// Token: 0x04000B0E RID: 2830
	[SerializeField]
	private Collider trigger;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private List<Condition> prerequisites = new List<Condition>();

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private FSMOwner fsmOwner;

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private DialogueTreeController dialogueTreeOwner;

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private CutScene.PlayTiming playTiming;

	// Token: 0x04000B13 RID: 2835
	private bool playing;

	// Token: 0x020004CB RID: 1227
	public enum PlayTiming
	{
		// Token: 0x04001CC7 RID: 7367
		Start,
		// Token: 0x04001CC8 RID: 7368
		OnTriggerEnter = 2,
		// Token: 0x04001CC9 RID: 7369
		Manual
	}
}
