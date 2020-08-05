using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plankton;
using PlanktonGh;
using Rhino.Geometry;

namespace MeshGrowth
{
    public class MeshGrowthSystem
    {

        private PlanktonMesh ptMesh;

        public bool Grow = false;
        public double EdgeLengthConstrainWeight;
        public double CollisionDistance;
        public double CollisionWeight;
        public double BendingResistanceWeight;

        private List<Vector3d> totalWeightedMoves;
        private List<double> totalWeights;

        public MeshGrowthSystem(Mesh startingMesh)
        {
            ptMesh = startingMesh.ToPlanktonMesh();
        }

        public Mesh GetRhinoMesh()
        {
            return ptMesh.ToRhinoMesh();
        }

        public void Update()
        {
            totalWeightedMoves = new List<Vector3d>();
            totalWeights = new List<double>();

            for (int i = 0; i < ptMesh.Vertices.Count; i++)
            {
                totalWeightedMoves.Add(new Vector3d(0, 0, 0));
                totalWeights.Add(0.0);
            }

            ProcessCollision();
            UpdateVertexPositions();
        }

        private void UpdateVertexPositions()
        {
            for (int i = 0; i < ptMesh.Vertices.Count; i++)
            {
                if (totalWeights[i] == 0.0) continue;

                Vector3d move = totalWeightedMoves[i] / totalWeights[i];
                Point3d newPosition = ptMesh.Vertices[i].ToPoint3d() + move;
                ptMesh.Vertices.SetVertex(i, newPosition.X, newPosition.Y, newPosition.Z);
            }
        }

        private void ProcessCollision()
        {
            int vertexCount = ptMesh.Vertices.Count;

            for(int i = 0;i<vertexCount;i++)
            {
                for(int j = i+1;j<vertexCount;j++)
                {
                    Vector3d move = ptMesh.Vertices[i].ToPoint3d() - ptMesh.Vertices[j].ToPoint3d();
                    double currentDistance = move.Length;
                    if (currentDistance > CollisionDistance) continue;

                    move *= 0.5 * (currentDistance - CollisionDistance) / currentDistance;

                    totalWeightedMoves[i] += move;
                    totalWeightedMoves[j] -= move;
                    totalWeights[i] += 1;
                    totalWeights[j] -= 1;
                }
            }
        }

        


        /*
        private void SplitEdge(int edgeIndex)
        {
            int newHalfEdgeIndex = Mesh.Halfedges.SplitEdge(edgeIndex);

            Mesh.Vertices.SetVertex(
                Mesh.Vertices.Count - 1,
                0.5 * (Mesh.Vertices[Mesh.Halfedges[edgeIndex].StartVertex].ToPoint3d() + Mesh.Vertices[Mesh.Halfedges[edgeIndex + 1].StartVertex].ToPoint3d()));

            if (Mesh.Halfedges[edgeIndex].AdjacentFace >= 0)
                Mesh.Faces.SplitFace(newHalfEdgeIndex, Mesh.Halfedges[edgeIndex].PrevHalfedge);

            if (Mesh.Halfedges[edgeIndex + 1].AdjacentFace >= 0)
                Mesh.Faces.SplitFace(edgeIndex + 1, Mesh.Halfedges[Mesh.Halfedges[edgeIndex + 1].NextHalfedge].NextHalfedge);
        }
        */
    }

}
