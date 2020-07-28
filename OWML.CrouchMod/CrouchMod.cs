using System.Collections.Generic;
using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;

namespace OWML.CrouchMod
{
    public class CrouchMod : ModBehaviour
    {
        private PlayerCharacterController _playerCharacterController;
        private float? _crouchStartTime;
        private IModInputCombination _crouchCombo;

        public override void Configure(IModConfig config)
        {
            if (_crouchCombo != null)
            {
                ModHelper.Input.UnregisterCombination(_crouchCombo);
            }
            var combination = config.GetSettingsValue<string>("Crouch combination");
            _crouchCombo = ModHelper.Input.RegisterCombination(this, "Crouch", combination) ?? TemporaryHack("Crouch");
        }

        private IModInputCombination TemporaryHack(string comboName)
        {
            var comboRegistry = ModHelper.Input.GetValue<Dictionary<long, HashSet<IModInputCombination>>>("_comboRegistry");
            foreach (var foo in comboRegistry)
            {
                foreach (var bar in foo.Value)
                {
                    if (bar.Name == comboName)
                    {
                        return bar;
                    }
                }
            }
            return null;
        }

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
