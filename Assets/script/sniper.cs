using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sniper : MonoBehaviour
{
    [Header("位置:")]
    public Transform recoilPosition;
    public Transform rotationPoint;
    [Space(10)]

    [Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

    [Header("UI Components")]
	public Text timescaleText;
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;
    public GameObject lowAmmoUI;
    public GameObject ReloadUI;

    [Header("作用力速度,2返回速度:")]
    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    [Space(10)]

    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 18f;
    [Space(10)]
   

    [Header("槍枝作用力設置:")]
    public Vector3 RecoilRotation = new Vector3(10 , 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f , 0f, -0.2f);
    [Space(10)]
    public Vector3 AimRecoilRotation = new Vector3(2.5f, 2.5f, 2.5f);
    public Vector3 AimRecoilKickBack = new Vector3(0.015f , 0f, -0.2f);
    [Space(10)]

    Vector3 rotationalRecoil;
    Vector3 PositionalRecoil;
    Vector3 Rot;
    [Header("狀態")]
    public bool aiming;

    [Header("槍設置:")]
    public float fireRate = 1f;//射速
    public float damage =50f;//傷害
    public float range =100f;//範圍
    public Camera fpsCam;//玩家視角
    public ParticleSystem muzzleFlash;//指定槍枝火花特效
    public ParticleSystem muzzleFlash2;
    public GameObject impactEffect;//指定子彈接觸到地面效果
    public GameObject impactEffect2;
    private float nexttimetofire = 0f;//設定觸發連續射擊時間
    public int maxAmmo = 5;//子彈數量
    private int currentAmmo;//當前子彈數量
    public float reloadTime = 3.2f;//換彈時間 
    private bool isreloading = false;
	[Header("Weapon Sway")]
	//Enables weapon sway
	[Tooltip("Toggle weapon sway.")]
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.44f;
	public float swaySmoothValue = 5.0f;

	private Vector3 initialSwayPosition;

	[System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform casingPrefab;
	}
	public prefabs Prefabs;
	
	[System.Serializable]
	public class spawnpoints
	{  
		[Header("Spawnpoints")]
		public Transform casingSpawnPoint;
	}
    public spawnpoints Spawnpoints;

    [Space(10)]
    [Header("聲音設置")]
	//Main audio source
	public AudioSource mainAudioSource;
	//Audio source used for shoot sound
	public AudioSource shootAudioSource;

	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip takeOutSound;
		public AudioClip holsterSound;
		public AudioClip reloadSoundOutOfAmmo;
		public AudioClip reloadSoundAmmoLeft;
		public AudioClip aimSound;
        public AudioClip ammoSound;
	}
    public soundClips SoundClips;

    private Recoil Recoil_Script;//Recoil腳本

    public Animator animator;//設置動畫控制器

    private bool soundHasPlayed = false;

    private void Start () {
        muzzleflashLight.enabled = false;
        Recoil_Script = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();//抓取Recoil所在物件位置
        currentAmmo = maxAmmo;   
        initialSwayPosition = transform.localPosition;
		//Set the shoot sound to audio source
		shootAudioSource.clip = SoundClips.shootSound;
	}

	private void LateUpdate () {
		
		//Weapon sway
		if (weaponSway == true) 
		{
			float movementX = -Input.GetAxis ("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis ("Mouse Y") * swayAmount;
			//Clamp movement to min and max values
			movementX = Mathf.Clamp 
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp 
				(movementY, -maxSwayAmount, maxSwayAmount);
			//Lerp local pos
			Vector3 finalSwayPosition = new Vector3 
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp 
				(transform.localPosition, finalSwayPosition + 
					initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}

    private IEnumerator MuzzleFlashLight () {
		
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}

    void Update()
    {   
        if(currentAmmo<=2)
        {
            lowAmmoUI.gameObject.SetActive(true);
            ReloadUI.gameObject.SetActive(false);
        }
        if(currentAmmo==0)
        {
            lowAmmoUI.gameObject.SetActive(false);
            ReloadUI.gameObject.SetActive(true);
        }
        if(currentAmmo>2)
        {
            lowAmmoUI.gameObject.SetActive(false);
            ReloadUI.gameObject.SetActive(false);
        }
        currentAmmoText.text = currentAmmo.ToString ();

        if(isreloading)
             return;
        if(Input.GetKeyDown("r"))//當前子彈小於零
        {
            StartCoroutine(Reload());
            return;//讓它在還沒reload完不會繼續執行GetButton"Fire1"
        }
        if(Input.GetButton("Fire1") && Time.time >= nexttimetofire&& currentAmmo>0)//按下左鍵且子彈大於零
        {
            nexttimetofire = Time.time + 1f/fireRate;//設置射速
            Shoot();
            GunRecoil();
            if(!aiming)
            {
                animator.Play("Fire",0,0f);
            }
            else
            {
                animator.Play("Fireaim",0,0f);
            }
            StartCoroutine (MuzzleFlashLight ());
        }
        if(Input.GetButtonDown("Fire1")&& currentAmmo==0)
        {
            mainAudioSource.clip = SoundClips.ammoSound;
				mainAudioSource.Play ();
                if( Input.GetButton("Fire2")==false)
                {
                animator.Play("FireEmpty",0,0f);
                }
        }
        
        if(Input.GetButton("Fire2"))
        {
            aiming = true;
            if (!soundHasPlayed) 
			{
				mainAudioSource.clip = SoundClips.aimSound;
				mainAudioSource.Play ();
				soundHasPlayed = true;
			}
            animator.SetBool("Aim",true);
        }
        else
        {
            aiming = false;
            animator.SetBool("Aim",false);
            soundHasPlayed = false;
        }
    }
    void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed*Time.deltaTime);
        PositionalRecoil = Vector3.Lerp(PositionalRecoil, Vector3.zero, positionalReturnSpeed*Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, PositionalRecoil, positionalRecoilSpeed*Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed*Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);        
    }
    
    void GunRecoil()
    {
        if(aiming)
        {
        rotationalRecoil += new Vector3(-AimRecoilRotation.x, Random.Range(-AimRecoilRotation.y, AimRecoilRotation.y), Random.Range(-AimRecoilRotation.z, AimRecoilRotation.z));
        PositionalRecoil += new Vector3(Random.Range(-AimRecoilKickBack.x, AimRecoilKickBack.x), Random.Range(-AimRecoilKickBack.y, AimRecoilKickBack.y), AimRecoilKickBack.z);
        }
        else
        {
        rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        PositionalRecoil += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
        }    
    }

    IEnumerator Reload()
    {
        isreloading = true;
        Debug.Log("reload");
        if(currentAmmo==0)
        {
            mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();
            reloadTime = 4.15f;
            animator.SetBool("Reloadempty",true);
            animator.SetBool("Reloading",false);//結束動畫
        }
        else
        {
            mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play ();
            reloadTime = 3.2f;
            animator.SetBool("Reloadempty",false);
            animator.SetBool("Reloading",true);//開始動畫
        }
        yield return new WaitForSeconds(reloadTime);//等待時間
        animator.SetBool("Reloadempty",false);
        animator.SetBool("Reloading",false);//結束動畫
        currentAmmo=maxAmmo;
        isreloading = false;
    }
    void Shoot ()
    {
        Instantiate (Prefabs.casingPrefab, 
					Spawnpoints.casingSpawnPoint.transform.position, 
					Spawnpoints.casingSpawnPoint.transform.rotation);
        Recoil_Script.RecoilFire();//反作用力

        currentAmmo--;
        shootAudioSource.clip = SoundClips.shootSound;
		shootAudioSource.Play ();

        muzzleFlash.Play();//火花特效
        muzzleFlash2.Play();
        RaycastHit hit;
       if(Physics.Raycast(fpsCam.transform.position , fpsCam.transform.forward , out hit, range))//設定子但從玩家攝相機發射且不能超過所設的範圍
       {
        Debug.Log(hit.transform.name);//確認
        Target target =hit.transform.GetComponent<Target>();//找到Target腳本
        if(target != null)
        {
            target.TakeDamage(damage);//繼續執行Target裡面的TakeDamage變數
            GameObject impactGo2 =Instantiate(impactEffect2, hit.point , Quaternion.LookRotation(hit.normal));//子彈接觸到地面效果
            Destroy(impactGo2, 2f);
        }
        GameObject impactGo =Instantiate(impactEffect, hit.point , Quaternion.LookRotation(hit.normal));//子彈接觸到地面效果
        Destroy(impactGo, 2f);//兩秒後清除子彈接觸到地面效果     
       }
    }
}
