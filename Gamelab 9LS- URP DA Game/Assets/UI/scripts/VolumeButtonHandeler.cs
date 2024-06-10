using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButtonHandeler : MonoBehaviour
{

    [SerializeField] private Sprite full, twoThird, oneThird, none;
    private AudioManager AM; 

    private int VolumeIndex;

    private Image sprite; 

    private void Start()
    {
        sprite = GetComponent<Image>();
        AM = AudioManager.Instance;
        VolumeIndex = 4; 
    }

    public void ToggleVolume()
    {
        VolumeIndex++; 
        if (VolumeIndex >= 4) { VolumeIndex = 0;  }

        switch (VolumeIndex)
        {
            case 0:
                sprite.sprite = none;
                AM.SetVolume(0f);
                break;
            case 1:
                sprite.sprite = oneThird;
                AM.SetVolume(.33f);

                break;
            case 2:
                sprite.sprite = twoThird;
                AM.SetVolume(.66f);

                break;
            case 3:
                sprite.sprite = full;
                AM.SetVolume(1f);

                break;
        }
    }

    public void ToggleMusicVolume()
    {
        VolumeIndex++;
        if (VolumeIndex >= 4) { VolumeIndex = 0; }

        switch (VolumeIndex)
        {
            case 0:
                sprite.sprite = none;
                AM.SetMusicVolume(0f);
                break;
            case 1:
                sprite.sprite = oneThird;
                AM.SetMusicVolume(.33f);

                break;
            case 2:
                sprite.sprite = twoThird;
                AM.SetMusicVolume(.66f);

                break;
            case 3:
                sprite.sprite = full;
                AM.SetMusicVolume(1f);

                break;
        }
    }

}
