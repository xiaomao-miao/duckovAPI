using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cinemachine.Utility;
using Duckov.Scenes;
using Duckov.Utilities;
using UI_Spline_Renderer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x02000278 RID: 632
	public class MiniMapDisplay : MonoBehaviour, IScrollHandler, IEventSystemHandler
	{
		// Token: 0x060013F6 RID: 5110 RVA: 0x00049AFC File Offset: 0x00047CFC
		public bool NoSignal()
		{
			foreach (MiniMapDisplayEntry miniMapDisplayEntry in this.MapEntryPool.ActiveEntries)
			{
				if (!(miniMapDisplayEntry == null) && !(miniMapDisplayEntry.SceneID != MultiSceneCore.ActiveSubSceneID) && miniMapDisplayEntry.NoSignal())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00049B74 File Offset: 0x00047D74
		private PrefabPool<MiniMapDisplayEntry> MapEntryPool
		{
			get
			{
				if (this._mapEntryPool == null)
				{
					this._mapEntryPool = new PrefabPool<MiniMapDisplayEntry>(this.mapDisplayEntryPrefab, base.transform, new Action<MiniMapDisplayEntry>(this.OnGetMapEntry), null, null, true, 10, 10000, null);
				}
				return this._mapEntryPool;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00049BC0 File Offset: 0x00047DC0
		private PrefabPool<PointOfInterestEntry> PointOfInterestEntryPool
		{
			get
			{
				if (this._pointOfInterestEntryPool == null)
				{
					this._pointOfInterestEntryPool = new PrefabPool<PointOfInterestEntry>(this.pointOfInterestEntryPrefab, base.transform, new Action<PointOfInterestEntry>(this.OnGetPointOfInterestEntry), null, null, true, 10, 10000, null);
				}
				return this._pointOfInterestEntryPool;
			}
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00049C09 File Offset: 0x00047E09
		private void OnGetPointOfInterestEntry(PointOfInterestEntry entry)
		{
			entry.gameObject.hideFlags |= HideFlags.DontSave;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00049C1F File Offset: 0x00047E1F
		private void OnGetMapEntry(MiniMapDisplayEntry entry)
		{
			entry.gameObject.hideFlags |= HideFlags.DontSave;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00049C35 File Offset: 0x00047E35
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<MiniMapView>();
			}
			this.mapDisplayEntryPrefab.gameObject.SetActive(false);
			this.pointOfInterestEntryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00049C73 File Offset: 0x00047E73
		private void OnEnable()
		{
			if (this.autoSetupOnEnable)
			{
				this.AutoSetup();
			}
			this.RegisterEvents();
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00049C89 File Offset: 0x00047E89
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00049C91 File Offset: 0x00047E91
		private void RegisterEvents()
		{
			PointsOfInterests.OnPointRegistered += this.HandlePointOfInterest;
			PointsOfInterests.OnPointUnregistered += this.ReleasePointOfInterest;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00049CB5 File Offset: 0x00047EB5
		private void UnregisterEvents()
		{
			PointsOfInterests.OnPointRegistered -= this.HandlePointOfInterest;
			PointsOfInterests.OnPointUnregistered -= this.ReleasePointOfInterest;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00049CDC File Offset: 0x00047EDC
		internal void AutoSetup()
		{
			MiniMapSettings miniMapSettings = UnityEngine.Object.FindAnyObjectByType<MiniMapSettings>();
			if (miniMapSettings)
			{
				this.Setup(miniMapSettings);
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00049D00 File Offset: 0x00047F00
		public void Setup(IMiniMapDataProvider dataProvider)
		{
			if (dataProvider == null)
			{
				return;
			}
			this.MapEntryPool.ReleaseAll();
			bool flag = dataProvider.CombinedSprite != null;
			foreach (IMiniMapEntry cur in dataProvider.Maps)
			{
				MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.Get(null);
				miniMapDisplayEntry.Setup(this, cur, !flag);
				miniMapDisplayEntry.gameObject.SetActive(true);
			}
			if (flag)
			{
				MiniMapDisplayEntry miniMapDisplayEntry2 = this.MapEntryPool.Get(null);
				miniMapDisplayEntry2.SetupCombined(this, dataProvider);
				miniMapDisplayEntry2.gameObject.SetActive(true);
				miniMapDisplayEntry2.transform.SetAsFirstSibling();
			}
			this.SetupRotation();
			this.FitContent();
			this.HandlePointsOfInterests();
			this.HandleTeleporters();
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00049DD0 File Offset: 0x00047FD0
		private void SetupRotation()
		{
			Vector3 to = LevelManager.Instance.GameCamera.mainVCam.transform.up.ProjectOntoPlane(Vector3.up);
			float z = Vector3.SignedAngle(Vector3.forward, to, Vector3.up);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00049E30 File Offset: 0x00048030
		private void HandlePointsOfInterests()
		{
			this.PointOfInterestEntryPool.ReleaseAll();
			foreach (MonoBehaviour monoBehaviour in PointsOfInterests.Points)
			{
				if (!(monoBehaviour == null))
				{
					this.HandlePointOfInterest(monoBehaviour);
				}
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00049E90 File Offset: 0x00048090
		private void HandlePointOfInterest(MonoBehaviour poi)
		{
			int targetSceneIndex = poi.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = poi as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				targetSceneIndex = pointOfInterest.OverrideScene;
			}
			if (MultiSceneCore.ActiveSubScene == null || targetSceneIndex != MultiSceneCore.ActiveSubScene.Value.buildIndex)
			{
				return;
			}
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e.SceneReference != null && e.SceneReference.BuildIndex == targetSceneIndex);
			if (miniMapDisplayEntry == null)
			{
				return;
			}
			if (miniMapDisplayEntry.Hide)
			{
				return;
			}
			this.PointOfInterestEntryPool.Get(null).Setup(this, poi, miniMapDisplayEntry);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00049F50 File Offset: 0x00048150
		private void ReleasePointOfInterest(MonoBehaviour poi)
		{
			PointOfInterestEntry pointOfInterestEntry = this.PointOfInterestEntryPool.ActiveEntries.FirstOrDefault((PointOfInterestEntry e) => e != null && e.Target == poi);
			if (!pointOfInterestEntry)
			{
				return;
			}
			this.PointOfInterestEntryPool.Release(pointOfInterestEntry);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00049F9C File Offset: 0x0004819C
		private void HandleTeleporters()
		{
			this.teleporterSplines.gameObject.SetActive(false);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00049FBC File Offset: 0x000481BC
		private void FitContent()
		{
			ReadOnlyCollection<MiniMapDisplayEntry> activeEntries = this.MapEntryPool.ActiveEntries;
			Vector2 vector = new Vector2(float.MinValue, float.MinValue);
			Vector2 vector2 = new Vector2(float.MaxValue, float.MaxValue);
			foreach (MiniMapDisplayEntry miniMapDisplayEntry in activeEntries)
			{
				RectTransform rectTransform = miniMapDisplayEntry.transform as RectTransform;
				Vector2 vector3 = rectTransform.anchoredPosition + rectTransform.rect.min;
				Vector2 vector4 = rectTransform.anchoredPosition + rectTransform.rect.max;
				vector.x = MathF.Max(vector4.x, vector.x);
				vector.y = MathF.Max(vector4.y, vector.y);
				vector2.x = MathF.Min(vector3.x, vector2.x);
				vector2.y = MathF.Min(vector3.y, vector2.y);
			}
			Vector2 v = (vector + vector2) / 2f;
			foreach (MiniMapDisplayEntry miniMapDisplayEntry2 in activeEntries)
			{
				miniMapDisplayEntry2.transform.localPosition -= v;
			}
			(base.transform as RectTransform).sizeDelta = new Vector2(vector.x - vector2.x + this.padding * 2f, vector.y - vector2.y + this.padding * 2f);
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004A18C File Offset: 0x0004838C
		public bool TryConvertWorldToMinimap(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = worldPosition;
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e != null && e.SceneID == sceneID);
			if (miniMapDisplayEntry == null)
			{
				return false;
			}
			Vector3 center = MiniMapCenter.GetCenter(sceneID);
			Vector3 vector = worldPosition - center;
			Vector3 point = new Vector3(vector.x, vector.z);
			Vector3 point2 = miniMapDisplayEntry.transform.localToWorldMatrix.MultiplyPoint(point);
			result = base.transform.worldToLocalMatrix.MultiplyPoint(point2);
			return true;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004A234 File Offset: 0x00048434
		public bool TryConvertToWorldPosition(Vector3 displayPosition, out Vector3 result)
		{
			result = default(Vector3);
			string activeSubsceneID = MultiSceneCore.ActiveSubSceneID;
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e != null && e.SceneID == activeSubsceneID);
			if (miniMapDisplayEntry == null)
			{
				return false;
			}
			Vector3 vector = miniMapDisplayEntry.transform.worldToLocalMatrix.MultiplyPoint(displayPosition);
			Vector3 b = new Vector3(vector.x, 0f, vector.y);
			Vector3 center = MiniMapCenter.GetCenter(activeSubsceneID);
			result = center + b;
			return true;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004A2CC File Offset: 0x000484CC
		internal void Center(Vector3 minimapPos)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector3 b = rectTransform.localToWorldMatrix.MultiplyPoint(minimapPos);
			Vector3 b2 = (rectTransform.parent as RectTransform).position - b;
			rectTransform.position += b2;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004A328 File Offset: 0x00048528
		public void OnScroll(PointerEventData eventData)
		{
			this.master.OnScroll(eventData);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0004A349 File Offset: 0x00048549
		[CompilerGenerated]
		internal static void <HandleTeleporters>g__ClearSplines|26_0(SplineContainer splineContainer)
		{
			while (splineContainer.Splines.Count > 0)
			{
				splineContainer.RemoveSplineAt(0);
			}
		}

		// Token: 0x04000EAB RID: 3755
		[SerializeField]
		private MiniMapView master;

		// Token: 0x04000EAC RID: 3756
		[SerializeField]
		private MiniMapDisplayEntry mapDisplayEntryPrefab;

		// Token: 0x04000EAD RID: 3757
		[SerializeField]
		private PointOfInterestEntry pointOfInterestEntryPrefab;

		// Token: 0x04000EAE RID: 3758
		[SerializeField]
		private UISplineRenderer teleporterSplines;

		// Token: 0x04000EAF RID: 3759
		[SerializeField]
		private bool autoSetupOnEnable;

		// Token: 0x04000EB0 RID: 3760
		[SerializeField]
		private float padding = 25f;

		// Token: 0x04000EB1 RID: 3761
		private PrefabPool<MiniMapDisplayEntry> _mapEntryPool;

		// Token: 0x04000EB2 RID: 3762
		private PrefabPool<PointOfInterestEntry> _pointOfInterestEntryPool;
	}
}
