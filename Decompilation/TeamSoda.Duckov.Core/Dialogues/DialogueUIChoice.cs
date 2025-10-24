using System;
using System.Collections.Generic;
using DG.Tweening;
using NodeCanvas.DialogueTrees;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogues
{
	// Token: 0x0200021C RID: 540
	public class DialogueUIChoice : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0003F086 File Offset: 0x0003D286
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0003F090 File Offset: 0x0003D290
		private void Awake()
		{
			MenuItem menuItem = this.menuItem;
			menuItem.onSelected = (Action<MenuItem>)Delegate.Combine(menuItem.onSelected, new Action<MenuItem>(this.Refresh));
			MenuItem menuItem2 = this.menuItem;
			menuItem2.onDeselected = (Action<MenuItem>)Delegate.Combine(menuItem2.onDeselected, new Action<MenuItem>(this.Refresh));
			MenuItem menuItem3 = this.menuItem;
			menuItem3.onFocusStatusChanged = (Action<MenuItem, bool>)Delegate.Combine(menuItem3.onFocusStatusChanged, new Action<MenuItem, bool>(this.Refresh));
			MenuItem menuItem4 = this.menuItem;
			menuItem4.onConfirmed = (Action<MenuItem>)Delegate.Combine(menuItem4.onConfirmed, new Action<MenuItem>(this.OnConfirm));
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0003F139 File Offset: 0x0003D339
		private void OnConfirm(MenuItem item)
		{
			this.Confirm();
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0003F144 File Offset: 0x0003D344
		private void AnimateConfirm()
		{
			this.confirmIndicator.DOKill(false);
			this.confirmIndicator.DOGradientColor(this.confirmAnimationColor, this.confirmAnimationDuration).OnComplete(delegate
			{
				this.confirmIndicator.color = Color.clear;
			}).OnKill(delegate
			{
				this.confirmIndicator.color = Color.clear;
			});
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0003F198 File Offset: 0x0003D398
		private void Refresh(MenuItem item, bool focus)
		{
			this.selectionIndicator.SetActive(this.menuItem.IsSelected);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0003F1B0 File Offset: 0x0003D3B0
		private void Refresh(MenuItem item)
		{
			this.selectionIndicator.SetActive(this.menuItem.IsSelected);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003F1C8 File Offset: 0x0003D3C8
		private void Confirm()
		{
			this.master.NotifyChoiceConfirmed(this);
			this.AnimateConfirm();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0003F1DC File Offset: 0x0003D3DC
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Confirm();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003F1E4 File Offset: 0x0003D3E4
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.menuItem.Select();
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0003F1F4 File Offset: 0x0003D3F4
		internal void Setup(DialogueUI master, KeyValuePair<IStatement, int> cur)
		{
			this.master = master;
			this.index = cur.Value;
			this.text.text = cur.Key.text;
			this.confirmIndicator.color = Color.clear;
			this.Refresh(this.menuItem);
		}

		// Token: 0x04000CF1 RID: 3313
		[SerializeField]
		private MenuItem menuItem;

		// Token: 0x04000CF2 RID: 3314
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04000CF3 RID: 3315
		[SerializeField]
		private Image confirmIndicator;

		// Token: 0x04000CF4 RID: 3316
		[SerializeField]
		private Gradient confirmAnimationColor;

		// Token: 0x04000CF5 RID: 3317
		[SerializeField]
		private float confirmAnimationDuration = 0.2f;

		// Token: 0x04000CF6 RID: 3318
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000CF7 RID: 3319
		private DialogueUI master;

		// Token: 0x04000CF8 RID: 3320
		private int index;
	}
}
