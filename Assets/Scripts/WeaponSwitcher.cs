using UnityEngine;
using System.Collections;

// Inspiration from Youtube Tutorial: https://www.youtube.com/watch?v=Dn_BUIVdAPg&t=91s

public class WeaponSwitcher: MonoBehaviour {

	public GameObject[] weapons;
    public GameObject player;
	public int currentWeapon = 0;
	private int nrWeapons = 2;

	void Start() {
		SwapGun(currentWeapon);
	}

	void Update() {

        // if (Input.GetKey(KeyCode.A)) {
        // if(OVRInput.Get(OVRInput.Button.PrimaryThumbstick)){
            // OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp);

        if (OVRInput.Get(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.One)) {
            player.GetComponent<Animator>().SetBool("swapped", true);
            for (int i = 0; i < nrWeapons; i++) weapons[i].gameObject.SetActive(false);
            currentWeapon = 1-currentWeapon;
            SwapGun(currentWeapon);

        } else {
            player.GetComponent<Animator>().SetBool("swapped", false);
        }

	}

	void SwapGun(int index) {
		for (int i = 0; i < nrWeapons; i++) {
		    if (i == index) weapons[i].gameObject.SetActive(true);
		}
	}

}