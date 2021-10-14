using OWML.ModHelper;
using OWML.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OWML.CrouchMod
{
	public class CrouchMod : ModBehaviour
	{
		private PlayerCharacterController _playerCharacterController;
		private float? _crouchStartTime;

		public void Start()
		{
			ModHelper.Events.Player.OnPlayerAwake += OnPlayerAwake;
		}

		private void OnPlayerAwake(PlayerBody playerBody)
		{
			_playerCharacterController = playerBody.GetComponent<PlayerCharacterController>();
		}

		public void Update()
		{
			if (_playerCharacterController == null)
			{
				return;
			}
			HandleInput();
			SetCrouchValue();
		}

		private void HandleInput()
		{
			if (Keyboard.current[Key.C].wasPressedThisFrame)
			{
				_crouchStartTime = Time.realtimeSinceStartup;
			}
			else if (Keyboard.current[Key.C].wasReleasedThisFrame &&
					 !OWInput.IsPressed(InputLibrary.jump))
			{
				_crouchStartTime = null;
				_playerCharacterController.SetValue("_jumpChargeTime", 0);
			}
			else if (OWInput.IsNewlyReleased(InputLibrary.jump))
			{
				_crouchStartTime = null;
			}
		}

		private void SetCrouchValue()
		{
			if (_crouchStartTime == null)
			{
				return;
			}
			var jumpChargeTime = Time.realtimeSinceStartup - _crouchStartTime.Value;
			_playerCharacterController.SetValue("_jumpChargeTime", jumpChargeTime);
		}
	}
}
