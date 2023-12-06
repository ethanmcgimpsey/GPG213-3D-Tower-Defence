using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Xml.Serialization;
using System.IO;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting,
        GameOver,
        Win
    };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int index;
        [XmlIgnore] public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public int currentWave;
    public float addiotnalHealth;
    public Transform waypointParent;
    public GameObject completeLevelUI, finalWaveUI;
    public Text waveTextRound;
    // public Text score;
    public AudioSource nextWaveAudio;
    public AudioSource gameOverAudio;
    public float timeBetweenWaves;
    public float waveCountdown;
    public SpawnState state = SpawnState.Counting;
    public bool stopWaveUI;

    [Header("Saving")]
    public string fileName = "GameSave";

    private string fullPath;

    [SerializeField] private float searchCountdown = 1f;


    void Start()
    {
        /* fullPath = Application.persistentDataPath + "/" + fileName + ".xml";

        if(File.Exists(fullPath))
        {
            Wave currentWave = Load(fullPath);
            SetWave(currentWave.index);
        }*/

        Enemy enemy = GetComponent<Enemy>();
        waveCountdown = 10f;
        finalWaveUI.SetActive(false);
        Time.timeScale = 1;
    }

    void SetWave(int waveIndex)
    {
        if (!stopWaveUI)
        {
            nextWaveAudio.Play();
            waveTextRound.text = "Wave: " + (currentWave + 1).ToString();
            currentWave = waveIndex;
        }
        waveCountdown = timeBetweenWaves;
        state = SpawnState.Counting;
    }

    void Update()
    {
        // score.text = PlayerStats.Score.ToString() + " :Points";
        if (state == SpawnState.Waiting)
        {
            if (EnemyIsAlive())
            {
                return;
            }
            else
            {
                Debug.Log("Wave Completed");
                currentWave++;
                SetWave(currentWave);
                waves[currentWave].index = currentWave;
                // Save(fullPath, waves[nextWave]);
            }
        }

        if (waveCountdown <= 0)
        {
            if(state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

        
        switch (currentWave)
        {
            case 9:
                finalWaveUI.gameObject.SetActive(true);
                stopWaveUI = true;
                if (state == SpawnState.Spawning)
                {
                    finalWaveUI.gameObject.SetActive(false);
                }
                break;
            case 10:
                state = SpawnState.Win;
                completeLevelUI.SetActive(true);
                Time.timeScale = 0;
                break;
        }
        

        /*if (currentWave == 4)
        {
            finalWaveUI.gameObject.SetActive(true);
            stopWaveUI = true;
            if (state == SpawnState.Spawning)
            {
                finalWaveUI.gameObject.SetActive(false);
            }
        }

        if (currentWave == 5)
        {
            state = SpawnState.GameOver;
            completeLevelUI.SetActive(true);
            Time.timeScale = 0;
        }*/
        
        if(PlayerStats.Lives <= 0)
        {
            if (!gameOverAudio.isPlaying)
            {
                Debug.Log("Play this audio source");
                gameOverAudio.Play();
            }
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Enemy" + _wave.name);
        state = SpawnState.Spawning;
        // Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemy(Transform oldEnemyTransform)
    {
        Debug.Log("Spawning Enemy: " + oldEnemyTransform.name);
        Enemy newEnemy = Instantiate(oldEnemyTransform, transform.position, transform.rotation).GetComponent<Enemy>();
        //Enemy enemy = clone.GetComponent<Enemy>();
        newEnemy.waypointParent = waypointParent;
        // Add health on each wave
        // Wave 1 zombies have 1 health
        // Wave 2 zombies have 1.2 health
        // Wave 2 zombies have 1.2 health
        //
        // Wave 1 zombie health = maxHealth + currentWave
        //                      = 1         + 1           = 2
        // Wave 2 zombie health = maxHealth + currentWave
        //                      = 1         + 2           = 3
        //
        // Wave 2 zombie health should be 1.2 health
        // Wave 3 zombie health should be 1.4 health

        newEnemy.maxHealth += ((float)currentWave / 5);
        newEnemy.curHealth = newEnemy.maxHealth;
    }


    /* public void Save(string path, Wave wave)
    {
        var serializer = new XmlSerializer(typeof(Wave));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, wave);
        }
    }*/

    public static Wave Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Wave));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Wave;
        }
    }
}