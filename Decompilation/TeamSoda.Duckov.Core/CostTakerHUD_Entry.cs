using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class CostTakerHUD_Entry : MonoBehaviour
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x0001DB0C File Offset: 0x0001BD0C
	// (set) Token: 0x0600069F RID: 1695 RVA: 0x0001DB14 File Offset: 0x0001BD14
	public CostTaker Target { get; private set; }

	// Token: 0x060006A0 RID: 1696 RVA: 0x0001DB1D File Offset: 0x0001BD1D
	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0001DB30 File Offset: 0x0001BD30
	private void LateUpdate()
	{
		this.UpdatePosition();
		this.UpdateFadeGroup();
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0001DB3E File Offset: 0x0001BD3E
	internal void Setup(CostTaker cur)
	{
		this.Target = cur;
		this.nameText.text = cur.InteractName;
		this.costDisplay.Setup(cur.Cost, 1);
		this.UpdatePosition();
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0001DB70 File Offset: 0x0001BD70
	private void UpdatePosition()
	{
		this.rectTransform.MatchWorldPosition(this.Target.transform.TransformPoint(this.Target.interactMarkerOffset), Vector3.up * 0.5f);
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0001DBA8 File Offset: 0x0001BDA8
	private void UpdateFadeGroup()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		bool flag = false;
		if (!(this.Target == null) && !(main == null))
		{
			Vector3 vector = main.transform.position - this.Target.transform.position;
			if (Mathf.Abs(vector.y) <= 2.5f && vector.magnitude <= 10f)
			{
				flag = true;
			}
		}
		if (flag && !this.fadeGroup.IsShown)
		{
			this.fadeGroup.Show();
			return;
		}
		if (!flag && this.fadeGroup.IsShown)
		{
			this.fadeGroup.Hide();
		}
	}

	// Token: 0x0400065E RID: 1630
	private RectTransform rectTransform;

	// Token: 0x0400065F RID: 1631
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000660 RID: 1632
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000661 RID: 1633
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000662 RID: 1634
	private const float HideDistance = 10f;

	// Token: 0x04000663 RID: 1635
	private const float HideDistanceYLimit = 2.5f;
}
