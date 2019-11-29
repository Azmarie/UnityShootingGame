# <center> Homework 3 Report </center>


## Task (a) to (e)

### (a) Creating the basic enemy character
**Set targets :**
Create targets and turn the enemy direction to target

**Change uniform :**
Copy & paste to get new texture, drag texture to enemy
### (b) Detecting the player 
**Follow player :**
Get angle to player and change walking direction to player ( via LookAt() )

**Run towards player:**
Add run animation and set run parameter for state transition

### (c) Shooting to the player 
**Shooting :**
Set animation and trigger for fire, use GetComponent<GunVR> and reduce the soldier’s health 

**Bullet control :**
Use time control based on Time.deltaTime

**Death animation :**
Add laying down animation and change camera position to start location

### (d) Player shooting and health 
**Player’s health UI:**
Add Text box and add combine text object with health

**Gun independent :**
Set gun a rigidbody and add collider to gun

### (e) Creating the environment 

**Create room :**
Duplicate walls and floors, change the texture scale of walls and floors

**Escape door:**
Add escape door and add Collider to door. Restart the game when collide with the door.

## Bonus

### (f) Ammo supply 

Add ammo object, get ammo box position and add health when reaching the box by every 2.5 frames

### (g) Enemies getting cover 

Follow cover and add the sitting animation 

### (h) Detecting body part hits 

Set layers to Enemy and reduce health according to body part

### (i) Swapping guns 

Activate game object while switching guns, add switching gun animation

## Notes

Bonus (g) would not work perfectly, but the enemy would should when it is besides the cover












