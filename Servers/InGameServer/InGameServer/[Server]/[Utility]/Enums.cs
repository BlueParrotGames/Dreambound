namespace BPS.InGameServer.Utility
{
    public enum PacketType
    {
        Verification = 0,

        //User Log Out
        LogoutRequest = 103,
        LogoutResponse = 104,

        //Ingame Fiends Menu
        AccountInfo = 201,
        OnlineFriendsRequest = 202,
        OnlineFriendsResponse = 203,
    }
}