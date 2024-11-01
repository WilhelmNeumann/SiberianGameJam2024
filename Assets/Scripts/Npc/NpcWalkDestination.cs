using UnityEngine;
using Utils;

namespace Npc
{
    public class NpcWalkDestination: Singleton<NpcWalkDestination>
    {
        public static Vector3 Position => Instance.transform.position;
        
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, .5f);
        }
    }
}