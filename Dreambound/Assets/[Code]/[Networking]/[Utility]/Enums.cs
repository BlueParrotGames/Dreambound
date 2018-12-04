namespace Dreambound.Networking.Utility
{
    public enum PacketType
    {
        Verification = 0,
        
        //Friending System
        AccountInfo = 201,
        OnlineFriendsRequest = 202,
        OnlineFriendsResponse = 203,

        //Peer to Peer
    }
}