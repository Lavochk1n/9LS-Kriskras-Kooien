using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class SpawnPlayerSetupMenu : MonoBehaviour
{
    public GameObject playerSetupmenuPrefab;
    public PlayerInput pi;

    [SerializeField] List<RenderTexture> renderTextures = new(); 

    private void Awake()
    {
        var rootMenu = GameObject.Find("MainLayout");

        if (rootMenu != null )
        {
            var menu = Instantiate(playerSetupmenuPrefab, rootMenu.transform);
            pi.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(pi);
            menu.GetComponent<PlayerSetupMenuController>().rawImage.texture = renderTextures[pi.playerIndex];
            
                menu.transform.SetSiblingIndex(pi.playerIndex);
            
        }
    }
}
