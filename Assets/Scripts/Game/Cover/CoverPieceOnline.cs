using Photon.Pun;

namespace SIVS
{
    public class CoverPieceOnline : CoverPiece
    {
        protected override void OnPieceHit()
        {
            var coverOnline = (CoverOnline) Cover;
            
            if (coverOnline == null || !coverOnline.PhotonView.IsMine)
                return;
            
            SoundPlayer.PlaySound(explosionSound, 0.65f);
            
            coverOnline.PhotonView.RPC("DestroyPieceRPC", RpcTarget.AllBuffered, ID);
        }
    }
}
