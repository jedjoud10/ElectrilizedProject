using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenuScript : MonoBehaviour
{
    public GameObject pausemenu;
    public Slider scaleslidergui;
    public Slider chunkdistanceslider;
    public Slider qualityslider;
    public CanvasScaler cs;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.GetComponent<PlayerController>().canMove)
            {
                pausemenu.SetActive(true);
                StartCoroutine(showdelay());
            }        
        }
        cs.scaleFactor = scaleslidergui.value;
    }
    public void show()
    {
        pausemenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.GetComponent<PlayerController>().canMove = true;
    }
    IEnumerator showdelay()
    {
        yield return new WaitForSeconds(0f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.GetComponent<PlayerController>().canMove = false;
    }
    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(Mathf.RoundToInt(qualityslider.value));
    }
}
