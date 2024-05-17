using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]private Image ScreenTransition;
    private void Start()
    {
        GameEvents.current.onLevelComplete += LevelUp;
        SceneStart();
    }

    void LevelUp(int i)
    {
        float angle = 1f;
        DOTween.To(() => angle, x => angle = x, 0f, 1.25f).SetEase(Ease.InOutSine)
            .OnUpdate(() => {
                ScreenTransition.material.SetFloat("_Progress",angle);
            })
            .OnComplete(() =>
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            });
    }
    void SceneStart()
    {
        float angle = 0;
        DOTween.To(() => angle, x => angle = x, 1f, 1.25f).SetEase(Ease.InOutSine)
            .OnUpdate(() =>
            {
                ScreenTransition.material.SetFloat("_Progress", angle);
            });
    }
}
