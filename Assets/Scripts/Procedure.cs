using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Procedure")]
public class Procedure : ScriptableObject {

    [Serializable]
    public struct Dose {
        public string drug;
        public int dosage;
    }

    [TextArea(3, 7)]
    public string description;
    public Dose[] firstSlug;
    public Dose[] secondSlug;
    public Dose[] thirdSlug;
    public Dose[] fourthSlug;
}
