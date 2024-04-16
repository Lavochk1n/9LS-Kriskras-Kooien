using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

namespace Quarantine 
{
    public class SceneManager : MonoBehaviour
    {
        public List<string> scenes = new List<string>();

        public void GetScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scenes[sceneIndex]);
        }

        
    }
}
