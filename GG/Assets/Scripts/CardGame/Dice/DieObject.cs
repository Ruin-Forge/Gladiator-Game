using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DieObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    #region Fields

    private static GameObject diePrefab;

    private DieInfo dieInfo;
    private int rollAmount = 0;

    private Image backgroundUI;
    private Text valueUI;

    #endregion Fields

    public static DieObject Instantiate(DieInfo dieInfo, Vector2 position)
    {
        if (dieInfo == null)
            return null;

        if (diePrefab == null)
            diePrefab = (GameObject)Resources.Load("Prefabs/CardGame/Die");

        GameObject obj = GameObject.Instantiate(diePrefab, GameObject.FindObjectOfType<Canvas>().transform);
        obj.transform.localPosition = position;

        DieObject die = obj.GetComponent<DieObject>();
        die.dieInfo = dieInfo;

        return die;
    }

    #region Main-Loop

    private void Awake()
    {
        backgroundUI = GetComponentInChildren<Image>();
        valueUI = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (rollAmount > 0)
        {
            dieInfo.Roll();
            rollAmount--;
        }

        valueUI.text = dieInfo.Value.ToString();
    }

    #endregion Main-Loop

    public void Roll()
    {
        rollAmount = Random.Range(300, 501);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Roll();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }
}