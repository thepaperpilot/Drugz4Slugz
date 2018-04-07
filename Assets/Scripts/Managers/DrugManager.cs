using System;
using System.Collections;
using UnityEngine;

public class DrugManager : MonoBehaviour {

    public static DrugManager instance;
    
    private Drug selected;

    void Awake() {
        instance = this;
    }

    public void Pickup(Drug drug) {
        if (selected == drug) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            selected = null;
        } else {
            Cursor.SetCursor(drug.cursorImage, Vector2.zero, CursorMode.ForceSoftware);
            selected = drug;
        }        
    }

    public void Drop(GameObject enclosure) {
        if (selected != null) {
            enclosure.GetComponentInChildren<Slug>().ApplyDrug(selected);

            //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            //selected = null;
        }
    }

    public static void Delay(float seconds, Action callback) {
        instance.StartCoroutine(instance._Delay(seconds, callback));
    }

    IEnumerator _Delay(float seconds, Action callback) {
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
