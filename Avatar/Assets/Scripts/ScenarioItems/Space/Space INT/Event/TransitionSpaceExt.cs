using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionSpaceExt : MonoBehaviour {

    public GameObject endQuad;

    public Renderer endQuadRenderer;

    public Material endQuadMaterial;

    public Transform videoCameraPlane;
    public GameObject greenScreen;

    private bool enableEXTransition = false;
    private bool isLoadingNextLevel = false;

    private Color endQuadColor;

    void Start()
    {
        endQuadColor = endQuadRenderer.sharedMaterial.GetColor("_TintColor");
    }

    void OnTriggerEnter (Collider col)
	{
        if(col.transform.tag == "Avatar")
        {
            endQuadColor.a = 0;
            endQuadRenderer.sharedMaterial.SetColor("_TintColor", endQuadColor);

            endQuad.SetActive(true);
            enableEXTransition = true;
        }
	}

    void Update()
    {
        if(enableEXTransition)
        {
            endQuadColor.a += 0.01f;
            endQuadRenderer.sharedMaterial.SetColor("_TintColor", endQuadColor);

            if (endQuadColor.a >= 1)
                laundNextLevel();
        }
    }

    public void laundNextLevel()
    {
        if (!isLoadingNextLevel)
        {
            //Application.LoadLevel("SpaceEXT");
            SceneManager.LoadScene("SpaceEXT"); 
            isLoadingNextLevel = true;
        }
    }
}
