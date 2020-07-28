using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;

namespace OWML.CrouchMod
{
    public class CrouchMod : ModBehaviour
    {
        private const string Combination = "C";

        private PlayerCharacterController _playerCharacterController;
        private float? _crouchStartTime;
        private IModInputCombination _crouchCombo;

        private void Start()
        {
            _crouchCombo = ModHelper.Input.RegisterCombination(this, "Crouch", Combination);

            ModHelper.Events.Player.OnPlayerAwake += OnPlayerAwake;
        }

        private void OnPlayerAwake(PlayerBody playerBody)
        {
            _playerCharacterController = playerBody.GetComponent<PlayerCharacterController>();
        }

        private void Update()
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
            if (ModHelper.Input.IsNewlyPressedExact(_crouchCombo))
            {
                _crouchStartTime = Time.realtimeSinceStartup;
            }
            else if (ModHelper.Input.WasNewlyReleased(_crouchCombo) &&
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