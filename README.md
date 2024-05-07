# Submerged Surveyor

### Name:
Eoghan O'Reilly
### Student Number:
C21430996

### Class Group:
TU984 - Game Design
### Video:

[![YouTube](THUMBNAIL LINK)](VIDEO LINK)

### Description of the Project
This is a game in which the player plays as a robot in some abandoned underwater structure. The game mainly involves walking around or observing the various behaviours of the fish outside the structure. 

### Instructions For Use
#### As the robot:
- Walk around using either the arrow keys or WASD. 

- Jump with space. 

- To pick up certain objects, use left click or press E when near them.

- To throw the object, click again. 

- Any held object can be dropped with E or by right-clicking.

#### Environment Settings
The environment settings menu is opened or closed by pressing Esc.

It contains several buttons with different functionalities.

**Bugs**

This opens the bug settings menu. From here, these parameters can be edited:

- **Max Bug Count** - Handles the maximum amount of bugs that can spawn at any one time. Moving this below or to the current amount of spawned bugs will stop new bugs from spawning. Should one die, it will not be replaced until the amount of bugs goes below the maximum once more.
- **Bugs Die From Touching Lights?** - Allows bugs to die from touching lights, in a similar vein to a bug zapper.
- **Kill All Bugs** - Self explanatory, bugs will respawn if the max count is not 0.

**Fish**

This opens the fish settings menu. From here, these parameters can be edited:

- **Max Orange Fish** - Handles orange fish spawns.
- **Max Red Fish** - Handles red fish spawns.
- **Max Schooling Fish** - Handles purple fish spawns.
- **Max School Leaders** - Handles leader fish spawns. 
- **Fish Can Eat?** - Allows red fish to kill other fish.
- **Fish Can Starve?** - Allows red fish to die after a certain period of time. Will also stop them from hunting other fish.
- **Fish Always Hungry?** - Makes red fish constantly hungry, causing them to forever hunt down and kill other fish, unable to truly satisfy their hunger.
- **Kill All Prey Fish** - Instantly kills all orange, purple and leader fish.
- **Kill All Predator Fish** - Instantly kills all red fish.

**Predator and Prey Buttons**

These buttons focus the camera on a random fish of the chosen type. You can use the X or Z keys to cycle between all living individuals of that fish type.

**Shutdown**

This quits the game.


### How It Works
The game works in two halves. In one half, the player is a robot walking around inside a submerged structure. The other half is direct observation of the fish outside the structure.

**Robot / The Player**

The robot is controlled with a character controller and the Unity New Input System. It has view bobbing via a Cinemachine Noise Preset. The camera is rotated using Cinemachine Hard Look To Target with the aim set to POV. They can also jump and pick up and throw objects.

**Fish Camera**

The fish camera has similar settings to the Player, only the Body settings are set to Framing Transposer. The Follow and Aim targets are assigned programmatically in the Camera Manager script. When either the predator or prey button is clicked, the camera's priority changes to be above that of the player's camera. It picks a random fish from one of two arrays and follows it around. The camera can be rotated, and the fish being followed can be changed with **Z** or **X**.

**Bugs**

Bugs are controlled using a boid, along with steering behaviours. Each bug has four states it can be in.

- **Wandering** uses the Noise Wander behaviour to wander around the structure. Obstacle Avoidance is used to ensure the bugs do not pass through walls.
- **Seek Light** uses the Seek behaviour to pick out a random light object from the scene. Obstacle Avoidance prevents it from touching the light, causing it to sort of hang around near the light. All bugs have a 1 in 10 chance to fly directly into a light and die if this is chosen.
- **Follow Player** uses the Pursue behaviour to lock on to the player's position and swarm around them.
- **Crashing Into Window** uses the Seek behaviour to find a random window in the scene and crash into it. These bugs are stupid.

All bugs switch states every 15-75 seconds. They will also die after 30-120 seconds.

**Fish**

Fish are also controlled using a boid and steering behaviours.

- **Orange Fish** have the Noise Wander and Flee behaviours. These fish will wander around, unless they get too close to a red fish. Should this happen, they will flee for 10 seconds before wandering again.
- **Red Fish** have the Noise Wander and Pursue behaviours. They have a stat for hunger that slowly goes down over time. If it reaches 60, they will begin hunting for a random prey fish to eat. If they get too close, they will eat the other fish and it will die. If they fail to catch another fish, they will starve to death.
- **Schooling Fish** have the Noise Wander, Flee and Offset Pursue behaviours. They will pick a random leader fish and follow it, only breaking formation to flee from a red fish. If there is no leader fish, they will wander around as if they were orange fish.
- **Leader Fish** have the Noise Wander, Flee and Pursue behaviours. They are followed by the schooling fish and will wander around, having a small chance to seek out and follow a random orange fish. They also flee from red fish.

In addition to this, all fish have the Obstacle Avoidance and Harmonic Behaviours, as well as a spine animator attached.
### List of Classes/Assets in the Project

