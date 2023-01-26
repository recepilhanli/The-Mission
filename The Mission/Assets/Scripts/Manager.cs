using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("Settings")]

    public Camera Camera;
    // Start is called before the first frame update
 

    [SerializeField]
    public GameObject[] Hostages;
    public float LevelTime = 2 * 60f;

    public static float TotalTime = 2 * 60f;

    [System.NonSerialized]
    public GameObject gameover = null;


    public static  List<GameObject> SelectedHostages;

    public static int Rescued = 0;
    public static int TotalHostages = 0;
    public static int TotalDeadHostages = 0;
    public static bool Noticed = false;

    public static int actuallevel = 1;
    public static float RemainingTime = 2 * 60f;
    private float BallTimer = 3.0f;
    public static AudioSource audioSource;

    //tools
    public static GameObject Lights;

    private Lean.Gui.LeanJoystick MobileController1;

    public static GameObject Canvas;

    public int GameMode = 0;



    [SerializeField]
    private bool NonUpdate = false;
    public static void PlaySound(string sound, float volume = 1f)
    {
   
        var clip = Resources.Load("Sounds/" + sound) as AudioClip;
        GameObject go = new GameObject();
        go.name = "adsource - " + sound;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;

        source.PlayOneShot(source.clip);
        Destroy(go, source.clip.length);
    }

    void Awake()
    {
        Canvas = GameObject.Find("Canvas");
       


        if (NonUpdate == true) return;
        TotalTime = LevelTime;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;

        //   Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, true);


        //MobileController1 = GameObject.Find("MobileController1").GetComponent<Lean.Gui.LeanJoystick>();

        Lights = GameObject.Find("Lights");

        SelectedHostages = new List<GameObject>();
        TotalHostages = Hostages.Length;
        RemainingTime = TotalTime;
        audioSource = GetComponent<AudioSource>();

        if (GameMode == 1)
        {
            SelectedHostages.Add(Hostages[0]);
            Hostages[0].GetComponent<Outline>().OutlineColor = Color.green;

        }
            
      
    }

    public void CompleteScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        string name = scene.name;
        SceneManager.LoadScene("MCompleted");
        SceneManager.UnloadSceneAsync(name);
    }

    public static void SortHostages()
    {
        foreach (GameObject go in SelectedHostages)
        {
            if (go == null)
            {
                SelectedHostages.Remove(go);
            }
        }

        SelectedHostages.Sort();

    }

    void GameOver(string reason = " ")
    {

        gameover = Instantiate(Resources.Load("Prefabs/UI/GameOVer", typeof(GameObject))) as GameObject;
        RectTransform rectTransform = gameover.GetComponent<RectTransform>();
        if (rectTransform == null) Debug.Log("ERROR");


        rectTransform.position = Canvas.transform.position;
        rectTransform.sizeDelta = new Vector2(0,0);
        rectTransform.SetParent(Canvas.transform);

        gameover.GetComponent<Script_GameOver>().reason = reason;
    }

    public static bool IsPointerOverUIObject(Vector2 POS)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = POS;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult r in results)
            if (r.gameObject.GetComponent<RectTransform>() != null)
                return true;

        return false;
    }

    public static void PlayNoticedSoundEffect()
    {
        if (Noticed == true) return;
        PlaySound("Effects/noticed");
    }







    void CamMovement()
    {


        //mobile controller
        if(MobileController1 != null)
        { 
        Vector3 controller1 = new Vector3(MobileController1.ScaledValue.x, MobileController1.ScaledValue.y, 0);
        Camera.transform.position += controller1 * 20 * Time.deltaTime;
        }
 

        if (Input.GetKey("a"))
        {
            Camera.transform.position -= new Vector3(1f, 0, 0) * 20 *  Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            Camera.transform.position += new Vector3(1f, 0, 0) * 20 * Time.deltaTime;
        }


        if (Input.GetKey("w"))
        {
            Camera.transform.position += new Vector3(0, 1f, 0) * 20 * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            Camera.transform.position -= new Vector3(0, 1f, 0) * 20 * Time.deltaTime;
        }

    }

    public void SelectHostage(GameObject hostage, bool select)
    {
        if (PanZoom.isMoving == true) return;
        if(select == true)
        { 
        SelectedHostages.Add(hostage);
        hostage.GetComponent<Outline>().OutlineColor = Color.green;
        PlaySound("effects/selecting", 0.5f);
        }
        else
        {
        SelectedHostages.Remove(hostage);
        hostage.GetComponent<Outline>().OutlineColor = Color.white;
        PlaySound("effects/selecting", 0.5f);
        }

    }







    IEnumerator Touched(RaycastHit raycastHit)
    {
        yield return new WaitForSeconds(0.095f);
        

        FinishTouching(raycastHit);
    }


    void FinishTouching(RaycastHit raycastHit)
    {
        
        if (PanZoom.isMoving == true) return;

        

        if (raycastHit.collider.gameObject.tag == "Distracting")
        {
            BallTimer = 0.01f;
            raycastHit.collider.gameObject.GetComponent<Script_Distcrating>().Distract();
            return;
        }





        else if (raycastHit.collider.gameObject.tag == "Electricity")
        {
            Script_Electricity electricity = raycastHit.collider.gameObject.GetComponent<Script_Electricity>();
            if (electricity != null)
            {
                if (electricity.cooldown != 0f) return;


                Vector3 pos = raycastHit.collider.gameObject.transform.position;

                foreach (GameObject go in SelectedHostages)
                {

                    if (go == null) continue;
                    if (go.activeInHierarchy == false) continue;

                    if (Vector3.Distance(pos, go.transform.position) <= (go.transform.localScale.x * 2 + 0.75f))
                    {
                        if (!Physics.Linecast(pos, go.transform.position))
                        {
                            BallTimer = 0.01f;
                            go.transform.LookAt(pos);
                            electricity.Run();
                            return;
                        }
                    }

                }


            }


        }


        else if (raycastHit.collider.gameObject.tag == "Cabinet")
        {
            Cabinet cab = raycastHit.collider.gameObject.GetComponent<Cabinet>();
           

            if (cab.Hided != null)
            {
                BallTimer = 0.01f;
                cab.GetOut();
                return;
            }

            else
            {

                Vector3 pos = raycastHit.collider.gameObject.transform.position;

                foreach (GameObject go in SelectedHostages)
                {

                    if (go == null) continue;
                    if (go.activeInHierarchy == false) continue;

                    if (Vector3.Distance(pos, go.transform.position) <= (go.transform.localScale.x*2 + 0.75f))
                    {
                        if (!Physics.Linecast(pos, go.transform.position))
                        {
                            Debug.Log(raycastHit.collider.gameObject.tag);
                            BallTimer = 0.01f;
                            cab.GetIn(go);
                            return;
                        }
                    }

                }

            }


        }



        else if (raycastHit.collider.gameObject.tag == "Hostage")
        {

            if (SelectedHostages.Contains(raycastHit.collider.gameObject))
            {

                SelectHostage(raycastHit.collider.gameObject, false);
                return;
            }


            SelectHostage(raycastHit.collider.gameObject, true);
            return;
        }

        if (SelectedHostages.Count == 0) return;

        bool dontchange = false;
        float distance = 0.1f;
   
        foreach (GameObject go in SelectedHostages)
        {
            if (go == null) continue;
            else if (go.activeInHierarchy == false) continue;

            NavMeshAgent navMeshAgent = go.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null) continue;
            Entity entity = go.GetComponent<Entity>();

            
            if (entity == null) continue;
            else if (entity.alive == false) continue;

            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(raycastHit.point);
            
            if (Vector3.Distance(go.transform.position, raycastHit.point) < 0.6f)
            {
                dontchange = true;
                BallTimer = 0.001f;
            }

            else if (Vector3.Distance(go.transform.position, raycastHit.point) < distance) distance = Vector3.Distance(go.transform.position, raycastHit.point);





        }

        if (dontchange == false)
        {
            transform.position = raycastHit.point;
            BallTimer = distance*15;
        }
        return;
    }

    void Touching()
    {

        


        if (transform.position != new Vector3(0, -10000, 0))
        {
            BallTimer -= Time.deltaTime;
            if (BallTimer < 0) BallTimer = 0.000f;
            if (BallTimer <= 0)
            {
                BallTimer = 3.0f;
                transform.position = new Vector3(0, -10000, 0);
            }
        }
        else transform.position = new Vector3(0, -10000, 0);



        RaycastHit raycastHit = new RaycastHit();
        bool touched = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && IsPointerOverUIObject(Input.GetTouch(0).position) == false)
        {

            Ray ray = Camera.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                raycastHit = hit;
                touched = true;

            }
        }

        else if (Input.GetMouseButtonDown(0) && Time.timeScale != 0 && IsPointerOverUIObject(Input.mousePosition) == false)
        {

            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                raycastHit = hit;
                touched = true;

            }


        }

        

        if (PanZoom.isMoving == true) return;

        if (touched == false) return;


