using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RiseBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		UpdateAlways();
        if (!GameModel.paused) {
			UpdateTick();
		}
	}

	/// <summary>
	/// This method will be called every tick if the game is not paused.
	/// </summary>
	public abstract void UpdateTick();
	/// <summary>
	/// This method will be called every tick, regardless of whether the game is paused.
	/// 
	/// It will be called before UpdateTick();.
	/// </summary>
	public abstract void UpdateAlways();
}
