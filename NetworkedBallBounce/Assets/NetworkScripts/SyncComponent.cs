using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LEXRCN.Networking {
    public abstract class SyncComponent : MonoBehaviour
    {

        public virtual async Task SendData(int ID)
        {

        }
    }
}


