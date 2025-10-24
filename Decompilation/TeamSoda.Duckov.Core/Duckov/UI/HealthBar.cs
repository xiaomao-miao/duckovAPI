using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A3 RID: 931
	public class HealthBar : MonoBehaviour, IPoolable
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x00073D53 File Offset: 0x00071F53
		// (set) Token: 0x0600213C RID: 8508 RVA: 0x00073D5B File Offset: 0x00071F5B
		public Health target { get; private set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x00073D64 File Offset: 0x00071F64
		private PrefabPool<HealthBar_DamageBar> DamageBarPool
		{
			get
			{
				if (this._damageBarPool == null)
				{
					this._damageBarPool = new PrefabPool<HealthBar_DamageBar>(this.damageBarTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._damageBarPool;
			}
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x00073D9D File Offset: 0x00071F9D
		public void NotifyPooled()
		{
			this.pooled = true;
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x00073DA6 File Offset: 0x00071FA6
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
			this.pooled = false;
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x00073DBC File Offset: 0x00071FBC
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x00073DCF File Offset: 0x00071FCF
		private void OnDestroy()
		{
			this.UnregisterEvents();
			Image image = this.followFill;
			if (image != null)
			{
				image.DOKill(false);
			}
			Image image2 = this.hurtBlink;
			if (image2 == null)
			{
				return;
			}
			image2.DOKill(false);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x00073DFC File Offset: 0x00071FFC
		private void LateUpdate()
		{
			if (this.target == null || !this.target.isActiveAndEnabled || this.target.Hidden)
			{
				this.Release();
				return;
			}
			this.UpdatePosition();
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00073E34 File Offset: 0x00072034
		private bool CheckInFrame()
		{
			this.rectTransform.GetWorldCorners(this.cornersBuffer);
			foreach (Vector3 vector in this.cornersBuffer)
			{
				if (vector.x > 0f && vector.x < (float)Screen.width && vector.y > 0f && vector.y < (float)Screen.height)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00073EAD File Offset: 0x000720AD
		private void UpdateFrame()
		{
			if (this.CheckInFrame())
			{
				this.lastTimeInFrame = Time.unscaledTime;
			}
			if (Time.unscaledTime - this.lastTimeInFrame > this.releaseAfterOutOfFrame)
			{
				this.Release();
			}
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x00073EDC File Offset: 0x000720DC
		private void UpdatePosition()
		{
			Vector3 position = this.target.transform.position + this.displayOffset;
			Vector3 position2 = Camera.main.WorldToScreenPoint(position);
			position2.y += this.screenYOffset * (float)Screen.height;
			base.transform.position = position2;
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00073F38 File Offset: 0x00072138
		public void Setup(Health target, DamageInfo? damage = null, Action releaseAction = null)
		{
			this.releaseAction = releaseAction;
			this.UnregisterEvents();
			if (target == null)
			{
				this.Release();
				return;
			}
			if (target.IsDead)
			{
				this.Release();
				return;
			}
			this.background.SetActive(true);
			this.deathIndicator.SetActive(false);
			this.fill.gameObject.SetActive(true);
			this.followFill.gameObject.SetActive(true);
			this.target = target;
			this.RefreshOffset();
			this.RegisterEvents();
			this.Refresh();
			this.lastTimeInFrame = Time.unscaledTime;
			this.damageBarTemplate.gameObject.SetActive(false);
			if (damage != null)
			{
				this.OnTargetHurt(damage.Value);
			}
			this.UpdatePosition();
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00073FFC File Offset: 0x000721FC
		public void RefreshOffset()
		{
			if (!this.target)
			{
				return;
			}
			this.displayOffset = Vector3.up * 1.5f;
			CharacterMainControl characterMainControl = this.target.TryGetCharacter();
			if (characterMainControl && characterMainControl.characterModel)
			{
				Transform helmatSocket = characterMainControl.characterModel.HelmatSocket;
				if (helmatSocket)
				{
					this.displayOffset = Vector3.up * (Vector3.Distance(characterMainControl.transform.position, helmatSocket.position) + 0.5f);
				}
			}
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00074090 File Offset: 0x00072290
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.RefreshCharacterIcon();
			this.target.OnMaxHealthChange.AddListener(new UnityAction<Health>(this.OnTargetMaxHealthChange));
			this.target.OnHealthChange.AddListener(new UnityAction<Health>(this.OnTargetHealthChange));
			this.target.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnTargetHurt));
			this.target.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnTargetDead));
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x00074124 File Offset: 0x00072324
		private void RefreshCharacterIcon()
		{
			if (!this.target)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			CharacterMainControl characterMainControl = this.target.TryGetCharacter();
			if (!characterMainControl)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
			if (!characterPreset)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			Sprite characterIcon = characterPreset.GetCharacterIcon();
			if (!characterIcon)
			{
				this.levelIcon.gameObject.SetActive(false);
			}
			else
			{
				this.levelIcon.sprite = characterIcon;
				this.levelIcon.gameObject.SetActive(true);
			}
			if (!characterPreset.showName)
			{
				this.nameText.gameObject.SetActive(false);
				return;
			}
			this.nameText.text = characterPreset.DisplayName;
			this.nameText.gameObject.SetActive(true);
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x00074248 File Offset: 0x00072448
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnMaxHealthChange.RemoveListener(new UnityAction<Health>(this.OnTargetMaxHealthChange));
			this.target.OnHealthChange.RemoveListener(new UnityAction<Health>(this.OnTargetHealthChange));
			this.target.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTargetHurt));
			this.target.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTargetDead));
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000742D4 File Offset: 0x000724D4
		private void OnTargetMaxHealthChange(Health obj)
		{
			this.Refresh();
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000742DC File Offset: 0x000724DC
		private void OnTargetHealthChange(Health obj)
		{
			this.Refresh();
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000742E4 File Offset: 0x000724E4
		private void OnTargetHurt(DamageInfo damage)
		{
			Color blinkEndColor = this.blinkColor;
			blinkEndColor.a = 0f;
			if (this.hurtBlink != null)
			{
				this.hurtBlink.DOColor(this.blinkColor, this.blinkDuration).From<TweenerCore<Color, Color, ColorOptions>>().OnKill(delegate
				{
					if (this.hurtBlink != null)
					{
						this.hurtBlink.color = blinkEndColor;
					}
				});
			}
			UnityEvent unityEvent = this.onHurt;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.ShowDamageBar(damage.finalDamage);
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00074374 File Offset: 0x00072574
		private void OnTargetDead(DamageInfo damage)
		{
			this.UnregisterEvents();
			UnityEvent unityEvent = this.onDead;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			if (!damage.toDamageReceiver || !damage.toDamageReceiver.health)
			{
				return;
			}
			this.DeathTask(damage.toDamageReceiver.health).Forget();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000743D0 File Offset: 0x000725D0
		internal void Release()
		{
			if (!this.pooled)
			{
				return;
			}
			if (this.target != null && this.target.IsMainCharacterHealth && !this.target.IsDead && this.target.gameObject.activeInHierarchy)
			{
				return;
			}
			this.UnregisterEvents();
			this.target != null;
			this.target = null;
			Action action = this.releaseAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0007444C File Offset: 0x0007264C
		private void Refresh()
		{
			float currentHealth = this.target.CurrentHealth;
			float maxHealth = this.target.MaxHealth;
			float num = 0f;
			if (maxHealth > 0f)
			{
				num = currentHealth / maxHealth;
			}
			this.fill.fillAmount = num;
			this.fill.color = this.colorOverAmount.Evaluate(num);
			if (this.followFill != null)
			{
				this.followFill.DOKill(false);
				this.followFill.DOFillAmount(num, this.followFillDuration);
			}
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000744D8 File Offset: 0x000726D8
		private void ShowDamageBar(float damageAmount)
		{
			float num = Mathf.Clamp01(damageAmount / this.target.MaxHealth);
			float num2 = Mathf.Clamp01(this.target.CurrentHealth / this.target.MaxHealth);
			float width = this.fill.rectTransform.rect.width;
			float damageBarWidth = width * num;
			float damageBarPostion = width * num2;
			HealthBar_DamageBar damageBar = this.DamageBarPool.Get(null);
			damageBar.Animate(damageBarPostion, damageBarWidth, delegate
			{
				this.DamageBarPool.Release(damageBar);
			}).Forget();
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00074578 File Offset: 0x00072778
		private UniTask DeathTask(Health health)
		{
			HealthBar.<DeathTask>d__52 <DeathTask>d__;
			<DeathTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DeathTask>d__.<>4__this = this;
			<DeathTask>d__.health = health;
			<DeathTask>d__.<>1__state = -1;
			<DeathTask>d__.<>t__builder.Start<HealthBar.<DeathTask>d__52>(ref <DeathTask>d__);
			return <DeathTask>d__.<>t__builder.Task;
		}

		// Token: 0x04001687 RID: 5767
		private RectTransform rectTransform;

		// Token: 0x04001688 RID: 5768
		[SerializeField]
		private GameObject background;

		// Token: 0x04001689 RID: 5769
		[SerializeField]
		private Image fill;

		// Token: 0x0400168A RID: 5770
		[SerializeField]
		private Image followFill;

		// Token: 0x0400168B RID: 5771
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400168C RID: 5772
		[SerializeField]
		private GameObject deathIndicator;

		// Token: 0x0400168D RID: 5773
		[SerializeField]
		private PunchReceiver deathIndicatorPunchReceiver;

		// Token: 0x0400168E RID: 5774
		[SerializeField]
		private Image hurtBlink;

		// Token: 0x0400168F RID: 5775
		[SerializeField]
		private HealthBar_DamageBar damageBarTemplate;

		// Token: 0x04001690 RID: 5776
		[SerializeField]
		private Gradient colorOverAmount = new Gradient();

		// Token: 0x04001691 RID: 5777
		[SerializeField]
		private float followFillDuration = 0.5f;

		// Token: 0x04001692 RID: 5778
		[SerializeField]
		private float blinkDuration = 0.1f;

		// Token: 0x04001693 RID: 5779
		[SerializeField]
		private Color blinkColor = Color.white;

		// Token: 0x04001694 RID: 5780
		private Vector3 displayOffset = Vector3.zero;

		// Token: 0x04001695 RID: 5781
		[SerializeField]
		private float releaseAfterOutOfFrame = 1f;

		// Token: 0x04001696 RID: 5782
		[SerializeField]
		private float disappearDelay = 0.2f;

		// Token: 0x04001697 RID: 5783
		[SerializeField]
		private Image levelIcon;

		// Token: 0x04001698 RID: 5784
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001699 RID: 5785
		[SerializeField]
		private UnityEvent onHurt;

		// Token: 0x0400169A RID: 5786
		[SerializeField]
		private UnityEvent onDead;

		// Token: 0x0400169C RID: 5788
		private Action releaseAction;

		// Token: 0x0400169D RID: 5789
		private float lastTimeInFrame = float.MinValue;

		// Token: 0x0400169E RID: 5790
		private float screenYOffset = 0.02f;

		// Token: 0x0400169F RID: 5791
		private PrefabPool<HealthBar_DamageBar> _damageBarPool;

		// Token: 0x040016A0 RID: 5792
		private bool pooled;

		// Token: 0x040016A1 RID: 5793
		private Vector3[] cornersBuffer = new Vector3[4];
	}
}
