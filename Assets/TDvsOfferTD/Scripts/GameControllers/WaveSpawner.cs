using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public float timeBetweenWaves = 5f;
    public float timeAfterWaves = 2f;
    public List<Text> waveCountDownTexts;
    public Text newWaveText;
    [SerializeField]
    public Waypoints[] paths;
    private float countdown;
    private int waveIndex = 0;

    void Start()
    {
        countdown = -timeAfterWaves;
    }

    void Update()
    {
        if (countdown <= -timeAfterWaves)
        {
            StartCoroutine(SpawnWave());
            newWaveText.text = string.Format("WAVE {0}", waveIndex);
            countdown = timeBetweenWaves + waveIndex * 24;
        }
        countdown -= Time.deltaTime;
        foreach (Text waveCountDownText in waveCountDownTexts)
        {
            waveCountDownText.text = string.Format("{0}", Mathf.RoundToInt(Mathf.Clamp(countdown, 0f, Mathf.Infinity)));
        }
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        PlayerStats.rounds++;
        int rating = waveIndex * 24;
        while (rating > 0)
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                Enemy enemy = enemyPrefab.GetComponent<Enemy>();
                if (rating < enemy.rating || Random.value < 0.5) continue;
                int units = enemy.units;
                rating -= enemy.rating;
                for (int i = 0; i < units; i++)
                {
                    foreach (Waypoints path in paths)
                    {
                        SpawnEnemy(path, enemyPrefab);
                    }

                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }

    void SpawnEnemy(Waypoints path, GameObject enemyPrefab)
    {
        EnemyMovement enemyMovement = Instantiate(enemyPrefab, path.GetStartPoint().position, path.GetStartPoint().rotation).GetComponent<EnemyMovement>();
        enemyMovement.path = path;
    }

    public void NextWave()
    {
        countdown = -Mathf.Infinity;
    }
}
