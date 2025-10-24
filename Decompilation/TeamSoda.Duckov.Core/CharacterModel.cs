using System;
using System.Collections.Generic;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CharacterModel : MonoBehaviour
{
	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000367 RID: 871 RVA: 0x0000EE68 File Offset: 0x0000D068
	// (remove) Token: 0x06000368 RID: 872 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
	public event Action<CharacterModel> OnDestroyEvent;

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000369 RID: 873 RVA: 0x0000EED5 File Offset: 0x0000D0D5
	public Transform LefthandSocket
	{
		get
		{
			return this.lefthandSocket;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600036A RID: 874 RVA: 0x0000EEDD File Offset: 0x0000D0DD
	public Transform RightHandSocket
	{
		get
		{
			return this.rightHandSocket;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600036B RID: 875 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
	public Transform ArmorSocket
	{
		get
		{
			return this.armorSocket;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600036C RID: 876 RVA: 0x0000EEED File Offset: 0x0000D0ED
	public Transform HelmatSocket
	{
		get
		{
			return this.helmatSocket;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600036D RID: 877 RVA: 0x0000EEF5 File Offset: 0x0000D0F5
	public Transform FaceMaskSocket
	{
		get
		{
			if (this.faceSocket)
			{
				return this.faceSocket;
			}
			return this.helmatSocket;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600036E RID: 878 RVA: 0x0000EF11 File Offset: 0x0000D111
	public Transform BackpackSocket
	{
		get
		{
			return this.backpackSocket;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600036F RID: 879 RVA: 0x0000EF19 File Offset: 0x0000D119
	public Transform MeleeWeaponSocket
	{
		get
		{
			return this.meleeWeaponSocket;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000370 RID: 880 RVA: 0x0000EF21 File Offset: 0x0000D121
	public Transform PopTextSocket
	{
		get
		{
			return this.popTextSocket;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000371 RID: 881 RVA: 0x0000EF29 File Offset: 0x0000D129
	public CustomFaceInstance CustomFace
	{
		get
		{
			return this.customFace;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000372 RID: 882 RVA: 0x0000EF31 File Offset: 0x0000D131
	public bool Hidden
	{
		get
		{
			return this.characterMainControl.Hidden;
		}
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000373 RID: 883 RVA: 0x0000EF40 File Offset: 0x0000D140
	// (remove) Token: 0x06000374 RID: 884 RVA: 0x0000EF78 File Offset: 0x0000D178
	public event Action OnCharacterSetEvent;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000375 RID: 885 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
	// (remove) Token: 0x06000376 RID: 886 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
	public event Action OnAttackOrShootEvent;

	// Token: 0x06000377 RID: 887 RVA: 0x0000F01D File Offset: 0x0000D21D
	private void Awake()
	{
		this.defaultRightHandLocalRotation = this.rightHandSocket.localRotation;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0000F030 File Offset: 0x0000D230
	private void Start()
	{
		CharacterSubVisuals component = base.GetComponent<CharacterSubVisuals>();
		if (component != null)
		{
			if (this.subVisuals.Contains(component))
			{
				this.RemoveVisual(component);
			}
			this.AddSubVisuals(component);
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0000F069 File Offset: 0x0000D269
	private void LateUpdate()
	{
		if (this.autoSyncRightHandRotation)
		{
			this.SyncRightHandRotation();
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0000F07C File Offset: 0x0000D27C
	public void OnMainCharacterSetted(CharacterMainControl _characterMainControl)
	{
		this.characterMainControl = _characterMainControl;
		if (!this.characterMainControl)
		{
			return;
		}
		if (this.characterMainControl.attackAction)
		{
			this.characterMainControl.attackAction.OnAttack += this.OnAttack;
		}
		this.characterMainControl.OnShootEvent += this.OnShoot;
		this.characterMainControl.EquipmentController.OnHelmatSlotContentChanged += this.OnHelmatSlotContentChange;
		this.characterMainControl.EquipmentController.OnFaceMaskSlotContentChanged += this.OnFaceMaskSlotContentChange;
		if (_characterMainControl.mainDamageReceiver != null)
		{
			CapsuleCollider component = _characterMainControl.mainDamageReceiver.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				component.radius = this.damageReceiverRadius;
				if (this.damageReceiverRadius <= 0f)
				{
					component.enabled = false;
				}
			}
		}
		Action onCharacterSetEvent = this.OnCharacterSetEvent;
		if (onCharacterSetEvent != null)
		{
			onCharacterSetEvent();
		}
		this.hurtVisual.SetHealth(_characterMainControl.Health);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0000F180 File Offset: 0x0000D380
	private void CharacterMainControl_OnShootEvent(DuckovItemAgent obj)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0000F188 File Offset: 0x0000D388
	private void OnHelmatSlotContentChange(Slot slot)
	{
		if (slot == null)
		{
			return;
		}
		this.helmatShowHair = (slot.Content == null || slot.Content.Constants.GetBool(this.showHairHash, false));
		this.helmatShowMouth = (slot.Content == null || slot.Content.Constants.GetBool(this.showMouthHash, true));
		if (this.customFace && this.customFace.hairSocket)
		{
			this.customFace.hairSocket.gameObject.SetActive(this.helmatShowHair && this.faceMaskShowHair);
		}
		if (this.customFace && this.customFace.mouthPart.socket)
		{
			this.customFace.mouthPart.socket.gameObject.SetActive(this.helmatShowMouth && this.faceMaskShowMouth);
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0000F28C File Offset: 0x0000D48C
	private void OnFaceMaskSlotContentChange(Slot slot)
	{
		if (slot == null)
		{
			return;
		}
		this.faceMaskShowHair = (slot.Content == null || slot.Content.Constants.GetBool(this.showHairHash, true));
		this.faceMaskShowMouth = (slot.Content == null || slot.Content.Constants.GetBool(this.showMouthHash, true));
		if (this.customFace && this.customFace.hairSocket)
		{
			this.customFace.hairSocket.gameObject.SetActive(this.helmatShowHair && this.faceMaskShowHair);
		}
		if (this.customFace && this.customFace.mouthPart.socket)
		{
			this.customFace.mouthPart.socket.gameObject.SetActive(this.helmatShowMouth && this.faceMaskShowMouth);
		}
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0000F390 File Offset: 0x0000D590
	private void OnDestroy()
	{
		if (this.destroied)
		{
			return;
		}
		this.destroied = true;
		Action<CharacterModel> onDestroyEvent = this.OnDestroyEvent;
		if (onDestroyEvent != null)
		{
			onDestroyEvent(this);
		}
		if (this.characterMainControl)
		{
			if (this.characterMainControl.attackAction)
			{
				this.characterMainControl.attackAction.OnAttack -= this.OnAttack;
			}
			this.characterMainControl.OnShootEvent -= this.OnShoot;
			this.characterMainControl.EquipmentController.OnHelmatSlotContentChanged -= this.OnHelmatSlotContentChange;
			this.characterMainControl.EquipmentController.OnFaceMaskSlotContentChanged -= this.OnFaceMaskSlotContentChange;
		}
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0000F44C File Offset: 0x0000D64C
	private void SyncRightHandRotation()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		if (this.characterMainControl.Running)
		{
			flag = false;
		}
		Quaternion to;
		if (flag)
		{
			to = Quaternion.LookRotation(this.characterMainControl.CurrentAimDirection, Vector3.up);
		}
		else
		{
			to = this.rightHandSocket.parent.transform.rotation * this.defaultRightHandLocalRotation;
		}
		float maxDegreesDelta = 999f;
		if (!flag2)
		{
			maxDegreesDelta = 360f * Time.deltaTime;
		}
		this.rightHandSocket.rotation = Quaternion.RotateTowards(this.rightHandSocket.rotation, to, maxDegreesDelta);
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0000F4E8 File Offset: 0x0000D6E8
	public void AddSubVisuals(CharacterSubVisuals visuals)
	{
		visuals.mainModel = this;
		if (this.subVisuals.Contains(visuals))
		{
			return;
		}
		this.subVisuals.Add(visuals);
		this.renderers.AddRange(visuals.renderers);
		this.hurtVisual.SetRenderers(this.renderers);
		visuals.SetRenderersHidden(this.Hidden);
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0000F548 File Offset: 0x0000D748
	public void RemoveVisual(CharacterSubVisuals _subVisuals)
	{
		this.subVisuals.Remove(_subVisuals);
		foreach (Renderer item in _subVisuals.renderers)
		{
			this.renderers.Remove(item);
		}
		this.hurtVisual.SetRenderers(this.renderers);
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
	public void SyncHiddenToMainCharacter()
	{
		bool renderersHidden = this.Hidden;
		if (!Team.IsEnemy(Teams.player, this.characterMainControl.Team))
		{
			renderersHidden = false;
		}
		if (this.subVisuals.Count > 0)
		{
			foreach (CharacterSubVisuals characterSubVisuals in this.subVisuals)
			{
				if (!(characterSubVisuals == null))
				{
					characterSubVisuals.SetRenderersHidden(renderersHidden);
				}
			}
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0000F648 File Offset: 0x0000D848
	public void SetFaceFromPreset(CustomFacePreset preset)
	{
		if (preset == null)
		{
			return;
		}
		if (!this.customFace)
		{
			return;
		}
		this.customFace.LoadFromData(preset.settings);
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0000F673 File Offset: 0x0000D873
	public void SetFaceFromData(CustomFaceSettingData data)
	{
		if (!this.customFace)
		{
			return;
		}
		this.customFace.LoadFromData(data);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0000F68F File Offset: 0x0000D88F
	private void OnAttack()
	{
		Action onAttackOrShootEvent = this.OnAttackOrShootEvent;
		if (onAttackOrShootEvent == null)
		{
			return;
		}
		onAttackOrShootEvent();
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0000F6A1 File Offset: 0x0000D8A1
	public void ForcePlayAttackAnimation()
	{
		this.OnAttack();
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0000F6A9 File Offset: 0x0000D8A9
	private void OnShoot(DuckovItemAgent agent)
	{
		Action onAttackOrShootEvent = this.OnAttackOrShootEvent;
		if (onAttackOrShootEvent == null)
		{
			return;
		}
		onAttackOrShootEvent();
	}

	// Token: 0x0400028C RID: 652
	public CharacterMainControl characterMainControl;

	// Token: 0x0400028D RID: 653
	public bool invisable;

	// Token: 0x0400028F RID: 655
	[SerializeField]
	private Transform lefthandSocket;

	// Token: 0x04000290 RID: 656
	[SerializeField]
	private Transform rightHandSocket;

	// Token: 0x04000291 RID: 657
	private Quaternion defaultRightHandLocalRotation;

	// Token: 0x04000292 RID: 658
	[SerializeField]
	private HurtVisual hurtVisual;

	// Token: 0x04000293 RID: 659
	[SerializeField]
	private Transform armorSocket;

	// Token: 0x04000294 RID: 660
	[SerializeField]
	private Transform helmatSocket;

	// Token: 0x04000295 RID: 661
	[SerializeField]
	private Transform faceSocket;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	private Transform backpackSocket;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	private Transform meleeWeaponSocket;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private Transform popTextSocket;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	private List<CharacterSubVisuals> subVisuals;

	// Token: 0x0400029A RID: 666
	[SerializeField]
	private List<Renderer> renderers;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private CustomFaceInstance customFace;

	// Token: 0x0400029C RID: 668
	public bool autoSyncRightHandRotation = true;

	// Token: 0x0400029D RID: 669
	public float damageReceiverRadius = 0.45f;

	// Token: 0x0400029E RID: 670
	private int showHairHash = "ShowHair".GetHashCode();

	// Token: 0x0400029F RID: 671
	private int showMouthHash = "ShowMouth".GetHashCode();

	// Token: 0x040002A2 RID: 674
	private bool helmatShowMouth = true;

	// Token: 0x040002A3 RID: 675
	private bool helmatShowHair = true;

	// Token: 0x040002A4 RID: 676
	private bool faceMaskShowHair = true;

	// Token: 0x040002A5 RID: 677
	private bool faceMaskShowMouth = true;

	// Token: 0x040002A6 RID: 678
	private bool destroied;
}
