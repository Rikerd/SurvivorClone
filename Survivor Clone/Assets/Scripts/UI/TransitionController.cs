using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public enum TransistionStatus
    {
        Fade,
        Unfade,
        Flash,
        Nothing
    }

    public Material transMat;
    public Material flashMat;

    public Texture2D currentTexture;

    public AudioSource audioSource;

    public float fadeTime = 1f;

    private float timer = 0;

    private Material currentMat;

    public TransistionStatus currentStatus;

    private bool flash;


    void Start()
    {
        timer = 0;

        currentMat = transMat;

        currentMat.SetFloat("_Cutoff", 1f);

        currentStatus = TransistionStatus.Unfade;
    }

    public void Update()
    {
        if (currentStatus == TransistionStatus.Fade)
        {
            timer += Time.unscaledDeltaTime / fadeTime;
            audioSource.volume = Mathf.Lerp(0.5f, 0f, timer);
            currentMat.SetFloat("_Cutoff", Mathf.Lerp(0, 1, timer));

            if (currentMat.GetFloat("_Cutoff") >= 1)
            {
                currentStatus = TransistionStatus.Nothing;
            }
        }

        if (currentStatus == TransistionStatus.Unfade)
        {
            timer += Time.unscaledDeltaTime / fadeTime;
            audioSource.volume = Mathf.Lerp(0f, 0.5f, timer);
            currentMat.SetFloat("_Cutoff", Mathf.Lerp(1, 0, timer));

            if (currentMat.GetFloat("_Cutoff") <= 0)
            {
                currentStatus = TransistionStatus.Nothing;
            }
        }

        if (currentStatus == TransistionStatus.Flash)
        {
            timer += Time.unscaledDeltaTime / (fadeTime / 2);

            if (flash)
            {
                currentMat.SetFloat("_Fade", Mathf.Lerp(0, 1, timer));
            }
            else
            {
                currentMat.SetFloat("_Fade", Mathf.Lerp(1, 0, timer));
            }

            if (flash && currentMat.GetFloat("_Fade") >= 1)
            {
                flash = false;
                timer = 0;
            }

            if (!flash && currentMat.GetFloat("_Fade") <= 0)
            {
                currentStatus = TransistionStatus.Nothing;
            }
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (currentMat != null)
            Graphics.Blit(src, dst, currentMat);
    }

    public void FadeToBlack()
    {
        currentMat = transMat;
        transMat.SetFloat("_Cutoff", 0f);
        currentStatus = TransistionStatus.Fade;
        timer = 0f;
    }

    public void FadeToBlack(Texture2D texture)
    {
        currentMat = transMat;
        currentMat.SetTexture("_TransitionTex", currentTexture);
        transMat.SetFloat("_Cutoff", 0f);
        currentStatus = TransistionStatus.Fade;
        timer = 0f;

    }

    public void UnfadeFromBlack()
    {
        currentMat = transMat;
        transMat.SetFloat("_Cutoff", 1f);
        currentStatus = TransistionStatus.Unfade;
        timer = 0f;
    }

    public void UnfadeFromBlack(Texture2D texture)
    {
        currentMat = transMat;
        currentMat.SetTexture("_TransitionTex", texture);
        transMat.SetFloat("_Cutoff", 1f);
        currentStatus = TransistionStatus.Unfade;
        timer = 0f;
    }

    public void StartFlash()
    {
        currentMat = flashMat;
        flashMat.SetFloat("_Cutoff", 1f);
        flashMat.SetFloat("_Fade", 0f);
        currentStatus = TransistionStatus.Flash;
        timer = 0f;

        flash = true;
    }

    public float GetFadeTime()
    {
        return fadeTime;
    }

}
