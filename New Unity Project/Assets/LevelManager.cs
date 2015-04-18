using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject towerLevel;

    private int towerHeight;

    private const int levelHeight = 15;

    public enum LevelState
    {
        None,
        Ascension,
        Preparation,
        Combat,
        End
    }

    private LevelState currentState;

    public void Awake()
    {
        instance = this;
    }

    // start with this level state
    public void Start()
    {
        currentState = LevelState.None;
        towerHeight = -1;
        RaiseLevel();
    }

    // launched when combat with current wave has ended, or at the beginning of the game, will launch animation for raising one level
    public void RaiseLevel()
    {
        currentState = LevelState.Ascension;

        towerHeight++;

        GameObject newTowerLevel = Instantiate(towerLevel) as GameObject;
        newTowerLevel.transform.position = new Vector3(0, towerHeight * levelHeight, 0);
        newTowerLevel.transform.eulerAngles = new Vector3(0, - towerHeight * 90f, 0);

        Transform[] parts = newTowerLevel.GetComponentsInChildren<Transform>();
        
        StartCoroutine(Construct(new object[] { parts, newTowerLevel.transform.position }));
    }

    private const float minWait = 0.01f;
    private const float maxWait = 0.1f;
    
    private IEnumerator Construct(object[] parameters)
    {
        Transform[] parts = (Transform[])parameters[0];
        Vector3 center = (Vector3)parameters[1];

        Vector3[] initialPositions = new Vector3[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            initialPositions[i] = parts[i].position;
            Vector3 direction = parts[i].position - center;
            direction.Normalize();

            Vector3 initialPosition = parts[i].position;

            if (parts[i].tag == "Constructed")
            {
                parts[i].position = initialPosition + direction * 100f;
                parts[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < parts.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            if (parts[i].tag == "Constructed")
            {
                parts[i].gameObject.SetActive(true);
                LeanTween.move(parts[i].gameObject, initialPositions[i], 0.5f).setEase(LeanTweenType.easeOutQuint);
            }
        }

        yield return null;
    }

    // launched when collision with next level
    public void Prepare()
    {
        currentState = LevelState.Preparation;
    }

    // launched when idle time expires, waves attack
    public void Fight()
    {
        currentState = LevelState.Combat;
    }
}