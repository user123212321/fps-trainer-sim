using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private GameManager gameManager;
    public int gameMode;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        button.onClick.AddListener(SetGameMode);
    }

    void SetGameMode()
    {
        gameManager.StartGame(gameMode);
    }

    // On hover, display description of different game modes.
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.DisplayDescriptions(gameMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.UnDisplayDescription(gameMode);
    }
}
