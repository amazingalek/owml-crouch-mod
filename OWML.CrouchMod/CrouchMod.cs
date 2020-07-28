using System;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;

namespace OWML.CrouchMod
{
    public class CrouchMod : ModBehaviour
    {
        private PlayerCharacterController _playerCharacterController;
        private DateTime? _crouchStartTime;

        private void Start()
        {
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                _crouchStartTime = DateTime.Now;
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.C) &&
                     !UnityEngine.Input.GetKey(KeyCode.Space))
            {
                _crouchStartTime = null;
                _playerCharacterController.SetValue("_jumpChargeTime", 0);
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.Space))
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