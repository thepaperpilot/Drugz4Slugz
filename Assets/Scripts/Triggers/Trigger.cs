using UnityEngine;

public class Trigger : Object {

    public enum Type {
        FIRST,
        RANDOM,
        EVENT
    }

    public Type type;
    public int adviceRating;
}
