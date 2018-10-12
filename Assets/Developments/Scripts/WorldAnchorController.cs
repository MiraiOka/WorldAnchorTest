using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;
using UnityEngine.XR.WSA.Input;

public class WorldAnchorController : MonoBehaviour {

    private WorldAnchorStore store;

    private bool isSave = false;

    private void Start()
    {
        WorldAnchorStore.GetAsync(StoreLoaded);
        InteractionManager.InteractionSourcePressed += InteractionSourcePressed;
    }

    void InteractionSourcePressed(InteractionSourcePressedEventArgs ev)
    {
        SaveGame();
    }

    private void StoreLoaded(WorldAnchorStore store)
    {
        this.store = store;

        string[] ids = this.store.GetAllIds();
        if (ids[0] == gameObject.name)
        {
            isSave = false;
            LoadGame();
        }
    }

    private void SaveGame()
    {
        if(!isSave)
        {
            WorldAnchor anchor = gameObject.AddComponent<WorldAnchor>();
            if (anchor.isLocated)
            {
                this.isSave = this.store.Save(gameObject.name, anchor);
            } else
            {
                anchor.OnTrackingChanged += OnTrackingChanged;
            }
            
        }
    }

    private void OnTrackingChanged(WorldAnchor self, bool located)
    {
        if(located)
        {
            store.Save(self.name, self);
            isSave = true;
            self.OnTrackingChanged -= OnTrackingChanged;
        }
    }

    private void LoadGame()
    {
        WorldAnchor anchor = store.Load(gameObject.name, gameObject);
    }
}