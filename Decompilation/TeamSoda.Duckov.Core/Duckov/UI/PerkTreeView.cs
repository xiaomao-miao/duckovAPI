using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.PerkTrees;
using Duckov.UI.Animations;
using Duckov.Utilities;
using NodeCanvas.Framework;
using TMPro;
using UI_Spline_Renderer;
using UnityEngine;
using UnityEngine.Splines;

namespace Duckov.UI
{
	// Token: 0x020003BD RID: 957
	public class PerkTreeView : View, ISingleSelectionMenu<PerkEntry>
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x000796F4 File Offset: 0x000778F4
		public static PerkTreeView Instance
		{
			get
			{
				return View.GetViewInstance<PerkTreeView>();
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000796FC File Offset: 0x000778FC
		private PrefabPool<PerkEntry> PerkEntryPool
		{
			get
			{
				if (this._perkEntryPool == null)
				{
					this._perkEntryPool = new PrefabPool<PerkEntry>(this.perkEntryPrefab, this.contentParent, null, null, null, true, 10, 10000, null);
				}
				return this._perkEntryPool;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x0007973C File Offset: 0x0007793C
		private PrefabPool<PerkLineEntry> PerkLinePool
		{
			get
			{
				if (this._perkLinePool == null)
				{
					this._perkLinePool = new PrefabPool<PerkLineEntry>(this.perkLinePrefab, this.contentParent, null, null, null, true, 10, 10000, null);
				}
				return this._perkLinePool;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x0007977A File Offset: 0x0007797A
		protected override bool ShowOpenCloseButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x060022C0 RID: 8896 RVA: 0x00079780 File Offset: 0x00077980
		// (remove) Token: 0x060022C1 RID: 8897 RVA: 0x000797B8 File Offset: 0x000779B8
		internal event Action<PerkEntry> onSelectionChanged;

		// Token: 0x060022C2 RID: 8898 RVA: 0x000797F0 File Offset: 0x000779F0
		private void PopulatePerks()
		{
			this.contentParent.ForceUpdateRectTransforms();
			this.PerkEntryPool.ReleaseAll();
			this.PerkLinePool.ReleaseAll();
			bool isDemo = GameMetaData.Instance.IsDemo;
			foreach (Perk perk in this.target.Perks)
			{
				if ((!isDemo || !perk.LockInDemo) && this.target.RelationGraphOwner.GetRelatedNode(perk) != null)
				{
					this.PerkEntryPool.Get(this.contentParent).Setup(this, perk);
				}
			}
			foreach (PerkLevelLineNode cur in this.target.RelationGraphOwner.graph.GetAllNodesOfType<PerkLevelLineNode>())
			{
				this.PerkLinePool.Get(this.contentParent).Setup(this, cur);
			}
			this.FitChildren();
			this.RefreshConnections();
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00079908 File Offset: 0x00077B08
		private void RefreshConnections()
		{
			bool isDemo = GameMetaData.Instance.IsDemo;
			this.activeConnectionsRenderer.enabled = false;
			this.inactiveConnectionsRenderer.enabled = false;
			SplineContainer splineContainer = this.activeConnectionsRenderer.splineContainer;
			SplineContainer splineContainer2 = this.inactiveConnectionsRenderer.splineContainer;
			PerkTreeView.<RefreshConnections>g__ClearSplines|27_0(splineContainer);
			PerkTreeView.<RefreshConnections>g__ClearSplines|27_0(splineContainer2);
			PerkTreeView.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.horizontal = this.target.Horizontal;
			CS$<>8__locals1.splineTangentVector = (CS$<>8__locals1.horizontal ? Vector3.left : Vector3.up) * this.splineTangent;
			foreach (Perk perk in this.target.Perks)
			{
				if (!isDemo || !perk.LockInDemo)
				{
					PerkRelationNode relatedNode = this.target.RelationGraphOwner.GetRelatedNode(perk);
					PerkEntry perkEntry = this.GetPerkEntry(perk);
					if (!(perkEntry == null) && relatedNode != null)
					{
						SplineContainer container = perk.Unlocked ? splineContainer : splineContainer2;
						foreach (Connection connection in relatedNode.outConnections)
						{
							PerkRelationNode perkRelationNode = connection.targetNode as PerkRelationNode;
							Perk relatedNode2 = perkRelationNode.relatedNode;
							if (relatedNode2 == null)
							{
								Debug.Log(string.Concat(new string[]
								{
									"Target Perk is Null (Connection from ",
									relatedNode.name,
									" to ",
									perkRelationNode.name,
									")"
								}));
							}
							else if (!isDemo || !relatedNode2.LockInDemo)
							{
								PerkEntry perkEntry2 = this.GetPerkEntry(relatedNode2);
								if (perkEntry2 == null)
								{
									Debug.Log(string.Concat(new string[]
									{
										"Target Perk Entry is Null (Connection from ",
										relatedNode.name,
										" to ",
										perkRelationNode.name,
										")"
									}));
								}
								else
								{
									PerkTreeView.<RefreshConnections>g__AddConnection|27_1(container, perkEntry.transform.localPosition, perkEntry2.transform.localPosition, ref CS$<>8__locals1);
								}
							}
						}
					}
				}
			}
			this.activeConnectionsRenderer.enabled = true;
			this.inactiveConnectionsRenderer.enabled = true;
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00079B8C File Offset: 0x00077D8C
		private PerkEntry GetPerkEntry(Perk ofPerk)
		{
			return this.PerkEntryPool.ActiveEntries.FirstOrDefault((PerkEntry e) => e != null && e.Target == ofPerk);
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00079BC4 File Offset: 0x00077DC4
		private void FitChildren()
		{
			this.contentParent.ForceUpdateRectTransforms();
			ReadOnlyCollection<PerkEntry> activeEntries = this.PerkEntryPool.ActiveEntries;
			float num2;
			float num = num2 = float.MaxValue;
			float num4;
			float num3 = num4 = float.MinValue;
			foreach (PerkEntry perkEntry in activeEntries)
			{
				RectTransform rectTransform = perkEntry.RectTransform;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.zero;
				Vector2 layoutPosition = perkEntry.GetLayoutPosition();
				layoutPosition.y *= -1f;
				Vector2 vector = layoutPosition * this.layoutFactor;
				rectTransform.anchoredPosition = vector;
				if (vector.x < num2)
				{
					num2 = vector.x;
				}
				if (vector.y < num)
				{
					num = vector.y;
				}
				if (vector.x > num4)
				{
					num4 = vector.x;
				}
				if (vector.y > num3)
				{
					num3 = vector.y;
				}
			}
			float num5 = num4 - num2;
			float num6 = num3 - num;
			Vector2 b = -new Vector2(num2, num);
			RectTransform rectTransform2 = this.contentParent;
			Vector2 sizeDelta = rectTransform2.sizeDelta;
			sizeDelta.y = num6 + this.padding.y * 2f;
			rectTransform2.sizeDelta = sizeDelta;
			foreach (PerkEntry perkEntry2 in activeEntries)
			{
				RectTransform rectTransform3 = perkEntry2.RectTransform;
				Vector2 vector2 = rectTransform3.anchoredPosition + b;
				if (num5 == 0f)
				{
					vector2.x = (rectTransform2.rect.width - this.padding.x * 2f) / 2f;
				}
				else
				{
					float num7 = (rectTransform2.rect.width - this.padding.x * 2f) / num5;
					vector2.x *= num7;
				}
				vector2 += this.padding;
				rectTransform3.anchoredPosition = vector2;
			}
			foreach (PerkLineEntry perkLineEntry in this.PerkLinePool.ActiveEntries)
			{
				RectTransform rectTransform4 = perkLineEntry.RectTransform;
				Vector2 layoutPosition2 = perkLineEntry.GetLayoutPosition();
				layoutPosition2.y *= -1f;
				Vector2 vector3 = layoutPosition2 * this.layoutFactor;
				vector3 += this.padding;
				vector3.x = rectTransform4.anchoredPosition.x;
				rectTransform4.anchoredPosition = vector3;
				rectTransform4.SetAsFirstSibling();
			}
			this.contentParent.anchoredPosition = Vector2.zero;
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00079EA4 File Offset: 0x000780A4
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00079EB7 File Offset: 0x000780B7
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x00079ECA File Offset: 0x000780CA
		public PerkEntry GetSelection()
		{
			return this.selectedPerkEntry;
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x00079ED2 File Offset: 0x000780D2
		public bool SetSelection(PerkEntry selection)
		{
			this.selectedPerkEntry = selection;
			this.OnSelectionChanged();
			return true;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x00079EE2 File Offset: 0x000780E2
		private void OnSelectionChanged()
		{
			Action<PerkEntry> action = this.onSelectionChanged;
			if (action != null)
			{
				action(this.selectedPerkEntry);
			}
			this.RefreshDetails();
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00079F01 File Offset: 0x00078101
		private void RefreshDetails()
		{
			PerkDetails perkDetails = this.details;
			PerkEntry perkEntry = this.selectedPerkEntry;
			perkDetails.Setup((perkEntry != null) ? perkEntry.Target : null, true);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00079F21 File Offset: 0x00078121
		private void Show_Local(PerkTree target)
		{
			this.UnregisterEvents();
			this.SetSelection(null);
			this.target = target;
			this.title.text = target.DisplayName;
			this.ShowTask().Forget();
			this.RegisterEvents();
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x00079F5A File Offset: 0x0007815A
		public static void Show(PerkTree target)
		{
			if (PerkTreeView.Instance == null)
			{
				return;
			}
			PerkTreeView.Instance.Show_Local(target);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x00079F75 File Offset: 0x00078175
		private void RegisterEvents()
		{
			if (this.target != null)
			{
				this.target.onPerkTreeStatusChanged += this.Refresh;
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00079F9C File Offset: 0x0007819C
		private void UnregisterEvents()
		{
			if (this.target != null)
			{
				this.target.onPerkTreeStatusChanged -= this.Refresh;
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00079FC3 File Offset: 0x000781C3
		private void Refresh(PerkTree tree)
		{
			this.RefreshConnections();
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00079FCC File Offset: 0x000781CC
		private UniTask ShowTask()
		{
			PerkTreeView.<ShowTask>d__41 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<PerkTreeView.<ShowTask>d__41>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x0007A00F File Offset: 0x0007820F
		public void Hide()
		{
			base.Close();
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x0007A017 File Offset: 0x00078217
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x0007A048 File Offset: 0x00078248
		[CompilerGenerated]
		internal static void <RefreshConnections>g__ClearSplines|27_0(SplineContainer splineContainer)
		{
			while (splineContainer.Splines.Count > 0)
			{
				splineContainer.RemoveSplineAt(0);
			}
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0007A064 File Offset: 0x00078264
		[CompilerGenerated]
		internal static void <RefreshConnections>g__AddConnection|27_1(SplineContainer container, Vector2 from, Vector2 to, ref PerkTreeView.<>c__DisplayClass27_0 A_3)
		{
			if (A_3.horizontal)
			{
				container.AddSpline(new Spline(new BezierKnot[]
				{
					new BezierKnot(from, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(from - A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(new Vector3(from.x, to.y) - 2f * A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(to, A_3.splineTangentVector, -A_3.splineTangentVector)
				}, false));
				return;
			}
			container.AddSpline(new Spline(new BezierKnot[]
			{
				new BezierKnot(from, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(from - A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(new Vector3(to.x, from.y) - 2f * A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(to, A_3.splineTangentVector, -A_3.splineTangentVector)
			}, false));
		}

		// Token: 0x040017A6 RID: 6054
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x040017A7 RID: 6055
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017A8 RID: 6056
		[SerializeField]
		private RectTransform contentParent;

		// Token: 0x040017A9 RID: 6057
		[SerializeField]
		private PerkDetails details;

		// Token: 0x040017AA RID: 6058
		[SerializeField]
		private PerkEntry perkEntryPrefab;

		// Token: 0x040017AB RID: 6059
		[SerializeField]
		private PerkLineEntry perkLinePrefab;

		// Token: 0x040017AC RID: 6060
		[SerializeField]
		private UISplineRenderer activeConnectionsRenderer;

		// Token: 0x040017AD RID: 6061
		[SerializeField]
		private UISplineRenderer inactiveConnectionsRenderer;

		// Token: 0x040017AE RID: 6062
		[SerializeField]
		private float splineTangent = 100f;

		// Token: 0x040017AF RID: 6063
		[SerializeField]
		private PerkTree target;

		// Token: 0x040017B0 RID: 6064
		private PrefabPool<PerkEntry> _perkEntryPool;

		// Token: 0x040017B1 RID: 6065
		private PrefabPool<PerkLineEntry> _perkLinePool;

		// Token: 0x040017B2 RID: 6066
		private PerkEntry selectedPerkEntry;

		// Token: 0x040017B3 RID: 6067
		[SerializeField]
		private float layoutFactor = 10f;

		// Token: 0x040017B4 RID: 6068
		[SerializeField]
		private Vector2 padding = Vector2.one;
	}
}
