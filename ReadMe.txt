

Part 0: Reloading was completed in the 'ReloadEvent fnc'. Hand mag is triggered

in animation by trigger event number, appears while reloading and disappears after.

Bullet hole and shot sound were implemented in the addEffects fnc by instantiating Gameobjects and promptly destroying them afterwards.



Part 1.1.1
The walkpaths for the enemies are implemented in the 'Walk' function. Enemies are given a red appearance after creating new material. If player not detected,
enemy keeps cycling through the walkpath targets.

Part 1.1.2
Enemy detecting player is implemented in the "Detect" function. Distance and angle between enemy and player is calculated. If distance is less than 20 and
angle less than 45, the player is detected. Additionally, if enemy is shot by player, the player is detected. Once player is detected the enemy runs to a
a distance of 10 from the player. If player moves, the enemy maintains this distance.

Part 1.1.3
This is implemented partly in the last part of the "Detect" function, the Update and the "Firing" function. Once the enemy is distance of 10 from the player,
it gets triggered to go in the 'standby' position in "Detect". Once the "Update" checks if this positon is trigerred and if it has been less that .2 seconds
from the previous bullet(max of 5 per second), it calls the "Firing" function. The shooting is carried out by using RayCast. An additional offset is added to
the raycast vector since it originall kept pointing the left of player. Moreover, a 'bullet spread' is implemented to make sure that not every bullet hits
the player. This is done by adding a random value within a small range to the raycast vector. NOTE: the accuracy is not exactly 20%, naturally it is more
accurate when the player is closer to the enemy and less accurate when player is far. Additionally, when player health reaches 0, the death animation is run
bu calling "Death" function in  Gun_script and the camera goes to the final camera position described in the "Character Movement" script that was given.
Player health is also controlled by the "Firing" function. Health decreases based on amounts described in the Bonus part.

Part 1.1.4
Player shooting enemy is implemented in the "ShotDetection" script. This script also handles decreasing the enemy's health when it gets shot. Once enemy's
health reaches 0 the "Death" fnc in the enemy script is called by Update in the enemy script, followed by a call to the "sep_gun" fnc which separates the gun
being a child of the enemy, and adds a rigid body and collider to the gun. Player health is added to UI and updated in the players "Update" function.
Reduction in player health is controlld by the enemy's "Firing" function, in accordance to values mentioned in the bonus part.

Part 1.1.5
The game environment consists of 3 rooms and 3 enemies. An escape door is implemented in the last room. When the player reaches within a certain distance of 
this door, the "DoorTrig" function the CharacterMovement script is called. This moves the camera back to its final position, and restarts the game in 10 
seconds. This also happens when the player dies

Part 1.2.1
A ammo crate was added to the 2nd room. If the player reaches within a certain distance of this crate, the player's ammo is refilled. This is implemented
in the "Update" function of Gun_Script

Part 1.2.2
Not Implemented

Part 1.2.3
This is implemented in the "Firing" function in the enemy scripts. Players health is reduced based on which part the bullet hits

Part 1.2.4
Not Implemented
