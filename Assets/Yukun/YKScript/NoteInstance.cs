using UnityEngine;

public class NoteInstance : MonoBehaviour
{
    public GameObject noteBG;
    public GameObject userIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShrinkNote()
    {
        noteBG.SetActive(false);
        userIcon.SetActive(true);
    }

    public void ExpandNote()
    {
        noteBG.SetActive(true);
        userIcon.SetActive(false);
    }
}
