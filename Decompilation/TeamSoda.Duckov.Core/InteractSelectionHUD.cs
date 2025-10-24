using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C3 RID: 195
public class InteractSelectionHUD : MonoBehaviour
{
	// Token: 0x1700012C RID: 300
	// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001BC62 File Offset: 0x00019E62
	public InteractableBase InteractTarget
	{
		get
		{
			return this.interactable;
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0001BC6A File Offset: 0x00019E6A
	public void SetInteractable(InteractableBase _interactable, bool _hasUpDown)
	{
		this.interactable = _interactable;
		this.text.text = this.interactable.GetInteractName();
		this.UpdateRequireItem(this.interactable);
		this.selectionPoint.SetActive(_hasUpDown);
		this.hasUpDown = _hasUpDown;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0001BCA8 File Offset: 0x00019EA8
	private void UpdateRequireItem(InteractableBase interactable)
	{
		if (!interactable || !interactable.requireItem)
		{
			this.requireCanvasGroup.alpha = 0f;
			return;
		}
		this.requireCanvasGroup.alpha = 1f;
		CharacterMainControl mainCharacter = LevelManager.Instance.MainCharacter;
		bool flag = interactable.whenToUseRequireItem > InteractableBase.WhenToUseRequireItemTypes.None;
		string str = flag ? this.requirUseItemTextKey.ToPlainText() : this.requirItemTextKey.ToPlainText();
		this.requireText.text = str + " " + interactable.GetRequiredItemName();
		if (flag)
		{
			TextMeshProUGUI textMeshProUGUI = this.requireText;
			textMeshProUGUI.text += " x1";
		}
		this.requirementIcon.sprite = interactable.GetRequireditemIcon();
		if (interactable.TryGetRequiredItem(mainCharacter).Item1)
		{
			this.requireItemBackgroundImage.color = this.hasRequireItemColor;
			return;
		}
		this.requireItemBackgroundImage.color = this.noRequireItemColor;
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0001BD94 File Offset: 0x00019F94
	public void SetSelection(bool _select)
	{
		this.selecting = _select;
		this.selectIndicator.SetActive(this.selecting);
		this.upDownIndicator.SetActive(this.selecting && this.hasUpDown);
		this.selectionPoint.SetActive(!this.selecting && this.hasUpDown);
		if (_select)
		{
			UnityEvent onSelectedEvent = this.OnSelectedEvent;
			if (onSelectedEvent != null)
			{
				onSelectedEvent.Invoke();
			}
			this.background.color = this.selectedColor;
			return;
		}
		this.background.color = this.unselectedColor;
	}

	// Token: 0x040005D9 RID: 1497
	private InteractableBase interactable;

	// Token: 0x040005DA RID: 1498
	public GameObject selectIndicator;

	// Token: 0x040005DB RID: 1499
	public TextMeshProUGUI text;

	// Token: 0x040005DC RID: 1500
	public ProceduralImage background;

	// Token: 0x040005DD RID: 1501
	public Color selectedColor;

	// Token: 0x040005DE RID: 1502
	public Color unselectedColor;

	// Token: 0x040005DF RID: 1503
	public CanvasGroup requireCanvasGroup;

	// Token: 0x040005E0 RID: 1504
	public ProceduralImage requireItemBackgroundImage;

	// Token: 0x040005E1 RID: 1505
	public TextMeshProUGUI requireText;

	// Token: 0x040005E2 RID: 1506
	[LocalizationKey("UI")]
	public string requirItemTextKey = "UI_RequireItem";

	// Token: 0x040005E3 RID: 1507
	[LocalizationKey("UI")]
	public string requirUseItemTextKey = "UI_RequireUseItem";

	// Token: 0x040005E4 RID: 1508
	public Image requirementIcon;

	// Token: 0x040005E5 RID: 1509
	public Color hasRequireItemColor;

	// Token: 0x040005E6 RID: 1510
	public Color noRequireItemColor;

	// Token: 0x040005E7 RID: 1511
	private bool selecting;

	// Token: 0x040005E8 RID: 1512
	public UnityEvent OnSelectedEvent;

	// Token: 0x040005E9 RID: 1513
	public GameObject selectionPoint;

	// Token: 0x040005EA RID: 1514
	public GameObject upDownIndicator;

	// Token: 0x040005EB RID: 1515
	private bool hasUpDown;
}
