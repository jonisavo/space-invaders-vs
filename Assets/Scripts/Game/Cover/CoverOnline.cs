using System;
using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class CoverOnline : Cover
    {
        [NonSerialized]
        public PhotonView PhotonView;

        protected override void Awake()
        {
            base.Awake();
            PhotonView = GetComponent<PhotonView>();
        }
        
        [PunRPC]
        public void DestroyPieceRPC(int id) => DestroyPiece(id);

        protected override void InitializePiece(GameObject piece, int id)
        {
            piece.GetComponent<CoverPieceOnline>().InitializeCoverPiece(id, this);
        }
    }
}
