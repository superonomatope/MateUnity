﻿using UnityEngine;
using System.Collections;

namespace M8.UIModal {
    using Dialogs;

    public struct DialogUtil {
        public const string characterDialogRef = "character";
        public const string confirmDialogRef = "confirm";
        public const string messageDialogRef = "message";

        public static void CharacterDialog(string text, string aName = null, string portraitSpriteRef = null, string[] choices = null) {
            Manager.instance.ModalOpen(characterDialogRef, 
                new ParamArg(CharacterDialogBase.paramText, text),
                new ParamArg(CharacterDialogBase.paramName, aName),
                new ParamArg(CharacterDialogBase.paramSpriteRef, portraitSpriteRef),
                new ParamArg(CharacterDialogBase.paramChoiceArray, choices));
        }

        public static void Confirm(ConfirmDialogBase.OnConfirm aCallback) {
            ConfirmDialogBase.Open(confirmDialogRef, null, null, aCallback);
        }

        public static void Confirm(string aTitle, string aText, ConfirmDialogBase.OnConfirm aCallback) {
            ConfirmDialogBase.Open(confirmDialogRef, aTitle, aText, aCallback);
        }

        public static void Message(string aTitle, string aText, MessageDialogBase.OnClick aCallback) {
            MessageDialogBase.Open(messageDialogRef, aTitle, aText, aCallback);
        }
    }
}