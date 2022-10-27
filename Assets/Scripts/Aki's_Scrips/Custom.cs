/*using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class Custom
{
    private const string TimeKey = "Time";

    private static readonly Hashtable TimeProp = new Hashtable();

    //現在時刻を取得する。
    public static void SetTime(this Player player,int time){
        TimeProp[TimeKey] = PhotonNetwork.ServerTimestamp;
        PhotonNetwork.CurrentRoom.SetCustomProperties(TimeProp);
        player.SetCustomProperties(TimeProp);
    }
}
*/