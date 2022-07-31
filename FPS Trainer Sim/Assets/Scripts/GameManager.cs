using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Camera gameCamera;
    public Camera menuCamera;

    public Target target;
    private float targetTimeoutDuration = 3.0f;
    private float spawnInterval = 1.0f;
    private int spawnCount = 10;
    private float frenzyInterval = 0.01f;

    public GameObject titleMenu;
    public GameObject playMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;

    public GameObject gameplayUI;

    public TextMeshProUGUI hitText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI missedTargetText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI descStandard;
    public TextMeshProUGUI descSurvival;
    public TextMeshProUGUI descFrenzy;

    private int hitCount;
    private int missCount;
    private int missedTargetCount;
    private float timer = 0;

    public bool isGameActive = false;
    public int gameMode = 0; // 0: Standard, 1: Survival, 2: Frenzy

    public TextMeshProUGUI spawnIntervalSettingLabel;
    public Slider spawnIntervalSetting;
    public TextMeshProUGUI despawnIntervalSettingLabel;
    public Slider despawnIntervalSetting;
    public TextMeshProUGUI spawnCountSettingLabel;
    public Slider spawnCountSetting;

    public MouseLook mouseLook;
    public float mouseSensitivity = 200f;
    public TextMeshProUGUI mouseSensitivitySettingLabel;
    public Slider mouseSensitivitySetting;

    private void Start()
    {
        spawnIntervalSettingLabel.text = "Target Spawn Interval: " + spawnInterval + " seconds";
        spawnIntervalSetting.value = spawnInterval;
        target.targetTimeoutDuration = targetTimeoutDuration;
        despawnIntervalSettingLabel.text = "Target Despawn Interval: " + targetTimeoutDuration + " seconds";
        despawnIntervalSetting.value = targetTimeoutDuration;
        spawnCountSettingLabel.text = "Target Spawn Count (Standard Mode): " + spawnCount;
        spawnCountSetting.value = spawnCount;
        mouseLook.mouseSensitivity = mouseSensitivity;
        mouseSensitivitySetting.value = mouseSensitivity;
        mouseSensitivitySettingLabel.text = "Mouse Sensitivity: " + mouseSensitivity;
    }

    private void Update()
    {
        if(isGameActive)
        {
            timer += Time.deltaTime;
            timerText.text = "Duration: " + (Mathf.Round(timer * 100.0f) * 0.01f); // Rounds timer to 2 decimal places.
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameplayUI.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);
        SetCameraToMenu();
    }

    IEnumerator SpawnTargetsFrenzy()
    {
        float temp = frenzyInterval;
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnInterval-temp);
            if (temp < spawnInterval-0.1f)
            {
                temp += frenzyInterval;
            }
            Debug.Log(temp);
            Instantiate(target);
        }
    }

    IEnumerator SpawnTargets()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(target);
        }
    }

    IEnumerator SpawnTargetsLimited()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(target);
        }
        yield return new WaitForSeconds(target.targetTimeoutDuration * 1.5f);
        GameOver();
    }

    public void UpdateHitCount()
    {
        hitCount++;
        hitText.text = "Hit: " + hitCount;
    }

    public void UpdateMissCount()
    {
        missCount++;
        missText.text = "Miss: " + missCount;
    }

    public void UpdateMissedTargetCount()
    {
        missedTargetCount++;
        missedTargetText.text = "Missed Targets: " + missedTargetCount;
    }

    public void TryAgainButton()
    {
        StartGame(gameMode);
    }

    public void SetCameraToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        gameCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);
    }

    public void SetCameraToGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameCamera.gameObject.SetActive(true);
        menuCamera.gameObject.SetActive(false);
    }

    void ResetAllScores()
    {
        hitCount = 0;
        missCount = 0;
        missedTargetCount = 0;
        timer = 0;

        hitText.text = "Hit: 0";
        missText.text = "Miss: 0";
        missedTargetText.text = "Missed Targets: 0";
    }

    public void StartGame(int gm)
    {
        // Game mode is set by buttons with GameModeButton.cs
        isGameActive = true;
        gameMode = gm;

        ResetAllScores();

        switch (gm)
        {
            case 0:
                StartCoroutine(SpawnTargetsLimited());
                break;
            case 1:
                StartCoroutine(SpawnTargets());
                break;
            case 2:
                StartCoroutine(SpawnTargetsFrenzy());
                break;
        }
        gameplayUI.gameObject.SetActive(true);
        playMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        SetCameraToGame();
    }

    // Click on the "play" button in the title screen.
    public void ClickPlayMenu()
    {
        titleMenu.gameObject.SetActive(false);
        playMenu.gameObject.SetActive(true);
    }

    public void ClickSettingsMenu()
    {
        titleMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    public void ClickBackButton()
    {
        settingsMenu.gameObject.SetActive(false);
        playMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        titleMenu.gameObject.SetActive(true);
    }

    public void ClickQuitButton()
    {
        Application.Quit();
    }

    public void DisplayDescriptions(int gm)
    {
        switch(gm)
        {
            case 0:
                descStandard.gameObject.SetActive(true);
                break;
            case 1: 
                descSurvival.gameObject.SetActive(true);
                break;
            case 2:
                descFrenzy.gameObject.SetActive(true);
                break;

        }
    }

    public void UnDisplayDescription(int gm)
    {
        switch (gm)
        {
            case 0:
                descStandard.gameObject.SetActive(false);
                break;
            case 1:
                descSurvival.gameObject.SetActive(false);
                break;
            case 2:
                descFrenzy.gameObject.SetActive(false);
                break;
        }
    }

    // Used by "Target Spawn Interval" setting.
    public void UpdateSpawnInterval()
    {
        // Round to 1 decimal place.
        spawnInterval = (Mathf.Round(spawnIntervalSetting.value * 10.0f) * 0.1f);
        spawnIntervalSettingLabel.text = "Target Spawn Interval: " + spawnInterval + " seconds";
    }

    public void UpdateDespawnInterval()
    {
        // Round to 1 decimal place.
        target.targetTimeoutDuration = (Mathf.Round(despawnIntervalSetting.value * 10.0f) * 0.1f);
        targetTimeoutDuration = (Mathf.Round(despawnIntervalSetting.value * 10.0f) * 0.1f);
        despawnIntervalSettingLabel.text = "Target Despawn Interval: " + targetTimeoutDuration + " seconds";
    }

    public void UpdateSpawnCount()
    {
        // Round to int.
        spawnCount = Mathf.RoundToInt(spawnCountSetting.value);
        spawnCountSettingLabel.text = "Target Spawn Count (Standard Mode): " + spawnCount;
    }

    public void UpdateMouseSensitivity()
    {
        mouseSensitivity = mouseSensitivitySetting.value;
        mouseLook.mouseSensitivity = mouseSensitivity;
        mouseSensitivitySettingLabel.text = "Mouse Sensitivity: " + mouseSensitivity;
    }
}
