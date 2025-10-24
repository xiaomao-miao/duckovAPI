using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.MiniMaps;
using Duckov.Utilities;
using Eflatun.SceneReference;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Scenes
{
	// Token: 0x0200032B RID: 811
	public class MultiSceneCore : MonoBehaviour
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x000636E8 File Offset: 0x000618E8
		// (set) Token: 0x06001B5B RID: 7003 RVA: 0x000636EF File Offset: 0x000618EF
		public static MultiSceneCore Instance { get; private set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x000636F7 File Offset: 0x000618F7
		public List<SubSceneEntry> SubScenes
		{
			get
			{
				return this.subScenes;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x00063700 File Offset: 0x00061900
		public static Scene? MainScene
		{
			get
			{
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				return new Scene?(MultiSceneCore.Instance.gameObject.scene);
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x00063738 File Offset: 0x00061938
		public static string ActiveSubSceneID
		{
			get
			{
				if (MultiSceneCore.ActiveSubScene == null)
				{
					return null;
				}
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				SubSceneEntry subSceneEntry = MultiSceneCore.Instance.SubScenes.Find((SubSceneEntry e) => e != null && MultiSceneCore.ActiveSubScene.Value.buildIndex == e.Info.BuildIndex);
				if (subSceneEntry == null)
				{
					return null;
				}
				return subSceneEntry.sceneID;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000637A0 File Offset: 0x000619A0
		public static Scene? ActiveSubScene
		{
			get
			{
				if (MultiSceneCore.Instance == null)
				{
					return null;
				}
				if (MultiSceneCore.Instance.isLoading)
				{
					return null;
				}
				return new Scene?(MultiSceneCore.Instance.activeSubScene);
			}
		}

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06001B60 RID: 7008 RVA: 0x000637EC File Offset: 0x000619EC
		// (remove) Token: 0x06001B61 RID: 7009 RVA: 0x00063820 File Offset: 0x00061A20
		public static event Action<MultiSceneCore, Scene> OnSubSceneWillBeUnloaded;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06001B62 RID: 7010 RVA: 0x00063854 File Offset: 0x00061A54
		// (remove) Token: 0x06001B63 RID: 7011 RVA: 0x00063888 File Offset: 0x00061A88
		public static event Action<MultiSceneCore, Scene> OnSubSceneLoaded;

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001B64 RID: 7012 RVA: 0x000638BC File Offset: 0x00061ABC
		public SceneInfoEntry SceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x000638E4 File Offset: 0x00061AE4
		public string DisplayName
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
				if (sceneInfo == null)
				{
					return "?";
				}
				return sceneInfo.DisplayName;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x0006391C File Offset: 0x00061B1C
		public string DisplaynameRaw
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(base.gameObject.scene.buildIndex);
				if (sceneInfo == null)
				{
					return "?";
				}
				return sceneInfo.DisplayNameRaw;
			}
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x00063954 File Offset: 0x00061B54
		public static void MoveToActiveWithScene(GameObject go, int sceneBuildIndex)
		{
			if (MultiSceneCore.Instance == null)
			{
				return;
			}
			Transform setActiveWithSceneParent = MultiSceneCore.Instance.GetSetActiveWithSceneParent(sceneBuildIndex);
			go.transform.SetParent(setActiveWithSceneParent);
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x00063988 File Offset: 0x00061B88
		public static void MoveToActiveWithScene(GameObject go)
		{
			int buildIndex = go.scene.buildIndex;
			MultiSceneCore.MoveToActiveWithScene(go, buildIndex);
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000639AC File Offset: 0x00061BAC
		public Transform GetSetActiveWithSceneParent(int sceneBuildIndex)
		{
			GameObject gameObject;
			if (this.setActiveWithSceneObjects.TryGetValue(sceneBuildIndex, out gameObject))
			{
				return gameObject.transform;
			}
			SceneInfoEntry sceneInfoEntry = SceneInfoCollection.GetSceneInfo(sceneBuildIndex);
			if (sceneInfoEntry == null)
			{
				sceneInfoEntry = new SceneInfoEntry();
				Debug.LogWarning(string.Format("BuildIndex {0} 的sceneInfo不存在", sceneBuildIndex));
			}
			GameObject gameObject2 = new GameObject(sceneInfoEntry.ID);
			gameObject2.transform.SetParent(base.transform);
			this.setActiveWithSceneObjects.Add(sceneBuildIndex, gameObject2);
			gameObject2.SetActive(sceneInfoEntry.IsLoaded);
			return gameObject2.transform;
		}

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06001B6A RID: 7018 RVA: 0x00063A34 File Offset: 0x00061C34
		// (remove) Token: 0x06001B6B RID: 7019 RVA: 0x00063A68 File Offset: 0x00061C68
		public static event Action<MultiSceneCore> OnInstanceAwake;

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06001B6C RID: 7020 RVA: 0x00063A9C File Offset: 0x00061C9C
		// (remove) Token: 0x06001B6D RID: 7021 RVA: 0x00063AD0 File Offset: 0x00061CD0
		public static event Action<MultiSceneCore> OnInstanceDestroy;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06001B6E RID: 7022 RVA: 0x00063B04 File Offset: 0x00061D04
		// (remove) Token: 0x06001B6F RID: 7023 RVA: 0x00063B38 File Offset: 0x00061D38
		public static event Action<string> OnSetSceneVisited;

		// Token: 0x06001B70 RID: 7024 RVA: 0x00063B6C File Offset: 0x00061D6C
		private void Awake()
		{
			if (MultiSceneCore.Instance == null)
			{
				MultiSceneCore.Instance = this;
			}
			else
			{
				Debug.LogError("Multiple Multi Scene Core detected!");
			}
			Action<MultiSceneCore> onInstanceAwake = MultiSceneCore.OnInstanceAwake;
			if (onInstanceAwake != null)
			{
				onInstanceAwake(this);
			}
			if (this.playAfterLevelInit)
			{
				if (LevelManager.AfterInit)
				{
					this.PlayStinger();
					return;
				}
				LevelManager.OnAfterLevelInitialized += this.OnAfterLevelInitialized;
			}
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x00063BD0 File Offset: 0x00061DD0
		private void OnDestroy()
		{
			Action<MultiSceneCore> onInstanceDestroy = MultiSceneCore.OnInstanceDestroy;
			if (onInstanceDestroy != null)
			{
				onInstanceDestroy(this);
			}
			LevelManager.OnAfterLevelInitialized -= this.OnAfterLevelInitialized;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x00063BF4 File Offset: 0x00061DF4
		private void OnAfterLevelInitialized()
		{
			if (this.playAfterLevelInit)
			{
				this.PlayStinger();
			}
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x00063C04 File Offset: 0x00061E04
		public void PlayStinger()
		{
			if (!string.IsNullOrWhiteSpace(this.playStinger))
			{
				AudioManager.PlayStringer(this.playStinger);
			}
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x00063C20 File Offset: 0x00061E20
		private void Start()
		{
			this.CreatePointsOfInterestsForLocations();
			AudioManager.StopBGM();
			AudioManager.SetState("Level", this.levelStateName);
			if (this.SceneInfo != null && !string.IsNullOrEmpty(this.SceneInfo.ID))
			{
				MultiSceneCore.SetVisited(this.SceneInfo.ID);
			}
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x00063C72 File Offset: 0x00061E72
		public static void SetVisited(string sceneID)
		{
			SavesSystem.Save<bool>("MultiSceneCore_Visited_" + sceneID, true);
			Action<string> onSetSceneVisited = MultiSceneCore.OnSetSceneVisited;
			if (onSetSceneVisited == null)
			{
				return;
			}
			onSetSceneVisited(sceneID);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x00063C95 File Offset: 0x00061E95
		public static bool GetVisited(string sceneID)
		{
			return SavesSystem.Load<bool>("MultiSceneCore_Visited_" + sceneID);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x00063CA8 File Offset: 0x00061EA8
		private void CreatePointsOfInterestsForLocations()
		{
			foreach (SubSceneEntry subSceneEntry in this.SubScenes)
			{
				foreach (SubSceneEntry.Location location in subSceneEntry.cachedLocations)
				{
					if (location.showInMap)
					{
						SimplePointOfInterest.Create(location.position, subSceneEntry.sceneID, location.DisplayNameRaw, null, true);
					}
				}
			}
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x00063D54 File Offset: 0x00061F54
		private void CreatePointsOfInterestsForTeleporters()
		{
			foreach (SubSceneEntry subSceneEntry in this.SubScenes)
			{
				foreach (SubSceneEntry.TeleporterInfo teleporterInfo in subSceneEntry.cachedTeleporters)
				{
					SimplePointOfInterest.Create(teleporterInfo.position, subSceneEntry.sceneID, "", GameplayDataSettings.UIStyle.DefaultTeleporterIcon, false).ScaleFactor = GameplayDataSettings.UIStyle.TeleporterIconScale;
				}
			}
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x00063E0C File Offset: 0x0006200C
		public void BeginLoadSubScene(SceneReference reference)
		{
			this.LoadSubScene(reference, true).Forget<bool>();
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x00063E1B File Offset: 0x0006201B
		public bool IsLoading
		{
			get
			{
				return this.isLoading;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001B7B RID: 7035 RVA: 0x00063E24 File Offset: 0x00062024
		public static string MainSceneID
		{
			get
			{
				return SceneInfoCollection.GetSceneID(MultiSceneCore.MainScene.Value.buildIndex);
			}
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00063E4C File Offset: 0x0006204C
		private SceneReference GetSubSceneReference(string sceneID)
		{
			SubSceneEntry subSceneEntry = this.subScenes.Find((SubSceneEntry e) => e.sceneID == sceneID);
			if (subSceneEntry == null)
			{
				return null;
			}
			return subSceneEntry.SceneReference;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00063E8C File Offset: 0x0006208C
		private UniTask<bool> LoadSubScene(SceneReference targetScene, bool withBlackScreen = true)
		{
			MultiSceneCore.<LoadSubScene>d__62 <LoadSubScene>d__;
			<LoadSubScene>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadSubScene>d__.<>4__this = this;
			<LoadSubScene>d__.targetScene = targetScene;
			<LoadSubScene>d__.withBlackScreen = withBlackScreen;
			<LoadSubScene>d__.<>1__state = -1;
			<LoadSubScene>d__.<>t__builder.Start<MultiSceneCore.<LoadSubScene>d__62>(ref <LoadSubScene>d__);
			return <LoadSubScene>d__.<>t__builder.Task;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00063EE0 File Offset: 0x000620E0
		private void LocalOnSubSceneWillBeUnloaded(Scene scene)
		{
			this.subScenes.Find((SubSceneEntry e) => e != null && e.Info.BuildIndex == scene.buildIndex);
			Transform setActiveWithSceneParent = this.GetSetActiveWithSceneParent(scene.buildIndex);
			Debug.Log(string.Format("Setting Active False {0}  {1}", setActiveWithSceneParent.name, scene.buildIndex));
			setActiveWithSceneParent.gameObject.SetActive(false);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x00063F58 File Offset: 0x00062158
		private void LocalOnSubSceneLoaded(Scene scene)
		{
			this.subScenes.Find((SubSceneEntry e) => e != null && e.Info.BuildIndex == scene.buildIndex);
			this.GetSetActiveWithSceneParent(scene.buildIndex).gameObject.SetActive(true);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x00063FA8 File Offset: 0x000621A8
		public UniTask<bool> LoadAndTeleport(MultiSceneLocation location)
		{
			MultiSceneCore.<LoadAndTeleport>d__65 <LoadAndTeleport>d__;
			<LoadAndTeleport>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAndTeleport>d__.<>4__this = this;
			<LoadAndTeleport>d__.location = location;
			<LoadAndTeleport>d__.<>1__state = -1;
			<LoadAndTeleport>d__.<>t__builder.Start<MultiSceneCore.<LoadAndTeleport>d__65>(ref <LoadAndTeleport>d__);
			return <LoadAndTeleport>d__.<>t__builder.Task;
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x00063FF4 File Offset: 0x000621F4
		public UniTask<bool> LoadAndTeleport(string sceneID, Vector3 position, bool subSceneLocation = false)
		{
			MultiSceneCore.<LoadAndTeleport>d__66 <LoadAndTeleport>d__;
			<LoadAndTeleport>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAndTeleport>d__.<>4__this = this;
			<LoadAndTeleport>d__.sceneID = sceneID;
			<LoadAndTeleport>d__.position = position;
			<LoadAndTeleport>d__.subSceneLocation = subSceneLocation;
			<LoadAndTeleport>d__.<>1__state = -1;
			<LoadAndTeleport>d__.<>t__builder.Start<MultiSceneCore.<LoadAndTeleport>d__66>(ref <LoadAndTeleport>d__);
			return <LoadAndTeleport>d__.<>t__builder.Task;
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x00064050 File Offset: 0x00062250
		public static void MoveToMainScene(GameObject gameObject)
		{
			if (MultiSceneCore.Instance == null)
			{
				Debug.LogError("移动到主场景失败，因为MultiSceneCore不存在");
				return;
			}
			SceneManager.MoveGameObjectToScene(gameObject, MultiSceneCore.MainScene.Value);
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00064088 File Offset: 0x00062288
		public void CacheLocations()
		{
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0006408A File Offset: 0x0006228A
		public void CacheTeleporters()
		{
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0006408C File Offset: 0x0006228C
		private Vector3 GetClosestTeleporterPosition(Vector3 pos)
		{
			float num = float.MaxValue;
			Vector3 result = pos;
			foreach (SubSceneEntry subSceneEntry in this.subScenes)
			{
				foreach (SubSceneEntry.TeleporterInfo teleporterInfo in subSceneEntry.cachedTeleporters)
				{
					float magnitude = (teleporterInfo.position - pos).magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						result = teleporterInfo.position;
					}
				}
			}
			return result;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00064144 File Offset: 0x00062344
		internal bool TryGetCachedPosition(MultiSceneLocation location, out Vector3 result)
		{
			return this.TryGetCachedPosition(location.SceneID, location.LocationName, out result);
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0006415C File Offset: 0x0006235C
		internal bool TryGetCachedPosition(string sceneID, string locationName, out Vector3 result)
		{
			result = default(Vector3);
			SubSceneEntry subSceneEntry = this.subScenes.Find((SubSceneEntry e) => e != null && e.sceneID == sceneID);
			return subSceneEntry != null && subSceneEntry.TryGetCachedPosition(locationName, out result);
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000641A8 File Offset: 0x000623A8
		internal SubSceneEntry GetSubSceneInfo(Scene scene)
		{
			return this.subScenes.Find((SubSceneEntry e) => e != null && e.Info != null && e.Info.BuildIndex == scene.buildIndex);
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000641D9 File Offset: 0x000623D9
		public SubSceneEntry GetSubSceneInfo()
		{
			return this.cachedSubsceneEntry;
		}

		// Token: 0x04001377 RID: 4983
		[SerializeField]
		private string levelStateName = "None";

		// Token: 0x04001378 RID: 4984
		[SerializeField]
		private string playStinger = "";

		// Token: 0x04001379 RID: 4985
		[SerializeField]
		private bool playAfterLevelInit;

		// Token: 0x0400137A RID: 4986
		[SerializeField]
		private List<SubSceneEntry> subScenes;

		// Token: 0x0400137B RID: 4987
		private Scene activeSubScene;

		// Token: 0x0400137C RID: 4988
		[HideInInspector]
		public List<int> usedCreatorIds = new List<int>();

		// Token: 0x0400137D RID: 4989
		[HideInInspector]
		public Dictionary<int, object> inLevelData = new Dictionary<int, object>();

		// Token: 0x04001380 RID: 4992
		[SerializeField]
		private bool teleportToRandomOnLevelInitialized;

		// Token: 0x04001381 RID: 4993
		private Dictionary<int, GameObject> setActiveWithSceneObjects = new Dictionary<int, GameObject>();

		// Token: 0x04001385 RID: 4997
		private bool isLoading;

		// Token: 0x04001386 RID: 4998
		private SubSceneEntry cachedSubsceneEntry;
	}
}
