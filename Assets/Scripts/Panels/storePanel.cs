using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class storePanel : MonoBehaviour
{
    // public GameObject gameStatistics;
    public int selectedButton = -1;
    public GameObject[] units;
    public GameObject[] buttons;
    int currentCredits;
    Plane plane = new Plane(new Vector3(0,0,1), 0); // the xy plane
    int startMouseDown = 0;
    int framesUntilSpawn = 0;
    private Vector3 storePaneltopleft;
    private Vector3 helpbuttontopright;
    private Vector3 infobuttontopleft;
    static int continuousSpawnStartDelay = gameStatistics.continuousSpawnStartDelay;
    static int continuousSpawnDelay = gameStatistics.continuousSpawnDelay;

    public GameObject helpMenu;
    public GameObject pauseMenu;
    public GameObject infoMenu;
    
    void PurchaseUnit(Vector3 screenPosition) {
        if (!gameStatistics.canPurchaseUnit) {
            return;
        }
        if (pauseMenu.activeSelf ||
            helpMenu.activeSelf ||
            infoMenu.activeSelf ) {
                return;
        }
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        float distance = 1f;
        plane.Raycast(ray, out distance);
        Vector3 scenePosition = ray.GetPoint(distance);
        
        if (gameStatistics.regeneratingLevel) return;

        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower")) {
            if (Vector3.Distance(tower.transform.position, scenePosition) < tower.GetComponent<Tower>().ShootRadius) {
                return;
            }
        }

        scenePosition.x += UnityEngine.Random.Range(-0.05f, 0.05f);
        scenePosition.y += UnityEngine.Random.Range(-0.05f, 0.05f);
        scenePosition.z = 0;

        Instantiate(units[selectedButton], scenePosition, Quaternion.identity);
        gameStatistics.currentCredits = currentCredits - gameStatistics.unitCosts[selectedButton];
    }

    void Start() {
        for (int i = 0; i < gameStatistics.unitCosts.Length; i++) {
            transform.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Convert.ToString(gameStatistics.unitCosts[i]);
        }
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                int temp = selectedButton;
                selectedButton = 0;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                int temp = selectedButton;
                selectedButton = 1;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                int temp = selectedButton;
                selectedButton = 2;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                int temp = selectedButton;
                selectedButton = 3;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                int temp = selectedButton;
                selectedButton = 4;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                int temp = selectedButton;
                selectedButton = 5;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                int temp = selectedButton;
                selectedButton = 6;
                if(currentCredits >= gameStatistics.unitCosts[selectedButton]) PurchaseUnit(Input.mousePosition);
                selectedButton = temp;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buttons[0].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buttons[1].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buttons[2].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buttons[3].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buttons[4].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                buttons[5].GetComponent<Button>().onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                buttons[6].GetComponent<Button>().onClick.Invoke();
            }
        }

        if (framesUntilSpawn > 0) {
            framesUntilSpawn -= 1;
        }
        currentCredits = gameStatistics.currentCredits;
        if (Input.GetMouseButtonDown(0)) // left click 
        {
            //Debug.Log(Input.mousePosition);
            startMouseDown = Time.frameCount;
            if (selectedButton != -1 && // there is a unit selected in the store
                currentCredits >= gameStatistics.unitCosts[selectedButton] // have enough credits
                )
            {
                PurchaseUnit(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (Time.frameCount > startMouseDown + continuousSpawnStartDelay &&
                framesUntilSpawn == 0 &&
                selectedButton != -1 && // there is a unit selected in the store
                currentCredits >= gameStatistics.unitCosts[selectedButton] // have enough credits
            )
            {
                PurchaseUnit(Input.mousePosition);
                framesUntilSpawn = continuousSpawnDelay;
            }
        }
        else if ( Input.GetMouseButtonUp(0))
        {

        }
        
    }
}
