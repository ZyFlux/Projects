//Built after read

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Trace  {
    //public static List<Event> listOfEvents = new List<Event>(); //All events listed here
    public static List<ActorEvent> allEvents = new List<ActorEvent>();
    public static int pointerToCurrEvent=0; //Points to the currently active event
}
