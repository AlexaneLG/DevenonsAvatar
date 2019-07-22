using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

using DG.Tweening;
public class EndForet : ScenarioItem
{
    public ScreenOverlay overlay;
    public override void OnEnable()
    {
        base.OnEnable();
        DOTween.To((f) =>
       {
           overlay.intensity = f;
       }, 0, 1, duration).SetEase(Ease.InQuad);
    }

    public override void OnDisable()
    {
        base.OnDisable();

    }
}
