using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000B8 RID: 184
public class BulletCountHUD : MonoBehaviour
{
	// Token: 0x060005F7 RID: 1527 RVA: 0x0001A90A File Offset: 0x00018B0A
	private void Awake()
	{
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0001A90C File Offset: 0x00018B0C
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnHoldAgentChanged += this.OnHoldAgentChanged;
				this.characterMainControl.CharacterItem.Inventory.onContentChanged += this.OnInventoryChanged;
				if (this.characterMainControl.CurrentHoldItemAgent != null)
				{
					this.OnHoldAgentChanged(this.characterMainControl.CurrentHoldItemAgent);
				}
				this.ChangeTotalCount();
				this.capacityText.text = this.totalCount.ToString("D2");
			}
		}
		if (this.gunAgnet == null)
		{
			this.canvasGroup.alpha = 0f;
			return;
		}
		bool flag = false;
		this.canvasGroup.alpha = 1f;
		int num = this.gunAgnet.BulletCount;
		if (this.bulletCount != num)
		{
			this.bulletCount = num;
			this.bulletCountText.text = num.ToString("D2");
			flag = true;
		}
		if (flag)
		{
			UnityEvent onValueChangeEvent = this.OnValueChangeEvent;
			if (onValueChangeEvent != null)
			{
				onValueChangeEvent.Invoke();
			}
			if (this.bulletCount <= 0 && (this.totalCount <= 0 || !this.capacityText.gameObject.activeInHierarchy))
			{
				this.background.color = this.emptyBackgroundColor;
				return;
			}
			this.background.color = this.normalBackgroundColor;
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001AA84 File Offset: 0x00018C84
	private void OnInventoryChanged(Inventory inventory, int index)
	{
		this.ChangeTotalCount();
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001AA8C File Offset: 0x00018C8C
	private void ChangeTotalCount()
	{
		int num = 0;
		if (this.gunAgnet)
		{
			num = this.gunAgnet.GetBulletCountInInventory();
		}
		if (this.totalCount != num)
		{
			this.totalCount = num;
			this.capacityText.text = this.totalCount.ToString("D2");
		}
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001AAE0 File Offset: 0x00018CE0
	private void OnDestroy()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.OnHoldAgentChanged -= this.OnHoldAgentChanged;
			this.characterMainControl.CharacterItem.Inventory.onContentChanged -= this.OnInventoryChanged;
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0001AB32 File Offset: 0x00018D32
	private void OnHoldAgentChanged(DuckovItemAgent newAgent)
	{
		if (newAgent == null)
		{
			this.gunAgnet = null;
		}
		this.gunAgnet = (newAgent as ItemAgent_Gun);
		this.ChangeTotalCount();
	}

	// Token: 0x04000580 RID: 1408
	private ItemAgent_Gun gunAgent;

	// Token: 0x04000581 RID: 1409
	private CharacterMainControl characterMainControl;

	// Token: 0x04000582 RID: 1410
	private ItemAgent_Gun gunAgnet;

	// Token: 0x04000583 RID: 1411
	public CanvasGroup canvasGroup;

	// Token: 0x04000584 RID: 1412
	public TextMeshProUGUI bulletCountText;

	// Token: 0x04000585 RID: 1413
	public TextMeshProUGUI capacityText;

	// Token: 0x04000586 RID: 1414
	public ProceduralImage background;

	// Token: 0x04000587 RID: 1415
	public Color normalBackgroundColor;

	// Token: 0x04000588 RID: 1416
	public Color emptyBackgroundColor;

	// Token: 0x04000589 RID: 1417
	private int bulletCount = -1;

	// Token: 0x0400058A RID: 1418
	private int totalCount = -1;

	// Token: 0x0400058B RID: 1419
	public UnityEvent OnValueChangeEvent;
}
