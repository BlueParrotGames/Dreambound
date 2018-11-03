namespace Dreambound.Networking.Utility
{
    public enum PacketType
    {
        Verification = 0,

        //Login System
        LoginRequest = 101,
        LoginResponse,

        //Friending System
        AccountInfo = 201,
    }
}