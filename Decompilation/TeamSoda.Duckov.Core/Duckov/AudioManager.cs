using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Options;
using Duckov.Scenes;
using Duckov.UI;
using FMOD.Studio;
using FMODUnity;
using ItemStatsSystem;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Duckov
{
	// Token: 0x0200022C RID: 556
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x0004237E File Offset: 0x0004057E
		public static AudioManager Instance
		{
			get
			{
				return GameManager.AudioManager;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x00042388 File Offset: 0x00040588
		public static bool IsStingerPlaying
		{
			get
			{
				if (AudioManager.Instance == null)
				{
					return false;
				}
				if (AudioManager.Instance.stingerSource == null)
				{
					return false;
				}
				return AudioManager.Instance.stingerSource.events.Any((EventInstance e) => e.isValid());
			}
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x000423EB File Offset: 0x000405EB
		private IEnumerable<AudioManager.Bus> AllBueses()
		{
			yield return this.masterBus;
			yield return this.sfxBus;
			yield return this.musicBus;
			yield break;
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x000423FB File Offset: 0x000405FB
		private Transform listener
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x00042403 File Offset: 0x00040603
		private static Transform SoundSourceParent
		{
			get
			{
				if (AudioManager._soundSourceParent == null)
				{
					GameObject gameObject = new GameObject("Sound Sources");
					AudioManager._soundSourceParent = gameObject.transform;
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				return AudioManager._soundSourceParent;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00042434 File Offset: 0x00040634
		private static ObjectPool<GameObject> SoundSourcePool
		{
			get
			{
				if (AudioManager._soundSourcePool == null)
				{
					AudioManager._soundSourcePool = new ObjectPool<GameObject>(delegate()
					{
						GameObject gameObject = new GameObject("SoundSource");
						gameObject.transform.SetParent(AudioManager.SoundSourceParent);
						return gameObject;
					}, delegate(GameObject e)
					{
						e.SetActive(true);
					}, delegate(GameObject e)
					{
						e.SetActive(false);
					}, null, true, 10, 10000);
				}
				return AudioManager._soundSourcePool;
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000424C0 File Offset: 0x000406C0
		public static EventInstance? Post(string eventName, GameObject gameObject)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			if (gameObject == null)
			{
				Debug.LogError(string.Format("Posting event but gameObject is null: {0}", gameObject));
			}
			if (!gameObject.activeSelf)
			{
				Debug.LogError(string.Format("Posting event but gameObject is not active: {0}", gameObject));
			}
			return AudioManager.Instance.MPost(eventName, gameObject);
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0004251C File Offset: 0x0004071C
		public static EventInstance? Post(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			return AudioManager.Instance.MPost(eventName, null);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00042548 File Offset: 0x00040748
		public static EventInstance? Post(string eventName, Vector3 position)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			return AudioManager.Instance.MPost(eventName, position);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00042573 File Offset: 0x00040773
		internal static EventInstance? PostQuak(string soundKey, AudioManager.VoiceType voiceType, GameObject gameObject)
		{
			AudioObject orCreate = AudioObject.GetOrCreate(gameObject);
			orCreate.VoiceType = voiceType;
			return orCreate.PostQuak(soundKey);
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00042588 File Offset: 0x00040788
		public static void PostHitMarker(bool crit)
		{
			AudioManager.Post(crit ? "SFX/Combat/Marker/hitmarker_head" : "SFX/Combat/Marker/hitmarker");
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0004259F File Offset: 0x0004079F
		public static void PostKillMarker(bool crit = false)
		{
			AudioManager.Post(crit ? "SFX/Combat/Marker/killmarker_head" : "SFX/Combat/Marker/killmarker");
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x000425B8 File Offset: 0x000407B8
		private void Awake()
		{
			CharacterSoundMaker.OnFootStepSound = (Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>)Delegate.Combine(CharacterSoundMaker.OnFootStepSound, new Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>(this.OnFootStepSound));
			Projectile.OnBulletFlyByCharacter = (Action<Vector3>)Delegate.Combine(Projectile.OnBulletFlyByCharacter, new Action<Vector3>(this.OnBulletFlyby));
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
			ItemUIUtilities.OnPutItem += this.OnPutItem;
			Health.OnDead += this.OnHealthDead;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
			OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.LoadOptions();
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x000426A8 File Offset: 0x000408A8
		private void OnDestroy()
		{
			CharacterSoundMaker.OnFootStepSound = (Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>)Delegate.Remove(CharacterSoundMaker.OnFootStepSound, new Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>(this.OnFootStepSound));
			Projectile.OnBulletFlyByCharacter = (Action<Vector3>)Delegate.Remove(Projectile.OnBulletFlyByCharacter, new Action<Vector3>(this.OnBulletFlyby));
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
			ItemUIUtilities.OnPutItem -= this.OnPutItem;
			Health.OnDead -= this.OnHealthDead;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
			OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0004275C File Offset: 0x0004095C
		private void OnOptionsChanged(string key)
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.NotifyOptionsChanged(key);
			}
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000427A8 File Offset: 0x000409A8
		public static AudioManager.Bus GetBus(string name)
		{
			if (AudioManager.Instance == null)
			{
				return null;
			}
			foreach (AudioManager.Bus bus in AudioManager.Instance.AllBueses())
			{
				if (bus.Name == name)
				{
					return bus;
				}
			}
			return null;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00042818 File Offset: 0x00040A18
		private void OnStartedLoadingScene(SceneLoadingContext context)
		{
			if (this.ambientSource)
			{
				this.ambientSource.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00042833 File Offset: 0x00040A33
		private void OnLevelInitialized()
		{
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00042835 File Offset: 0x00040A35
		private void Start()
		{
			this.UpdateBuses();
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0004283D File Offset: 0x00040A3D
		private void OnHealthDead(Health health, DamageInfo info)
		{
			if (health.TryGetCharacter() == CharacterMainControl.Main)
			{
				AudioManager.StopBGM();
				AudioManager.Post("Music/Stinger/stg_death");
			}
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00042861 File Offset: 0x00040A61
		private void OnPutItem(Item item, bool pickup = false)
		{
			AudioManager.PlayPutItemSFX(item, pickup);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0004286A File Offset: 0x00040A6A
		public static void PlayPutItemSFX(Item item, bool pickup = false)
		{
			if (item == null)
			{
				return;
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			AudioManager.Post((pickup ? "SFX/Item/pickup_{soundkey}" : "SFX/Item/put_{soundkey}").Format(new
			{
				soundkey = item.SoundKey.ToLower()
			}));
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000428A8 File Offset: 0x00040AA8
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Opening ears";
			SubSceneEntry subSceneInfo = core.GetSubSceneInfo(scene);
			if (subSceneInfo == null)
			{
				return;
			}
			if (this.ambientSource)
			{
				LevelManager.LevelInitializingComment = "Hearing Ambient";
				this.ambientSource.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
				this.ambientSource.Post("Amb/amb_{soundkey}".Format(new
				{
					soundkey = subSceneInfo.AmbientSound.ToLower()
				}), true);
			}
			LevelManager.LevelInitializingComment = "Hearing Buses";
			this.ApplyBuses();
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x00042925 File Offset: 0x00040B25
		public static bool PlayingBGM
		{
			get
			{
				return AudioManager.playingBGM;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0004292C File Offset: 0x00040B2C
		private static bool LogEvent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00042930 File Offset: 0x00040B30
		public static bool TryCreateEventInstance(string eventPath, out EventInstance eventInstance)
		{
			eventInstance = default(EventInstance);
			if (AudioManager.Instance.useArchivedSound)
			{
				eventPath = "Archived/" + eventPath;
			}
			string text = "event:/" + eventPath;
			try
			{
				eventInstance = RuntimeManager.CreateInstance(text);
				return true;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				if (AudioManager.LogEvent)
				{
					Debug.LogError("[AudioEvent][Failed] " + text);
				}
			}
			return false;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000429AC File Offset: 0x00040BAC
		public static void PlayBGM(string name)
		{
			AudioManager.StopBGM();
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.playingBGM = true;
			if (string.IsNullOrWhiteSpace(name))
			{
				return;
			}
			string eventName = "Music/Loop/{soundkey}".Format(new
			{
				soundkey = name
			});
			if (AudioManager.Instance.bgmSource.Post(eventName, true) == null)
			{
				AudioManager.currentBGMName = null;
				return;
			}
			AudioManager.currentBGMName = name;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00042A14 File Offset: 0x00040C14
		public static void StopBGM()
		{
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.Instance.bgmSource.StopAll(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.currentBGMName = null;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00042A3C File Offset: 0x00040C3C
		public static void PlayStringer(string key)
		{
			string eventName = "Music/Stinger/{key}".Format(new
			{
				key
			});
			AudioManager.Instance.stingerSource.Post(eventName, true);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00042A6C File Offset: 0x00040C6C
		private void OnBulletFlyby(Vector3 vector)
		{
			AudioManager.Post("SFX/Combat/Bullet/flyby", vector);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00042A7A File Offset: 0x00040C7A
		public static void SetState(string stateGroup, string state)
		{
			AudioManager.globalStates[stateGroup] = state;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00042A88 File Offset: 0x00040C88
		public static string GetState(string stateGroup)
		{
			string result;
			if (AudioManager.globalStates.TryGetValue(stateGroup, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00042AA7 File Offset: 0x00040CA7
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Backslash))
			{
				this.useArchivedSound = !this.useArchivedSound;
				Debug.Log(string.Format("USE ARCHIVED SOUND:{0}", this.useArchivedSound));
			}
			this.UpdateListener();
			this.UpdateBuses();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00042AE8 File Offset: 0x00040CE8
		private void UpdateListener()
		{
			if (LevelManager.Instance == null)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					this.listener.transform.position = main.transform.position;
					this.listener.transform.rotation = main.transform.rotation;
				}
				return;
			}
			GameCamera gameCamera = LevelManager.Instance.GameCamera;
			if (gameCamera != null)
			{
				if (CharacterMainControl.Main != null)
				{
					this.listener.transform.position = CharacterMainControl.Main.transform.position + Vector3.up * 2f;
				}
				else
				{
					this.listener.transform.position = gameCamera.renderCamera.transform.position;
				}
				this.listener.transform.rotation = gameCamera.renderCamera.transform.rotation;
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00042BE4 File Offset: 0x00040DE4
		private void UpdateBuses()
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				if (bus.Dirty)
				{
					bus.Apply();
				}
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00042C38 File Offset: 0x00040E38
		private void ApplyBuses()
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.Apply();
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00042C84 File Offset: 0x00040E84
		private void OnFootStepSound(Vector3 position, CharacterSoundMaker.FootStepTypes type, CharacterMainControl character)
		{
			if (character == null)
			{
				return;
			}
			GameObject gameObject = character.gameObject;
			string value = "floor";
			this.MSetParameter(gameObject, "terrain", value);
			if (character.FootStepMaterialType != AudioManager.FootStepMaterialType.noSound)
			{
				string charaType = character.FootStepMaterialType.ToString();
				string strengthType = "light";
				switch (type)
				{
				case CharacterSoundMaker.FootStepTypes.walkLight:
				case CharacterSoundMaker.FootStepTypes.runLight:
					strengthType = "light";
					break;
				case CharacterSoundMaker.FootStepTypes.walkHeavy:
				case CharacterSoundMaker.FootStepTypes.runHeavy:
					strengthType = "heavy";
					break;
				}
				AudioManager.Post("Char/Footstep/footstep_{charaType}_{strengthType}".Format(new
				{
					charaType,
					strengthType
				}), character.gameObject);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x00042D1D File Offset: 0x00040F1D
		public static bool Initialized
		{
			get
			{
				return RuntimeManager.IsInitialized;
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00042D24 File Offset: 0x00040F24
		private void MSetParameter(GameObject gameObject, string parameterName, string value)
		{
			if (gameObject == null)
			{
				Debug.LogError("Game Object must exist");
				return;
			}
			AudioObject.GetOrCreate(gameObject).SetParameterByNameWithLabel(parameterName, value);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00042D48 File Offset: 0x00040F48
		private EventInstance? MPost(string eventName, GameObject gameObject = null)
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(eventName))
			{
				return null;
			}
			if (gameObject == null)
			{
				gameObject = AudioManager.Instance.gameObject;
			}
			else if (!gameObject.activeInHierarchy)
			{
				Debug.LogWarning("Posting event on inactive object, canceled");
				return null;
			}
			return AudioObject.GetOrCreate(gameObject).Post(eventName ?? "", true);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00042DC4 File Offset: 0x00040FC4
		private EventInstance? MPost(string eventName, Vector3 position)
		{
			AudioManager.SoundSourcePool.Get().transform.position = position;
			EventInstance value;
			if (!AudioManager.TryCreateEventInstance(eventName ?? "", out value))
			{
				return null;
			}
			value.set3DAttributes(position.To3DAttributes());
			value.start();
			value.release();
			return new EventInstance?(value);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00042E27 File Offset: 0x00041027
		public static void StopAll(GameObject gameObject, FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
		{
			AudioObject.GetOrCreate(gameObject).StopAll(mode);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00042E38 File Offset: 0x00041038
		internal void MSetRTPC(string key, float value, GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				RuntimeManager.StudioSystem.setParameterByName("parameter:/" + key, value, false);
				if (AudioManager.LogEvent)
				{
					Debug.Log(string.Format("[AudioEvent][Parameter][Global] {0} = {1}", key, value));
					return;
				}
			}
			else
			{
				AudioObject.GetOrCreate(gameObject).SetParameterByName("parameter:/" + key, value);
				if (AudioManager.LogEvent)
				{
					Debug.Log(string.Format("[AudioEvent][Parameter][GameObject] {0} = {1}", key, value), gameObject);
				}
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00042EBC File Offset: 0x000410BC
		internal static void SetRTPC(string key, float value, GameObject gameObject = null)
		{
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.Instance.MSetRTPC(key, value, gameObject);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00042ED9 File Offset: 0x000410D9
		public static void SetVoiceType(GameObject gameObject, AudioManager.VoiceType voiceType)
		{
			if (gameObject == null)
			{
				return;
			}
			AudioObject.GetOrCreate(gameObject).VoiceType = voiceType;
		}

		// Token: 0x04000D53 RID: 3411
		private bool useArchivedSound;

		// Token: 0x04000D54 RID: 3412
		[SerializeField]
		private AudioObject ambientSource;

		// Token: 0x04000D55 RID: 3413
		[SerializeField]
		private AudioObject bgmSource;

		// Token: 0x04000D56 RID: 3414
		[SerializeField]
		private AudioObject stingerSource;

		// Token: 0x04000D57 RID: 3415
		[SerializeField]
		private AudioManager.Bus masterBus = new AudioManager.Bus("Master");

		// Token: 0x04000D58 RID: 3416
		[SerializeField]
		private AudioManager.Bus sfxBus = new AudioManager.Bus("Master/SFX");

		// Token: 0x04000D59 RID: 3417
		[SerializeField]
		private AudioManager.Bus musicBus = new AudioManager.Bus("Master/Music");

		// Token: 0x04000D5A RID: 3418
		private static Transform _soundSourceParent;

		// Token: 0x04000D5B RID: 3419
		private static ObjectPool<GameObject> _soundSourcePool;

		// Token: 0x04000D5C RID: 3420
		private const string path_hitmarker_norm = "SFX/Combat/Marker/hitmarker";

		// Token: 0x04000D5D RID: 3421
		private const string path_hitmarker_crit = "SFX/Combat/Marker/hitmarker_head";

		// Token: 0x04000D5E RID: 3422
		private const string path_killmarker_norm = "SFX/Combat/Marker/killmarker";

		// Token: 0x04000D5F RID: 3423
		private const string path_killmarker_crit = "SFX/Combat/Marker/killmarker_head";

		// Token: 0x04000D60 RID: 3424
		private const string path_music_death = "Music/Stinger/stg_death";

		// Token: 0x04000D61 RID: 3425
		private const string path_bullet_flyby = "SFX/Combat/Bullet/flyby";

		// Token: 0x04000D62 RID: 3426
		private const string path_pickup_item_fmt_soundkey = "SFX/Item/pickup_{soundkey}";

		// Token: 0x04000D63 RID: 3427
		private const string path_put_item_fmt_soundkey = "SFX/Item/put_{soundkey}";

		// Token: 0x04000D64 RID: 3428
		private const string path_ambient_fmt_soundkey = "Amb/amb_{soundkey}";

		// Token: 0x04000D65 RID: 3429
		private const string path_music_loop_fmt_soundkey = "Music/Loop/{soundkey}";

		// Token: 0x04000D66 RID: 3430
		private const string path_footstep_fmt_soundkey = "Char/Footstep/footstep_{charaType}_{strengthType}";

		// Token: 0x04000D67 RID: 3431
		public const string path_reload_fmt_soundkey = "SFX/Combat/Gun/Reload/{soundkey}";

		// Token: 0x04000D68 RID: 3432
		public const string path_shoot_fmt_gunkey = "SFX/Combat/Gun/Shoot/{soundkey}";

		// Token: 0x04000D69 RID: 3433
		public const string path_task_finished = "UI/mission_small";

		// Token: 0x04000D6A RID: 3434
		public const string path_building_built = "UI/building_up";

		// Token: 0x04000D6B RID: 3435
		public const string path_gun_unload = "SFX/Combat/Gun/unload";

		// Token: 0x04000D6C RID: 3436
		public const string path_stinger_fmt_key = "Music/Stinger/{key}";

		// Token: 0x04000D6D RID: 3437
		private static bool playingBGM;

		// Token: 0x04000D6E RID: 3438
		private static EventInstance bgmEvent;

		// Token: 0x04000D6F RID: 3439
		private static string currentBGMName;

		// Token: 0x04000D70 RID: 3440
		private static Dictionary<string, string> globalStates = new Dictionary<string, string>();

		// Token: 0x04000D71 RID: 3441
		private static Dictionary<int, AudioManager.VoiceType> gameObjectVoiceTypes = new Dictionary<int, AudioManager.VoiceType>();

		// Token: 0x02000522 RID: 1314
		[Serializable]
		public class Bus
		{
			// Token: 0x1700074D RID: 1869
			// (get) Token: 0x0600278B RID: 10123 RVA: 0x00090A04 File Offset: 0x0008EC04
			public string Name
			{
				get
				{
					return this.volumeRTPC;
				}
			}

			// Token: 0x1700074E RID: 1870
			// (get) Token: 0x0600278C RID: 10124 RVA: 0x00090A0C File Offset: 0x0008EC0C
			// (set) Token: 0x0600278D RID: 10125 RVA: 0x00090A14 File Offset: 0x0008EC14
			public float Volume
			{
				get
				{
					return this.volume;
				}
				set
				{
					this.volume = value;
					this.Apply();
				}
			}

			// Token: 0x1700074F RID: 1871
			// (get) Token: 0x0600278E RID: 10126 RVA: 0x00090A23 File Offset: 0x0008EC23
			// (set) Token: 0x0600278F RID: 10127 RVA: 0x00090A2B File Offset: 0x0008EC2B
			public bool Mute
			{
				get
				{
					return this.mute;
				}
				set
				{
					this.mute = value;
					this.Apply();
				}
			}

			// Token: 0x17000750 RID: 1872
			// (get) Token: 0x06002790 RID: 10128 RVA: 0x00090A3A File Offset: 0x0008EC3A
			public bool Dirty
			{
				get
				{
					return this.appliedVolume != this.Volume;
				}
			}

			// Token: 0x06002791 RID: 10129 RVA: 0x00090A50 File Offset: 0x0008EC50
			public void Apply()
			{
				try
				{
					FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + this.volumeRTPC);
					bus.setVolume(this.Volume);
					bus.setMute(this.Mute);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				this.appliedVolume = this.Volume;
				OptionsManager.Save<float>(this.SaveKey, this.volume);
			}

			// Token: 0x17000751 RID: 1873
			// (get) Token: 0x06002792 RID: 10130 RVA: 0x00090AC8 File Offset: 0x0008ECC8
			private string SaveKey
			{
				get
				{
					return "Audio/" + this.volumeRTPC;
				}
			}

			// Token: 0x06002793 RID: 10131 RVA: 0x00090ADA File Offset: 0x0008ECDA
			public Bus(string rtpc)
			{
				this.volumeRTPC = rtpc;
			}

			// Token: 0x06002794 RID: 10132 RVA: 0x00090B0A File Offset: 0x0008ED0A
			internal void LoadOptions()
			{
				this.volume = OptionsManager.Load<float>(this.SaveKey, 1f);
			}

			// Token: 0x06002795 RID: 10133 RVA: 0x00090B22 File Offset: 0x0008ED22
			internal void NotifyOptionsChanged(string key)
			{
				if (key == this.SaveKey)
				{
					this.LoadOptions();
				}
			}

			// Token: 0x04001E3B RID: 7739
			[SerializeField]
			private string volumeRTPC = "Master";

			// Token: 0x04001E3C RID: 7740
			[HideInInspector]
			[SerializeField]
			private float volume = 1f;

			// Token: 0x04001E3D RID: 7741
			[HideInInspector]
			[SerializeField]
			private bool mute;

			// Token: 0x04001E3E RID: 7742
			private float appliedVolume = float.MinValue;
		}

		// Token: 0x02000523 RID: 1315
		public enum FootStepMaterialType
		{
			// Token: 0x04001E40 RID: 7744
			organic,
			// Token: 0x04001E41 RID: 7745
			mech,
			// Token: 0x04001E42 RID: 7746
			danger,
			// Token: 0x04001E43 RID: 7747
			noSound
		}

		// Token: 0x02000524 RID: 1316
		public enum VoiceType
		{
			// Token: 0x04001E45 RID: 7749
			Duck,
			// Token: 0x04001E46 RID: 7750
			Robot,
			// Token: 0x04001E47 RID: 7751
			Wolf,
			// Token: 0x04001E48 RID: 7752
			Chicken,
			// Token: 0x04001E49 RID: 7753
			Crow,
			// Token: 0x04001E4A RID: 7754
			Eagle
		}
	}
}
