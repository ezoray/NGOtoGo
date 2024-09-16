using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.NetworkVariables.NetworkVariableDictionary
{
    /// <summary>
    /// In-scene network object containing the NetworkVariable dictionary field.
    /// When the dictionary contents change OnValueChanged is triggered and any added, removed or changed dictionary values are
    /// found by comparing the previous and current dictionaries, those values are then logged.
    /// Changes are found manually and using Linq with the time taken being logged.
    /// </summary>
    public class InScene : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI countText;

        NetworkVariable<Dictionary<int, int>> netDictionary = new NetworkVariable<Dictionary<int, int>>(new Dictionary<int, int>(),
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        private void Awake()
        {            
            netDictionary.OnValueChanged += OnDictionaryChanged;
        }

        private void Start()
        {
            Debug.Log("InScene Start");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log("InScene OnNetworkSpawn dictionary count: " + netDictionary.Value.Count);

            countText.text = netDictionary.Value.Count.ToString();

            foreach (var entry in netDictionary.Value)
            {
                Debug.Log($"Dictionary key: {entry.Key} value: {entry.Value}");
            }
        }

        private void OnDictionaryChanged(Dictionary<int, int> previous, Dictionary<int, int> current)
        {
            Debug.Log($"InScene OnDictionaryChanged previous: {previous.Count} current: {current.Count}");

            countText.text = netDictionary.Value.Count.ToString();

            LogDictionaryChangesLinq(previous, current);
            LogDictionaryChangesManual(previous, current);

            foreach (KeyValuePair<int, int> item in current)
            {
                Debug.Log($"Dictionary - key: {item.Key} value: {item.Value}");
            }
        }

        private void LogDictionaryChangesLinq(Dictionary<int, int> previous, Dictionary<int, int> current)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

            stopWatch.Start();
            Dictionary<int, int> added = current.Where(kvp => !previous.ContainsKey(kvp.Key)).ToDictionary(item => item.Key, item => item.Value);
            Dictionary<int, int> removed = previous.Where(kvp => !current.ContainsKey(kvp.Key)).ToDictionary(item => item.Key, item => item.Value);
            Dictionary<int, int> changed = previous.Where(kvp => current.ContainsKey(kvp.Key) && current[kvp.Key] != kvp.Value).ToDictionary(item => item.Key, item => current[item.Key]);
            //    Dictionary<int, int> unchanged = previous.Where(kvp => current.ContainsKey(kvp.Key) && current[kvp.Key] == kvp.Value).ToDictionary(item => item.Key, item => item.Value);
            stopWatch.Stop();
                     
            foreach (KeyValuePair<int, int> item in added)
            {
                Debug.Log($"Linq Added - key: {item.Key} value: {item.Value}");
            }
            
            foreach (KeyValuePair<int, int> item in removed)
            {
                Debug.Log($"Linq Removed - key: {item.Key} value: {item.Value}");
            }
            
            foreach (KeyValuePair<int, int> item in changed)
            {
                Debug.Log($"Linq Changed - key: {item.Key} value: {item.Value}");
            }

            //foreach (KeyValuePair<int, int> item in unchanged)
            //{
            //    Debug.Log($"Linq Unchanged - key: {item.Key} value: {item.Value}");
            //}

            Debug.Log($"Linq time taken: {(double)stopWatch.ElapsedTicks / (System.Diagnostics.Stopwatch.Frequency / 1000.0)} ms");
        }

        private void LogDictionaryChangesManual(Dictionary<int, int> previous, Dictionary<int, int> current)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

            stopWatch.Start();
            Dictionary<int, int> added = new Dictionary<int, int>();
            Dictionary<int, int> removed = new Dictionary<int, int>();
            Dictionary<int, int> changed = new Dictionary<int, int>();
         //   Dictionary<int, int> unchanged = new Dictionary<int, int>();

            // added
            foreach (KeyValuePair<int, int> kvp in current)
            {
                if (!previous.ContainsKey(kvp.Key))
                {
                    added.Add(kvp.Key, kvp.Value);
                }
            }

            // removed
            foreach (var kvp in previous)
            {
                if (!current.ContainsKey(kvp.Key))
                {
                    removed.Add(kvp.Key, kvp.Value);
                }
            }

            // changed
            foreach (var kvp in previous)
            {
                if (current.ContainsKey(kvp.Key) && current[kvp.Key] != kvp.Value)
                {                    
                    changed.Add(kvp.Key, current[kvp.Key]);
                }
            }

            // unchanged
            //foreach (var kvp in previous)
            //{
            //    if (current.ContainsKey(kvp.Key) && current[kvp.Key] == kvp.Value)
            //    {
            //        unchanged.Add(kvp.Key, kvp.Value);
            //    }
            //}
            stopWatch.Stop();

            foreach (KeyValuePair<int, int> item in added)
            {
                Debug.Log($"Manual Added - key: {item.Key} value: {item.Value}");
            }

            foreach (KeyValuePair<int, int> item in removed)
            {
                Debug.Log($"Manual Removed - key: {item.Key} value: {item.Value}");
            }

            foreach (KeyValuePair<int, int> item in changed)
            {
                Debug.Log($"Manual Changed - key: {item.Key} value: {item.Value}");
            }

            //foreach (KeyValuePair<int, int> item in unchanged)
            //{
            //    Debug.Log($"Manual Unchanged - key: {item.Key} value: {item.Value}");
            //}

            Debug.Log($"Manual time taken: {(double)stopWatch.ElapsedTicks / (System.Diagnostics.Stopwatch.Frequency / 1000.0)} ms");
        }

        public void DictionaryUpdate(UpdateType updateType, int index = -1, int value = -1)
        {
            Debug.Log("DictionaryUpdate: " + updateType);

            switch (updateType)
            {
                case UpdateType.Add:
                    if (!netDictionary.Value.ContainsKey(index))
                    {
                        netDictionary.Value.Add(index, value);
                    }
                    break;

                case UpdateType.RemoveByValue:

                    int foundIndex = -1;

                    foreach (var item in netDictionary.Value)
                    {
                        if (item.Value == value)
                        {
                            foundIndex = item.Key;
                            break;
                        }
                    }

                    if(foundIndex >= 0)
                    {
                        netDictionary.Value.Remove(foundIndex);
                    }
                    break;

                case UpdateType.RemoveByIndex:
                    if (netDictionary.Value.ContainsKey(index))
                    {
                        netDictionary.Value.Remove(index);       
                    }
                    break;

                case UpdateType.UpdateValue:
                    if (netDictionary.Value.ContainsKey(index))
                    {
                        netDictionary.Value[index] = value;                   
                    }
                    break;

                case UpdateType.Clear:
                    netDictionary.Value.Clear();
                    break;
            }

            netDictionary.CheckDirtyState();
        }

        public override void OnLostOwnership()
        {
            Debug.Log("InScene OnLostOwnership:" + OwnerClientId);
        }

        public override void OnGainedOwnership()
        {
            Debug.Log("InScene OnGainedOwnership:" + OwnerClientId);
        }

        public override void OnNetworkDespawn()
        {
            netDictionary.OnValueChanged -= OnDictionaryChanged;
        }

        public Dictionary<int, int> NetDictionary { get => netDictionary.Value; set => netDictionary.Value = value; }
    }
}
