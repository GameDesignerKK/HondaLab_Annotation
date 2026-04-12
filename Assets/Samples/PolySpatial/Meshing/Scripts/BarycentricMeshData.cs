using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace PolySpatial.Samples
{
    public class BarycentricMeshData : MonoBehaviour
    {
        [SerializeField]
        ARMeshManager m_MeshManager;

        [SerializeField]
        BarycentricDataBuilder m_DataBuilder;

        void OnEnable()
        {
            if (m_MeshManager != null)
                m_MeshManager.meshInfosChanged.AddListener(MeshManagerOnMeshesChanged);
        }

        void OnDisable()
        {
            if (m_MeshManager != null)
                m_MeshManager.meshInfosChanged.RemoveListener(MeshManagerOnMeshesChanged);
        }

        void MeshManagerOnMeshesChanged(ARMeshInfosChangedEventArgs args)
        {
            // Handle added meshes
            foreach (var info in args.added)
            {
                var filter = info.meshFilter;
                if (filter != null && filter.sharedMesh != null)
                {
                    m_DataBuilder.GenerateData(filter.sharedMesh);
                }
            }

            // Handle updated meshes
            foreach (var info in args.updated)
            {
                var filter = info.meshFilter;
                if (filter != null && filter.sharedMesh != null)
                {
                    m_DataBuilder.GenerateData(filter.sharedMesh);
                }
            }
        }
    }
}
