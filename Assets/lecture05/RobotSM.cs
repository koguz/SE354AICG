using UnityEngine;
using System.Collections;

public interface State {
	void Enter(RobotSM robot);
	void Execute(RobotSM robot);
	void Exit(RobotSM robot);
}

public class Search:State {
	public void Enter(RobotSM robot) {
		Vector3 minv = Vector3.zero;
		float minf = float.MaxValue;
		GameObject[] copler = GameObject.FindGameObjectsWithTag("waste");
		for(int i=0;i<copler.Length;i++) {
			float d = (copler[i].transform.position-robot.transform.position).magnitude;
			if(d < minf) {
				minv = copler[i].transform.position;
				minf = d;
			}
		}
		robot.changeState(new WalkToTrash(minv));
	}
	
	public void Execute(RobotSM robot) {
	}
	
	public void Exit(RobotSM robot) {
	}
}

public class WalkToTrash:State {
	public Vector3 ptrash = Vector3.zero;
	public WalkToTrash(Vector3 tp) {
		ptrash = tp;
	}
	public void Enter(RobotSM robot) {
		robot.GetComponent<Animation>().CrossFade("walk");
	}
	public void Execute(RobotSM robot) {
		Vector3 pos = robot.transform.position;
		Vector3 dir = ptrash - pos;
		if(dir.magnitude < 0.1f) {
			GameObject[] copler = GameObject.FindGameObjectsWithTag("waste");
			for(int i=0;i<copler.Length;i++) {
				if((copler[i].transform.position - robot.transform.position).magnitude < 0.1f) {
					
					int px = Random.Range(0, robot.getSize()-1);
					int py = Random.Range(0, robot.getSize()-1);
					GameObject.Instantiate(
						copler[i], new Vector3(px, 0, py), Quaternion.identity);
					GameObject.Destroy(copler[i]);
					break;
				}
			}
			robot.changeState(new Pickup());
		}
		dir.Normalize();
		robot.transform.LookAt(ptrash);
		robot.transform.position += dir * Time.deltaTime;
	}
	public void Exit(RobotSM robot) {
	}
}

public class Pickup:State {
	float tstart;
	public Pickup() {
		tstart = 0;
	}
	
	public void Enter(RobotSM robot) {
		tstart = Time.time;
		robot.GetComponent<Animation>().CrossFade("idle");
	}
	
	public void Execute(RobotSM robot) {
		if (Time.time - tstart > 4.5f) {
			robot.changeState(new WalkToDisposer());
		}
	}
	
	public void Exit(RobotSM robot) {
	}
}

public class WalkToDisposer:State {
	private Vector3 disp = new Vector3(5, 0, 0);
	public void Enter(RobotSM robot) {
		robot.GetComponent<Animation>().CrossFade("walk");
	}
	public void Execute(RobotSM robot) {
		Vector3 dir = disp - robot.transform.position;
		if (dir.magnitude < 0.1f) {
			robot.changeState(new Disposing());
			return;
		}
		dir.Normalize();
		robot.transform.LookAt(disp);
		robot.transform.position += dir * Time.deltaTime;
	}
	public void Exit(RobotSM robot) {
	}
}

public class Disposing:State {
	float tstart;
	public void Enter(RobotSM robot) {
		tstart = Time.time;
		robot.GetComponent<Animation>().CrossFade("charging");
		robot.transform.LookAt(new Vector3(5, 0, -1));
	}
	public void Execute(RobotSM robot) {
		if (Time.time - tstart > 4.5f) {
			robot.changeState(new Search());
		}
	}
	public void Exit(RobotSM robot) {
	}
}

public class RobotSM : MonoBehaviour {
	
	private const int size = 15;
	private int[,] alan = new int[size, size];
	public GameObject[] copler = new GameObject[3];
	private State currentState;
	public int getSize() { return size; }
	// Use this for initialization
	void Start () {
		for(int i=0;i<size;i++)
			for(int j=0;j<size;j++)
				alan[i,j] = 0;
		int cop = 3;
		do {
			int x = Random.Range(0, size-1);
			int y = Random.Range(0, size-1);
			if(alan[x,y] == 0) {
				alan[x,y] = 1;
				cop--;
			}
		} while(cop != 0);
		
		GameObject charger = GameObject.FindGameObjectWithTag("charger");
		charger.transform.position = new Vector3(-1, 0, 5);
		charger.transform.rotation = Quaternion.Euler(0, 90, 0);
		
		GameObject wastebin = GameObject.FindGameObjectWithTag("cop");
		wastebin.transform.position = new Vector3(5, 0, -1);
		
		copler = GameObject.FindGameObjectsWithTag("waste");
		//
		
		GameObject kup = GameObject.CreatePrimitive(PrimitiveType.Cube);
		for(int i=0;i<size;i++) {
			for(int j=0;j<size;j++) {
				Vector3 pos = new Vector3(i, 0, j);
                GameObject g = (GameObject)GameObject.Instantiate(
                        kup,
                        pos,
                        Quaternion.identity);
				g.transform.localScale = new Vector3(0.9f, 0.05f, 0.9f);
				if(alan[i,j] == 1) {
					GameObject.Instantiate(
						copler[Random.Range(0,2)], pos, Quaternion.identity);
				}
			}
		}
		GameObject.DestroyObject(kup);
		for(int i=0;i<3;i++) copler[i].SetActive(false);
		
		changeState(new Search());
	}
	
	// Update is called once per frame
	void Update () {
		if(currentState != null) currentState.Execute(this);
	}
	
	public void changeState(State s) {
		if(currentState != null) {
			currentState.Exit(this);
		}
		currentState = s;
		s.Enter(this);
	}
}
