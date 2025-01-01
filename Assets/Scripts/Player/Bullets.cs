using UnityEngine;

public class Bullets : MonoBehaviour
{
    public Shell[] shells;
    
    public Shell getShell(string name) 
    {
        foreach (Shell shell in shells)
        {
            if (shell.shellName == name)
            {
                return shell;
            }
        }
        return null;
    }
}
