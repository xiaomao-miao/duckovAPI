using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Utilities
{
	// Token: 0x020003FA RID: 1018
	public class SetActiveByPlayerDistance : MonoBehaviour
	{
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x0007F7DA File Offset: 0x0007D9DA
		// (set) Token: 0x060024E0 RID: 9440 RVA: 0x0007F7E1 File Offset: 0x0007D9E1
		public static SetActiveByPlayerDistance Instance { get; private set; }

		// Token: 0x060024E1 RID: 9441 RVA: 0x0007F7EC File Offset: 0x0007D9EC
		private static List<GameObject> GetListByScene(int sceneBuildIndex, bool createIfNotExist = true)
		{
			List<GameObject> result;
			if (SetActiveByPlayerDistance.listsOfScenes.TryGetValue(sceneBuildIndex, out result))
			{
				return result;
			}
			if (createIfNotExist)
			{
				List<GameObject> list = new List<GameObject>();
				SetActiveByPlayerDistance.listsOfScenes[sceneBuildIndex] = list;
				return list;
			}
			return null;
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0007F822 File Offset: 0x0007DA22
		private static List<GameObject> GetListByScene(Scene scene, bool createIfNotExist = true)
		{
			return SetActiveByPlayerDistance.GetListByScene(scene.buildIndex, createIfNotExist);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x0007F831 File Offset: 0x0007DA31
		public static void Register(GameObject gameObject, int sceneBuildIndex)
		{
			SetActiveByPlayerDistance.GetListByScene(sceneBuildIndex, true).Add(gameObject);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x0007F840 File Offset: 0x0007DA40
		public static bool Unregister(GameObject gameObject, int sceneBuildIndex)
		{
			List<GameObject> listByScene = SetActiveByPlayerDistance.GetListByScene(sceneBuildIndex, false);
			return listByScene != null && listByScene.Remove(gameObject);
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x0007F861 File Offset: 0x0007DA61
		public static void Register(GameObject gameObject, Scene scene)
		{
			SetActiveByPlayerDistance.Register(gameObject, scene.buildIndex);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0007F870 File Offset: 0x0007DA70
		public static void Unregister(GameObject gameObject, Scene scene)
		{
			SetActiveByPlayerDistance.Unregister(gameObject, scene.buildIndex);
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x0007F880 File Offset: 0x0007DA80
		public float Distance
		{
			get
			{
				return this.distance;
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0007F888 File Offset: 0x0007DA88
		private void Awake()
		{
			if (SetActiveByPlayerDistance.Instance == null)
			{
				SetActiveByPlayerDistance.Instance = this;
			}
			this.CleanUp();
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.cachedActiveScene = SceneManager.GetActiveScene();
			this.RefreshCache();
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x0007F8C8 File Offset: 0x0007DAC8
		private void CleanUp()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, List<GameObject>> keyValuePair in SetActiveByPlayerDistance.listsOfScenes)
			{
				List<GameObject> value = keyValuePair.Value;
				value.RemoveAll((GameObject e) => e == null);
				if (value == null || value.Count < 1)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (int key in list)
			{
				SetActiveByPlayerDistance.listsOfScenes.Remove(key);
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x0007F9A8 File Offset: 0x0007DBA8
		private void OnActiveSceneChanged(Scene prev, Scene cur)
		{
			this.RefreshCache();
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x0007F9B0 File Offset: 0x0007DBB0
		private void RefreshCache()
		{
			this.cachedActiveScene = SceneManager.GetActiveScene();
			this.cachedListRef = SetActiveByPlayerDistance.GetListByScene(this.cachedActiveScene, true);
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x0007F9CF File Offset: 0x0007DBCF
		private Transform PlayerTransform
		{
			get
			{
				if (!this.cachedPlayerTransform)
				{
					CharacterMainControl main = CharacterMainControl.Main;
					this.cachedPlayerTransform = ((main != null) ? main.transform : null);
				}
				return this.cachedPlayerTransform;
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x0007F9FC File Offset: 0x0007DBFC
		private void FixedUpdate()
		{
			if (this.PlayerTransform == null)
			{
				return;
			}
			if (this.cachedListRef == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.cachedListRef)
			{
				if (!(gameObject == null))
				{
					bool active = (this.PlayerTransform.position - gameObject.transform.position).sqrMagnitude < this.distance * this.distance;
					gameObject.gameObject.SetActive(active);
				}
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x0007FAA8 File Offset: 0x0007DCA8
		private void DebugRegister(GameObject go)
		{
			SetActiveByPlayerDistance.Register(go, go.gameObject.scene);
		}

		// Token: 0x04001922 RID: 6434
		private static Dictionary<int, List<GameObject>> listsOfScenes = new Dictionary<int, List<GameObject>>();

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		private float distance = 100f;

		// Token: 0x04001924 RID: 6436
		private Scene cachedActiveScene;

		// Token: 0x04001925 RID: 6437
		private List<GameObject> cachedListRef;

		// Token: 0x04001926 RID: 6438
		private Transform cachedPlayerTransform;
	}
}
