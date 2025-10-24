using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200007C RID: 124
public class SkillHUD : MonoBehaviour
{
	// Token: 0x060004AC RID: 1196 RVA: 0x00015568 File Offset: 0x00013768
	private void Awake()
	{
		this.SyncHud();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00015570 File Offset: 0x00013770
	private void SyncHud()
	{
		if (this.rangeCache < 0f)
		{
			this.rangeCache = this.skillJoystick.joystickRangePercent;
		}
		this.activeParent.SetActive(this.skillHudActive);
		if (this.skillHudActive)
		{
			this.skillIcon.sprite = this.skillKeeper.Skill.icon;
			if (this.skillKeeper.Skill.SkillContext.castRange > 0f)
			{
				this.skillJoystick.canCancle = true;
				this.skillJoystick.joystickRangePercent = this.rangeCache;
				return;
			}
			this.skillJoystick.canCancle = false;
			this.skillJoystick.joystickRangePercent = 0f;
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00015628 File Offset: 0x00013828
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
			this.OnInit();
		}
		if (this.skillHudActive && (this.skillKeeper == null || !this.skillKeeper.CheckSkillAndBinding()))
		{
			this.skillHudActive = false;
			this.SyncHud();
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00015690 File Offset: 0x00013890
	private void OnInit()
	{
		SkillTypes skillTypes = this.skillType;
		if (skillTypes != SkillTypes.itemSkill)
		{
			if (skillTypes == SkillTypes.characterSkill)
			{
				this.skillKeeper = this.characterMainControl.skillAction.characterSkillKeeper;
				this.skillJoystick.UpdateValueEvent.AddListener(new UnityAction<Vector2, bool>(this.touchInputController.SetCharacterSkillAimInput));
				this.skillJoystick.OnTouchEvent.AddListener(new UnityAction(this.touchInputController.StartCharacterSkillAim));
				this.skillJoystick.OnUpEvent.AddListener(new UnityAction<bool>(this.touchInputController.CharacterSkillRelease));
			}
		}
		else
		{
			this.skillKeeper = this.characterMainControl.skillAction.holdItemSkillKeeper;
			this.skillJoystick.UpdateValueEvent.AddListener(new UnityAction<Vector2, bool>(this.touchInputController.SetItemSkillAimInput));
			this.skillJoystick.OnTouchEvent.AddListener(new UnityAction(this.touchInputController.StartItemSkillAim));
			this.skillJoystick.OnUpEvent.AddListener(new UnityAction<bool>(this.touchInputController.ItemSkillRelease));
		}
		CharacterSkillKeeper characterSkillKeeper = this.skillKeeper;
		characterSkillKeeper.OnSkillChanged = (Action)Delegate.Combine(characterSkillKeeper.OnSkillChanged, new Action(this.OnSkillChanged));
		if (this.skillKeeper.CheckSkillAndBinding())
		{
			this.OnSkillChanged();
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x000157E1 File Offset: 0x000139E1
	private void OnSkillChanged()
	{
		this.skillHudActive = this.skillKeeper.CheckSkillAndBinding();
		if (this.skillJoystick.Holding)
		{
			this.skillJoystick.CancleTouch();
		}
		this.SyncHud();
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00015812 File Offset: 0x00013A12
	private void OnDestroy()
	{
		if (this.skillKeeper != null)
		{
			CharacterSkillKeeper characterSkillKeeper = this.skillKeeper;
			characterSkillKeeper.OnSkillChanged = (Action)Delegate.Remove(characterSkillKeeper.OnSkillChanged, new Action(this.OnSkillChanged));
		}
	}

	// Token: 0x040003EF RID: 1007
	private CharacterMainControl characterMainControl;

	// Token: 0x040003F0 RID: 1008
	public CharacterTouchInputControl touchInputController;

	// Token: 0x040003F1 RID: 1009
	public Image skillIcon;

	// Token: 0x040003F2 RID: 1010
	private bool skillHudActive;

	// Token: 0x040003F3 RID: 1011
	public Soda_Joysticks skillJoystick;

	// Token: 0x040003F4 RID: 1012
	public GameObject skillButton;

	// Token: 0x040003F5 RID: 1013
	public GameObject activeParent;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	private SkillTypes skillType;

	// Token: 0x040003F7 RID: 1015
	private CharacterSkillKeeper skillKeeper;

	// Token: 0x040003F8 RID: 1016
	private float rangeCache = -1f;
}
