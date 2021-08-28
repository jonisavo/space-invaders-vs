using Photon.Pun;

namespace SIVS
{
    public class CoverPieceOnline : CoverPiece
    {
        private CoverOnline _cover;

        protected override void OnPieceHit()
        {
            if (!_cover.PhotonView.IsMine)
                return;
            
            SoundPlayer.PlaySound(explosionSound, 0.65f);
            
            _cover.PhotonView.RPC("DestroyPieceRPC", RpcTarget.AllBuffered, _id);
        }

        public void InitializeCoverPiece(int id, CoverOnline coverComponent)
        {
            _id = id;
            _cover = coverComponent;
        }
    }
}
