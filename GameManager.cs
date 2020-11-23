using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameBeginCanvas;
    public GameObject gameOverCanvas;
    public GameObject gameWinCanvas;
    public GameObject gameCreateObject;
    public GameObject popupManagerObject;
    public Canvas masterCanvas;
    private PopupManager _popupManager;
    public GameObject currentCanvas;
    private CanvasCreate _canvasCreate;
    public int gameLength = 10;
    public int currentLevel = 0;
    private int[] _gameSeed;
    private float beginTime = 0;
    private MapType _mapType;
    private  ImageManager imageManager = new ImageManager();

    public ImageManager GetImageManager()
    {
        return imageManager;
    }
    public PopupManager getPopupManagerObject()
    {
        return _popupManager;
    }
    public void SetMapTypeAsAlien()
    {
        currentLevel = 0;
        _mapType = MapType.Alien;
        imageManager.setType(_mapType);
    }
    public void SetMapTypeAsDot()
    {
        currentLevel = 0;
        _mapType = MapType.Dot;
        imageManager.setType(_mapType);
    }
    public void SetMapTypeAsGreen()
    {
        currentLevel = 0;
        _mapType = MapType.Green;
        imageManager.setType(_mapType);
    }
    public void SetMapTypeAsCatEye()
    {
        currentLevel = 0;
        _mapType = MapType.CatEye;
        imageManager.setType(_mapType);
    }
    public void SetMapTypeAsWhiteWorld()
    {
        currentLevel = 0;
        _mapType = MapType.WhiteWorld;
        imageManager.setType(_mapType);
    }

    public void Awake()
    {
        _canvasCreate=gameCreateObject.GetComponent<CanvasCreate>();
        _popupManager=popupManagerObject.GetComponent<PopupManager>();
    }

    public void initBeginCanvas()
    {
        currentCanvas = GameObject.Instantiate(gameBeginCanvas,masterCanvas.transform);
        
        Transform button = currentCanvas.transform.GetChild(0);
        Button btn=button.GetComponent<Button>();
        btn.onClick.AddListener(this.nextLevel);
    }
    public void DetroyCurrentCanvas()
    {
        if (currentCanvas != null)
        {
         Destroy(currentCanvas);
         currentCanvas = null;
        }
        _canvasCreate.DestroyCanvas();
    }
    public void nextLevel()
    {
        print(currentLevel);
        if (currentLevel == 0)
        {
            beginTime = Time.time;
            DetroyCurrentCanvas();
            _gameSeed=getRandomIntArray(gameLength);
        }

        if (currentLevel >= gameLength)
        {
            _canvasCreate.ShowWinPopup();
            return;
        }
        _canvasCreate.createGameCanvas(currentLevel,_gameSeed);
        currentLevel++;

    }

    public static void shuffle(int[] arr)
    {
        System.Random rng = new System.Random();
        int n = arr.Length;
        for (int i = 0 ; i < n; i++) {
            // 从 i 到最后随机选一个元素
            int rand = rng.Next(i, n - 1);
            swap(arr,i,rand);
        }

    }
    private int[] getRandomIntArray(int range)
    {
        int[] arr=new int[range];
        for (int i = 0; i < range; i++)
        {
            arr[i] = i;
        }

        shuffle(arr);
        return arr;
    }

    public static void swap(int[] arr,int index1,int index2)
    {
        int temp = arr[index1];
        arr[index1] = arr[index2];
        arr[index2] = temp;
    }

    public void GameOver()
    {
        currentLevel = 0;
        DetroyCurrentCanvas();
        initOverCanvas();
    }
    public void GameWin()
    {
        currentLevel = 0;
        DetroyCurrentCanvas();
        initWinCanvas();
    }

    public void initOverCanvas()
    {
        currentCanvas= GameObject.Instantiate(gameOverCanvas,masterCanvas.transform);
        currentCanvas.GetComponentInChildren<Button>().onClick.AddListener(returnToBegin);
    }
    
    public void initWinCanvas()
    {
        currentCanvas= GameObject.Instantiate(gameWinCanvas,masterCanvas.transform);
        currentCanvas.GetComponentInChildren<Button>().onClick.AddListener(returnToBegin);
        Text text=currentCanvas.GetComponentInChildren<Text>();
        text.text = text.text + (Time.time - beginTime).ToString("\r耗费时间：#0.0s");
    }

    public void returnToBegin()
    {
        DetroyCurrentCanvas();
        initBeginCanvas();
    }

    public float getCostTime()
    {
        return Time.time - beginTime;
    }
}
