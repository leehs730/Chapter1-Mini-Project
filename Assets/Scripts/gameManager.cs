using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public Text timeTxt;
    public Text minTimeText;
    int matchScore = 0;
    public Text totalMatchScore;
    //public GameObject endTxt;
    public GameObject endPanel;
    public GameObject card;
    float time;
    public static gameManager I;

    public GameObject firstCard;
    public GameObject secondCard;

    public AudioClip match;
    public AudioClip dismatch;
    public AudioSource audioSource;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        Time.timeScale = 1f;

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            // i =(0,1,2,3),(0+4,1+4,2+4,3+4),(0+8,1,2,3),(0+12,1,2,3)
            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time > 30.0f)
        {
            gameManager.I.gameOver();
        }
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if(firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);
            matchScore += 1;
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            Debug.Log(cardsLeft);
            if (cardsLeft == 2)
            {
                // 종료시키자!!
                gameManager.I.gameOver();
                Debug.Log("끝?");
                //Invoke("GameEnd", 1f);
            }
        }
        else
        {
            audioSource.PlayOneShot(dismatch);
            matchScore += 1;
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }

        firstCard = null;
        secondCard = null;
    }

    public void gameOver()
    {
        Debug.Log("게임오버발동");
        endPanel.SetActive(true);
        totalMatchScore.text = matchScore.ToString();
        Time.timeScale = 0.0f;

        if (PlayerPrefs.HasKey("bestScore") == false)
        {
            PlayerPrefs.SetFloat("bestScore", time);
        }
        else
        {
            if (PlayerPrefs.GetFloat("bestScore") < time)
            {
                PlayerPrefs.SetFloat("bestScore", time);
            }
        }
        float minTime = PlayerPrefs.GetFloat("bestScore");
        minTimeText.text = minTime.ToString("N2");
    }

}
