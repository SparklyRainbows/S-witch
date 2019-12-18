using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {

    [SerializeField]
    private PlayerSelectSpace purpleSelect;
    [SerializeField]
    private PlayerSelectSpace pinkSelect;

    private void Start() {
        StartCoroutine(CheckForNewConnection());
    }

    private IEnumerator CheckForNewConnection() {
        while (true) {
            string[] temp = Input.GetJoystickNames();
            int length = temp.Length - 1;

            if (length == 0) {
                purpleSelect.Disconnect();
                pinkSelect.Disconnect();
            }
            if (length == 1) {
                purpleSelect.Connect();
                pinkSelect.Disconnect();
            }
            if (length == 2) {
                purpleSelect.Connect();
                pinkSelect.Connect();
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
