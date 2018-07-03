namespace HeadsUp {

    using HoloToolkit.Unity;
    using HoloToolkit.Unity.SpatialMapping;
    using UnityEngine;

    public class ModelAnchorManager : MonoBehaviour {
       
        public WorldAnchorManager AnchorManager { get; private set; }
        public SpatialMappingManager SpatialMappingManager { get; private set; }
        public string SavedAnchorFriendlyName {
            get {
                if (string.IsNullOrEmpty(savedAnchorFriendlyName))
                    savedAnchorFriendlyName = System.Guid.NewGuid().ToString();
                return savedAnchorFriendlyName;
            }
        }

        //protected WorldAnchorManager anchorManager;
        //protected SpatialMappingManager spatialMappingManager;
        protected string savedAnchorFriendlyName;

        void Start() {

            this.AnchorManager = WorldAnchorManager.Instance;
            if (this.AnchorManager == null)
                Debug.LogError("This script expects that you have a WorldAnchorManager component in your scene.");

            this.SpatialMappingManager = SpatialMappingManager.Instance;
            if (this.SpatialMappingManager == null)
                Debug.LogError("This script expects that you have a SpatialMappingManager component in your scene.");

            if (this.AnchorManager != null && this.SpatialMappingManager != null)
                this.AnchorManager.AttachAnchor(gameObject, SavedAnchorFriendlyName);
            else
                Destroy(this);
        }
    }
}
