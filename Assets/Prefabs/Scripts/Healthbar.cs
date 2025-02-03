using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Healthbar : BarUI
{
    public Sprite heartImage;
    public float barLength;
    public float barHeight;
    public float health;
    public float maxHealth;
    public float baseHealth;
    public float currentHealth;

    public void setHealth(float hp) {
        SetValue(maxHealth, hp);
    }

}
