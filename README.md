# Vonder Timeline Interaction Test

## Overview

This project is a Unity test scene demonstrating:

* NPC interaction
* Dialogue system
* Timeline cutscenes
* Timeline Signals
* Item collection flow
* UI notifications
* Camera transitions using Cinemachine

---

# How to Start the Test Scene

1. Open the project in Unity.
2. Navigate to:

```
Assets/Scenes/Game.unity
```

3. Press **Play** in the Unity Editor.

---

# Gameplay Flow

1. Talk to the NPC.
2. A dialogue sequence will begin.
3. First timeline cutscene is played.
4. After the dialogue is completed, the Coin interactable is spawned/enabled.
5. Follow the item arrow indicator.
6. Collect the Coin.
7. Item-collected notification appears.
8. Talk to NPC again.
9. Second timeline cutscene is played.
10. NPC Dissapear with VFX.

---

# Interactable Objects

## NPC : 
interact for dialogue, it can't be interact if sequence have item condition.
## Coin : 
only spawn when first sequence was finished, interact for unlock next sequence from npc.

---

# Timeline Assets

Timeline assets are located at:

```
Assets/TimelineAssets/
```
---

# Timeline Signals Used

Timeline signals in this scene were used for:
- Show or advance dialogue text.
- Disable or enable player control.
- Switch NPC animation state based on the current dialogue context.

---

# How Dialogue UI is Triggered

```DialogueManager.cs``` was setup when player start sequence (by checking player step index data in ```PlayerInstanceData.cs```)
and inside of ```SequenceInfo.cs``` there is a ```DialogueDataList.cs``` that contains ```DialogueData.cs``` as scriptable object to stored set speaker's name, dialogue and camera followed trigger flag for more immersive dialogue.

and when dialogues were loaded to manager.

the signal receiver now can trigger Advance(); in ```DialogueManager.cs``` to display dialogue and advance to the next dialogue whenever a signal is triggered.

You can see my custom signal receiver ```StartDialogueSignalReceiver.cs```

---

#  How the item arrow and item-collected notification are triggered

```ArrowIndicatorUI.cs``` and ```NotificationManager.cs``` stored in the GameManager (Singleton) so it can be accessed by any class.

and item can triggered those via their own functions like OnEnable(); for arrow indicator can set target on it or Interact(); to called any binding methods when interact this item included notification (this function is defined in the IInteractable interface and implemented by ```NPCInteract.cs``` as well.)

---

#  Know Issues (Unfinishes)
- Error log called sometimes in editor, SerializedObjectNotCreatableException: Object at index 0 is null This is believed to be related to Playable Director references not being properly assigned.
- NPCs do not currently turn to face the player during dialogue sequences.

# Design Observations & Future Improvements
- The current presentation flow feels closer to a tutorial sequence than a production-ready gameplay experience.
- Some Signals overlap with Cinemachine Track functionality, creating redundancy and additional visual clutter in the Timeline.
- The overall Timeline structure in this project could be simplified and organized further to improve maintainability.
- Addressables were included in anticipation of future asset-loading requirements, but they are not currently necessary for the scope of this prototype and may be considered overengineering.

---

# Timestamp Work Breakdown

4/6/2569 [still in work day]

| Time          | Task                           
| ------------- | ------------------------------ 
| 21:00 - 22:00 | Read requirements in pdf                         
| 22:00 - 23:00 | Download Unity 6000.3.15f      
| 23:00 - 23:30 | Roghly design program structure in Diagram (photo below)       
| 23:30 - 00:30 | Setup project, Import Dotween, UniTask, Cinemachine and set GameManager.cs      

<img width="1399" height="655" alt="image" src="https://github.com/user-attachments/assets/3217f3a9-a16f-4b55-8dbb-8b9862c882b0" />


5/6/2569 [still in work day]

| Time          | Task                           
| ------------- | ------------------------------ 
| 21:00 - 22:00 | [Initial] Test and learning how timeline signals & cinemachine track work. ( I hadn't used those much before this. )                        
| 22:00 - 23:00 | [Initial] PlayerInstanceData scripting and adjust how it can handle in-game.   
| 23:00 - 23:30 | [Initial] Sequence and Timeline handling with Script.    
| 23:30 - 00:30 | Start scripting dialogue with testing dialogue. (with claude code help for lean codes, making test tool, and fixing bugs) 

6/6/2569 [weekend]

| Time          | Task                           
| ------------- | ------------------------------ 
| 10:00 - 12:00 | Interaction system and set unity scene.            
| 13:00 - 18:00 | Setting unity scene, adding dialogue data, set timeline_0 and timeline_1, set signals, set object interact.
| 23:45 - 00:00 | Split the changes in git and pushing system and major changes separately to ```origin/develop```.


7/6/2569 [weekend]

| Time          | Task                           
| ------------- | ------------------------------ 
| 00:00 - 01:35 | Split the changes in git and pushing system and major changes separately to origin/develop & fixing bug with polishing some text.
| 03:00 - 03:50 | Fixed dialogue do not supported the advance per signal & writing README.
| 12:00 - 14:00 | Prepared for delivery and wrote the README.

---
