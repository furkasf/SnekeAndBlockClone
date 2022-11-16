using System.Collections;
using System.Runtime.Remoting.Contexts;
using Level;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Trigger : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("First"))
            {
                LevelSignals.onGetNextLevel();
            }
        }
    }
}