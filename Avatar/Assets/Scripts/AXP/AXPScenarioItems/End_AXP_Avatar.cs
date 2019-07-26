using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_AXP_Avatar : AugmentedScenarioItem
{
    public GameObject avatarGame;
    public Transform avatarGameLeftHand;
    public Transform avatarGameRightHand;

    override protected void Awake()
    {
        durationIncr = 50;

        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        dataManager.DisplayHUD(false);

        GameObject avatar3DGame = dataManager.characterManager.GetComponent<CharacterControllerBasedOnAxis>().avatars3D[0].gameObject;
        avatar3DGame.transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
        avatar3DGame.SetActive(true);

        // Get Avatar_Game
        avatarGame = GameObject.FindGameObjectWithTag("AvatarGameBox");
        avatarGameLeftHand = GameObject.FindGameObjectWithTag("AvatarGameLeftHand").transform;
        avatarGameRightHand = GameObject.FindGameObjectWithTag("AvatarGameRightHand").transform;

        // Set invisible material
        Material[] materials = avatarGame.GetComponent<SkinnedMeshRenderer>().materials;
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i] = Resources.Load("InvisibleMaterial") as Material;
        }

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        yield return StartCoroutine(DisplayDebugHands());
    }

    private IEnumerator DisplayDebugHands()
    {
        Transform leftHand = GameObject.Find("Image-LeftHand").transform;
        Transform rightHand = GameObject.Find("Image-RightHand").transform;

        while (true)
        {
            leftHand.position = dataManager.mainCamera.WorldToScreenPoint(avatarGameLeftHand.position);
            rightHand.position = dataManager.mainCamera.WorldToScreenPoint(avatarGameRightHand.position);
            // mirror
            leftHand.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                leftHand.gameObject.GetComponent<RectTransform>().anchoredPosition.x * -1,
                leftHand.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
            rightHand.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                rightHand.gameObject.GetComponent<RectTransform>().anchoredPosition.x * -1,
                rightHand.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
            yield return null;
        }
    }
}
