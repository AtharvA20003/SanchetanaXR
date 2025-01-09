using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;
    public int currentWave = 0;
    public float waveCoolDown = 5f;

    public bool inCooldown;
    public float coolDownCounter = 0;

    public List<Enemy> currentZombiesAlive;
    public GameObject zombiePrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI coolDownCounterUI;

    // Start is called before the first frame update
    void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i = 0; i<currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(1f, 1f), 0f, UnityEngine.Random.Range(1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();
            currentZombiesAlive.Add(enemyScript);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach(Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        foreach(Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();
        if(currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCoolDown());
        }
        if (inCooldown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCoolDown;
        }
        coolDownCounterUI.text = coolDownCounter.ToString("F0");
    }

    private IEnumerator WaveCoolDown()
    {
       // GetComponent<PlayerHealth>().RestoreHealth(100);
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCoolDown);
        inCooldown = false;
        
        currentZombiesPerWave += 3;
        StartNextWave();
        waveOverUI.gameObject.SetActive(false);
    }
}
