using System;
using UnityEngine;

public class Trigger {

    public enum Type {
        FIRST,
        RANDOM,
        EVENT
    }

    public Type type;
    public int adviceRating;

    // For use in Type.EVENT triggers
    public virtual bool CheckValid(Slug slug, Drug.DrugState state) {
        return false;
    }
}
