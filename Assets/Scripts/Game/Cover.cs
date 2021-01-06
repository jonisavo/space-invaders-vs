using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class Cover : MonoBehaviourPunCallbacks
    {
        [Tooltip("The cover piece object the cover consists of.")]
        public GameObject coverPiece;
        
        [Tooltip("The number of cover piece rows.")]
        public int rows = 5;

        [Tooltip("The number of cover piece columns.")]
        public int columns = 4;
        
        private Dictionary<int, GameObject> _pieces;
        
        #region Unity Callbacks

        private void Awake()
        {
            _pieces = new Dictionary<int, GameObject>();
        }

        private void Start()
        {
            InstantiateAllPieces();
        }
        
        #endregion

        [PunRPC]
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

            piece.GetComponent<CoverPiece>().InitializeCoverPiece(id, this);

            _pieces[id] = piece;
        }
    }
}
