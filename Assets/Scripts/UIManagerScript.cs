using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

    [SerializeField] private Image jumpChargeImage;

    public void ChangeChargeValue(float value) {
        float shift = 1 - (1 * value);
        this.jumpChargeImage.color = new Color(1f, 1f * shift, 1f * shift);
    }
}
