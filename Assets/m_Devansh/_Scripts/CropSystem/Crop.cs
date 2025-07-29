using UnityEngine;

namespace m_Devansh._Scripts.CropSystem
{
    [CreateAssetMenu(fileName = "Crop", menuName = "Scriptable Objects/Crop", order = 0)]
    public class Crop : ScriptableObject
    {
        public int currStage = 0;
        public Sprite cropIcon;
        public GameObject[] cropStagesPrefabs;
        [Header("Crop Properties")]
        public string cropName;
        public string plantDescription;
        public float growthTime;
        public AudioClip plantSound;
    }
}
