using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;
    GameObject tutorial;
    GameObject gameOver;
    Text distanceFinal;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("Distance Text").GetComponent<Text>();
        distanceFinal = GameObject.Find("Distance").GetComponent<Text>();
        gameOver = GameObject.Find("Game Over");
        gameOver.SetActive(false);
        tutorial = GameObject.Find("Tutorial");

    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if (player.isDead == true)
        {
            Debug.Log(player.isDead);
            gameOver.SetActive(true);
            distanceFinal.text = distance + " m";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tutorial.SetActive(false);
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Retry()
    {
        SceneManager.LoadScene("Scene");
    }
}
