using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RigidbodyController player = default;
    [SerializeField] private TextMeshProUGUI livesText = default;
    [SerializeField] private Animator anim = default;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private int nbLives = 5;
    [SerializeField] private int nbAddLives = 5;
    [SerializeField] private bool lockedCursorOnLaunch = false;
    
    private int currentNbLives;
    private Vector3 spawnPoint = new Vector3(0f, 1.5f, 0f);
    private List<Vector3> passedCheckpoints = new List<Vector3>();

    private void Awake()
    {
        Cursor.lockState = (lockedCursorOnLaunch) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockedCursorOnLaunch;
        ResetLives();
    }
    private void Start()
    {
        passedCheckpoints.Add(spawnPoint);
    }

    public void Win()
    {
        StartCoroutine(CoroutineWin());
    }
    public void WinCut()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator CoroutineWin()
    {
        anim.SetBool("IsWin", true);
        anim.SetTrigger("Fade");

        yield return new WaitForSeconds(fadeTime);

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Lose()
    {
        StartCoroutine(CoroutineLose());
    }

    private IEnumerator CoroutineLose()
    {
        anim.SetTrigger("Fade");

        yield return new WaitForSeconds(fadeTime);

        if (passedCheckpoints.Count > 0)
        {
            if (ChangeNbLives(currentNbLives - 1) <= 0)
            {
                ResetCheckpoints();
                ResetLives();
            }
        }
        player.Teleport(spawnPoint);
    }
    public void ReachCheckpoint(Vector3 newPoint)
    {
        if (!passedCheckpoints.Contains(newPoint))
        {
            passedCheckpoints.Add(newPoint);
            spawnPoint = newPoint;
            AddLives();
        }
    }

    private void ResetLives()
    {
        ChangeNbLives(nbLives);
    }
    private void AddLives()
    {
        ChangeNbLives(currentNbLives + nbAddLives);
    }
    private void ResetCheckpoints()
    {
        passedCheckpoints = new List<Vector3>();
        spawnPoint = Const.startPoint;
        passedCheckpoints.Add(spawnPoint);
    }
    private int ChangeNbLives(int newNbLives)
    {
        currentNbLives = newNbLives;
        if (livesText != default)
            livesText.text = currentNbLives.ToString();
        return currentNbLives;
    }
}
