using UnityEngine;

namespace JGM.GameStore.Panels.Helpers
{
    public class PackItem3DCameraPositioner
    {
        public Vector3 GetCameraPositionFromPreviewName(in string previewName)
        {
            var cameraPosition = Vector3.zero;

            switch (previewName)
            {
                case "PF_Character1":
                    cameraPosition = new Vector3(3.39f, 2.35f, 3.37f);
                    break;

                case "PF_Character2":
                    cameraPosition = new Vector3(2.9f, 2.2f, 3f);
                    break;

                case "PF_Character3":
                    cameraPosition = new Vector3(2.55f, 2.72f, 2.46f);
                    break;

                case "PF_ItemCoins":
                    cameraPosition = new Vector3(3f, 1.26f, 2.82f);
                    break;

                case "PF_ItemGems":
                    cameraPosition = new Vector3(3f, 1.1f, 3f);
                    break;
            }

            return cameraPosition;
        }
    }
}