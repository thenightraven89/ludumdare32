using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject towerLevel;
    public GameObject ladder;
    public GameObject altar;

    private int towerHeight;

    private const int levelHeight = 14;

    public Material[] pieceMaterials;

    public enum LevelState
    {
        None,
        TowerRaising,
        LadderRaising,
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
        Cursor.lockState = CursorLockMode.Locked;

        currentState = LevelState.None;
        towerHeight = -1;
        RaiseTowerLevel();
    }

    private const float altarRaiseTime = 5f;

    public void RaiseAltar()
    {
        LeanTween.move(altar, new Vector3(0, towerHeight * levelHeight, 0), altarRaiseTime).setEase(LeanTweenType.easeInOutQuad);
    }

    // launched when combat with current wave has ended, or at the beginning of the game, will launch animation for raising one level
    public void RaiseTowerLevel()
    {
        currentState = LevelState.TowerRaising;

        towerHeight++;

        GameObject newTowerLevel = Instantiate(towerLevel) as GameObject;
        newTowerLevel.transform.position = new Vector3(0, towerHeight * levelHeight, 0);
        //newTowerLevel.transform.eulerAngles = new Vector3(0, -towerHeight * 180f, 0);

        Transform[] parts = newTowerLevel.GetComponentsInChildren<Transform>();
        
        StartCoroutine(Construct(new object[] { parts, newTowerLevel.transform.position }));
    }

    public void RaiseLadder()
    {
        currentState = LevelState.LadderRaising;

        GameObject newLadder = Instantiate(ladder) as GameObject;
        newLadder.transform.position = new Vector3(0, towerHeight * levelHeight, 0);
        //newLadder.transform.eulerAngles = new Vector3(0, -towerHeight * 180f, 0);

        Transform[] parts = newLadder.GetComponentsInChildren<Transform>();

        StartCoroutine(Raise(parts));
    }

    private IEnumerator Raise(Transform[] parts)
    {
        Vector3[] initialPositions = new Vector3[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].CompareTag("Constructed"))
            {
                initialPositions[i] = parts[i].localPosition;
                parts[i].localPosition = parts[i].localPosition + new Vector3(0f, -100f, 0f);
                parts[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].CompareTag("Constructed"))
            {
                yield return new WaitForSeconds(Random.Range(minWait, maxWait));
                AssignRandomMaterials(parts[i]);
                parts[i].gameObject.SetActive(true);
                LeanTween.moveLocal(parts[i].gameObject, initialPositions[i], 2f).setEase(LeanTweenType.easeOutQuint);
            }
        }

        RaiseTowerLevel();
    }


    private const float minWait = 0.01f;
    private const float maxWait = 0.05f;
    
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

            if (parts[i].CompareTag("Constructed"))
            {
                parts[i].position = initialPosition + direction * 100f;
                parts[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < parts.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            if (parts[i].CompareTag("Constructed"))
            {
                AssignRandomMaterials(parts[i]);
                parts[i].gameObject.SetActive(true);
                LeanTween.move(parts[i].gameObject, initialPositions[i], 0.5f).setEase(LeanTweenType.easeOutQuint);
            }
        }

        RaiseAltar();

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

    private void AssignRandomMaterials(Transform target)
    {
        MeshRenderer mrenderer = target.GetComponent<MeshRenderer>();
        if (mrenderer != null)
        {
            mrenderer.material = pieceMaterials[Random.Range(0, pieceMaterials.Length)];
        }
    }
}