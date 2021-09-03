using System.Collections;
using System.Collections.Generic;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    public class Cover : MonoBehaviour
    {
        [Tooltip("The cover piece object the cover consists of.")]
        [NotNull]
        public GameObject coverPiece;
        
        [Tooltip("The number of cover piece rows.")]
        public int rows = 5;

        [Tooltip("The number of cover piece columns.")]
        public int columns = 4;
        
        private Dictionary<int, CoverPiece> _pieces;

        protected virtual void Awake() => _pieces = new Dictionary<int, CoverPiece>();

        protected void Start() => InstantiateAllPieces();

        private void OnEnable() => SIVSPlayer.OnLivesChange += HandleLivesChange;

        private void OnDisable() => SIVSPlayer.OnLivesChange -= HandleLivesChange;

        public void DestroyPiece(int id)
        {
            if (!_pieces.ContainsKey(id)) return;

            Destroy(_pieces[id].gameObject);

            _pieces.Remove(id);
        }

        public void DestroyAllPieces()
        {
            foreach (var piece in _pieces.Values)
                Destroy(piece);

            _pieces.Clear();
        }

        public void AddPieces(int count)
        {
            var maximumAllowedSize = rows * columns;

            if (_pieces.Count + count > maximumAllowedSize)
                count = maximumAllowedSize - _pieces.Count;

            if (count <= 0) return;

            InstantiatePieces(count);
        }

        private void InstantiateAllPieces() => InstantiatePieces(rows * columns);

        private void InstantiatePieces(int count)
        {
            var id = 0;

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    if (!_pieces.ContainsKey(id))
                    {
                        InstantiatePiece(id, row, column);
                        count--;
                    }

                    id++;

                    if (count <= 0) return;
                }
            }
        }

        private void InstantiatePiece(int id, int row, int column)
        {
            var piece = Instantiate(coverPiece, new Vector3(row * 0.08f, column * 0.08f, 0),
                Quaternion.identity);
                    
            piece.transform.SetParent(gameObject.transform, false);

            InitializePiece(piece, id);
        }

        private void InitializePiece(GameObject piece, int id)
        {
            var coverPieceComponent = piece.GetComponent<CoverPiece>();
            
            coverPieceComponent.InitializeCoverPiece(id, this);

            _pieces[id] = coverPieceComponent;
        }

        private void HandleLivesChange(SIVSPlayer player, int newLives)
        {
            if (!BelongsToPlayer(player) || newLives > 0)
                return;

            StartCoroutine(DropCoroutine());
        }

        private IEnumerator DropCoroutine()
        {
            foreach (var piece in _pieces.Values)
                piece.MakeRigidbodyDynamic();

            yield return new WaitForSeconds(10f);
            
            DestroyAllPieces();
        }

        protected virtual bool BelongsToPlayer(SIVSPlayer player) =>
            GetComponent<Ownership>().Owner.Number == player.Number;
    }
}
