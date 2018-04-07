using UnityEngine;

public class DrugManager : MonoBehaviour {
    
    private Drug selected;

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
}
