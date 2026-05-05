using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Pergerakan Player Utama")]
    public float laneSpeed = 10f;
    public float xLimit = 8f;

    [Header("Initial Soldier")]
    [SerializeField] private GameObject initialSoldier;

    [Header("Pengaturan Kerumunan Masif")]
    public GameObject soldierPrefab;
    public int currentArmyCount = 1;
    public float smoothSpeed = 5f;

    [Range(1f, 10f)]
    public float maxCrowdSpread = 6f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("HUD")]
    [SerializeField] private TMP_Text squadText;

    private bool isGameOver;

    private readonly List<GameObject> armyList = new List<GameObject>();
    private readonly List<float> armyRelativeX = new List<float>();
    private readonly List<float> armyRelativeZ = new List<float>();

    private void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (initialSoldier != null)
        {
            RegisterSoldier(initialSoldier, isInitial: true);
        }
        else
        {
            Debug.LogWarning("Initial Soldier belum di-assign di PlayerController!", this);
        }

        currentArmyCount = armyList.Count;
        UpdateSquadText();
        CheckGameOver();
    }

    private void Update()
    {
        if (isGameOver) return;

        MovePlayer();
        MoveMassiveCrowd();
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 newPos = transform.position + new Vector3(horizontalInput, 0f, 0f) * laneSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, -xLimit, xLimit);
        transform.position = newPos;
    }

    private void MoveMassiveCrowd()
    {
        int count = Mathf.Min(armyList.Count, armyRelativeX.Count, armyRelativeZ.Count);

        for (int i = 0; i < count; i++)
        {
            GameObject soldier = armyList[i];

            if (soldier == null) continue;

            // Soldier awal dibiarkan ikut parent Player.
            if (soldier == initialSoldier) continue;

            float desiredX = transform.position.x + armyRelativeX[i];
            float desiredZ = transform.position.z + armyRelativeZ[i];

            float crowdPadding = 0.5f;
            desiredX = Mathf.Clamp(desiredX, -(xLimit + crowdPadding), xLimit + crowdPadding);

            Vector3 safeTargetPos = new Vector3(desiredX, transform.position.y, desiredZ);

            soldier.transform.position = Vector3.Lerp(
                soldier.transform.position,
                safeTargetPos,
                smoothSpeed * Time.deltaTime
            );
        }
    }

    public void ExecuteGateLogic(GateLogic.GateType tipe, int nilai)
    {
        if (isGameOver) return;

        int amountToAdd = 0;

        switch (tipe)
        {
            case GateLogic.GateType.Tambah:
                amountToAdd = nilai;
                break;

            case GateLogic.GateType.Kurang:
                RemoveSoldiers(nilai);
                break;

            case GateLogic.GateType.Kali:
                amountToAdd = (currentArmyCount * nilai) - currentArmyCount;
                break;

            case GateLogic.GateType.Bagi:
                if (nilai == 0) return;

                int targetCount = currentArmyCount / nilai;
                int amountToRemove = currentArmyCount - targetCount;
                RemoveSoldiers(amountToRemove);
                break;
        }

        for (int i = 0; i < amountToAdd; i++)
        {
            SpawnNewSoldier();
        }

        currentArmyCount = armyList.Count;
        UpdateSquadText();
        CheckGameOver();
    }

    public int GetArmyCount()
    {
        return currentArmyCount;
    }

    public List<Transform> GetActiveFirePoints()
    {
        List<Transform> firePoints = new List<Transform>();

        for (int i = 0; i < armyList.Count; i++)
        {
            GameObject soldier = armyList[i];

            if (soldier == null) continue;

            Transform firePoint = soldier.transform.Find("FirePoint");

            if (firePoint != null)
            {
                firePoints.Add(firePoint);
            }
        }

        return firePoints;
    }

    public void TakeArmyDamage(int damage)
    {
        if (isGameOver) return;

        RemoveSoldiers(damage);

        currentArmyCount = armyList.Count;
        UpdateSquadText();
        CheckGameOver();
    }

    private void RemoveSoldiers(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (armyList.Count <= 0) break;

            int lastIndex = armyList.Count - 1;
            GameObject soldierToDestroy = armyList[lastIndex];

            armyList.RemoveAt(lastIndex);

            if (lastIndex < armyRelativeX.Count)
            {
                armyRelativeX.RemoveAt(lastIndex);
            }

            if (lastIndex < armyRelativeZ.Count)
            {
                armyRelativeZ.RemoveAt(lastIndex);
            }

            if (soldierToDestroy != null)
            {
                Destroy(soldierToDestroy);
            }
        }

        currentArmyCount = armyList.Count;
        UpdateSquadText();
        CheckGameOver();
    }

    private void SpawnNewSoldier()
    {
        float randomX = Random.Range(-maxCrowdSpread, maxCrowdSpread);
        float randomZ = Random.Range(-maxCrowdSpread, maxCrowdSpread);

        Vector3 spawnPos = transform.position + new Vector3(randomX, 0f, randomZ);

        GameObject newSoldier = Instantiate(soldierPrefab, spawnPos, Quaternion.identity);

        RegisterSoldier(newSoldier, isInitial: false);
    }

    private void RegisterSoldier(GameObject soldier, bool isInitial)
    {
        if (soldier == null) return;
        if (armyList.Contains(soldier)) return;

        armyList.Add(soldier);

        if (isInitial)
        {
            armyRelativeX.Add(0f);
            armyRelativeZ.Add(0f);
        }
        else
        {
            armyRelativeX.Add(soldier.transform.position.x - transform.position.x);
            armyRelativeZ.Add(soldier.transform.position.z - transform.position.z);
        }

        currentArmyCount = armyList.Count;
        UpdateSquadText();
    }

    private void UpdateSquadText()
    {
        if (squadText != null)
        {
            squadText.text = currentArmyCount.ToString();
        }
    }

    private void CheckGameOver()
    {
        if (isGameOver) return;

        if (currentArmyCount <= 0 || armyList.Count <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        UpdateSquadText();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}