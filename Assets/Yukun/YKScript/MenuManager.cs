using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject postNotePrefab;
    public Transform notesRoot;
    public Transform trySpawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPostNote()
    {
        if (postNotePrefab != null)
        {
            Instantiate(postNotePrefab, trySpawnPos.position, trySpawnPos.rotation, notesRoot);
        }
    }
}
