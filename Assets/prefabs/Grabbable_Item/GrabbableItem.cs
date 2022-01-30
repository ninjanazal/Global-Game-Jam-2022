using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
	public enum ItemType{ WEAPON, FINALITEM}


	public ItemType typedItem = ItemType.WEAPON;
    public Sprite weapon_Sprite;

	public float weapon_dmg;


	/// <Summary> Reference to the messageHandler </Summary>
	[SerializeField] private MessageHandler _msg_handle;


	[SerializeField] private SpriteRenderer _renderer;


	private void Start() {
		_renderer.sprite = weapon_Sprite;
		_msg_handle.ToggleCanvas(false);
		_msg_handle.SetMessage("Prime F para apanhar!");

	}


	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player"){
			GameCore.Instance.player.RegistItemInRange(this);
			_msg_handle.ToggleCanvas(true);

		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.tag == "Player"){
			GameCore.Instance.player.RemoveItemInRange(this);
			_msg_handle.ToggleCanvas(false);

		}
	}


	public void activate_item(){
		switch (typedItem)
		{
			case ItemType.WEAPON:
				GameCore.Instance.player.RemoveItemInRange(this);
				GameCore.Instance.player.DefineWeapon(weapon_Sprite, weapon_dmg);
				break;
			
			case ItemType.FINALITEM:
				GameCore.Instance.player.RemoveItemInRange(this);
				GameCore.Instance.viewCore.win_panel.StartEndAnimation();
				break;
		}
		Destroy(this.gameObject);

	}
}
