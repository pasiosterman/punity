using UnityEngine;
using System;

namespace PGeneric.Events
{
    /// <summary>
    /// Event arguments for notifying about changes in collections.
    /// I.E when CollectionChanged or when Item added or removed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionEventArgs<T> : EventArgs
    {
        public T[] Items { get; private set; }
        public T TargetItem { get; private set; }

        public EventType Event { get; private set; }
        public enum EventType { Attach, CollectionChanged, ItemAdded, ItemRemoved, SortingChanged };

        public void SetTargetItem(T targetItem)
        {
            TargetItem = targetItem;
        }

        public void SetItems(T[] items)
        {
            Items = items;
        }

        public K GetTargetItemAs<K>() where K : MonoBehaviour
        {
            if (TargetItem != null)
            {
                MonoBehaviour monoBehaviour = TargetItem as MonoBehaviour;
                if (monoBehaviour != null)
                {
                    K comp = monoBehaviour.GetComponent<K>();
                    return comp;
                }
            }
            return default(K);
        }

        /// <summary>
        /// Event for multi item events such as CollectionChanged
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="items">collection related to event</param>
        /// <returns>Collection event with arguments</returns>
        public static CollectionEventArgs<T> Create(EventType eventType, T[] items)
        {
            CollectionEventArgs<T> args = new CollectionEventArgs<T>
            {
                Event = eventType,
                Items = items
            };
            return args;
        }

        /// <summary>
        /// Event for single item events such as ItemAdded, ItemRemoved
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="item">item related to event</param>
        /// <returns>Collection event with arguments</returns>
        public static CollectionEventArgs<T> Create(EventType eventType, T item)
        {
            CollectionEventArgs<T> args = new CollectionEventArgs<T>
            {
                Event = eventType,
                TargetItem = item
            };
            return args;
        }
    }
}