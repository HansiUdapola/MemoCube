using System.Collections;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{

    GameObject BoxSelectSound;

    Color SelectedBoxColor;
    

    // Start is called before the first frame update
    void Start()
    {
        BoxSelectSound = GameObject.Find("SelectBoxSound");

        ColorUtility.TryParseHtmlString("#65FF00", out SelectedBoxColor);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        GameObject thePlayer = GameObject.Find("Player");
        Spawn spa = thePlayer.GetComponent<Spawn>();
        spa.selectedBoxNames.Add(this.name);

        this.GetComponent<MeshRenderer>().material.color = SelectedBoxColor;
        //this.GetComponent<MeshRenderer>().material.color = Color.gray;
        //Play Sound
        BoxSelectSound.GetComponent<AudioSource>().Play();
        StartCoroutine(SelectTheBox());
    }

    IEnumerator SelectTheBox() //when user selected the box, it displayed in gray, then display again in white after 1s.
    {
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<MeshRenderer>().material.color = Color.black;
    }
}
