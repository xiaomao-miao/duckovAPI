using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000231 RID: 561
	public class CursorManager : MonoBehaviour
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x00043970 File Offset: 0x00041B70
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x00043977 File Offset: 0x00041B77
		public static CursorManager Instance { get; private set; }

		// Token: 0x06001171 RID: 4465 RVA: 0x0004397F File Offset: 0x00041B7F
		public static void Register(ICursorDataProvider dataProvider)
		{
			CursorManager.cursorDataStack.Add(dataProvider);
			CursorManager.ApplyStackData();
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00043991 File Offset: 0x00041B91
		public static bool Unregister(ICursorDataProvider dataProvider)
		{
			if (CursorManager.cursorDataStack.Count < 1)
			{
				return false;
			}
			if (!CursorManager.cursorDataStack.Contains(dataProvider))
			{
				return false;
			}
			bool result = CursorManager.cursorDataStack.Remove(dataProvider);
			CursorManager.ApplyStackData();
			return result;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x000439C4 File Offset: 0x00041BC4
		private static void ApplyStackData()
		{
			if (CursorManager.Instance == null)
			{
				return;
			}
			if (CursorManager.cursorDataStack.Count <= 0)
			{
				CursorManager.Instance.MSetDefaultCursor();
				return;
			}
			ICursorDataProvider cursorDataProvider = CursorManager.cursorDataStack[CursorManager.cursorDataStack.Count - 1];
			if (cursorDataProvider == null)
			{
				CursorManager.Instance.MSetDefaultCursor();
			}
			CursorManager.Instance.MSetCursor(cursorDataProvider.GetCursorData());
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00043A2B File Offset: 0x00041C2B
		private void Awake()
		{
			CursorManager.Instance = this;
			this.MSetCursor(this.defaultCursor);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00043A40 File Offset: 0x00041C40
		private void Update()
		{
			if (this.currentCursor == null)
			{
				return;
			}
			if (this.currentCursor.textures.Length < 2)
			{
				return;
			}
			this.fpsBuffer += Time.unscaledDeltaTime * this.currentCursor.fps;
			if (this.fpsBuffer > 1f)
			{
				this.fpsBuffer = 0f;
				this.frame++;
				this.RefreshCursor();
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00043AB1 File Offset: 0x00041CB1
		private void RefreshCursor()
		{
			if (this.currentCursor == null)
			{
				return;
			}
			this.currentCursor.Apply(this.frame);
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00043ACD File Offset: 0x00041CCD
		public void MSetDefaultCursor()
		{
			this.MSetCursor(this.defaultCursor);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00043ADB File Offset: 0x00041CDB
		public void MSetCursor(CursorData data)
		{
			this.currentCursor = data;
			this.frame = 12;
			this.RefreshCursor();
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00043AF4 File Offset: 0x00041CF4
		private void OnDestroy()
		{
			Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00043B11 File Offset: 0x00041D11
		internal static void NotifyRefresh()
		{
			CursorManager.ApplyStackData();
		}

		// Token: 0x04000D83 RID: 3459
		[SerializeField]
		private CursorData defaultCursor;

		// Token: 0x04000D84 RID: 3460
		public CursorData currentCursor;

		// Token: 0x04000D85 RID: 3461
		private static List<ICursorDataProvider> cursorDataStack = new List<ICursorDataProvider>();

		// Token: 0x04000D86 RID: 3462
		private int frame;

		// Token: 0x04000D87 RID: 3463
		private float fpsBuffer;
	}
}
