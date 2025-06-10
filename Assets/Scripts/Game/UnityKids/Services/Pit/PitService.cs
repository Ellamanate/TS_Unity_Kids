using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Game
{
    public class PitService : IDisposable
    {
        private readonly AnimationsConfig _animationsConfig;
        private readonly RectTransform _pitTransform;
        private readonly Canvas _canvas;
        private readonly Material _maskableMaterial;
        private readonly CancellationTokenSource _tokenSource;

        public PitService(
            AnimationsConfig animationsConfig,
            RectTransform pitTransform,
            Canvas canvas,
            Material maskableMaterial)
        {
            _animationsConfig = animationsConfig;
            _pitTransform = pitTransform;
            _canvas = canvas;
            _maskableMaterial = maskableMaterial;
            _tokenSource = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        public void SetCube(CubeView view)
        {
            view.Dispose();
            _ = PlayAnimation(view, _tokenSource.Token);
        }

        private async UniTaskVoid PlayAnimation(CubeView cube, CancellationToken cancellationToken)
        {
            float cubeHeight = cube.RectTransform.sizeDelta.y * _canvas.scaleFactor;
            var upOffset = new Vector3(0, cubeHeight * _animationsConfig.PitOffsetUp, 0);
            var downOffset = new Vector3(0, cubeHeight * _animationsConfig.PitOffsetDown, 0);
            var upPosition = _pitTransform.position + upOffset;
            var downPosition = _pitTransform.position - downOffset;

            var sequence = DOTween.Sequence()
                .Append(cube.RectTransform.DOMove(upPosition, _animationsConfig.PitFallDuration / 2f))
                .AppendCallback(() => cube.SetMaterial(_maskableMaterial))
                .Append(cube.RectTransform.DOMove(downPosition, _animationsConfig.PitFallDuration / 2f))
                .SetEase(_animationsConfig.PitFallEase);

            await sequence.AsyncWaitForKill(cancellationToken);
            
            UnityEngine.Object.Destroy(cube.gameObject);
        }
    }
}