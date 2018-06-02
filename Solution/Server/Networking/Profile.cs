using ProtoBuf;
using UnityEngine;

namespace Networking
{
    /// <summary>
    /// A profile contains all the data we serialize per client
    /// </summary>
    [ProtoContract]
    public class Profile : System.IEquatable<Profile>
    {
        public Profile() { }
        public Profile(Peer peer)
        {
            _id = peer.ProfileID;
        }

        public int ID { get { return _id; } }

        [ProtoMember(1)]
        private readonly int _id;
        [ProtoMember(2)]
        public Vector2 Position;

        public void Save()
        {
            PeerController controller = PeerController.Get(_id);

            if (controller.Colonist != null)
                Position = controller.Colonist.transform.position;
        }
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
        public override string ToString()
        {
            return _id.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Profile)
            {
                return Equals(obj as Profile);
            }

            return false;
        }
        public bool Equals(Profile other)
        {
            if (other._id == default(int))
                return false;

            return _id == other._id;
        }
    }
}