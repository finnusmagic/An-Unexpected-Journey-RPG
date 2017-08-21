using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class EditorPathScript : MonoBehaviour
    {
        public Color rayColor = Color.yellow;
        public List<Transform> path_objs = new List<Transform>();
        Transform[] theArray;

        private void OnDrawGizmos()
        {
            Gizmos.color = rayColor;
            theArray = GetComponentsInChildren<Transform>();
            path_objs.Clear();

            Vector3 firstPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firstPosition;

            foreach (Transform path_obj in theArray)
            {
                Gizmos.DrawSphere(path_obj.position, .2f);
                Gizmos.DrawLine(previousPosition, path_obj.position);
                previousPosition = path_obj.position;

                if (path_obj != null)
                {
                    path_objs.Add(path_obj);
                }
            }
        }
    }
}

