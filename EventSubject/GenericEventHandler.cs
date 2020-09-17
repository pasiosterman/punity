using System.Collections.Generic;
using UnityEngine;
using System;

namespace PUnity.EventHandling
{
	public class GenericEventHandler<T> where T : EventArgs
	{
		private readonly object _sender;
		private readonly List<IEventListener<T>> _eventListeners = new List<IEventListener<T>>();

		public GenericEventHandler(object sender)
		{
			_sender = sender;
		}

		public void Attach(IEventListener<T> newListener)
		{
			if (!_eventListeners.Contains(newListener))
			{
				_eventListeners.Add(newListener);
			}
			else
			{
				UnityEngine.Object context = newListener as UnityEngine.Object;
				if (context != null)
					Debug.LogError(LogTags.SYSTEM_ERROR + " listener already attached ", context);
				else
					Debug.LogError(LogTags.SYSTEM_ERROR + " listener already attached ");
			}
		}

		public void Detach(IEventListener<T> removeListener)
		{
			if (_eventListeners.Contains(removeListener))
			{
				_eventListeners.Remove(removeListener);
			}
			else
			{
				UnityEngine.Object context = removeListener as UnityEngine.Object;
				if(context != null)
					Debug.LogError(LogTags.SYSTEM_ERROR + " no such listener attached ", context);
				else
					Debug.LogError(LogTags.SYSTEM_ERROR + " no such listener attached ");
			}
		}

		public void SendEvent(T args)
		{
            if (_eventListeners.Count == 0)
                return;

			for (int i = _eventListeners.Count - 1; i >= 0; i--)
			{
				if (_eventListeners[i] == null)
				{
					UnityEngine.Object context = _eventListeners[i] as UnityEngine.Object;
					if (context != null)
						Debug.LogError(LogTags.SYSTEM_ERROR + " removed null event listener", context);
					else
						Debug.LogError(LogTags.SYSTEM_ERROR + " removed null event listener");

					_eventListeners.RemoveAt(i);
					continue;
				}

				_eventListeners[i].HandleEvent(_sender, args);
			}
		}

	}
}