using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrugManager : MonoBehaviour {

    public static DrugManager instance;
    
    private Drug selected;

    void Awake() {
        instance = this;
    }

    public void Pickup(Drug drug) {
        if (selected == drug || drug == null) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            selected = null;
        } else {
            Cursor.SetCursor(drug.cursorImage, Vector2.zero, CursorMode.ForceSoftware);
            selected = drug;
        }        
    }

    public void Drop(GameObject enclosure) {
        Slug slug = enclosure.GetComponentInChildren<Slug>();
        if (slug == null) {
            SlugManager.instance.Spawn(enclosure);
        } else if (selected != null) {
            slug.ApplyDrug(selected);

            //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            //selected = null;
        }
    }

    public static IEnumerable<string> GetDrugs() {
        return typeof(Drug)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Drug)) && !t.IsAbstract)
            .Select(t => t.Name);
    }
}
