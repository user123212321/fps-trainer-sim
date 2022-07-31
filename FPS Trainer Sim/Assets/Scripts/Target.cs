using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private float xSpawnRange = 8.0f;
    private float yBottom = 1.0f;
    private float ySpawnHeight = 4.5f;
    private float zSpawnLocation = 5.95f;
    public float targetTimeoutDuration = 3.0f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = RandomSpawnPos();
        StartCoroutine(RemoveObjectTimeout());
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.isGameActive)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            gameManager.UpdateHitCount();
        }
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xSpawnRange, xSpawnRange), Random.Range(yBottom, ySpawnHeight), zSpawnLocation);
    }

    IEnumerator RemoveObjectTimeout()
    {
        yield return new WaitForSeconds(targetTimeoutDuration);
        transform.Translate(Vector3.forward * 3, Space.World);
        if (gameManager.isGameActive)
        {
            gameManager.UpdateMissedTargetCount();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if ((gameManager.gameMode == 1 || gameManager.gameMode == 2) && gameManager.isGameActive)
        {
            gameManager.GameOver();
        }

    }

}
