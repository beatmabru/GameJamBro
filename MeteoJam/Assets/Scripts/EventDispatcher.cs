using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDispatcher : MonoBehaviour {

    public static EventDispatcher instance;
    public enum EventId
    {
        WEATHER_RESPITE_CHANGE,
        CLOTHES_THROW,
        CLOTHES_GET,
    }

    public class Event
    {
        public EventId id;
        public object data;

        public Event(EventId _id, object _data) { id = _id; data = _data; }
    }

	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;
    }
    // Update is called once per frame
    void Update()
    {
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
