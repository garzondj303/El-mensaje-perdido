using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnClickTab(int tabNumber)
    {
        for(int i=0; i<this.tabImages.Length; i++)
        {
            this.tabImages[i].color = Color.gray;
            this.pages[i].SetActive(false);
        }
        this.tabImages[tabNumber].color= Color.white;
        this.pages[tabNumber].SetActive(true);
    }

}
