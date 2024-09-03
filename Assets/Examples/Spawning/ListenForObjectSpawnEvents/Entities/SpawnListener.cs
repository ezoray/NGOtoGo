using UnityEngine;

namespace NGOtoGo.Examples.Spawning.ListenForObjectSpawnEvents
{
    /// <summary>
    /// Listens for spawn events by subscribing to the OnSpawn event of EventNetworkBehaviour.
    /// </summary>
    public class SpawnListener : MonoBehaviour
    {
        private void Start()
        {
            EventNetworkBehaviour.OnSpawn += OnSpawn;
        }

        private void OnSpawn(EventNetworkBehaviour behaviour)
        {
            if(behaviour is SpawnObject && behaviour.TryGetComponent<SpawnObject>(out var spawnObject))
            {
                Debug.Log($"SpawnListener OnSpawn networkObjectId: {spawnObject.NetworkObjectId} spawnObject: {spawnObject}");
            }
        }

        private void OnDestroy()
        {
            EventNetworkBehaviour.OnSpawn -= OnSpawn;
        }
    }
}
