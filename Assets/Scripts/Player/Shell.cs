using UnityEngine;

public class Shell : MonoBehaviour
{
    public string shellName;
    public float shootingForce;
    public float spread;
    public int pellets;
    public float range;
    public float damagePerPellet;

    public Shell(string n, float sf, float s, int p, float r, float dpp)
    {
        shellName = n;
        shootingForce = sf;
        spread = s;
        pellets = p;
        range = r;
        damagePerPellet = dpp;
    }
}
