using UnityEngine;
using ViewModels;

namespace Views
{
    public class BoardView : MonoBehaviour
    {
        private const float CELL_SIZE = 1.5f;

        private BoardViewModel m_viewModel;

        public void Initialize(BoardViewModel _viewModel)
        {
            m_viewModel = _viewModel;
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            Clear();

            var size = m_viewModel.size.Value;
            var halfSize = size * CELL_SIZE * 0.5f;
            var startOffset = -halfSize + CELL_SIZE * 0.5f;

            for (var x = 0; x < size; x++)
            {
                for (var z = 0; z < size; z++)
                {
                    var cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.name = $"Cell {x}_{z}";
                    cell.transform.SetParent(transform, false);

                    cell.transform.localPosition = new Vector3(
                        startOffset + x * CELL_SIZE,
                        -0.025f,
                        startOffset + z * CELL_SIZE
                    );

                    cell.transform.localScale = new Vector3(CELL_SIZE, 0.05f, CELL_SIZE);

                    var renderer = cell.GetComponent<Renderer>();
                    renderer.sharedMaterial = CreateCellMaterial(GetCellColor(x, z));

                    var collider = cell.GetComponent<Collider>();

                    if (collider != null)
                        Destroy(collider);
                }
            }
        }

        private Color GetCellColor(int _x, int _z)
        {
            var isBlack = (_x + _z) % 2 == 0;
            return isBlack ? m_viewModel.blackCellColor.Value : m_viewModel.whiteCellColor.Value;
        }

        private static Material CreateCellMaterial(Color _color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");

            if (shader == null)
                shader = Shader.Find("Standard");

            var material = new Material(shader);
            material.color = _color;

            return material;
        }

        private void Clear()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}