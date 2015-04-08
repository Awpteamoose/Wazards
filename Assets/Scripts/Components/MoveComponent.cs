using UnityEngine;
using System.Collections;

public class MoveComponent: MonoBehaviour
{
	public float speed;
	public float rotation_factor;

	public float mod_speed = 1f;

	public Direction direction;
	public PlayerControl owner { get; set; }

	void Awake()
	{
		direction.angle = GetComponent<Rigidbody2D>().rotation;
		owner = GetComponent<PlayerControl>();
	}

	void FixedUpdate()
	{
		/*
		 * owner.rigidbody.rotation - текущий угол персонажа
		 * direction.angle - угол, в который я хочу повернуться
		 * difference - правый угол вокруг начала оси координат между angle и direction.angle. Правый в смысле, что угол между 45 и -45 будет -90 т.к. кратчайшим путём будет поворот по часовой стрелке (потому минус) на 90 градусов.
		 * Mathf.Rad2Deg и Mathf.Deg2Rad - это константы, которые нужны чтоб превращать углы из градусов в радианы и наоборот. Например, 90 * Mathf.Deg2Rad == 1.57...
		 * Mathf.Atan2(y, x) - арктангенс, тригорометрическая функция, которая по факту возвращет угол между осью Х и вектором (x, y) - http://i.stack.imgur.com/xQiWG.png
		 * Mathf.Sin в данном контексте возвращает мне Y - компоненту моего вектора разворота.
		 * Mathf.Cos делает тоже самое, но для X - компоненты.
		 * Если взять единичный круг (http://www.geocities.ws/petrinamaher/UnitCircle.GIF), то поворот с 0 до 90 градусов - это переход с вектора (cos(0), sin(0)) => (1, 0) на вектор (sin(90), cos(90)) => (0, 1).
		 * Моя задача заключается в том, чтоб узнать в какую сторону поворачиваться короче всего, для этого мне понадобился арк-тангенс.
		 */
		float difference =
			Mathf.Rad2Deg * Mathf.Atan2(
				Mathf.Sin(Mathf.Deg2Rad * direction.angle - Mathf.Deg2Rad * owner.rigidbody.rotation),
				Mathf.Cos(Mathf.Deg2Rad * direction.angle - Mathf.Deg2Rad * owner.rigidbody.rotation)
			);
		owner.rigidbody.angularVelocity = difference * rotation_factor;

		if (owner.rigidbody.velocity.magnitude > 1f)
		{
			//owner.animator.SetBool("Moving", true);
		}
		else
		{
			//owner.animator.SetBool("Moving", false);
		}
	}
}