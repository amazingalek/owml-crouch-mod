using System;
using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;

namespace OWML.CrouchMod
{
    public class CrouchMod : ModBehaviour
    {
        private PlayerCharacterController _playerCharacterController;
        private DateTime? _crouchStartTime;

        private IModInputCombination _crouchCombo;

        private void Start()
        {
            _crouchCombo = ModHelper.Input.RegisterCombination(this, "Crouch", "C");

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
            if (ModHelper.Input.IsNewlyPressed(_crouchCombo))
            {
                _crouchStartTime = DateTime.Now;
            }
            else if (ModHelper.Input.WasNewlyReleased(_crouchCombo) &&
                     !Input.GetKey(KeyCode.Space))
            {
                _crouchStartTime = null;
                _playerCharacterController.SetValue("_jumpChargeTime", 0);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
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
            var jumpChargeTime = (float)(DateTime.Now - _crouchStartTime.Value).TotalSeconds;
            _playerCharacterController.SetValue("_jumpChargeTime", jumpChargeTime);
        }

    }
}