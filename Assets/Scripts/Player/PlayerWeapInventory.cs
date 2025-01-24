using UnityEngine;

public class PlayerWeapInventory : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject[] weapons;

    public GameObject equipped;

    private void Start()
    {
        // primary as default
        equipped = weapons[0];
    }

    private void Update()
    {
        InputCheck();
    }

    void InputCheck() 
    {
        // scrollwheel up / primary
        if (((Input.GetAxis("Mouse ScrollWheel") > 0f) || Input.GetKeyDown("1")) && equipped != weapons[0])
        {
            equipped.SetActive(false);
            equipped = weapons[0];
            equipped.SetActive(true);
        }
        // scrollwheel down / secondary
        if (((Input.GetAxis("Mouse ScrollWheel") < 0f) || Input.GetKeyDown("2")) && equipped != weapons[1])
        {
            equipped.SetActive(false);
            equipped = weapons[1];
            equipped.SetActive(true);
        }
    }

    public void ShowEquipped(bool show) 
    {
        equipped.SetActive(!show);
        Debug.Log(equipped.ToString() + " Is " + !show);
    }

}
