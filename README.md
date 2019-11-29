# <center> Homework 3 Report </center>


## Task (a) to (e)

### (a) Creating the basic enemy character
**Set targets :**
Create targets and turn the enemy direction to target for each unique pair of target and enemy

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
Use time control based on Time.deltaTime, randomize the shooting vertors with small angles

**Death animation :**
Add laying down animation and change camera position to the final location

### (d) Player shooting and health 
**Player’s health UI:**
Add Text box and add combine text object with health in bold red font

**Gun independent :**
Add gun a rigidbody component and add collider to gun, and make it an independent object without a parent.

### (e) Creating the environment 

**Create room :**
Duplicate walls and floors, change the texture scale of walls and floors

**Escape door:**
Add escape door and add Collider to door. Restart the game after 10 seconds when collide with the door

## Bonus

### (f) Ammo supply 

Add ammo object, get ammo box position and add player's remaining number of bullets when coming close to the box

### (g) Enemies getting cover 

When a cover a available, enemy 1 will hid around it (in room 1) and shoot the player from there; enemy 2 and 3 doesn't have cover and will not take cover

### (h) Detecting body part hits 

Set layers to Enemy and reduce health according to body part

### (i) Swapping guns 

Activate game object while switching guns, add switching gun animation

## Notes

Bonus (g) would not work perfectly, but the enemy would should when it is besides the cover

Bonus (i)'s anaimation is from Mixamo Rifle Put Away. When you first enter/ load the game, the animation may not feel as smooth, though it will improve over time. Also, the animation will put away the gun and then bring it back, if you were wearing the headset, maybe need to look down/ round to see the put away/ pull out motion.












