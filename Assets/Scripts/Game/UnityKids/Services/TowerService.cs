using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class TowerService
    {
        private readonly List<CubeView> _cubes;
        private readonly float _screenHeight;

        public TowerService()
        {
            _cubes = new List<CubeView>();
            _screenHeight = Screen.height;
        }

        public bool TryGetLastCube(out CubeView cube)
        {
            cube = _cubes.Last();
            return cube != null;
        }
        
        public bool IsInTower(CubeView cube)
        {
            return _cubes.Contains(cube);
        }
        
        public bool HasCubes()
        {
            return _cubes.Count > 0;
        }
        
        public bool CanAdd()
        {
            if (!HasCubes()) return true;
            
            var top = _cubes[^1];
            var nextY = top.RectTransform.position.y + top.RectTransform.sizeDelta.y;
            return nextY <= _screenHeight;
        }

        public void Add(CubeView cube)
        {
            var position = _cubes.Count == 0
                ? cube.RectTransform.position
                : _cubes[^1].RectTransform.position + new Vector3(0, cube.RectTransform.sizeDelta.y, 0);

            cube.RectTransform.position = position;
            _cubes.Add(cube);
        }
        
        public void Remove(CubeView cube)
        {
            int index = _cubes.IndexOf(cube);
            
            if (index >= 0)
            {
                _cubes.RemoveAt(index);
                
                for (int i = index; i < _cubes.Count; i++)
                {
                    var downTarget = _cubes[i].RectTransform.position;
                    downTarget.y -= cube.RectTransform.sizeDelta.y;
                    _cubes[i].RectTransform.position = downTarget;
                }
            }
        }
    }
}