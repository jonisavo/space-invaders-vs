using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class CoverPiece : MonoBehaviour
    {
        private Cover _cover;

        private int _id;
        
        #region MonoBehaviour Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet") && !other.gameObject.CompareTag("EnemyBullet"))
                return;
            
            Destroy(other.gameObject);

            if (!_cover.photonView.IsMine) return;

            _cover.photonView.RPC("DestroyPiece", RpcTarget.AllBuffered, _id);
        }
        
        #endregion

        public void InitializeCoverPiece(int id, Cover coverComponent)
        {
            _id = id;
            _cover = coverComponent;
        }
    }
}
