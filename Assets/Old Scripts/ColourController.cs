<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourController : MonoBehaviour {
    public PlayerController player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other){
        player.colourStatus = 1;
        Debug.Log(player.colourStatus);
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourController : MonoBehaviour {
    public PlayerController player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other){
        player.colourStatus = 1;
        Debug.Log(player.colourStatus);
    }
}
>>>>>>> 7cb96a7c3d01e6536c2b1b3a7f2517be11a0872d