#if UNITY_ANDROID || UNITY_IOS
        StartCoroutine(Touched(raycastHit));

#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                 FinishTouching(raycastHit);
#endif





    }




    public static void RestartGame(bool reloadlevel = true)
    {
        Rescued = 0;
        TotalDeadHostages = 0;
        TotalHostages = 0;
        Noticed = false;
        Time.timeScale = 1;
        if (reloadlevel == true)
        { 
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene.name);
        SceneManager.LoadScene(scene.name);
        }
    }

    void Update()
    {
        if (NonUpdate == true) return;

        gameObject.transform.Rotate(240 * Time.deltaTime, 0, 0);

        if (Rescued+TotalDeadHostages == TotalHostages && Rescued != 0)
        {
            CompleteScene();
            return;
        }


        if (gameover != null) return;


        RemainingTime -= Time.deltaTime;
        if(RemainingTime <= 0 && gameover == null)
        {
                GameOver("No time left for hostages.");          
        }

        int deadcount = 0;

        for(int i = 0; i < Hostages.Length; i++)
        {
            if (Hostages[i] == null) continue;
            if (Hostages[i].GetComponent<Entity>() == null) deadcount++;
            else if (Hostages[i].GetComponent<Entity>().alive == false) deadcount++;
        }
     

        if (Hostages.Length == deadcount && gameover == null)
        {
            if (GameMode == 0) GameOver("All of the hostages are dead.");
            else GameOver("The actor is dead.");
            return;
        }
   


        CamMovement();
        Touching();
  

     
    }




}
