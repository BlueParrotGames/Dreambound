namespace Dreambound.Networking.Utility
{
    public enum PacketType
    {
        Verification = 0,

        //Log -in/out  System
        LoginRequest = 101,
        LoginResponse,
        LogoutRequest,
        LogoutResponse,

        //Friending System
        AccountInfo = 201,
        OnlineFriendsRequest = 202,
        OnlineFriendsResponse = 203,
    }
}