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
        
        protected Dictionary<int, GameObject> _pieces;
        
        #region Unity Callbacks

        protected virtual void Awake() => _pieces = new Dictionary<int, GameObject>();

        protected void Start() => InstantiateAllPieces();

        #endregion
        
        public void DestroyPiece(int id)
        {
            if (!_pieces.ContainsKey(id)) return;

            Destroy(_pieces[id]);

            _pieces.Remove(id);
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

            _pieces[id] = piece;
        }

        protected virtual void InitializePiece(GameObject piece, int id)
        {
            piece.GetComponent<CoverPiece>().InitializeCoverPiece(id, this);
        }
    }
}
