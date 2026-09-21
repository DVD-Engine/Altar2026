using UnityEngine;

//[ExecuteAlways]
public class WeatherController : MonoBehaviour
{

    //Referencias
    public Animator animatorManager;
    public ParticleSystem rainManager;
    public GameObject clouds;

    //Controles
    private int currentWeather = 0;
    private bool isRaining = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rainManager.Stop();
        clouds.SetActive(false);
    }


    //Setters de ambiente.

    public void updateWeather()
    {
        //Cicla entre 3 estados: Dia, Atardecer y noche. Toda la logica se maneja por animaciones, para hacerlo mas bonito y mas personalizable
        currentWeather++;
        currentWeather %= 3;
        animatorManager.SetInteger("weatherState", currentWeather);
        animatorManager.SetBool("isRaining", false);
        clouds.SetActive(false);
        rainManager.Stop();
    }

    //Controla la lluvia, pues la implementación es ligeramente distinta
    public void toggleRain()
    {
        isRaining = !isRaining;
        animatorManager.SetBool("isRaining", isRaining);
        if (!isRaining)
        {
            rainManager.Stop();
            clouds.SetActive(false);
        }
        else
        {
            rainManager.Play();
            clouds.SetActive(true);
            
        }
    }
}
