using UnityEngine;
using System.Collections;

// Inspiration from Youtube Tutorial: https://www.youtube.com/watch?v=Dn_BUIVdAPg&t=91s

public class WeaponSwitcher: MonoBehaviour {

	public GameObject[] weapons;
	public int currentWeapon = 0;
	private int nrWeapons = 2;

	void Start() {
		SwapGun(currentWeapon);
	}

	void Update() {
        if (Input.GetKey(KeyCode.A)) {
		    for (int i = 1; i <= nrWeapons; i++) {
				currentWeapon = i - 1;
				SwapGun(currentWeapon);
                print(currentWeapon);

			}
		}

        // if (Input.GetKey(KeyCode.A)) {
        //     print(currentWeapon);
        //     currentWeapon = 1-currentWeapon;
		// }

	}

	void SwapGun(int index) {
        print("SwapGun is happening");
        // print(index);
		for (int i = 0; i < nrWeapons; i++) {
			if (i == index) {
				weapons[i].gameObject.SetActive(true);
			} else {
				weapons[i].gameObject.SetActive(false);
			}
		}
	}

}