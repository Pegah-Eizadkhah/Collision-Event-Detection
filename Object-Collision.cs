/*
Pegah Eizadkhah
This C# script is part of a larger set of scripts for a Gaming project.

In this game the player catches & guides a fish (attached to a hook) across a "highway" trying not to collide
with the enemies (Penguines, other fishes) as the fish is "reeled" back up to the top,
using the arrow keys for control. The enemies move horizontally, similar to how cars move
on the highway and the fish moves vertically, and must move across two layers of
highway to get to its home on the other side. There are some collectibles that spawn
randomly in the fish's path. The challenge of this arcarde-style
game is to not let the fish collide with enemies using good timing and reaction time.

This specific script controls the collision events in the game,
(i.e what happens when the fish collides with an enemy)
This script uses some functions defined in the Unity game engine.

This code is owned by Pegah Eizadkhah and is not meant for redistribution.
*/

using UnityEngine;
using System.Collections;

public class fishCollider : MonoBehaviour
{
    //declare game objects used in collision
    public Transform HookAndLineParent;
    public GameObject line;
    //start position of the hook
    public Vector3 hookStartPos;
    //Start position of the hook line (vector)
    public Vector3 lineStartPos;
    //Position renderer in Unity
    public Renderer rend;
    //This is the game object of "Fish Attached to Hook"
    public GameObject hookedFish = null;

    //This function resets the hook
    public void resetHook ()
    {
	//resest the hook position when player loses
        this.line.transform.position = this.lineStartPos;
        this.transform.position = this.hookStartPos;
    }

   //instantiate game objects
    void Start()
    {
	//start position renderer
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        hookStartPos = this.transform.position;
        lineStartPos = line.transform.position;
    }

    //OnTrigger function in Unity detects object colision
    void OnTriggerEnter(Collider other)
    {
	//Fish is attached to the hook, nothing to do
        if (other.tag.Equals("hook"))
        {
            return;
        }
	//If hook hits the ceiling (top of the game) with a caught fish, the game is won
        else if (other.tag.Equals("ceiling") && hookedFish != null)
        {
            screenOverlays.SetWon(true);
            Destroy(hookedFish);
            hookedFish = null; //reset game object
        }
	//If hook hits an enemy fish, the game is lost
        else if ( other.tag.Equals("Fish"))
		{
            screenOverlays.addLife(-1);
            rend.enabled = true;
            line.transform.position = lineStartPos;
            this.transform.position = hookStartPos;
            Destroy(other.gameObject);
            Destroy(hookedFish);
        }
	//If hooks hits a randomly generated collectible in its path, then
	//destroy the collectible game object, and keep going
	else if(other.tag.Equals("Collectible")){

		//destroy hook if there is no fish on the hook
		if(hookedFish == null)
		{
		Destroy(hookedFish);
		}
		
		//Collectible replaces fish on the hook for a second
		GameObject collectFish = other.gameObject;
		//collectible object is destroyed
		Destroy(collectFish.GetComponent<FishMover>());
		collectFish.GetComponent<BoxCollider>().isTrigger = true;
		//destroy the fish on the hook game object
		Destroy(other.gameObject);
		//reset the fish on the hook game object, and set the position to the
		//current position of the player
		GameObject fish = Instantiate(collectFish, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
		//reset the collision component on the fish game object
		fish.AddComponent<enemyCollider>();
		fish.gameObject.tag = "hook";
                fish.transform.SetParent(HookAndLineParent);
		//hookedfish game object is fish again (as oppposed to the collectibel)
                hookedFish = fish;
		}
        Debug.Log("hook collided with: " + other);
    }
}
