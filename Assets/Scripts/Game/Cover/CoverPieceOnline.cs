using Photon.Pun;

namespace SIVS
{
    public class CoverPieceOnline : CoverPiece
    {
        protected override void OnPieceHit()
        {
            var coverOnline = (CoverOnline) _cover;
            
            if (coverOnline == null || !coverOnline.PhotonView.IsMine)
                return;
            
            SoundPlayer.PlaySound(explosionSound, 0.65f);
            
            coverOnline.PhotonView.RPC("DestroyPieceRPC", RpcTarget.AllBuffered, _id);
        }

        public void InitializeCoverPiece(int id, CoverOnline coverComponent)
        {
            _id = id;
            _cover = coverComponent;
        }
    }
}
