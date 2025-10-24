using System;
using System.Collections.Generic;
using Duckov;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200007A RID: 122
public class HitMarker : MonoBehaviour
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x0600049A RID: 1178 RVA: 0x00014F30 File Offset: 0x00013130
	// (remove) Token: 0x0600049B RID: 1179 RVA: 0x00014F64 File Offset: 0x00013164
	public static event Action OnHitMarker;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x0600049C RID: 1180 RVA: 0x00014F98 File Offset: 0x00013198
	// (remove) Token: 0x0600049D RID: 1181 RVA: 0x00014FCC File Offset: 0x000131CC
	public static event Action OnKillMarker;

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600049E RID: 1182 RVA: 0x00015000 File Offset: 0x00013200
	private Camera MainCam
	{
		get
		{
			if (!this._cam)
			{
				if (LevelManager.Instance == null)
				{
					return null;
				}
				if (LevelManager.Instance.GameCamera == null)
				{
					return null;
				}
				this._cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			return this._cam;
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00015058 File Offset: 0x00013258
	private void Awake()
	{
		Health.OnHurt += this.OnHealthHitEvent;
		Health.OnDead += this.OnHealthKillEvent;
		HealthSimpleBase.OnSimpleHealthHit += this.OnSimpleHealthHit;
		HealthSimpleBase.OnSimpleHealthDead += this.OnSimpleHealthKill;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x000150AC File Offset: 0x000132AC
	private void OnDestroy()
	{
		Health.OnHurt -= this.OnHealthHitEvent;
		Health.OnDead -= this.OnHealthKillEvent;
		HealthSimpleBase.OnSimpleHealthHit -= this.OnSimpleHealthHit;
		HealthSimpleBase.OnSimpleHealthDead -= this.OnSimpleHealthKill;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000150FD File Offset: 0x000132FD
	private void OnHealthHitEvent(Health _health, DamageInfo dmgInfo)
	{
		if (dmgInfo.isFromBuffOrEffect)
		{
			return;
		}
		if (dmgInfo.damageValue <= 1.01f)
		{
			return;
		}
		this.OnHit(dmgInfo);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00015120 File Offset: 0x00013320
	private void OnHit(DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		if (dmgInfo.toDamageReceiver && dmgInfo.toDamageReceiver.IsMainCharacter)
		{
			return;
		}
		bool flag = (float)dmgInfo.crit > 0f;
		Vector3 v = this.MainCam.WorldToScreenPoint(dmgInfo.damagePoint);
		Vector2 v2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, v, null, out v2);
		base.transform.localPosition = Vector3.ClampMagnitude(v2, 10f);
		ItemAgent_Gun gun = CharacterMainControl.Main.GetGun();
		if (gun != null)
		{
			this.scatterOnHit = gun.CurrentScatter;
		}
		int stateHashName = flag ? (this.hitMarkerIndex ? this.critHash1 : this.critHash2) : (this.hitMarkerIndex ? this.hitHash1 : this.hitHash2);
		int shortNameHash = this.animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
		if (shortNameHash != this.killHash && shortNameHash != this.killCritHash)
		{
			this.hitMarkerIndex = !this.hitMarkerIndex;
			this.animator.CrossFade(stateHashName, 0.02f);
		}
		Action onHitMarker = HitMarker.OnHitMarker;
		if (onHitMarker != null)
		{
			onHitMarker();
		}
		if (!dmgInfo.toDamageReceiver || !dmgInfo.toDamageReceiver.useSimpleHealth)
		{
			AudioManager.PostHitMarker(flag);
		}
		UnityEvent unityEvent = this.hitEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0001529D File Offset: 0x0001349D
	private void OnHealthKillEvent(Health _health, DamageInfo dmgInfo)
	{
		this.OnKill(dmgInfo);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000152A8 File Offset: 0x000134A8
	private void OnKill(DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		if (dmgInfo.toDamageReceiver && dmgInfo.toDamageReceiver.IsMainCharacter)
		{
			return;
		}
		bool flag = (float)dmgInfo.crit > 0f;
		int stateHashName = flag ? this.killCritHash : this.killHash;
		this.animator.CrossFade(stateHashName, 0.02f);
		if (!dmgInfo.toDamageReceiver || !dmgInfo.toDamageReceiver.useSimpleHealth)
		{
			AudioManager.PostKillMarker(flag);
		}
		Action onKillMarker = HitMarker.OnKillMarker;
		if (onKillMarker != null)
		{
			onKillMarker();
		}
		UnityEvent unityEvent = this.killEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0001535D File Offset: 0x0001355D
	private void OnSimpleHealthHit(HealthSimpleBase health, DamageInfo dmgInfo)
	{
		if (dmgInfo.damageValue <= 1.01f)
		{
			return;
		}
		this.OnHit(dmgInfo);
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00015374 File Offset: 0x00013574
	private void OnSimpleHealthKill(HealthSimpleBase health, DamageInfo dmgInfo)
	{
		this.OnKill(dmgInfo);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00015380 File Offset: 0x00013580
	private void LateUpdate()
	{
		foreach (RectTransform rectTransform in this.hitMarkerImages)
		{
			rectTransform.anchoredPosition += rectTransform.anchoredPosition.normalized * this.scatterOnHit * 3f;
		}
	}

	// Token: 0x040003D8 RID: 984
	public UnityEvent hitEvent;

	// Token: 0x040003D9 RID: 985
	public UnityEvent killEvent;

	// Token: 0x040003DC RID: 988
	public Animator animator;

	// Token: 0x040003DD RID: 989
	private readonly int hitHash1 = Animator.StringToHash("HitMarkerHit1");

	// Token: 0x040003DE RID: 990
	private readonly int hitHash2 = Animator.StringToHash("HitMarkerHit2");

	// Token: 0x040003DF RID: 991
	private readonly int critHash1 = Animator.StringToHash("HitMarkerCrit1");

	// Token: 0x040003E0 RID: 992
	private readonly int critHash2 = Animator.StringToHash("HitMarkerCrit2");

	// Token: 0x040003E1 RID: 993
	private bool hitMarkerIndex;

	// Token: 0x040003E2 RID: 994
	private readonly int killHash = Animator.StringToHash("HitMarkerKill");

	// Token: 0x040003E3 RID: 995
	private readonly int killCritHash = Animator.StringToHash("HitMarkerKillCrit");

	// Token: 0x040003E4 RID: 996
	public List<RectTransform> hitMarkerImages;

	// Token: 0x040003E5 RID: 997
	private float scatterOnHit;

	// Token: 0x040003E6 RID: 998
	private Camera _cam;
}
