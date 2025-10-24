using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001AB RID: 427
public class CraftViewFilterBtnEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000CA7 RID: 3239 RVA: 0x000351D6 File Offset: 0x000333D6
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.master == null)
		{
			return;
		}
		this.master.SetFilter(this.index);
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x000351F8 File Offset: 0x000333F8
	public void Setup(CraftView master, CraftView.FilterInfo filterInfo, int index, bool selected)
	{
		this.master = master;
		this.info = filterInfo;
		this.index = index;
		this.icon.sprite = filterInfo.icon;
		this.displayNameText.text = filterInfo.displayNameKey.ToPlainText();
		this.selectedIndicator.SetActive(selected);
	}

	// Token: 0x04000AF5 RID: 2805
	[SerializeField]
	private Image icon;

	// Token: 0x04000AF6 RID: 2806
	[SerializeField]
	private TextMeshProUGUI displayNameText;

	// Token: 0x04000AF7 RID: 2807
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000AF8 RID: 2808
	private CraftView.FilterInfo info;

	// Token: 0x04000AF9 RID: 2809
	private CraftView master;

	// Token: 0x04000AFA RID: 2810
	private int index;
}
