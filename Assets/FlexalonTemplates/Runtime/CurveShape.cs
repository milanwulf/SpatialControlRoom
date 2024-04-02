using UnityEngine;
using System.Collections.Generic;

namespace Flexalon.Templates
{
    // Modifies a Curve Layout such that the points form the desired shape.
    [ExecuteAlways, RequireComponent(typeof(FlexalonCurveLayout)), DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Curve Shape")]
    public class CurveShape : MonoBehaviour
    {
        public enum ShapeType
        {
            Line,
            Triangle,
            Rectangle,
            Pentagon,
            Star,
            Hexagon,
            Octogon,
        }

        public enum ShapePlane
        {
            XZ,
            XY,
            ZY
        }

        // Shape to form.
        [SerializeField]
        private ShapeType _shape = ShapeType.Rectangle;
        public ShapeType Shape
        {
            get => _shape;
            set { _shape = value; }
        }

        // Which plane to draw the shape on.
        [SerializeField]
        private ShapePlane _plane = ShapePlane.XZ;
        public ShapePlane Plane
        {
            get => _plane;
            set { _plane = value; }
        }

        // Width of the shape.
        [SerializeField]
        private float _width = 1;
        public float Width
        {
            get => _width;
            set { _width = value; }
        }

        // Height of the shape.
        [SerializeField]
        private float _height = 1;
        public float Height
        {
            get => _height;
            set { _height = value; }
        }

        private ShapeType _lastShape;
        private float _lastWidth;
        private float _lastHeight;
        private ShapePlane _lastAxis;
        private FlexalonCurveLayout _curveLayout;

        void Update()
        {
            if (_lastShape == _shape && _lastWidth == _width && _lastHeight == _height && _plane == _lastAxis)
            {
                return;
            }

            if (_curveLayout == null)
            {
                _curveLayout = GetComponent<FlexalonCurveLayout>();
                _curveLayout.LockPositions = true;
                _curveLayout.LockTangents = true;
            }

            _lastShape = _shape;
            _lastWidth = _width;
            _lastHeight = _height;
            _lastAxis = _plane;

            var points = new List<Vector2>();

            var functions = new Dictionary<ShapeType, System.Action<List<Vector2>>> {
                { ShapeType.Line, MakeLine },
                { ShapeType.Triangle, MakeTriangle },
                { ShapeType.Rectangle, MakeRectangle },
                { ShapeType.Pentagon, MakePentagon },
                { ShapeType.Star, MakeStar },
                { ShapeType.Hexagon, MakeHexagon },
                { ShapeType.Octogon, MakeOctogon }
            };

            functions[_shape](points);

            var curvePoints = new List<FlexalonCurveLayout.CurvePoint>();
            foreach (var point in points)
            {
                var pos = Vector3.zero;
                switch (_plane)
                {
                    case ShapePlane.XY:
                        pos.x = point.x * _width;
                        pos.y = point.y * _height;
                        break;
                    case ShapePlane.XZ:
                        pos.x = point.x * _width;
                        pos.z = point.y * _height;
                        break;
                    case ShapePlane.ZY:
                        pos.z = point.x * _width;
                        pos.y = point.y * _height;
                        break;
                }

                curvePoints.Add(new FlexalonCurveLayout.CurvePoint {
                    Position = pos,
                    Tangent = Vector3.zero,
                    TangentMode = FlexalonCurveLayout.TangentMode.Manual
                });
            }

            _curveLayout.SetPoints(curvePoints);
        }

        private void MakeLine(List<Vector2> points)
        {
            points.Add(new Vector2(-0.5f, -0.5f));
            points.Add(new Vector2(0.5f, 0.5f));
        }

        private void MakeTriangle(List<Vector2> points)
        {
            // There's no way to make a perfect triangle with equal width/height.
            var halfHeight = 0.433012702f;
            points.Add(new Vector2(-0.5f, -halfHeight));
            points.Add(new Vector2(0, halfHeight));
            points.Add(new Vector2(0.5f, -halfHeight));
            points.Add(new Vector2(-0.5f, -halfHeight));
        }

        private void MakeRectangle(List<Vector2> points)
        {
            points.Add(new Vector2(-0.5f, -0.5f));
            points.Add(new Vector2(-0.5f, 0.5f));
            points.Add(new Vector2(0.5f, 0.5f));
            points.Add(new Vector2(0.5f, -0.5f));
            points.Add(new Vector2(-0.5f, -0.5f));
        }

        private void MakePentagon(List<Vector2> points)
        {
            points.Add(PointOnCircle(5 * Mathf.PI / 10));
            points.Add(PointOnCircle(1 * Mathf.PI / 10));
            points.Add(PointOnCircle(17 * Mathf.PI / 10));
            points.Add(PointOnCircle(13 * Mathf.PI / 10));
            points.Add(PointOnCircle(9 * Mathf.PI / 10));
            points.Add(PointOnCircle(5 * Mathf.PI / 10));
        }

        private void MakeStar(List<Vector2> points)
        {
            points.Add(PointOnCircle(5 * Mathf.PI / 10));
            points.Add(PointOnCircle(3 * Mathf.PI / 10) * 0.5f);
            points.Add(PointOnCircle(1 * Mathf.PI / 10));
            points.Add(PointOnCircle(19 * Mathf.PI / 10) * 0.5f);
            points.Add(PointOnCircle(17 * Mathf.PI / 10));
            points.Add(PointOnCircle(15 * Mathf.PI / 10) * 0.5f);
            points.Add(PointOnCircle(13 * Mathf.PI / 10));
            points.Add(PointOnCircle(11 * Mathf.PI / 10) * 0.5f);
            points.Add(PointOnCircle(9 * Mathf.PI / 10));
            points.Add(PointOnCircle(7 * Mathf.PI / 10) * 0.5f);
            points.Add(PointOnCircle(5 * Mathf.PI / 10));
        }

        private void MakeHexagon(List<Vector2> points)
        {
            points.Add(PointOnCircle(0));
            points.Add(PointOnCircle(10 * Mathf.PI / 6));
            points.Add(PointOnCircle(8 * Mathf.PI / 6));
            points.Add(PointOnCircle(6 * Mathf.PI / 6));
            points.Add(PointOnCircle(4 * Mathf.PI / 6));
            points.Add(PointOnCircle(2 * Mathf.PI / 6));
            points.Add(PointOnCircle(0));
        }

        private void MakeOctogon(List<Vector2> points)
        {
            points.Add(PointOnCircle(0));
            points.Add(PointOnCircle(14 * Mathf.PI / 8));
            points.Add(PointOnCircle(12 * Mathf.PI / 8));
            points.Add(PointOnCircle(10 * Mathf.PI / 8));
            points.Add(PointOnCircle(8 * Mathf.PI / 8));
            points.Add(PointOnCircle(6 * Mathf.PI / 8));
            points.Add(PointOnCircle(4 * Mathf.PI / 8));
            points.Add(PointOnCircle(2 * Mathf.PI / 8));
            points.Add(PointOnCircle(0));
        }

        private Vector2 PointOnCircle(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}