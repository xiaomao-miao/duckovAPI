using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C5 RID: 965
	[RequireComponent(typeof(ScrollRect))]
	[ExecuteInEditMode]
	public class ScrollViewMaxHeight : UIBehaviour, ILayoutElement
	{
		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600231F RID: 8991 RVA: 0x0007AF04 File Offset: 0x00079104
		public float preferredHeight
		{
			get
			{
				float y = this.scrollRect.content.sizeDelta.y;
				float num = this.maxHeight;
				if (this.useTargetParentSize)
				{
					float num2 = 0f;
					foreach (RectTransform rectTransform in this.siblings)
					{
						num2 += rectTransform.rect.height;
					}
					num = this.targetParentHeight - num2 - this.parentLayoutMargin;
				}
				if (y > num)
				{
					return num;
				}
				return y;
			}
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0007AFA8 File Offset: 0x000791A8
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0007AFAA File Offset: 0x000791AA
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x0007AFAC File Offset: 0x000791AC
		public virtual float minWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x0007AFB3 File Offset: 0x000791B3
		public virtual float minHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x0007AFBA File Offset: 0x000791BA
		public virtual float preferredWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x0007AFC1 File Offset: 0x000791C1
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x0007AFC8 File Offset: 0x000791C8
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0007AFCF File Offset: 0x000791CF
		public virtual int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0007AFD7 File Offset: 0x000791D7
		private void OnContentRectChange(RectTransform rectTransform)
		{
			this.SetDirty();
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x0007AFDF File Offset: 0x000791DF
		private RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0007B008 File Offset: 0x00079208
		protected override void OnEnable()
		{
			if (this.scrollRect == null)
			{
				this.scrollRect = base.GetComponent<ScrollRect>();
			}
			if (this.contentRectChangeEventEmitter == null)
			{
				this.contentRectChangeEventEmitter = this.scrollRect.content.GetComponent<RectTransformChangeEventEmitter>();
			}
			if (this.contentRectChangeEventEmitter == null)
			{
				this.contentRectChangeEventEmitter = this.scrollRect.content.gameObject.AddComponent<RectTransformChangeEventEmitter>();
			}
			base.OnEnable();
			this.contentRectChangeEventEmitter.OnRectTransformChange += this.OnContentRectChange;
			this.SetDirty();
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0007B09F File Offset: 0x0007929F
		protected override void OnDisable()
		{
			this.contentRectChangeEventEmitter.OnRectTransformChange -= this.OnContentRectChange;
			this.SetDirty();
			base.OnDisable();
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0007B0C4 File Offset: 0x000792C4
		private void Update()
		{
			if (this.preferredHeight != this.rectTransform.rect.height)
			{
				this.SetDirty();
			}
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0007B0F2 File Offset: 0x000792F2
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
		}

		// Token: 0x040017DE RID: 6110
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x040017DF RID: 6111
		[SerializeField]
		private RectTransformChangeEventEmitter contentRectChangeEventEmitter;

		// Token: 0x040017E0 RID: 6112
		[SerializeField]
		private int m_layoutPriority = 1;

		// Token: 0x040017E1 RID: 6113
		[SerializeField]
		private bool useTargetParentSize;

		// Token: 0x040017E2 RID: 6114
		[SerializeField]
		private float targetParentHeight = 935f;

		// Token: 0x040017E3 RID: 6115
		[SerializeField]
		private List<RectTransform> siblings = new List<RectTransform>();

		// Token: 0x040017E4 RID: 6116
		[SerializeField]
		private float parentLayoutMargin = 16f;

		// Token: 0x040017E5 RID: 6117
		[SerializeField]
		private float maxHeight = 100f;

		// Token: 0x040017E6 RID: 6118
		private RectTransform _rectTransform;
	}
}
