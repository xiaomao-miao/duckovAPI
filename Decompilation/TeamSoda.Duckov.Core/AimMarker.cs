using System;
using System.Collections.Generic;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000076 RID: 118
public class AimMarker : MonoBehaviour
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x0600044F RID: 1103 RVA: 0x00013B1C File Offset: 0x00011D1C
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

	// Token: 0x06000450 RID: 1104 RVA: 0x00013B74 File Offset: 0x00011D74
	private void Awake()
	{
		if (!this.currentAdsAimMarker)
		{
			this.SwitchAdsAimMarker(this.defaultAdsAimMarker);
		}
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00013B8F File Offset: 0x00011D8F
	private void Start()
	{
		this.rootCanvasGroup.alpha = 1f;
		ItemAgent_Gun.OnMainCharacterShootEvent += this.OnMainCharacterShoot;
		Health.OnDead += this.OnKill;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00013BC3 File Offset: 0x00011DC3
	private void OnDestroy()
	{
		ItemAgent_Gun.OnMainCharacterShootEvent -= this.OnMainCharacterShoot;
		Health.OnDead -= this.OnKill;
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00013BE8 File Offset: 0x00011DE8
	private void Update()
	{
		this.aimMarkerAnimator.SetBool(this.inProgressHash, this.reloadProgressBar.InProgress);
		if (this.killMarkerTimer > 0f)
		{
			this.killMarkerTimer -= Time.deltaTime;
			this.aimMarkerAnimator.SetBool(this.killMarkerHash, this.killMarkerTimer > 0f);
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		if (main.Health.IsDead)
		{
			this.rootCanvasGroup.alpha = 0f;
			return;
		}
		InputManager inputManager = LevelManager.Instance.InputManager;
		if (inputManager == null)
		{
			return;
		}
		Vector3 inputAimPoint = inputManager.InputAimPoint;
		Vector3 aimMarkerPosScreenSpace = this.MainCam.WorldToScreenPoint(inputAimPoint);
		aimMarkerPosScreenSpace = inputManager.AimScreenPoint;
		this.SetAimMarkerPosScreenSpace(aimMarkerPosScreenSpace);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00013CB8 File Offset: 0x00011EB8
	private void LateUpdate()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		InputManager inputManager = LevelManager.Instance.InputManager;
		if (inputManager == null)
		{
			return;
		}
		Vector3 inputAimPoint = inputManager.InputAimPoint;
		ItemAgent_Gun gun = main.GetGun();
		Color color = this.distanceTextColorFull;
		float num;
		if (gun != null)
		{
			if (this.adsValue == 0f && gun.AdsValue > 0f)
			{
				this.OnStartAdsWithGun(gun);
			}
			this.adsValue = gun.AdsValue;
			this.scatter = Mathf.MoveTowards(this.scatter, gun.CurrentScatter, 500f * Time.deltaTime);
			this.minScatter = Mathf.MoveTowards(this.minScatter, gun.MinScatter, 500f * Time.deltaTime);
			this.left.anchoredPosition = Vector3.left * (20f + this.scatter * 5f);
			this.right.anchoredPosition = Vector3.right * (20f + this.scatter * 5f);
			this.up.anchoredPosition = Vector3.up * (20f + this.scatter * 5f);
			this.down.anchoredPosition = Vector3.down * (20f + this.scatter * 5f);
			num = Vector3.Distance(inputAimPoint, gun.muzzle.position);
			float bulletDistance = gun.BulletDistance;
			if (num < bulletDistance * 0.495f)
			{
				color = this.distanceTextColorFull;
			}
			else if (num < bulletDistance)
			{
				color = this.distanceTextColorHalf;
			}
			else
			{
				color = this.distanceTextColorOver;
			}
		}
		else
		{
			this.adsValue = 0f;
			this.scatter = 0f;
			this.minScatter = 0f;
			num = Vector3.Distance(inputAimPoint, main.transform.position + Vector3.up * 0.5f);
			color = this.distanceTextColorFull;
		}
		float alpha = Mathf.Clamp01((0.5f - this.adsValue) * 2f);
		if (this.currentAdsAimMarker)
		{
			this.currentAdsAimMarker.SetScatter(this.scatter, this.minScatter);
			this.currentAdsAimMarker.SetAdsValue(this.adsValue);
			if (!this.currentAdsAimMarker.hideNormalCrosshair)
			{
				alpha = 1f;
			}
		}
		else
		{
			alpha = 1f;
		}
		this.normalAimCanvasGroup.alpha = alpha;
		if (this.distanceText)
		{
			this.distanceText.text = num.ToString("00") + " M";
			this.distanceText.color = color;
			this.distanceGlow.Color = color;
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00013F93 File Offset: 0x00012193
	public void SetAimMarkerPosScreenSpace(Vector3 pos)
	{
		this.aimMarkerUI.position = pos;
		if (this.currentAdsAimMarker)
		{
			this.currentAdsAimMarker.SetAimMarkerPos(pos);
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00013FBC File Offset: 0x000121BC
	private void OnStartAdsWithGun(ItemAgent_Gun gun)
	{
		ADSAimMarker aimMarkerPfb = gun.GetAimMarkerPfb();
		if (!aimMarkerPfb)
		{
			return;
		}
		this.SwitchAdsAimMarker(aimMarkerPfb);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00013FE0 File Offset: 0x000121E0
	private void SwitchAdsAimMarker(ADSAimMarker newAimMarkerPfb)
	{
		if (newAimMarkerPfb == null)
		{
			UnityEngine.Object.Destroy(this.currentAdsAimMarker.gameObject);
			this.currentAdsAimMarker = null;
			return;
		}
		if (this.currentAdsAimMarker && newAimMarkerPfb == this.currentAdsAimMarker.selfPrefab)
		{
			return;
		}
		if (this.currentAdsAimMarker)
		{
			UnityEngine.Object.Destroy(this.currentAdsAimMarker.gameObject);
		}
		this.currentAdsAimMarker = UnityEngine.Object.Instantiate<ADSAimMarker>(newAimMarkerPfb);
		this.currentAdsAimMarker.selfPrefab = newAimMarkerPfb;
		this.currentAdsAimMarker.transform.SetParent(base.transform);
		this.currentAdsAimMarker.parentAimMarker = this;
		RectTransform rectTransform = this.currentAdsAimMarker.transform as RectTransform;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.sizeDelta = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x000140CC File Offset: 0x000122CC
	private void SetAimMarkerColor(Color col)
	{
		int count = this.aimMarkerImages.Count;
		for (int i = 0; i < count; i++)
		{
			this.aimMarkerImages[i].color = col;
		}
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00014103 File Offset: 0x00012303
	private void OnKill(Health _health, DamageInfo dmgInfo)
	{
		if (_health == null || _health.team == Teams.player)
		{
			return;
		}
		this.killMarkerTimer = this.killMarkerTime;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00014123 File Offset: 0x00012323
	private void OnMainCharacterShoot(ItemAgent_Gun gunAgnet)
	{
		UnityEvent unityEvent = this.onShoot;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.currentAdsAimMarker)
		{
			this.currentAdsAimMarker.OnShoot();
		}
	}

	// Token: 0x040003AA RID: 938
	public RectTransform aimMarkerUI;

	// Token: 0x040003AB RID: 939
	public List<Image> aimMarkerImages;

	// Token: 0x040003AC RID: 940
	public RectTransform left;

	// Token: 0x040003AD RID: 941
	public RectTransform right;

	// Token: 0x040003AE RID: 942
	public RectTransform up;

	// Token: 0x040003AF RID: 943
	public RectTransform down;

	// Token: 0x040003B0 RID: 944
	private float scatter;

	// Token: 0x040003B1 RID: 945
	private float minScatter;

	// Token: 0x040003B2 RID: 946
	public CanvasGroup rootCanvasGroup;

	// Token: 0x040003B3 RID: 947
	public CanvasGroup normalAimCanvasGroup;

	// Token: 0x040003B4 RID: 948
	public Animator aimMarkerAnimator;

	// Token: 0x040003B5 RID: 949
	public ActionProgressHUD reloadProgressBar;

	// Token: 0x040003B6 RID: 950
	public UnityEvent onShoot;

	// Token: 0x040003B7 RID: 951
	private ADSAimMarker currentAdsAimMarker;

	// Token: 0x040003B8 RID: 952
	[SerializeField]
	private ADSAimMarker defaultAdsAimMarker;

	// Token: 0x040003B9 RID: 953
	private readonly int inProgressHash = Animator.StringToHash("InProgress");

	// Token: 0x040003BA RID: 954
	private readonly int killMarkerHash = Animator.StringToHash("KillMarkerShow");

	// Token: 0x040003BB RID: 955
	[SerializeField]
	private TextMeshProUGUI distanceText;

	// Token: 0x040003BC RID: 956
	[SerializeField]
	private TrueShadow distanceGlow;

	// Token: 0x040003BD RID: 957
	[SerializeField]
	private Color distanceTextColorFull;

	// Token: 0x040003BE RID: 958
	[SerializeField]
	private Color distanceTextColorHalf;

	// Token: 0x040003BF RID: 959
	[SerializeField]
	private Color distanceTextColorOver;

	// Token: 0x040003C0 RID: 960
	private float adsValue;

	// Token: 0x040003C1 RID: 961
	private float killMarkerTime = 0.6f;

	// Token: 0x040003C2 RID: 962
	private float killMarkerTimer;

	// Token: 0x040003C3 RID: 963
	private Camera _cam;
}
