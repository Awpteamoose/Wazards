using UnityEngine;
using System.Collections;

public class PlayerCastState: StateMachine.State
{
	private PlayerControl pc;
	private Vector3 world_reticle;
	private float t_start;
	private float t_charge;
	private float t_mincharge;
	private float t_charged;
	private Vector3 box_center;
	private bool canTurn;

	//HACK:
	private float vert;
	private float hor;
    private float mode0_distance;
    private Vector3 mode1_lastPosition;
	
	public PlayerCastState(PlayerControl parent)
	{
		pc = parent;
		
		pc.castComponent.reticle = pc.transform.Find("reticle").gameObject;
	}
	public override void Start()
	{
		t_start = Time.time;
		t_charge = pc.castComponent.spellBook.get().secondsToCharge;
		t_mincharge = pc.castComponent.spellBook.get().secondsMinCharge;
		t_charged = 0;
		pc.castComponent.reticle.SetActive(true);
        pc.castComponent.reticle.transform.localScale = new Vector3(Camera.main.orthographicSize / 5f, Camera.main.orthographicSize / 5f, 0);
        if (pc.castComponent.aimMode == 0)
        {
            mode0_distance = pc.castComponent.reticle_distance;
            world_reticle = pc.transform.position + (pc.moveComponent.direction.vector * mode0_distance);
            pc.castComponent.reticle.transform.position = world_reticle;
        }
        else if (pc.castComponent.aimMode == 1)
        {
            pc.castComponent.reticle.transform.parent = null;
            pc.castComponent.reticle.transform.position = pc.transform.position + (pc.moveComponent.direction.vector * pc.castComponent.reticle_distance);
            world_reticle = pc.castComponent.reticle.transform.position;
            mode1_lastPosition = pc.transform.position;
        }
		pc.rigidbody2D.fixedAngle = true;

		//HACK:
		canTurn = false;
		vert = Input.GetAxisRaw(pc.player+" Vertical");
		hor = Input.GetAxisRaw(pc.player+" Horizontal");

		pc.castComponent.bgBar.SetActive(true);
		pc.castComponent.fgBar.transform.localScale = new Vector3(0, pc.castComponent.bgBar.transform.localScale.y, 0);
		pc.castComponent.fgBar.SetActive(true);
		pc.castComponent.pcBar.transform.localScale = new Vector3(pc.castComponent.bgBar.transform.localScale.x*(t_mincharge/t_charge), pc.castComponent.bgBar.transform.localScale.y, 0);
		pc.castComponent.pcBar.SetActive(true);

        pc.StartChant();

		#if UNITY_EDITOR
		//Debug.Log("Cast: Enter");
		#endif
	}
	public override void Update()
	{
		t_charged = Time.time - t_start;
		if (pc.inputComponent.getFireUp(pc.castComponent.spellBook.active+1))
			pc.sm.set(pc.sm.states[PlayerControl.States.Move]);
		if (t_charged >= t_charge)
		{
			pc.castComponent.bgBar.SetActive(false);
			pc.castComponent.fgBar.SetActive(false);
			pc.castComponent.pcBar.SetActive(false);
		}

        pc.castComponent.reticle.transform.localScale = new Vector3(Camera.main.orthographicSize / 5f, Camera.main.orthographicSize / 5f, 0);
        if (pc.castComponent.aimMode == 0)
        {
		    world_reticle = pc.transform.position + (pc.moveComponent.direction.vector * mode0_distance);
		    pc.castComponent.reticle.transform.position = world_reticle;
        }
        else if (pc.castComponent.aimMode == 1)
        {
            //TODO: move the reticle if aimMode is 1
            world_reticle = pc.castComponent.reticle.transform.position;
        }
		pc.castComponent.fgBar.transform.localScale = new Vector3(((Time.time - t_start)/t_charge*pc.castComponent.bgBar.transform.localScale.x), pc.castComponent.fgBar.transform.localScale.y, 0);
	}

	public override void FixedUpdate()
	{
		//TODO: move all this shit into a component, make calls to it
		float newvert = Input.GetAxisRaw(pc.player+" Vertical");
		float newhor = Input.GetAxisRaw(pc.player+" Horizontal");
		if (!canTurn && (newvert != vert || newhor != hor))
		{
			canTurn = true;
		}
        else if (canTurn)
		{
            //Asteroids-like reticle movement
            if (pc.castComponent.aimMode == 0)
            {
                if (newvert != 0)
                {
                    mode0_distance += newvert * 0.1f * pc.castComponent.reticle_speed;
                    if (mode0_distance < pc.castComponent.reticle_minimumDistance) mode0_distance = pc.castComponent.reticle_minimumDistance;
                }

                if (newhor != 0)
                {
                    pc.moveComponent.direction.angle -= newhor * (10f / mode0_distance) * pc.castComponent.reticle_speed;
                }
            }

            //Orthogonal reticle movement
            else if (pc.castComponent.aimMode == 1)
            {
                if (mode1_lastPosition != pc.transform.position)
                {
                    pc.castComponent.reticle.transform.position = pc.castComponent.reticle.transform.position + pc.transform.position - mode1_lastPosition;
                    mode1_lastPosition = pc.transform.position;
                }
                pc.castComponent.reticle.rigidbody2D.AddForce(new Vector2(newhor * pc.castComponent.reticle_speed * 80f, newvert * pc.castComponent.reticle_speed * 80f));
                pc.moveComponent.direction.vector = pc.castComponent.reticle.transform.position - pc.transform.position;
            }
            pc.rigidbody2D.MoveRotation(pc.moveComponent.direction.angle);
		}
	}
	public override void OnGUI()
	{
	}
	public override void Exit()
	{
		if (t_charged > pc.castComponent.spellBook.get().secondsMinCharge)
			pc.castComponent.cast(t_charged>=t_charge, world_reticle, pc);
		pc.rigidbody2D.fixedAngle = false;
		
		pc.castComponent.bgBar.SetActive(false);
		pc.castComponent.fgBar.SetActive(false);
		pc.castComponent.pcBar.SetActive(false);
		pc.castComponent.reticle.SetActive(false);

        pc.StopChant();

		#if UNITY_EDITOR
		//Debug.Log("Cast: Exit");
		#endif
	}
}