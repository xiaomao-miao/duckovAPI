using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000B9 RID: 185
public class BulletTypeHUD : MonoBehaviour
{
	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001AB6C File Offset: 0x00018D6C
	private PrefabPool<BulletTypeSelectButton> Selections
	{
		get
		{
			if (this._selectionsCache == null)
			{
				this._selectionsCache = new PrefabPool<BulletTypeSelectButton>(this.originSelectButton, null, null, null, null, true, 10, 10000, null);
			}
			return this._selectionsCache;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001ABA8 File Offset: 0x00018DA8
	private bool CanOpenList
	{
		get
		{
			return this.characterMainControl && (!this.characterMainControl.CurrentAction || !this.characterMainControl.CurrentAction.Running) && InputManager.InputActived;
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001ABF4 File Offset: 0x00018DF4
	private void Awake()
	{
		this.selectionsHUD = new List<BulletTypeSelectButton>();
		this.originSelectButton.gameObject.SetActive(false);
		WeaponButton.OnWeaponButtonSelected += this.OnWeaponButtonSelected;
		this.typeList.SetActive(false);
		InputManager.OnSwitchBulletTypeInput += this.OnSwitchInput;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001AC4C File Offset: 0x00018E4C
	private void OnDestroy()
	{
		WeaponButton.OnWeaponButtonSelected -= this.OnWeaponButtonSelected;
		if (this.characterMainControl)
		{
			this.characterMainControl.OnHoldAgentChanged -= this.OnHoldAgentChanged;
		}
		InputManager.OnSwitchBulletTypeInput -= this.OnSwitchInput;
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001ACA0 File Offset: 0x00018EA0
	private void OnWeaponButtonSelected(WeaponButton button)
	{
		Transform transform = this.canvasGroup.transform as RectTransform;
		RectTransform rectTransform = button.transform as RectTransform;
		transform.position = rectTransform.position + (rectTransform.rect.center + (rectTransform.rect.height / 2f + 8f) * rectTransform.up) * rectTransform.lossyScale;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001AD30 File Offset: 0x00018F30
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnHoldAgentChanged += this.OnHoldAgentChanged;
				if (this.characterMainControl.CurrentHoldItemAgent != null)
				{
					this.OnHoldAgentChanged(this.characterMainControl.CurrentHoldItemAgent);
				}
			}
		}
		if (this.gunAgent == null)
		{
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			return;
		}
		this.canvasGroup.alpha = 1f;
		this.canvasGroup.interactable = true;
		if (this.bulletTypeText != null && this.gunAgent.GunItemSetting != null)
		{
			int targetBulletID = this.gunAgent.GunItemSetting.TargetBulletID;
			if (this.bulletTpyeID != targetBulletID)
			{
				this.bulletTpyeID = targetBulletID;
				if (this.bulletTpyeID >= 0)
				{
					this.bulletTypeText.text = this.gunAgent.GunItemSetting.CurrentBulletName;
					this.bulletTypeText.color = Color.black;
					this.background.color = this.normalColor;
				}
				else
				{
					this.bulletTypeText.text = "UI_Bullet_NotAssigned".ToPlainText();
					this.bulletTypeText.color = Color.white;
					this.background.color = this.emptyColor;
				}
				UnityEvent onTypeChangeEvent = this.OnTypeChangeEvent;
				if (onTypeChangeEvent != null)
				{
					onTypeChangeEvent.Invoke();
				}
			}
		}
		if (this.listOpen && !this.CanOpenList)
		{
			this.CloseList();
		}
		if (CharacterInputControl.GetChangeBulletTypeWasPressed())
		{
			if (!this.listOpen)
			{
				this.OpenList();
				return;
			}
			if (this.selectIndex < this.selectionsHUD.Count && this.selectionsHUD[this.selectIndex] != null)
			{
				this.SetBulletType(this.selectionsHUD[this.selectIndex].BulletTypeID);
			}
			this.CloseList();
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001AF3C File Offset: 0x0001913C
	private void OnHoldAgentChanged(DuckovItemAgent newAgent)
	{
		if (newAgent == null)
		{
			this.gunAgent = null;
		}
		this.gunAgent = (newAgent as ItemAgent_Gun);
		this.CloseList();
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001AF60 File Offset: 0x00019160
	private void OnSwitchInput(int dir)
	{
		if (!this.listOpen)
		{
			return;
		}
		this.selectIndex -= dir;
		if (this.totalSelctionCount == 0)
		{
			this.selectIndex = 0;
		}
		else if (this.selectIndex >= this.totalSelctionCount)
		{
			this.selectIndex = 0;
		}
		else if (this.selectIndex < 0)
		{
			this.selectIndex = this.totalSelctionCount - 1;
		}
		for (int i = 0; i < this.selectionsHUD.Count; i++)
		{
			this.selectionsHUD[i].SetSelection(i == this.selectIndex);
		}
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0001AFF4 File Offset: 0x000191F4
	private void OpenList()
	{
		Debug.Log("OpenList");
		if (!this.CanOpenList)
		{
			return;
		}
		if (this.listOpen)
		{
			return;
		}
		this.typeList.SetActive(true);
		this.listOpen = true;
		this.indicator.SetActive(false);
		this.RefreshContent();
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0001B042 File Offset: 0x00019242
	public void CloseList()
	{
		if (!this.listOpen)
		{
			return;
		}
		this.typeList.SetActive(false);
		this.listOpen = false;
		this.indicator.SetActive(true);
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0001B06C File Offset: 0x0001926C
	private void RefreshContent()
	{
		this.selectionsHUD.Clear();
		this.Selections.ReleaseAll();
		Dictionary<int, BulletTypeInfo> dictionary = new Dictionary<int, BulletTypeInfo>();
		ItemSetting_Gun gunItemSetting = this.gunAgent.GunItemSetting;
		if (gunItemSetting != null)
		{
			dictionary = gunItemSetting.GetBulletTypesInInventory(this.characterMainControl.CharacterItem.Inventory);
		}
		if (this.bulletTpyeID > 0 && !dictionary.ContainsKey(this.bulletTpyeID))
		{
			BulletTypeInfo bulletTypeInfo = new BulletTypeInfo();
			bulletTypeInfo.bulletTypeID = this.bulletTpyeID;
			bulletTypeInfo.count = 0;
			dictionary.Add(this.bulletTpyeID, bulletTypeInfo);
		}
		if (dictionary.Count <= 0)
		{
			dictionary.Add(-1, new BulletTypeInfo
			{
				bulletTypeID = -1,
				count = 0
			});
		}
		this.totalSelctionCount = dictionary.Count;
		int num = 0;
		this.selectIndex = 0;
		foreach (KeyValuePair<int, BulletTypeInfo> keyValuePair in dictionary)
		{
			BulletTypeSelectButton bulletTypeSelectButton = this.Selections.Get(this.typeList.transform);
			bulletTypeSelectButton.gameObject.SetActive(true);
			bulletTypeSelectButton.transform.SetAsLastSibling();
			bulletTypeSelectButton.Init(keyValuePair.Value.bulletTypeID, keyValuePair.Value.count);
			if (this.bulletTpyeID == keyValuePair.Value.bulletTypeID)
			{
				bulletTypeSelectButton.SetSelection(true);
				this.selectIndex = num;
			}
			this.selectionsHUD.Add(bulletTypeSelectButton);
			Debug.Log(string.Format("BUlletType {0}:{1}", this.selectIndex, keyValuePair.Value.bulletTypeID));
			num++;
		}
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0001B230 File Offset: 0x00019430
	public void SetBulletType(int typeID)
	{
		this.CloseList();
		if (!this.gunAgent || !this.gunAgent.GunItemSetting)
		{
			return;
		}
		bool flag = this.gunAgent.GunItemSetting.TargetBulletID != typeID;
		this.gunAgent.GunItemSetting.SetTargetBulletType(typeID);
		if (flag)
		{
			this.characterMainControl.TryToReload(null);
		}
	}

	// Token: 0x0400058C RID: 1420
	private CharacterMainControl characterMainControl;

	// Token: 0x0400058D RID: 1421
	private ItemAgent_Gun gunAgent;

	// Token: 0x0400058E RID: 1422
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x0400058F RID: 1423
	[SerializeField]
	private TextMeshProUGUI bulletTypeText;

	// Token: 0x04000590 RID: 1424
	[SerializeField]
	private ProceduralImage background;

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000592 RID: 1426
	[SerializeField]
	private Color emptyColor;

	// Token: 0x04000593 RID: 1427
	private int bulletTpyeID = -2;

	// Token: 0x04000594 RID: 1428
	[SerializeField]
	private GameObject typeList;

	// Token: 0x04000595 RID: 1429
	public UnityEvent OnTypeChangeEvent;

	// Token: 0x04000596 RID: 1430
	public GameObject indicator;

	// Token: 0x04000597 RID: 1431
	private int selectIndex;

	// Token: 0x04000598 RID: 1432
	private int totalSelctionCount;

	// Token: 0x04000599 RID: 1433
	[SerializeField]
	private BulletTypeSelectButton originSelectButton;

	// Token: 0x0400059A RID: 1434
	private List<BulletTypeSelectButton> selectionsHUD;

	// Token: 0x0400059B RID: 1435
	private PrefabPool<BulletTypeSelectButton> _selectionsCache;

	// Token: 0x0400059C RID: 1436
	private bool listOpen;
}
