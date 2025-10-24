using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200022D RID: 557
	public class AudioObject : MonoBehaviour
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x00042F3F File Offset: 0x0004113F
		// (set) Token: 0x0600114C RID: 4428 RVA: 0x00042F47 File Offset: 0x00041147
		public AudioManager.VoiceType VoiceType
		{
			get
			{
				return this.voiceType;
			}
			set
			{
				this.voiceType = value;
			}
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00042F50 File Offset: 0x00041150
		internal static AudioObject GetOrCreate(GameObject from)
		{
			AudioObject component = from.GetComponent<AudioObject>();
			if (component != null)
			{
				return component;
			}
			return from.AddComponent<AudioObject>();
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00042F78 File Offset: 0x00041178
		public EventInstance? PostQuak(string soundKey)
		{
			string eventName = "Char/Voice/vo_" + this.voiceType.ToString().ToLower() + "_" + soundKey;
			return this.Post(eventName, true);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00042FB4 File Offset: 0x000411B4
		public EventInstance? Post(string eventName, bool doRelease = true)
		{
			EventInstance eventInstance;
			if (!AudioManager.TryCreateEventInstance(eventName ?? "", out eventInstance))
			{
				return null;
			}
			eventInstance.setCallback(new EVENT_CALLBACK(AudioObject.EventCallback), (EVENT_CALLBACK_TYPE)4294967295U);
			this.events.Add(eventInstance);
			eventInstance.set3DAttributes(base.gameObject.transform.position.To3DAttributes());
			this.ApplyParameters(eventInstance);
			eventInstance.start();
			if (doRelease)
			{
				eventInstance.release();
			}
			return new EventInstance?(eventInstance);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0004303C File Offset: 0x0004123C
		public void Stop(string eventName, FMOD.Studio.STOP_MODE mode)
		{
			foreach (EventInstance eventInstance in this.events)
			{
				EventDescription eventDescription;
				string str;
				if (eventInstance.getDescription(out eventDescription) == RESULT.OK && eventDescription.getPath(out str) == RESULT.OK && !("event:/" + str != eventName))
				{
					eventInstance.stop(mode);
					break;
				}
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x000430BC File Offset: 0x000412BC
		private static RESULT EventCallback(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
		{
			if (type <= EVENT_CALLBACK_TYPE.PLUGIN_DESTROYED)
			{
				if (type <= EVENT_CALLBACK_TYPE.STOPPED)
				{
					if (type <= EVENT_CALLBACK_TYPE.STARTED)
					{
						switch (type)
						{
						case EVENT_CALLBACK_TYPE.CREATED:
						case EVENT_CALLBACK_TYPE.DESTROYED:
						case EVENT_CALLBACK_TYPE.CREATED | EVENT_CALLBACK_TYPE.DESTROYED:
						case EVENT_CALLBACK_TYPE.STARTING:
							break;
						default:
							if (type != EVENT_CALLBACK_TYPE.STARTED)
							{
							}
							break;
						}
					}
					else if (type != EVENT_CALLBACK_TYPE.RESTARTED && type != EVENT_CALLBACK_TYPE.STOPPED)
					{
					}
				}
				else if (type <= EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND)
				{
					if (type != EVENT_CALLBACK_TYPE.START_FAILED && type != EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND)
					{
					}
				}
				else if (type != EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND && type != EVENT_CALLBACK_TYPE.PLUGIN_CREATED && type != EVENT_CALLBACK_TYPE.PLUGIN_DESTROYED)
				{
				}
			}
			else if (type <= EVENT_CALLBACK_TYPE.SOUND_STOPPED)
			{
				if (type <= EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
				{
					if (type != EVENT_CALLBACK_TYPE.TIMELINE_MARKER && type != EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
					{
					}
				}
				else if (type != EVENT_CALLBACK_TYPE.SOUND_PLAYED && type != EVENT_CALLBACK_TYPE.SOUND_STOPPED)
				{
				}
			}
			else if (type <= EVENT_CALLBACK_TYPE.VIRTUAL_TO_REAL)
			{
				if (type != EVENT_CALLBACK_TYPE.REAL_TO_VIRTUAL && type != EVENT_CALLBACK_TYPE.VIRTUAL_TO_REAL)
				{
				}
			}
			else if (type == EVENT_CALLBACK_TYPE.START_EVENT_COMMAND || type != EVENT_CALLBACK_TYPE.NESTED_TIMELINE_BEAT)
			{
			}
			return RESULT.OK;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x000431AC File Offset: 0x000413AC
		private void FixedUpdate()
		{
			if (this == null)
			{
				return;
			}
			if (base.transform == null)
			{
				return;
			}
			if (this.events == null)
			{
				return;
			}
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.set3DAttributes(base.transform.position.To3DAttributes());
				}
			}
			if (this.needCleanup)
			{
				this.events.RemoveAll((EventInstance e) => !e.isValid());
				this.needCleanup = false;
			}
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00043280 File Offset: 0x00041480
		internal void SetParameterByName(string parameter, float value)
		{
			this.parameters[parameter] = value;
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.setParameterByName(parameter, value, false);
				}
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000432F8 File Offset: 0x000414F8
		internal void SetParameterByNameWithLabel(string parameter, string label)
		{
			this.strParameters[parameter] = label;
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.setParameterByNameWithLabel(parameter, label, false);
				}
			}
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00043370 File Offset: 0x00041570
		private void ApplyParameters(EventInstance eventInstance)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
			{
				eventInstance.setParameterByName(keyValuePair.Key, keyValuePair.Value, false);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in this.strParameters)
			{
				eventInstance.setParameterByNameWithLabel(keyValuePair2.Key, keyValuePair2.Value, false);
			}
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00043428 File Offset: 0x00041628
		internal void StopAll(FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
		{
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.stop(mode);
				}
			}
		}

		// Token: 0x04000D72 RID: 3442
		private Dictionary<string, float> parameters = new Dictionary<string, float>();

		// Token: 0x04000D73 RID: 3443
		private Dictionary<string, string> strParameters = new Dictionary<string, string>();

		// Token: 0x04000D74 RID: 3444
		private AudioManager.VoiceType voiceType;

		// Token: 0x04000D75 RID: 3445
		public List<EventInstance> events = new List<EventInstance>();

		// Token: 0x04000D76 RID: 3446
		private bool needCleanup;
	}
}
