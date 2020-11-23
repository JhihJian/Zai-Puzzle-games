using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour
{

    private Sprite[] _textures;
    private int _index = 0;
    // Start is called before the first frame update
    private MapType _mapType = MapType.CatEye;
    public void setType(MapType mapType)
    {
        print("maptype:"+mapType);
        _mapType = mapType;
    }
    public Sprite GetImage()
    {
        _textures = Resources.LoadAll<Sprite>("images/"+_mapType.ToString());
        if (_index >= _textures.Length)
        {
            _index = 0;
        }

        return _textures[_index++];
    }
    public Sprite GetImage(int index)
    {
        _textures = Resources.LoadAll<Sprite>("images/"+_mapType.ToString());
        return _textures[index];
    }
}
