//此Script是用來新增Canvas中的Image，每個Image都是一個Page

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public List<GameObject> pages = new List<GameObject>();//代表頁面的圖片
    public List<GameObject> Covers = new List<GameObject>();//覆蓋頁面的圖片
    [HideInInspector]
    public float ScreenWidth;
    [HideInInspector]
    public int currentPage;
    /// <summary>
    /// 頁面間的間距
    /// </summary>
    public float PageDistance;
    /// <summary>
    /// 移動速度
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// 是否直接變換頁面
    /// </summary>
    public bool DirectChange = true;

    public void ArrangeElement(int PageNumber, float distance, GameObject CanvasObj)
    {
        PageDistance = distance;
        //增加element數量
        for (; pages.Count < PageNumber;)
        {
            pages.Add(null);
            Covers.Add(null);
        }
        //減少element數量
        for (int i = pages.Count - 1; i >= PageNumber; i--)
        {
            DestroyImmediate(pages[i].gameObject);
            pages.RemoveAt(i);
            Covers.RemoveAt(i);
        }

        //為每個element添加Object
        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i] == null)
            {
                GameObject img = Instantiate((GameObject)Resources.Load("BlankPage", typeof(GameObject)), Vector3.zero, Quaternion.identity);
                img.transform.parent = CanvasObj.transform;
                pages[i] = img;
                img.name = "Page " + (i + 1).ToString();
                Covers[i] = img.transform.Find("CoverImage").gameObject;
                Covers[i].SetActive(false);
            }
            //設定位置
            pages[i].GetComponent<Image>().rectTransform.offsetMin = new Vector2((ScreenWidth + PageDistance) * i, 0);
            pages[i].GetComponent<Image>().rectTransform.offsetMax = new Vector2((ScreenWidth + PageDistance) * i, 0);
        }
    }

    public void MovePage(int CurrentPage ) 
    {
        if (CurrentPage < 0 || CurrentPage >= pages.Count) 
        {
            return;
        }
        float position = pages[CurrentPage].GetComponent<Image>().rectTransform.offsetMin.x;
        currentPage = CurrentPage;
        StartCoroutine( MovingPage(position) );
    }

    #region Jackie 增加
    public void PrevPage()
    {
        if (currentPage < 0 || currentPage >= pages.Count)
        {
            return;
        }
        currentPage--;
        float position = pages[currentPage].GetComponent<Image>().rectTransform.offsetMin.x;
        StartCoroutine(MovingPage(position));
    }
    #endregion

    IEnumerator MovingPage(float position) 
    {
        List<Image> Page_Image = new List<Image>();
        //get every image component and turn on the cover
        for (int i = 0; i < pages.Count; i++) 
        {
            Page_Image.Add(pages[i].GetComponent<Image>());
            Covers[i].SetActive(true);
        }

        float MovePosition = Mathf.Abs(position);
        
        // Move Page
        while (!DirectChange)
        {
            if (MovePosition - Mathf.Abs(position) / MoveSpeed * Time.deltaTime > 0)
            {
                MovePosition -= Mathf.Abs(position) / MoveSpeed * Time.deltaTime;
                for (int j = 0; j < Page_Image.Count; j++)
                {
                    Page_Image[j].rectTransform.offsetMin -= new Vector2(position / MoveSpeed, 0) * Time.deltaTime;
                    Page_Image[j].rectTransform.offsetMax -= new Vector2(position / MoveSpeed, 0) * Time.deltaTime;
                }
            }
            else
            {
                if (position < 0) { MovePosition = -MovePosition; }
                for (int j = 0; j < Page_Image.Count; j++)
                {
                    Page_Image[j].rectTransform.offsetMin -= new Vector2(MovePosition, 0);
                    Page_Image[j].rectTransform.offsetMax -= new Vector2(MovePosition, 0);
                }
                break;
            }
            yield return null;
        }
        //Direct Change Page
        if (DirectChange) 
        {
            for (int j = 0; j < Page_Image.Count; j++)
            {
                Page_Image[j].rectTransform.offsetMin -= new Vector2(position, 0);
                Page_Image[j].rectTransform.offsetMax -= new Vector2(position, 0);
            }
        }
        Debug.Log(DirectChange);
        //turn off the cover
        for (int i = 0; i < pages.Count; i++)
        {
            Covers[i].SetActive(false);
        }
    }
}
