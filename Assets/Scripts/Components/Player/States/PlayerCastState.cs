using UnityEngine;
using System.Collections;

public class PlayerCastState : StateMachine.State
{
	private PlayerControl pc;
	private Vector3 world_reticle;
	private Spell spell;
	private bool canTurn;

	//HACK:
	private float vert;
	private float hor;
	private float mode0_distance;
	private Vector3 mode1_lastPosition;


	
	public PlayerCastState(PlayerControl parent)
	{
		pc = parent;
		
		pc.castComponent.reticle = pc.transform.FindChild("reticle").gameObject;
	}
	public override void Start()
	{
		pc.castComponent.reticle.SetActive(true);
		pc.castComponent.reticle.transform.localScale = new Vector3(Camera.main.orthographicSize / 5f, Camera.main.orthographicSize / 5f, 1);
		if (pc.castComponent.altAimMode)
		{
			mode0_distance = pc.castComponent.reticle_distance;
			world_reticle = pc.transform.position + (pc.moveComponent.direction.vector * mode0_distance);
			pc.castComponent.reticle.transform.position = world_reticle;
		}
		else if (!pc.castComponent.altAimMode)
		{
			pc.castComponent.reticle.transform.parent = null;
			pc.castComponent.reticle.transform.position = pc.transform.position + (pc.moveComponent.direction.vector * pc.castComponent.reticle_distance);
			world_reticle = pc.castComponent.reticle.transform.position;
			mode1_lastPosition = pc.transform.position;
		}
		pc.moveComponent.rotation_factor *= 4f;

		//HACK:
		canTurn = false;
		vert = pc.inputComponent.getVertical();
		hor = pc.inputComponent.getHorizontal();

		spell = pc.castComponent.spellBook.Get();
		spell.Begin(world_reticle);

		if (spell.chargeDuration > 0)
		{
			pc.castComponent.bgBar.SetActive(true);
			pc.castComponent.fgBar.transform.localScale = new Vector3(0, pc.castComponent.bgBar.transform.localScale.y, 0);
			pc.castComponent.fgBar.SetActive(true);

			pc.castComponent.pcBar.transform.localScale = new Vector3(pc.castComponent.bgBar.transform.localScale.x * (spell.min_chargeDuration / spell.chargeDuration), pc.castComponent.bgBar.transform.localScale.y, 0);
			pc.castComponent.pcBar.SetActive(true);
		}

		#if UNITY_EDITOR
		//Debug.Log("Cast: Enter");
		#endif
	}
	public override void Update()
	{
		spell.Chant(world_reticle);
		if (pc.inputComponent.getFireUp(pc.castComponent.spellBook.active+1))
			pc.sm.set(pc.sm.states[PlayerControl.States.Move]);
		for (int i = 1; i <= 4; i++)
		{
			if ((i != pc.castComponent.spellBook.active + 1) && pc.inputComponent.getFireUp(i))
			{
				spell.charge = -1;
				pc.castComponent.mana -= 15 * pc.castComponent.mod_manacost;
				pc.sm.set(pc.sm.states[PlayerControl.States.Move]);
			}
		}
		if (spell.charge >= spell.chargeDuration && !spell.charged)
		{
			pc.castComponent.bgBar.SetActive(false);
			pc.castComponent.fgBar.SetActive(false);
			pc.castComponent.pcBar.SetActive(false);
			spell.Charge(world_reticle);
		}

		pc.castComponent.reticle.transform.localScale = new Vector3(Camera.main.orthographicSize / 5f, Camera.main.orthographicSize / 5f, 0);
		if (pc.castComponent.altAimMode)
		{
			world_reticle = pc.transform.position + (pc.moveComponent.direction.vector * mode0_distance);
			pc.castComponent.reticle.transform.position = world_reticle;
		}
		else if (!pc.castComponent.altAimMode)
		{
			world_reticle = pc.castComponent.reticle.transform.position;
		}
		if (spell.chargeDuration > 0)
			pc.castComponent.fgBar.transform.localScale = new Vector3((spell.charge / spell.chargeDuration * pc.castComponent.bgBar.transform.localScale.x), pc.castComponent.fgBar.transform.localScale.y, 0);
		else
			pc.castComponent.fgBar.transform.localScale = Vector3.zero;
	}

	public override void FixedUpdate()
	{
		//TODO: move all this shit into a component, make calls to it
		float newvert = pc.inputComponent.getVertical(); ;
		float newhor = pc.inputComponent.getHorizontal(); ;
		if (!canTurn && (newvert != vert || newhor != hor))
		{
			canTurn = true;
		}
		else if (canTurn)
		{
			//Asteroids-like reticle movement
			if (pc.castComponent.altAimMode)
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
			else if (!pc.castComponent.altAimMode)
			{
				pc.castComponent.reticle.GetComponent<Rigidbody2D>().AddForce(new Vector2(newhor * pc.castComponent.reticle_speed * 80f, newvert * pc.castComponent.reticle_speed * 80f));
				pc.moveComponent.direction.vector = pc.castComponent.reticle.transform.position - pc.transform.position;
			}
		}

		if (!pc.castComponent.altAimMode && mode1_lastPosition != pc.transform.position)
		{
			pc.castComponent.reticle.transform.position = pc.castComponent.reticle.transform.position + pc.transform.position - mode1_lastPosition;
			mode1_lastPosition = pc.transform.position;
		}
	}
	public override void OnGUI()
	{
	}
	public override void Exit()
	{
		spell.End(world_reticle);
		pc.moveComponent.rotation_factor *= 0.25f;
		
		pc.castComponent.bgBar.SetActive(false);
		pc.castComponent.fgBar.SetActive(false);
		pc.castComponent.pcBar.SetActive(false);
		pc.castComponent.reticle.SetActive(false);

		if (pc.ultimate.active && !pc.ultimate.expired)
			Camera.main.Shake(0.1f);

		#if UNITY_EDITOR
		//Debug.Log("Cast: Exit");
		#endif
	}
}