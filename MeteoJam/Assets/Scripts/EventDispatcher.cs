using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDispatcher : ScriptableObject {

    public static EventDispatcher instance = ScriptableObject.CreateInstance<EventDispatcher>();
    public enum EventId
    {
        WEATHER_RESPITE_CHANGE,
        CLOTHES_THROW,
        CLOTHES_GET,
        PLAYER_DEATH,
    }

    public class Event
    {
        public EventId id;
        public object data;

        public Event(EventId _id, object _data) { id = _id; data = _data; }
    }

    public List<IEventListener> listeners = new List<IEventListener>();

    public void ThrowEvent(Event e)
    {
        foreach(IEventListener listener in listeners)
        {
            if (listener.HandleEvent(e) )
                return;
        }
    }

    public interface IEventListener
    {
        //Handles the event, return true if was handled
        bool HandleEvent(Event e);
    }
}
