using System.Linq;
using TMPro;
using UnityEngine;

public class SlugManager : MonoBehaviour {

    public static SlugManager instance;

    public GameObject slugPrefab;
    public string[] slimeNames;

    void Awake() {
        instance = this;
    }

    public void Spawn(GameObject enclosure) {
        GameObject newSlug = Instantiate(slugPrefab, enclosure.transform);
        newSlug.transform.SetAsFirstSibling();
        newSlug.GetComponentInChildren<Slug>().Generate();

        enclosure.transform.GetChild(1).gameObject.SetActive(false);
        enclosure.transform.GetChild(2).gameObject.SetActive(true);
        enclosure.transform.GetChild(2).GetComponentInChildren<TMP_InputField>().text = 
            slimeNames.OrderBy(n => Random.value).First();
    }
}
