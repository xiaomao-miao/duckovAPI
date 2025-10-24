using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C6 RID: 966
	public class KontextMenu : MonoBehaviour
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600232F RID: 9007 RVA: 0x0007B148 File Offset: 0x00079348
		private Transform ContentRoot
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002330 RID: 9008 RVA: 0x0007B150 File Offset: 0x00079350
		private PrefabPool<KontextMenuEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<KontextMenuEntry>(this.entryPrefab, this.ContentRoot, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0007B18E File Offset: 0x0007938E
		private void Awake()
		{
			if (KontextMenu.instance == null)
			{
				KontextMenu.instance = this;
			}
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0007B1B4 File Offset: 0x000793B4
		private void OnDestroy()
		{
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0007B1B8 File Offset: 0x000793B8
		private void Update()
		{
			if (this.watchRectTransform)
			{
				if ((this.cachedTransformPosition - this.watchRectTransform.position).magnitude > this.positionMoveCloseThreshold)
				{
					KontextMenu.Hide(null);
					return;
				}
			}
			else if (this.isWatchingRectTransform)
			{
				KontextMenu.Hide(null);
			}
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0007B210 File Offset: 0x00079410
		public void InstanceShow(object target, RectTransform targetRectTransform, params KontextMenuDataEntry[] entries)
		{
			this.target = target;
			this.watchRectTransform = targetRectTransform;
			this.isWatchingRectTransform = true;
			this.cachedTransformPosition = this.watchRectTransform.position;
			Vector3[] array = new Vector3[4];
			targetRectTransform.GetWorldCorners(array);
			float num = Mathf.Min(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x,
				array[3].x
			});
			float num2 = Mathf.Max(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x,
				array[3].x
			});
			float num3 = Mathf.Min(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y,
				array[3].y
			});
			float num4 = Mathf.Max(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y,
				array[3].y
			});
			float num5 = num;
			float num6 = (float)Screen.width - num2;
			float num7 = num3;
			float num8 = (float)Screen.height - num4;
			float x = (num5 > num6) ? num : num2;
			float y = (num7 > num8) ? num3 : num4;
			Vector2 vector = new Vector2(x, y);
			if (entries.Length < 1)
			{
				this.InstanceHide();
				return;
			}
			Vector2 vector2 = new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height);
			float x2 = (float)((vector2.x < 0.5f) ? 0 : 1);
			float y2 = (float)((vector2.y < 0.5f) ? 0 : 1);
			this.rectTransform.pivot = new Vector2(x2, y2);
			base.gameObject.SetActive(true);
			this.fadeGroup.SkipHide();
			this.Setup(entries);
			this.fadeGroup.Show();
			base.transform.position = vector;
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0007B454 File Offset: 0x00079654
		public void InstanceShow(object target, Vector2 screenPoint, params KontextMenuDataEntry[] entries)
		{
			this.target = target;
			this.watchRectTransform = null;
			this.isWatchingRectTransform = false;
			if (entries.Length < 1)
			{
				this.InstanceHide();
				return;
			}
			Vector2 vector = new Vector2(screenPoint.x / (float)Screen.width, screenPoint.y / (float)Screen.height);
			float x = (float)((vector.x < 0.5f) ? 0 : 1);
			float y = (float)((vector.y < 0.5f) ? 0 : 1);
			this.rectTransform.pivot = new Vector2(x, y);
			base.gameObject.SetActive(true);
			this.fadeGroup.SkipHide();
			this.Setup(entries);
			this.fadeGroup.Show();
			base.transform.position = screenPoint;
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0007B514 File Offset: 0x00079714
		private void Clear()
		{
			this.EntryPool.ReleaseAll();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.ContentRoot.childCount; i++)
			{
				Transform child = this.ContentRoot.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					list.Add(child.gameObject);
				}
			}
			foreach (GameObject obj in list)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0007B5AC File Offset: 0x000797AC
		private void Setup(IEnumerable<KontextMenuDataEntry> entries)
		{
			this.Clear();
			int num = 0;
			foreach (KontextMenuDataEntry kontextMenuDataEntry in entries)
			{
				if (kontextMenuDataEntry != null)
				{
					KontextMenuEntry kontextMenuEntry = this.EntryPool.Get(this.ContentRoot);
					num++;
					kontextMenuEntry.Setup(this, num, kontextMenuDataEntry);
					kontextMenuEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0007B620 File Offset: 0x00079820
		public void InstanceHide()
		{
			this.target = null;
			this.watchRectTransform = null;
			this.fadeGroup.Hide();
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0007B63B File Offset: 0x0007983B
		public static void Show(object target, RectTransform watchRectTransform, params KontextMenuDataEntry[] entries)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			KontextMenu.instance.InstanceShow(target, watchRectTransform, entries);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0007B658 File Offset: 0x00079858
		public static void Show(object target, Vector2 position, params KontextMenuDataEntry[] entries)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			KontextMenu.instance.InstanceShow(target, position, entries);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0007B675 File Offset: 0x00079875
		public static void Hide(object target)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			if (target != null && target != KontextMenu.instance.target)
			{
				return;
			}
			if (KontextMenu.instance.fadeGroup.IsHidingInProgress)
			{
				return;
			}
			KontextMenu.instance.InstanceHide();
		}

		// Token: 0x040017E7 RID: 6119
		private static KontextMenu instance;

		// Token: 0x040017E8 RID: 6120
		private RectTransform rectTransform;

		// Token: 0x040017E9 RID: 6121
		[SerializeField]
		private KontextMenuEntry entryPrefab;

		// Token: 0x040017EA RID: 6122
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017EB RID: 6123
		[SerializeField]
		private float positionMoveCloseThreshold = 10f;

		// Token: 0x040017EC RID: 6124
		private object target;

		// Token: 0x040017ED RID: 6125
		private bool isWatchingRectTransform;

		// Token: 0x040017EE RID: 6126
		private RectTransform watchRectTransform;

		// Token: 0x040017EF RID: 6127
		private Vector3 cachedTransformPosition;

		// Token: 0x040017F0 RID: 6128
		private PrefabPool<KontextMenuEntry> _entryPool;
	}
}
