using UnityEngine;

namespace Core
{
    public class Command
    {
        public GameObject Attacker;
        public GameObject Target;

        public Command(GameObject attacker, GameObject target)
        {
            Attacker = attacker;
            Target = target;
        }
    }
}