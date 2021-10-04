using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SwapTimeManager : MonoBehaviour
{
    public float MoveTime;
    public float ShootTime;
    public int cycles;
    public bool infiniteCycles;

    public GameObject GunGlob;
    public GameObject MoveGlob;

    public GameObject GunUI;
    public GameObject MoveUI;

    public Volume cc;
    ChromaticAberration chromaticAberration;
    FilmGrain filmGrain;
    Vignette vignette;
    ColorAdjustments colorAdjustments;
    bool maxCC;
    public bool dying;
    public bool paused;
    public GameObject canvasPaused;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startCycles());
        ChromaticAberration ca;

        if (cc.profile.TryGet<ChromaticAberration>(out ca))
        {
            chromaticAberration = ca;
        }

        FilmGrain fg;

        if (cc.profile.TryGet<FilmGrain>(out fg))
        {
            filmGrain = fg;
        }

        Vignette v;

        if (cc.profile.TryGet<Vignette>(out v))
        {
            vignette = v;
        }

        ColorAdjustments c;

        if (cc.profile.TryGet<ColorAdjustments>(out c))
        {
            colorAdjustments = c;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
        if (paused)
        {
            canvasPaused.SetActive(true);
        }
        else
        {
            canvasPaused.SetActive(false);


        }


        if (dying)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, -100f, 2.5f * Time.deltaTime);
        }
        else
        {
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, 0f, 15f * Time.deltaTime);

        }
        if (maxCC&&!dying && !paused)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value,1, 15f * Time.deltaTime);
            filmGrain.intensity.value = Mathf.Lerp(filmGrain.intensity.value, .4f, .03f);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, .45f, .03f);



        }
        else
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0, 15f * Time.deltaTime);
            filmGrain.intensity.value = Mathf.Lerp(filmGrain.intensity.value, 0, .03f);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, .03f);
        }
    }

    void swap()
    {
        if (!dying&&!paused)
        {
            GetComponent<ScoreManager>().swapSoundPlay();
            GunUI.SetActive(!GunUI.activeInHierarchy);
            MoveUI.SetActive(!MoveUI.activeInHierarchy);
            GunGlob.GetComponent<playerGun>().canShoot = !GunGlob.GetComponent<playerGun>().canShoot;
            MoveGlob.GetComponent<playerMovement>().canMove = !MoveGlob.GetComponent<playerMovement>().canMove;
        }
    }
    public void startDeath()
    {
        StartCoroutine(death());
    }

    IEnumerator startCycles()
    {
        if (!infiniteCycles)
        {
            for(int i = 0; i < cycles; i++)
            {
                if (!dying)
                {
                    yield return new WaitForSeconds(MoveTime);
                    maxCC = true;
                    yield return new WaitForSeconds(.4f);
                    maxCC = false;
                    swap();
                    yield return new WaitForSeconds(ShootTime);
                    maxCC = true;
                    yield return new WaitForSeconds(.4f);
                    maxCC = false;
                    swap();
                }
                
            }
        }
        else
        {
            bool endwhile=true;
            while (endwhile)
            {
                if (!dying)
                {
                    yield return new WaitForSeconds(MoveTime);
                    maxCC = true;
                    yield return new WaitForSeconds(.4f);
                    maxCC = false;
                    swap();
                    yield return new WaitForSeconds(ShootTime);
                    maxCC = true;
                    yield return new WaitForSeconds(.4f);
                    maxCC = false;
                    swap();
                }
                else
                {
                    endwhile = false;
                }
            }
        }
    }
    public IEnumerator death()
    {
        //maxCC = true;
        dying = true;
        yield return new WaitForSeconds(3);
        //maxCC = false;
        //dying = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
