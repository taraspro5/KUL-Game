using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuStyle{
	horizontal,vertical
}

public class GunInventory : MonoBehaviour {
	[Tooltip("Current weapon gameObject.")]
	public GameObject currentGun;
	private Animator currentHAndsAnimator;
	private int currentGunCounter = 0;

	[Tooltip("Put Strings of weapon objects from Resources Folder.")]
	public List<string> gunsIHave = new List<string>();
	[Tooltip("Icons from weapons.(Fetched when you run the game)*MUST HAVE ICONS WITH CORRESPONDING NAMES IN RESOUCES FOLDER*")]
	public Texture[] icons;

	[HideInInspector]
	public float switchWeaponCooldown;

	void Awake(){
		StartCoroutine("UpdateIconsFromResources");

		StartCoroutine ("SpawnWeaponUponStart");

		if (gunsIHave.Count == 0)
			print ("No guns in the inventory");
	}

	IEnumerator SpawnWeaponUponStart(){
		yield return new WaitForSeconds (0.5f);
		StartCoroutine("Spawn",0);
	}

	void Update(){

		switchWeaponCooldown += 1 * Time.deltaTime;
		if(switchWeaponCooldown > 1.2f && Input.GetKey(KeyCode.LeftShift) == false){
			Create_Weapon();
		}

	}


	IEnumerator UpdateIconsFromResources(){
		yield return new WaitForEndOfFrame ();

		icons = new Texture[gunsIHave.Count];
		for(int i = 0; i < gunsIHave.Count; i++){
			icons[i] = (Texture) Resources.Load("Weap_Icons/" + gunsIHave[i].ToString() + "_img");
		}

	}

	void Create_Weapon(){

		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0){
			switchWeaponCooldown = 0;

			currentGunCounter++;
			if(currentGunCounter > gunsIHave.Count-1){
				currentGunCounter = 0;
			}
			StartCoroutine("Spawn",currentGunCounter);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0){
			switchWeaponCooldown = 0;

			currentGunCounter--;
			if(currentGunCounter < 0){
				currentGunCounter = gunsIHave.Count-1;
			}
			StartCoroutine("Spawn",currentGunCounter);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1) && currentGunCounter != 0){
			switchWeaponCooldown = 0;
			currentGunCounter = 0;
			StartCoroutine("Spawn",currentGunCounter);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) && currentGunCounter != 1){
			switchWeaponCooldown = 0;
			currentGunCounter = 1;
			StartCoroutine("Spawn",currentGunCounter);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && currentGunCounter != 2)
        {
            switchWeaponCooldown = 0;
            currentGunCounter = 2;
            StartCoroutine("Spawn", currentGunCounter);
        }

	}

	IEnumerator Spawn(int _redniBroj){
		if (weaponChanging)
			weaponChanging.Play ();
		else
			print ("Missing Weapon Changing music clip.");
		if(currentGun){
			if(currentGun.name.Contains("Gun")){

				currentHAndsAnimator.SetBool("changingWeapon", true);

				yield return new WaitForSeconds(0.8f);
				Destroy(currentGun);

				GameObject resource = (GameObject) Resources.Load(gunsIHave[_redniBroj].ToString());
				currentGun = (GameObject) Instantiate(resource, transform.position, Quaternion.identity);
				AssignHandsAnimator(currentGun);
			}
			else if(currentGun.name.Contains("Sword")){
				currentHAndsAnimator.SetBool("changingWeapon", true);
				yield return new WaitForSeconds(0.25f);

				currentHAndsAnimator.SetBool("changingWeapon", false);

				yield return new WaitForSeconds(0.6f);
				Destroy(currentGun);

				GameObject resource = (GameObject) Resources.Load(gunsIHave[_redniBroj].ToString());
				currentGun = (GameObject) Instantiate(resource, transform.position, Quaternion.identity);
				AssignHandsAnimator(currentGun);
			}
		}
		else{
			GameObject resource = (GameObject) Resources.Load(gunsIHave[_redniBroj].ToString());
			currentGun = (GameObject) Instantiate(resource, transform.position, Quaternion.identity);

			AssignHandsAnimator(currentGun);
		}


	}


	void AssignHandsAnimator(GameObject _currentGun){
		if(_currentGun.name.Contains("Gun")){
			currentHAndsAnimator = currentGun.GetComponent<GunScript>().handsAnimator;
		}
	}

	void OnGUI(){

		if(currentGun){
			for(int i = 0; i < gunsIHave.Count; i++){
				DrawCorrespondingImage(i);
			}
		}

	}

	[Header("GUI Gun preview variables")]
	[Tooltip("Weapon icons style to pick.")]
	public MenuStyle menuStyle = MenuStyle.horizontal;
	[Tooltip("Spacing between icons.")]
	public int spacing = 10;
	[Tooltip("Begin position in percetanges of screen.")]
	public Vector2 beginPosition;
	[Tooltip("Size of icon in percetanges of screen.")]
	public Vector2 size;
	void DrawCorrespondingImage(int _number){

		string deleteCloneFromName = currentGun.name.Substring(0,currentGun.name.Length - 7);

		if(menuStyle == MenuStyle.horizontal){
			if(deleteCloneFromName == gunsIHave[_number]){
				GUI.DrawTexture(new Rect(vec2(beginPosition).x +(_number*position_x(spacing)),vec2(beginPosition).y,//position variables
					vec2(size).x, vec2(size).y),
					icons[_number]);
			}
			else{			
				GUI.DrawTexture(new Rect(vec2(beginPosition).x +(_number*position_x(spacing) + 10),vec2(beginPosition).y + 10,//position variables
					vec2(size).x - 20, vec2(size).y- 20),
					icons[_number]);
			}
		}
		else if(menuStyle == MenuStyle.vertical){
			if(deleteCloneFromName == gunsIHave[_number]){
				GUI.DrawTexture(new Rect(vec2(beginPosition).x,vec2(beginPosition).y +(_number*position_y(spacing)),//position variables
					vec2(size).x, vec2(size).y),
					icons[_number]);
			}
			else{			
				GUI.DrawTexture(new Rect(vec2(beginPosition).x,vec2(beginPosition).y + 10  +(_number*position_y(spacing)),//position variables
					vec2(size).x - 20, vec2(size).y- 20),
					icons[_number]);
			}
		}



	}

	public void DeadMethod(){
		Destroy (currentGun);
		Destroy (this);
	}


	private float position_x(float var){
		return Screen.width * var / 100;
	}
	private float position_y(float var)
	{
		return Screen.height * var / 100;
	}
	private float size_x(float var)
	{
		return Screen.width * var / 100;
	}
	private float size_y(float var)
	{
		return Screen.height * var / 100;
	}
	private Vector2 vec2(Vector2 _vec2){
		return new Vector2(Screen.width * _vec2.x / 100, Screen.height * _vec2.y / 100);
	}
	[Header("Sounds")]
	[Tooltip("Sound of weapon changing.")]
	public AudioSource weaponChanging;
}