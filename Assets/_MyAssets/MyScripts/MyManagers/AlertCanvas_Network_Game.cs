using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertCanvas_Network_Game : MonoBehaviour
{
    [SerializeField]
    private Animator myAnim;

    [SerializeField]
    private Transform mySpinner,targetSpinner;

    [SerializeField]
    private TextMeshProUGUI scoreTxt, bestScoreTxt;

    public float rotSpeed = 200;
    public bool isOnTarget = false;

    int score = 0;
    bool isGameStarted = false;
    bool isGameOver = false;
    Vector2 initTargetScale;
    float initSpeed;

    private void Start()
    {
        initTargetScale = targetSpinner.localScale;
        initSpeed = rotSpeed;
        isGameOver = true;
    }

    private void Reset()
    {
        myAnim.Play("resetAnim");
        score = 0;
        scoreTxt.text = score.ToString();
        targetSpinner.localScale = initTargetScale;
        rotSpeed = initSpeed;
        targetSpinner.parent.transform.rotation = Quaternion.EulerRotation(Vector3.forward * Random.Range(0f, 360f));
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1); 
        isGameStarted = true;
    }

    private void OnEnable()
    {
        GameOverBehavior();
    }

    void Update()
    {
        if (isGameStarted)
        {
            mySpinner.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
        }
    }

    void IncrScore()
    {
        myAnim.Play("scoreIncAnim");
        score += 10;
        rotSpeed += 20;
        scoreTxt.text = score.ToString();
        targetSpinner.localScale = new Vector2(targetSpinner.localScale.x * 0.9f, targetSpinner.localScale.y);

        if (targetSpinner.localScale.x < 0.8f)
        {
            targetSpinner.localScale = new Vector2(targetSpinner.localScale.x, targetSpinner.localScale.y);
        }

        if (rotSpeed > 1000)
        {
            rotSpeed = 1000;
        }
        targetSpinner.parent.transform.rotation = Quaternion.EulerRotation(Vector3.forward * Random.Range(0f, 360f));
    }

    void GameOverBehavior()
    {
        myAnim.Play("loseAnim");
        int bestScore = PlayerPrefs.GetInt("bestScore", 0);
        if (score > bestScore)
            bestScore = score;

        PlayerPrefs.SetInt("bestScore", bestScore);
        bestScoreTxt.text = bestScore.ToString();
        isGameStarted = false;
    }

    public void SwitchDirection()
    {

        if (isGameStarted)
        {
            if (isOnTarget)
            {
                //Success
                IncrScore();
                rotSpeed *= -1;
            }
            else
            {
                //Failed
                GameOverBehavior();
            }

        }
        else
        {
            if (isGameOver)
            {
                Reset();
            }
            else
            {
                isGameStarted = true;
                isGameOver = false;
            }
        }
    }

}
