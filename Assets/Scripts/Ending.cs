using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public void DelayedLoad()
    {
        Invoke(nameof(LoadMenu), 5f);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