| Class/Asset           | Source                                                                                                                                                                                       |
|-----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| BugAI.cs              | Self written                                                                                                                                                                                 |
| FishAI.cs             | Self written                                                                                                                                                                                 |
| SchoolingFishAI.cs    | Self written                                                                                                                                                                                 |	
| LeaderAI.cs           | Self written                                                                                                                                                                                 |
| Outline Shader        | [CG Smoothie - YouTube](https://www.youtube.com/watch?v=Bm6Bmcjd1Mw)                                                                                                                         |
| CountSlider.cs        | Self written                                                                                                                                                                                 |
| FMODEvents.cs         | Self written                                                                                                                                                                                 |
| AudioManager.cs       | Self written                                                                                                                                                                                 |
| EyeHandler.cs         | Self written                                                                                                                                                                                 |
| FinSwoosher.cs        | Self written                                                                                                                                                                                 |
| GlobalVariables.cs    | Self written                                                                                                                                                                                 |
| OpenClose.cs          | Self written                                                                                                                                                                                 |
| PauseGame.cs          | Self written                                                                                                                                                                                 |
| PerlinNoise.cs        | Modified from example project                                                                                                                                                                |
| PlayerController.cs   | Frankenstein of [samyam - YouTube](https://www.youtube.com/watch?v=we4CGmkPQ6Q), [Unity Documentation](https://docs.unity3d.com/ScriptReference/CharacterController.Move.html), and self written code |
| PlayerInputManager.cs | Modified from [samyam - YouTube](https://www.youtube.com/watch?v=we4CGmkPQ6Q)                                                                                                                |
| ShowMyVariable.cs     | Self written                                                                                                                                                                                 |
| Spawner.cs            | Self written                                                                                                                                                                                 |
| ToggleFunc.cs         | Self written                                                                                                                                                                                 |
| WingsAnim.cs          | Self written                                                                                                                                                                                 |
| BugAnimDestroy.cs     | Self written                                                                                                                                                                                 |
| ButtonSound.cs        | Self written                                                                                                                                                                                 |
| CameraManager.cs      | Self written                                                                                                                                                                                 |
| DeskSound.cs          | Self written                                                                                                                                                                                 |
| DestroyOtherStuff.cs  | Self written                                                                                                                                                                                 |
| PickUp.cs             | Self written                                                                                                                                                                                 |
| RandomText.cs         | Self written                                                                                                                                                                                 |
| NoiseWander.cs        | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| Boid.cs               | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| Flee.cs               | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| Pursue.cs             | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| Seek.cs               | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| SpineAnimator.cs      | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| SteeringBehaviour.cs  | [skooter500 - GitHub](https://github.com/skooter500)                                                                                                                                         |
| Quit.cs               | Self written                                                                                                                                                                                 |
| DigitalDream.ttf      | [PizzaDude - 1001 Fonts](https://www.1001fonts.com/digital-dream-font.html)                                                                                                                  |
| SUBWT__.ttf           |[K_Style - 1001 Fonts](https://www.1001fonts.com/subway-ticker-font.html)
### List of Sound Effects

| Sound Effect                                                | Source                                                                 |
|-------------------------------------------------------------|------------------------------------------------------------------------|
| 504641__fission9__underwater-ambience.wav                   | [Fission9 - Freesound](https://freesound.org/s/504641/)                | 
| 701704__stavsounds__ui-submit.wav                           | [StavSounds - Freesound](https://freesound.org/s/701704/)              |
| 343006__zenithinfinitivestudios__bad11.wav                  | [ZenithInfinitiveStudios - Freesound](https://freesound.org/s/343006/) |
| 264446__kickhat__open-button-1.wav                          | [kickhat - Freesound](https://freesound.org/s/264446/)                 |
| 368557__indianaparkwars__mechanical-whirring.mp3            | [IndianaParkWars - Freesound](https://freesound.org/s/368557/)         |
|256575__sjnewton__thump.mp3                                  | [sjnewton - Freesound](https://freesound.org/s/266575/)                |
| 372877__hakren__iron-wood-slam.wav                          | [Hakren - Freesound](https://freesound.org/s/372877/)                  |
| 93781__rdneubauer__metal-door-slamming-1.wav                | [rdneubauer - Freesound](https://freesound.org/s/93781/)               |
| 466283__shatterstars__boat-footsteps-soft.wav               | [shatterstars - Freesound](https://freesound.org/s/466283/)            |
| 346373__denao270__throwing-whip-effect.wav                  | [denao270 - Freesound](https://freesound.org/s/346373/)                |
| 731434__geoff-bremner-audio__abandoned-industrial-water-basement-air-tone.wav | [Geoff-Bremner-Audio - Freesound](https://freesound.org/s/731434)      |

### References

* [Unity Reference](https://docs.unity3d.com/ScriptReference/index.html)
* [Unity Forums](https://forum.unity.com/)
* [Unity Discussions](https://discussions.unity.com/)

### What I Am Most Proud Of:
I am most proud of the overall feel of the game. I spent a lot of time making sure that the game had the feel I was going for. The initial idea for the level came to me in a dream, and I tried my best to make something close to it a reality in the game. All the game's models are made of primitive shapes, the most complex being a beveled cube. I think I conveyed the feeling of being alone well. I am also proud of getting everything to work. There were many problems I had when developing, especially with the character controller script, so I am glad I was able to iron out most of the issues. In addition to this, getting the fish to interact with each other was very fun. 

### What I Learned:
I learned a lot about some of the dos and don'ts of creating state machines. A problem I had was that I could not change any states at runtime due to the fact they were being updated every frame. I must think of a better way to implement these states without causing a problem such as this. Another thing I learned was about the dangers of feature creep when developing a game. I spent a lot of time developing features that I had not intended to be a part of the experience from the beginning. The ability to throw and pick up objects was a last minute addition, that almost did not work. It is always important to focus on the core features of the game first and foremost. One more example of this was FMOD integration. I was told to use it for another project, and as practice, I decided to implement it into this game. I did it all in one night, and nearly lost a lot of progress due to a merge conflict. Thankfully, everything worked out fine in the end.

### Reference Material:
[![YouTube](https://i3.ytimg.com/vi/QK0LDMSg-Ks/maxresdefault.jpg)](https://www.youtube.com/watch?v=QK0LDMSg-Ks)
[![YouTube](https://i3.ytimg.com/vi/cC9r0jHF-Fw/maxresdefault.jpg)](https://www.youtube.com/watch?v=cC9r0jHF-Fw)