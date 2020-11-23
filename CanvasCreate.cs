using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCreate : MonoBehaviour
{
    public GameObject canvasPrefb;
    public GameObject readyPrefb;
    public GameObject gameManager;
    public UIPopup  gameOverPopup;
    public UIPopup  gameWinPopup;

    private GameObject currentCanvas=null;
    
    private GameManager _gameManager;
    private ImageManager _imageManager;
    public void Start()
    {
        _gameManager=gameManager.GetComponent<GameManager>();
        _imageManager = _gameManager.GetImageManager();
    }

    public void createGameCanvas(int level,int[] gameSeed)
    {
        DestroyCanvas();

        if (level == 0)
        {
            currentCanvas=GameObject.Instantiate(readyPrefb,_gameManager.masterCanvas.transform);
            currentCanvas.transform.GetChild(2).GetComponent<Image>().sprite = _imageManager.GetImage(gameSeed[level]);
            currentCanvas.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(_gameManager.nextLevel);
            return;
        }
        int[] imageIndexArray=new int[4];
        imageIndexArray[0] =gameSeed[level - 1];
        int imageIndexArrayIndex = 1;
        foreach (int num in getRandomWithout(gameSeed.Length,gameSeed[level - 1],3))
        {
            imageIndexArray[imageIndexArrayIndex++] = num;
        }
        GameManager.shuffle(imageIndexArray);
            
        currentCanvas=GameObject.Instantiate(canvasPrefb,_gameManager.masterCanvas.transform);
        for (int i = 0; i < 4; i++)
        {
            setImage(_imageManager.GetImage(imageIndexArray[i]), i);
            if (imageIndexArray[i] == gameSeed[level - 1])
            {
                setImageAsExit(i);
            }
            else
            {
                setImageAsOver(i);   
            }
        }

        setCover(_imageManager.GetImage(gameSeed[level]));
    }

    public List<int> getRandomWithout(int range, int withoutNum,int length)
    {
        System.Random rng = new System.Random();
        List<int> nums = new List<int>();
        for (int i = 0; i < length; i++)
        {
            int num = rng.Next(range);
            while (num == withoutNum||nums.Contains(num))
            {
                num = rng.Next(range);
            }
            nums.Add(num);
        }


        return nums;
    }


    public void setImageAsExit(int index)
    {
        Button button=currentCanvas.transform.GetChild(1).GetChild(index).GetComponent<Button>();
        button.onClick.AddListener(_gameManager.nextLevel);
    }

    private UIPopup _popup;

    public void hidePopup()
    {
        _popup.Hide();
    }
    public void ShowFailedPopup()
    {
        _popup=GameObject.Instantiate(gameOverPopup,_gameManager.masterCanvas.transform);
        Button button=_popup.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        button.onClick.AddListener(this.hidePopup);
        _popup.Show();
    }
    public void ShowWinPopup()
    {
        _popup=GameObject.Instantiate(gameWinPopup,_gameManager.masterCanvas.transform);
        Text text=_popup.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        text.text = _gameManager.getCostTime().ToString("耗费时间：#0.0s");
        Button button=_popup.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        button.onClick.AddListener(this.hidePopup);
        _popup.Show();
    }
    public void setImageAsOver(int index)
    {
        Button button=currentCanvas.transform.GetChild(1).GetChild(index).GetComponent<Button>();
        button.onClick.AddListener(ShowFailedPopup);
    }
    private void setImage(Sprite sprite,int index)
    {
        Transform gridObject = currentCanvas.transform.GetChild(1);
        Image imageComponent= gridObject.GetChild(index).GetComponent<Image>();
        imageComponent.sprite= sprite;
    }
    private void setCover(Sprite sprite)
    {
        Image imageComponent=currentCanvas.transform.GetChild(3).GetComponent<Image>();
        imageComponent.sprite= sprite;
    }

    public void DestroyCanvas()
    {
        if (currentCanvas != null)
        {
            Destroy(currentCanvas);
            currentCanvas = null;
        }

        if (_popup != null)
        {
            Destroy(_popup);
        }
    }
}
