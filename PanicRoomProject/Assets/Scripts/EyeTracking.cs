using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobii.Research.Unity.Examples
{
    public class EyeTracking : MonoBehaviour
    {
        private VREyeTracker _eyeTracker;
        private VRGazeTrail _gazeTrail;
        private VRCalibration _calibration;
        private VRSaveData _saveData;
        private VRPositioningGuide _positioningGuide;

        void Start()
        {
            // Cache our prefab scripts.
            _eyeTracker = VREyeTracker.Instance;
            _gazeTrail = VRGazeTrail.Instance;
            _calibration = VRCalibration.Instance;
            _saveData = VRSaveData.Instance;
            _positioningGuide = VRPositioningGuide.Instance;

            // Move HUD to be in front of user.
            var etOrigin = VRUtility.EyeTrackerOriginVive;
            //var holder = _threeDText.transform.parent;
            /*holder.parent = etOrigin;
            holder.localPosition = new Vector3(0, -1.35f, 3);
            holder.localRotation = Quaternion.Euler(25, 0, 0);*/
        }

        void Update()
        {
            // We are expecting to have all objects.
            if (!_eyeTracker || !_gazeTrail || !_calibration || !_saveData || !_positioningGuide)
            {
                return;
            }

            // Thin out updates a bit.
            if (Time.frameCount % 9 != 0)
            {
                return;
            }

            // Create an informational string.
            var info = string.Format("{0}\nLatest hit object: {1}\nCalibration in progress: {2}, Saving data: {3}\nPositioning guide visible: {4}",
                string.Format("L: {0}\nR: {1}",
                    _eyeTracker.LatestProcessedGazeData.Left.GazeRayWorldValid ? _eyeTracker.LatestProcessedGazeData.Left.GazeRayWorld.ToString() : "No gaze",
                    _eyeTracker.LatestProcessedGazeData.Right.GazeRayWorldValid ? _eyeTracker.LatestProcessedGazeData.Right.GazeRayWorld.ToString() : "No gaze"),
                _gazeTrail.LatestHitObject != null ? _gazeTrail.LatestHitObject.name : "Nothing",
                _calibration.CalibrationInProgress ? "Yes" : "No",
                _saveData.SaveData ? "Yes" : "No",
                _positioningGuide.PositioningGuideActive ? "Yes" : "No");

            Transform latest = _gazeTrail.LatestHitObject;
            if (latest != null)
            {

            }

            Debug.Log(info);
        }
    }
}
